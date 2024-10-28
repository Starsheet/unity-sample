using System;
using Newtonsoft.Json;
using System.Collections.Generic;

[Serializable]
public class CrosspromoBanner
{
    [JsonProperty("title")]
    public string title;

    [JsonProperty("description")]
    public string description;

    [JsonProperty("button_title")]
    public string buttonTitle;

    [JsonProperty("button_url")]
    public string buttonUrl;

    
    [JsonProperty("icon")]
    public Dictionary<string, string> icon;
    
}
