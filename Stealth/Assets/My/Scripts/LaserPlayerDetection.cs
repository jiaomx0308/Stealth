using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPlayerDetection : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == Tags.PLAYER && this.GetComponent<Renderer>().enabled && !other.GetComponent<PlayerHealth>().isPlayerDead)
        {
            LastPlayerSighting.Instance.position = other.transform.position;
           
        }
    }
}
