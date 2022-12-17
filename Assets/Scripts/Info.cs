using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    public Button btnInfo;
    // Start is called before the first frame update
    void Start()
    {
        btnInfo.onClick.AddListener(delegate
        {
            Application.OpenURL("https://smartium.github.io/NDI-Limitimer/");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
