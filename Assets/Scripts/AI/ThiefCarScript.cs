using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefCarScript : MonoBehaviour {

    Vector3 currentPos;
    public float middlePos;
    public float startPos;
    public float endPos;
    bool isMoving;

    public ParticleSystem tireSmoke;

    AudioSource audioScript;
    public AudioClip honk;
    public AudioClip startEngine;

    // Use this for initialization
    void Start()
    {
        audioScript = GetComponent<AudioSource>();
        currentPos = transform.position;
        middlePos = currentPos.x;
        startPos = middlePos - 30;
        endPos = middlePos + 13;
        transform.position = new Vector3(startPos, currentPos.y, currentPos.z);

        if (tireSmoke == null)
        {
            transform.GetChild(0).GetComponent<ParticleSystem>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(LerpCar(middlePos));
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(LerpCar(endPos));
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            transform.position = new Vector3(startPos, currentPos.y, currentPos.z);
        }
    }

    public IEnumerator LerpCar(float targetX)
    {
        if (!isMoving)
        {
            isMoving = true;
            float t = 0;
            currentPos = transform.position;
            Vector3 destination = new Vector3(targetX, currentPos.y, currentPos.z);
            tireSmoke.Play();
            if (transform.position.x == middlePos)
            {
                audioScript.PlayOneShot(startEngine);
            }

            while (transform.position.x != destination.x)
            {
                transform.position = Vector3.Lerp(currentPos, destination, Mathf.SmoothStep(0.0f, 1.0f, t));
                t += Time.deltaTime * 0.5f;
                yield return null;
            }
            tireSmoke.Stop();
            isMoving = false;

            if (transform.position.x == endPos)
            {
                transform.position = new Vector3(startPos, currentPos.y, currentPos.z);
            }

            else if (transform.position.x == middlePos)
            {
                audioScript.PlayOneShot(honk);
            }
        }
    }
}
