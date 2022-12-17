using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    public Text txt;
    public string mode;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BlinkText());
        mode = gameObject.GetComponent<TimerProgressive>().mode;
    }

    // Update is called once per frame
    void Update()
    {
        mode = gameObject.GetComponent<TimerProgressive>().mode;
        txt = gameObject.GetComponent<TimerProgressive>().txtCounter;
        //print(txt.text);
    }

    IEnumerator BlinkText()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.75f);
            if (mode == "progressive")
            {
                txt.enabled = !txt.enabled;
            }
            if (mode == "regressive")
            {
                txt.enabled = true;
            }
        }
    }
}
