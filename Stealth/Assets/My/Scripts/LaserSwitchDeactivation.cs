using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSwitchDeactivation : MonoBehaviour
{
    public Material unlockMat;
    public GameObject laser;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == Tags.PLAYER && Input.GetButtonDown("Switch"))
        {
            laser.SetActive(false);  //关闭激光栅栏
            transform.Find("prop_switchUnit_screen").GetComponent<Renderer>().material = unlockMat;
            this.GetComponent<AudioSource>().Play();
        }
    }
}
