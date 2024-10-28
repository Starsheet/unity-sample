using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Splash : MonoBehaviour
{
    private UIDocument _document;
    Button _downloadButton;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _downloadButton = _document.rootVisualElement.Q("Download") as Button;
        _downloadButton.clicked += () =>
        {
            StarsheetDownloader sd = new StarsheetDownloader("https://demo.starsheet.app/game/live.json");
            sd.OnDebugMessage += DisplayLoadingMessage;
            sd.OnComplete += ShowContinueButton;
            sd.OnDataLoaded += HandleStartsheetData;
            StartCoroutine(sd.GetDataWithImages());

            _downloadButton.style.display = DisplayStyle.None;
        };

        
    }

    private void DisplayLoadingMessage(string msg)
    {
        Label label = _document.rootVisualElement.Q("Status") as Label;
        label.text += $"{msg}\n";
    }

    private void ShowContinueButton() {



        Button button = _document.rootVisualElement.Q("GoToMainMenu") as Button;
        button.clicked += () =>
        {
            SceneManager.LoadScene("Menu");
        };
        button.style.display = DisplayStyle.Flex;
    }

    private void HandleStartsheetData(JObject json)
    {
        JEnumerable<JToken> results = json["crosspromo_banners"].Children();

        foreach( JToken result in results )
        {
            ConfigManager.Instance.crosspromoBanners.Add(result.ToObject<CrosspromoBanner>());
        }
    } 

    
}
