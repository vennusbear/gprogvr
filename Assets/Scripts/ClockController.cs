using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ClockController : MonoBehaviour {

    private int seconds;
    public Transform minuteHand;
    public Transform hourHand;
    public Transform dongObj;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(HourMoving("default"));
    }
	
	// Update is called once per frame
	void Update ()
    {
        dongObj.localEulerAngles = new Vector3(0, 0, Mathf.PingPong(Time.time * 10, 10)); //TikTok!
    }

    void Initalize()
    {

    }

    public IEnumerator HourMoving(string TimeOfDay)
    {
        int start = 0;
        int end = 0;
        int duration = 0;
        switch (TimeOfDay)
        {
            case "Morning":
                start = 8;
                end = 11;
                duration = 300;
                break;
            case "Afternoon":
                start = 12;
                end = 3;
                duration = 600;
                break;
            default:
                start = 8;
                end = 9;
                duration = 60;
                break;
        }
            
        Coroutine minuteRoutine = StartCoroutine(MinuteTick(Mathf.Abs(start - end) * 60));
        float startDegree = (start / 12f) * 360f;
        float endDegree = (end / 12f) * 360f;
        float t = 0;
        while (true)
        {
            float angle = Mathf.Lerp(startDegree, endDegree, t);
            hourHand.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            t += Time.deltaTime / duration; // 1 minute per Hour
            yield return null;
        }
    }

    IEnumerator MinuteTick(int target)
    {
        seconds = 0;
        while (seconds < target)
        {
            float angle = (seconds / 60f) * 360f;
            minuteHand.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            seconds += 1;
            if (seconds == 60)
            {
                seconds = 0;
            }
            yield return new WaitForSeconds(1);
        }
    }
}
