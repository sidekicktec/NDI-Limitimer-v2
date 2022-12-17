using System;
using System.Collections;
using System.Collections.Generic;
//using System.Net;
//using System.Text.RegularExpressions;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class TimerOscSimpl : MonoBehaviour
{
    public bool columnsTrigger;
    public Text txtCounter;
    public int time;
    public int OscInPort;
    public int OscOutPort;

    public GameObject inputTime;
    public Button btnConfirm;
    public Button btnStart;
    OscIn oscIn;
    OscOut oscOut;
    OscMessage outMessage;

    private int counter;
    private bool isPlaying = false;
    private Coroutine counterRoutine;

    const string startOscAddr = "/limitimer/timer/start";
    const string resetOscAddr = "/limitimer/timer/reset";
    const string addMinOscAddr = "/limitimer/timer/addminute";
    const string subMinOscAddr = "/limitimer/timer/subminute";
    const string setTimeMinOscAddr = "/limitimer/timer/settime";

    IEnumerator Start()
    {
        counter = time;
        oscIn = gameObject.AddComponent<OscIn>();
        oscOut = gameObject.AddComponent<OscOut>();

        oscIn.Open(OscInPort);
        oscOut.Open(OscOutPort);

        oscIn.Map(startOscAddr, OscMessageReceive);
        oscIn.Map(resetOscAddr, OscMessageReceive);
        oscIn.Map(addMinOscAddr, OscMessageReceive);
        oscIn.Map(subMinOscAddr, OscMessageReceive);
        oscIn.Map(setTimeMinOscAddr, OscMessageReceive);

        //StartCoroutine(OscListener());
        //inputTime.GetComponentInChildren<Text>().text = "test";
        time *= 60;

        btnStart.onClick.AddListener(delegate
        {
            if (isPlaying)
            {
                stopTimer();
            }
            else
            {
                if (counter < 1)
                {
                    stopTimer("reset");
                }
                else
                {
                    startTimer(time);
                }
            }

            isPlaying = !isPlaying;
        });

        btnConfirm.onClick.AddListener(delegate
        {
            time = int.Parse(inputTime.GetComponentInChildren<Text>().text) * 60;
            //txtCounter.text = "";
            counter = time;
            updateCounterDisplay();
        });

        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            updateCounterDisplay();
        }

        //if (Input.GetKeyDown("space"))
        //{
        //    print("space key was pressed");
        //    if (isPlaying)
        //    {
        //        stopTimer();
        //    }
        //    else
        //    {
        //        if (counter < 1)
        //        {
        //            stopTimer("reset");
        //        }
        //        else
        //        {
        //            startTimer(time);
        //        }
        //    }

        //    isPlaying = !isPlaying;
        //}
    }

    IEnumerator Countdown(int seconds)
    {
        counter = seconds;
        while (counter > -1)
        {
            yield return new WaitForSeconds(1);
            //print(counter);
            if (counter <= Math.Round(time * 0.2f) + 20 && counter > Math.Round(time * 0.1f))
            {
                txtCounter.color = Color.yellow;
            }

            else if (counter <= Math.Round(time * 0.1f) + 1)
            {
                txtCounter.color = Color.red;
            }

            counter -= 1;
        }
        isPlaying = !isPlaying;
        btnStart.GetComponentInChildren<Text>().text = "READY";
        btnStart.GetComponentInChildren<Text>().color = Color.black;
        btnStart.image.color = Color.yellow;
        txtCounter.text = "OVER";

    }

    IEnumerator OscListener()
    {
        yield break;
    }

    void AddMinute()
    {
        counter = counter + 60;
    }

    void RemoveMinute()
    {
        counter = counter - 60;
    }

    void startTimer(int _time)
    {
        txtCounter.color = Color.green;
        counterRoutine = StartCoroutine(Countdown(_time));
        btnStart.GetComponentInChildren<Text>().text = "STOP";
        btnStart.GetComponentInChildren<Text>().color = Color.white;
        inputTime.SetActive(false);
        btnConfirm.gameObject.SetActive(false);
        btnStart.image.color = Color.red;
    }

    void stopTimer(string _reset = "no")
    {
        if (_reset != "reset")
        {
            StopCoroutine(counterRoutine);
            btnStart.image.color = Color.green;
        }
        btnStart.GetComponentInChildren<Text>().text = "START";
        btnStart.GetComponentInChildren<Text>().color = Color.black;

        counter = time;
        inputTime.SetActive(true);
        btnConfirm.gameObject.SetActive(true);
    }

    void updateCounterDisplay()
    {
        int hours = counter / 60 / 60;
        int minutes = counter / 60 % 60;
        int seconds = counter % 60;
        string strMinutes = null;
        string strSeconds = null;

        if (minutes < 10)
        {
            strMinutes = "0" + minutes;
        }
        else
        {
            strMinutes = minutes.ToString();
        }
        if (seconds < 10)
        {
            strSeconds = "0" + seconds;
        }
        else
        {
            strSeconds = seconds.ToString();
        }
        txtCounter.text = hours + ":" + strMinutes + ":" + strSeconds;
    }

    void OscMessageReceive(OscMessage incomingMessage)
    {
        string addr = incomingMessage.address;

        if (addr == "/limitimer/timer/start")
        {
            if (isPlaying)
            {
                float _column;
                incomingMessage.TryGet(0, out _column);
                string column = _column.ToString();
                if (columnsTrigger) SendOscMessage(column);
                stopTimer();
            }
            else
            {
                if (counter < 1)
                {
                    stopTimer("reset");
                }
                else
                {
                    float _column;
                    incomingMessage.TryGet(0, out _column);
                    string column = _column.ToString();
                    if (columnsTrigger) SendOscMessage(column);
                    startTimer(time);
                }
            }
            isPlaying = !isPlaying;
        }

        if (addr == "/limitimer/timer/addminute") AddMinute();
        if (addr == "/limitimer/timer/subminute") RemoveMinute();

        if (addr == "/limitimer/timer/settime")
        {
            float newTime;
            incomingMessage.TryGet(0, out newTime);
            counter = (int)newTime;
        }
    }

    void SendOscMessage(string column)
    {
        outMessage = new OscMessage($"/composition/columns/{column}/connect");
        // Update the message.
        outMessage.Set(0, 1);

        // Send the message. It will be bundles with other messages and send in LateUpdate.
        oscOut.Send(outMessage);
    }
}


