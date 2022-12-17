using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadConfig : MonoBehaviour
{
    public Canvas cnvControl;

    void Start() {  }

    void Update() { if (Input.GetKeyDown("space")) cnvControl.enabled = !cnvControl.isActiveAndEnabled; }
}
