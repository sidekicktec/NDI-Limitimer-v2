using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorDetect : MonoBehaviour
{
    public Canvas cnvControl;

    void Start()
    {
        cnvControl.enabled = Application.isEditor ? false : false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
