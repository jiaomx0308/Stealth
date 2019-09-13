using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{

    public AudioClip audioClip;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == Tags.PLAYER)
        {
            other.GetComponent<PlayerInvertory>().HasKey = true;
            AudioSource.PlayClipAtPoint(audioClip, this.transform.position);
            Destroy(this.gameObject);
        }
    }
}
