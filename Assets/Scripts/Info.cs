using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    [Rename("Info Button")]
    public Button btnInfo;

    [Rename("URL (https://)")]
    public string url;

    void Start()
    {
        btnInfo.onClick.AddListener(delegate
        {
            Application.OpenURL($"https://{url}");
        });
    }
}
