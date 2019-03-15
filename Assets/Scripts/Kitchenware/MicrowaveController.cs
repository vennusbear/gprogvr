using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Controllables.ArtificialBased;
using TMPro;

public class MicrowaveController : MonoBehaviour
{
    public int microwaveTimer;
    private int maxTimer = 180;

    public bool isCooking;
    public bool buttonCD;

    private TextMeshPro display;
    private int minutes;
    private int seconds;

    public Transform turnTable;
    public GameObject door;

    public VRTK_ArtificialPusher minusButton;
    private float mbPos;

    public VRTK_ArtificialPusher plusButton;
    private float pbPos;

    public VRTK_ArtificialPusher startButton;
    private float sbPos;

    Coroutine displayRoutine;
    Coroutine turnRoutine;

    public Transform cookingArea;

    #region //Audio 
    AudioSource audioScript;
    public AudioClip[] microwaveAudio;
    #endregion

    // Use this for initialization
    void Start ()
    {
        audioScript = GetComponent<AudioSource>();
        display = transform.GetChild(0).GetComponent<TextMeshPro>();
        StartCoroutine(Delay(3));

        display.text = "Welcome";
    }

    IEnumerator GetMBPos() //Check if Minus Button is Pressed
    {
        while (true) //Runs every frame
        {
            if (!isCooking) //if not cooking, we can change the value
            {
                mbPos = minusButton.GetNormalizedValue();
                if (mbPos >= 1 && !buttonCD && microwaveTimer >= 10) //NormalizedValue = 1 means it is pressed, -10 seconds for every press
                {
                    StartCoroutine(ButtonCooldownChecker(minusButton));
                    microwaveTimer -= 10;
                    UpdateTimer();
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator GetPBPos()
    {
        while (true)
        {
            if (!isCooking)
            {
                pbPos = plusButton.GetNormalizedValue();
                if (pbPos >= 1 && !buttonCD && microwaveTimer < maxTimer)
                {
                    StartCoroutine(ButtonCooldownChecker(plusButton));
                    microwaveTimer += 10;
                    UpdateTimer();
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator GetSBPos()
    {
        while (true)
        {
            sbPos = startButton.GetNormalizedValue();
            if (sbPos >= 1 && !buttonCD)
            {
                StartCoroutine(ButtonCooldownChecker(startButton));
                if (microwaveTimer == 0)
                {
                    display.color = Color.red;
                    display.text = "Invalid Time";
                    StartCoroutine(DisplayBlink(2, display.text));
                    yield return new WaitForSeconds(2f);
                    UpdateTimer();
                }

                else if (door.GetComponent<VRTK_ArtificialRotator>().GetNormalizedValue() > 0) 
                {
                    display.color = Color.red;
                    display.text = "Close Door";
                    StartCoroutine(DisplayBlink(2, display.text));
                    yield return new WaitForSeconds(2f);
                    UpdateTimer();
                }

                else if (isCooking)
                {
                    isCooking = false;
                    microwaveTimer = 0;
                    StopCoroutine(displayRoutine);
                    StopCoroutine(turnRoutine);
                    display.color = Color.red;
                    display.text = ("Stopped");
                    StartCoroutine(DisplayBlink(3, display.text));
                    door.GetComponent<VRTK_ArtificialRotator>().isLocked = false;
                    yield return new WaitForSeconds(3f);
                    UpdateTimer();
                }

                else if (!isCooking && microwaveTimer > 0)
                {
                    isCooking = true;
                    CheckInside();
                    door.GetComponent<VRTK_ArtificialRotator>().isLocked = true;
                    displayRoutine = StartCoroutine(CookingTimer(microwaveTimer));
                    turnRoutine = StartCoroutine(Rotate(microwaveTimer));
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ButtonCooldownChecker(VRTK_ArtificialPusher button) //Cooldown for button, wait until its halfway back to unpush to allow it to be pushed again
    {
        buttonCD = true;
        while (button.GetNormalizedValue() > 0.5f)
        {
            yield return new WaitForEndOfFrame();
        }
        buttonCD = false;
        yield return null;
    }

    IEnumerator Delay(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        StartCoroutine(GetMBPos());
        StartCoroutine(GetPBPos());
        StartCoroutine(GetSBPos());
    } //To overcome the weird bug that causes the buttons to be pushed at the start of the scene 

    IEnumerator CookingTimer(int cookTime)
    {

        display.color = new Color(1, 0.5f, 0, 1);
        while (microwaveTimer > 0)
        {
            yield return new WaitForSeconds(1);
            microwaveTimer -= 1;
            UpdateTimer();
        }
        StartCoroutine(PlayRepeatedAudio(2, 5, 0.5f));
        display.color = Color.green;
        display.text = "Completed";
        PlayAudio(2);
        StartCoroutine(DisplayBlink(3, display.text));
        door.GetComponent<VRTK_ArtificialRotator>().isLocked = false;
        isCooking = false;
    } //Make the Display show remaining time left when cooking

    private string ConvertToTime(int totalSeconds)
    {
        string seconds = (totalSeconds % 60).ToString("00"); 
        string minutes = Mathf.Floor(totalSeconds / 60).ToString("0");
        return minutes + ":" + seconds;
    } //Convert seconds to Minute:Seconds format

    IEnumerator DisplayBlink(float duration, string desiredText)
    {
        //float t = 0;
        while (/*t < duration && */display.text == desiredText)
        {
            display.GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(0.2f);
            display.GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(0.2f);
            //t += Time.deltaTime + 0.2f + 0.2f;
        }

        display.GetComponent<Renderer>().enabled = true;
    } //Blink function when display is showing important info

    IEnumerator Rotate(int duration)
    {
        float startRotation = turnTable.transform.localEulerAngles.y;
        float endRotation = startRotation + 360.0f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            turnTable.transform.localEulerAngles = new Vector3(turnTable.transform.localEulerAngles.x, yRotation, turnTable.transform.localEulerAngles.z);
            yield return null;
        }
    } //Rotate the glass plate in the microwave

    void UpdateTimer()
    {
        if (display.color != Color.green && !isCooking)
        {
            display.color = Color.green;
        }

        display.text = ConvertToTime(microwaveTimer);
    } //Update Timer with new microwaveTimer

    void CheckInside() //Check what Items are inside the microwave & cook it
    {
        Collider[] hitColliders = Physics.OverlapBox(cookingArea.transform.position, cookingArea.transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Items"));
        foreach (Collider item in hitColliders)
        {
            if (item.transform.GetComponent<Food>())
            {
                item.transform.parent = turnTable;
                StartCoroutine(item.transform.GetComponent<Food>().CookMicrowave(transform));
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(cookingArea.position, cookingArea.localScale / 2);
    }

    public void PlayAudio(int i)
    {
        audioScript.clip = microwaveAudio[i];
        audioScript.Play();
    }

    IEnumerator PlayRepeatedAudio(int i, int repeat, float interval)
    {
        audioScript.clip = microwaveAudio[i];
        for (int a = 0; a < repeat; a++)
        {
            audioScript.Play();
            yield return new WaitForSeconds(interval);
        }
    }
}
