using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class StarsheetDownloader
{
    public static string CacheDirectory = Application.persistentDataPath + "/cache/";
    public static string ImageCacheDirectory = CacheDirectory + "images/";

    protected string _url;
    protected int _cachedImageCount = 0;
    protected int _downloadedImageCount = 0;

    protected string CacheKey
    {
        get {
            return _url.GetHashCode().ToString();
        }
    }

    protected string CachePath {
        get {
            return CacheDirectory + CacheKey + ".json";
        }
    }

    protected string EtagPath {
        get {
            return CacheDirectory + CacheKey + ".etag.txt";
        }
    }

    public event Action<string> OnDebugMessage; 
    public event Action OnComplete; 
    public event Action<JObject> OnDataLoaded; 

    public StarsheetDownloader(string url) {
        _url = url;
        SetupCacheFolders();
    }

    protected void SetupCacheFolders() {
        if(!Directory.Exists(CacheDirectory)) {
            Directory.CreateDirectory(CacheDirectory);
        }

        if(!Directory.Exists(ImageCacheDirectory)) {
            Directory.CreateDirectory(ImageCacheDirectory);
        }
    }

    public string GetBaseUrl() 
    {
        string[] urlWithoutScheme = _url.Split("://", 2);
        string[] urlParts = urlWithoutScheme[1].Split("/", 2);

        return string.Concat(urlWithoutScheme[0], "://", urlParts[0]);
    }

    public IEnumerator GetDataWithImages()
    {
        OnDebugMessage?.Invoke($"[Starsheet] Loading data from {_url}");
        using (UnityWebRequest req = UnityWebRequest.Get(_url))
        {
            string etag = GetCachedResponseEtag();
            if(etag != string.Empty)
            {
                OnDebugMessage?.Invoke($"[Starsheet] Locally cached copy of data that URL found (Etag: {etag})");
                req.SetRequestHeader("If-None-Match", etag);
            }

            yield return req.SendWebRequest();
            string response = "";

            if (req.result == UnityWebRequest.Result.Success)
            {
                if(req.responseCode == 200) {
                    OnDebugMessage?.Invoke("[Starsheet] Remote data downlaoded");
                    
                    System.IO.File.WriteAllText(CachePath, req.downloadHandler.text);
                    System.IO.File.WriteAllText(EtagPath, req.GetResponseHeader("ETag"));
                    
                    response = req.downloadHandler.text;
                }
                else if(req.responseCode == 304) {
                    OnDebugMessage?.Invoke("[Starsheet] Cached data is up to date. Using local copy");
                    response = File.ReadAllText(CachePath);
                    
                }

                JObject parsedResponse = JObject.Parse(response);
                OnDataLoaded?.Invoke(parsedResponse);
                

                Dictionary<string, StarsheetImage> images = parsedResponse.GetValue("$images").ToObject<Dictionary<string, StarsheetImage>>();
                OnDebugMessage?.Invoke($"[Starsheet] {images.Count} remote images referenced in data. Starting sync");
                foreach(StarsheetImage image in images.Values)
                {
                    yield return DownloadImage(image);
                }
                OnDebugMessage?.Invoke($"[Starsheet] {_downloadedImageCount} images downloaded. {_cachedImageCount} from local cache. ");
                
            }
            OnDebugMessage?.Invoke($"[Starsheet] Loading Complete");
            OnComplete?.Invoke();
        }
    }

    protected IEnumerator DownloadImage(StarsheetImage image) {
        string fullUrl = string.Concat(GetBaseUrl(), image.url);

        string fileCachePath = ImageCacheDirectory + image.fileName;
        if(File.Exists(fileCachePath))
        {
            _cachedImageCount++;
            yield break;
        }

        _downloadedImageCount++;
        using (UnityWebRequest req = UnityWebRequest.Get(fullUrl))
        {
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success) 
            {
                System.IO.File.WriteAllBytes(fileCachePath, req.downloadHandler.data);
            }
        }
    }


    protected string GetCachedResponseEtag() 
    { 
        string etag = "";
        if(File.Exists(EtagPath) && File.Exists(CachePath))
        {
            etag = File.ReadAllText(EtagPath);
        }

        return etag;

    }

}
