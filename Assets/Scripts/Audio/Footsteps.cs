using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour {

    #region //GameManager
    AudioSource audioScript;
    public AudioClip[] footSteps;
    public AudioClip jumpSound;
    public AudioClip snatch;
    public AudioClip oof;
    #endregion

    private void Awake()
    {
        audioScript = GetComponent<AudioSource>();
    }

    public void Death()
    {
        AudioClip clip = oof;
        audioScript.PlayOneShot(clip);
    }

    public void Steps()
    {
        AudioClip clip = GetRandomClip();
        audioScript.PlayOneShot(clip);
    }

    public void Jump()
    {
        AudioClip clip = jumpSound;
        audioScript.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip()
    {
        return footSteps[UnityEngine.Random.Range(0, footSteps.Length)];
    }

    public void Snatch()
    {
        AudioClip clip = snatch;
        audioScript.PlayOneShot(clip);
    }
}
