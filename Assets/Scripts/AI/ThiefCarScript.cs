using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefCarScript : MonoBehaviour {

    Vector3 currentPos;
    float middlePos;
    float startPos;
    float endPos;
    bool isMoving;

    public ParticleSystem tireSmoke;

	// Use this for initialization
	void Start () {
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
	void Update () {
		if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(LerpCar(middlePos));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(LerpCar(endPos));
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(LerpCar(startPos));
        }
    }

    IEnumerator LerpCar(float targetX)
    {
        if (!isMoving)
        {
            isMoving = true;
            float t = 0;
            currentPos = transform.position;
            Vector3 destination = new Vector3(targetX, currentPos.y, currentPos.z);
            tireSmoke.Play();
            while (transform.position.x != destination.x)
            {
                transform.position = Vector3.Lerp(currentPos, destination, Mathf.SmoothStep(0.0f, 1.0f, t));
                t += Time.deltaTime * 0.5f;
                yield return null;
            }
            tireSmoke.Stop();
            isMoving = false;
        }
    }
}
