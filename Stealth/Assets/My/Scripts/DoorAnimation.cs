using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    public bool requireKey;   //开门是否需要钥匙
    public AudioClip doorSwitchClip;
    public AudioClip accessDeniedClip;  //不能开门的音频
    public AudioClip winGameClip;

    private Animator animator;
    private AudioSource audioSource;
    private int cout;


    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == Tags.PLAYER)
        {
            if (requireKey)
            {
                if (!other.GetComponent<PlayerInvertory>().HasKey)
                {
                    audioSource.clip = accessDeniedClip;
                    audioSource.Play();
                    return;
                }
          
                audioSource.clip = doorSwitchClip;
                audioSource.Play();
                cout++;
            }
            else
            {
                audioSource.clip = doorSwitchClip;
                audioSource.Play();
                cout++;
            }
        }
        else if (other.transform.tag == Tags.ENEMY)
        {
            cout++;
        }

        animator.SetBool("Open", cout > 0);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == Tags.PLAYER || other.gameObject.tag == Tags.ENEMY)
        {
            cout = Mathf.Max(0, cout - 1);
        }

        animator.SetBool("Open", cout > 0);
    }
}

