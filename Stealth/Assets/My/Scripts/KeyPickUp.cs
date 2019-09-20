using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPickUp : MonoBehaviour
{

    public AudioClip audioClip;
    private RawImage keyCard;
    private void Start()
    {
        keyCard = this.transform.root.Find("EasyTouchControlsCanvas/RawImage").GetComponent<RawImage>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == Tags.PLAYER)
        {
            other.GetComponent<PlayerInvertory>().HasKey = true;
            if (keyCard)
                keyCard.enabled = true;
            AudioSource.PlayClipAtPoint(audioClip, this.transform.position);
            Destroy(this.gameObject);
        }
    }
}
