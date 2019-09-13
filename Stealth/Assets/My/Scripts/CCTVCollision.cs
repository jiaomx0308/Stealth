using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVCollision : MonoBehaviour
{

    private LastPlayerSighting lastPlayerSighting;

    private void Start()
    {
        lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.GAMECONTROLLER).GetComponent<LastPlayerSighting>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == Tags.PLAYER)
        {
            lastPlayerSighting.position = other.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == Tags.PLAYER)
        {
            lastPlayerSighting.position = lastPlayerSighting.resetPosition;
        }
    }
}
