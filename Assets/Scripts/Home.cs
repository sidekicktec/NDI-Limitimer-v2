using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
    public Button btnModeText;
    public Button btnModeProgressive;

    public Button btnModeResolume;
    
    // Start is called before the first frame update
    void Start()
    {
        btnModeText.onClick.AddListener(delegate
        {
            SceneManager.LoadScene("ModeText", LoadSceneMode.Single);
        });

        btnModeProgressive.onClick.AddListener(delegate
        {
            SceneManager.LoadScene("ModeProgressive", LoadSceneMode.Single);
        });

        btnModeResolume.onClick.AddListener(delegate
        {
            SceneManager.LoadScene("ResolumeControl", LoadSceneMode.Single);
        });
    }
}
