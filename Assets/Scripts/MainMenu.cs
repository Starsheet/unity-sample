using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    private UIDocument _document;

    private void Awake()
    {
        
        //Debug.Log(StarsheetConfig.Instance.foo);
        _document = GetComponent<UIDocument>();

        VisualElement newsContainer = _document.rootVisualElement.Q("NewsContainer") as VisualElement;

        foreach( CrosspromoBanner banner in ConfigManager.Instance.crosspromoBanners )
        {
            byte[] data = System.IO.File.ReadAllBytes (StarsheetDownloader.ImageCacheDirectory + "/" + banner.icon["filename"]);
            Texture2D tex = new Texture2D(64,64);
            tex.LoadImage(data);        

            VisualElement newsItem = new VisualElement();
            newsItem.AddToClassList("news-item");

            Label title = new Label();
            title.AddToClassList("news-title");
            title.text = banner.title;
            newsItem.Add(title);

            Label body = new Label();
            body.AddToClassList("news-body");
            body.text = banner.description;
            newsItem.Add(body);

            VisualElement icon = new VisualElement();
            icon.AddToClassList("news-icon");
            icon.style.backgroundImage = new StyleBackground(tex);
            newsItem.Add(icon);

            Button button = new Button();
            button.text = banner.buttonTitle;
            button.clicked += () =>
            {
                Application.OpenURL(banner.buttonUrl);
            };
            newsItem.Add(button);

            newsContainer.Add(newsItem);
        }
        

        

        

        
        




       

    }
}
