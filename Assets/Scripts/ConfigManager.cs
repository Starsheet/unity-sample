using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    
    public static ConfigManager Instance;
    public List<CrosspromoBanner> crosspromoBanners = new List<CrosspromoBanner>();

    void Awake() {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
