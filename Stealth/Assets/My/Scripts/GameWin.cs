using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWin : MonoBehaviour
{
    private bool isGameWin = false;
    private ScreenFadeInOut screenFadeInOut;

    private void Start()
    {
        screenFadeInOut = GameObject.FindGameObjectWithTag(Tags.FADER).GetComponent<ScreenFadeInOut>();
    }

    private void Update()
    {
        if (isGameWin)
        {
            screenFadeInOut.EndScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.PLAYER)
        {
            this.GetComponent<AudioSource>().Play();
            this.isGameWin = true;
        }
    }
}
