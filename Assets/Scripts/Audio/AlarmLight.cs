using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLight : MonoBehaviour {

    public float fadeSpeed = 2f;
    public float highIntensity = 2f;
    public float lowIntensity = 0.5f;
    public float changeMargin = 0.2f;
    public bool alarmOn;

    public Light light;

    #region //Audio 
    AudioSource audioScript;
    #endregion

    private float targetIntensity;

    private void Awake()
    {
        audioScript = GetComponent<AudioSource>();
        light = GetComponent<Light>();
        light.intensity = 0f;
        targetIntensity = highIntensity;
    }
	
	// Update is called once per frame
	void Update () {
		if (alarmOn)
        {
            light.intensity = Mathf.Lerp(light.intensity, targetIntensity, fadeSpeed * Time.deltaTime);
            CheckIntensity();

            if (!audioScript.isPlaying)
            {
                audioScript.Play();
            }
        }

        else
        {
            light.intensity = Mathf.Lerp(light.intensity, 0f, fadeSpeed * Time.deltaTime);

            if (audioScript.isPlaying)
            {
                audioScript.Stop();
            }
        }
	}

    void CheckIntensity()
    {
        if(Mathf.Abs(targetIntensity - light.intensity) < changeMargin)
        {
            if (targetIntensity == highIntensity)
            {
                targetIntensity = lowIntensity;
            }

            else
            {
                targetIntensity = highIntensity;
            }
        }
    }
}
