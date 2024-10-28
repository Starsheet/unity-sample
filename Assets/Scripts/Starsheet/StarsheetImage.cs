using System;
using Newtonsoft.Json;

[Serializable]
public class StarsheetImage
{
    [JsonProperty("url")]
    public string url;

    [JsonProperty("size_bytes")]
    public string sizeBytes;

    [JsonProperty("content_type")]
    public string contentType;

    public string fileName {
        get {
            //return url.Split('/').Last();
            int index = url.LastIndexOf('/');
            return url.Substring(index + 1);
        }
    }
}
