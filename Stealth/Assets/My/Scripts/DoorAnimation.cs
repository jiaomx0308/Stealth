using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorAnimation : MonoBehaviour
{
    public bool requireKey;   //开门是否需要钥匙
    public AudioClip doorSwitchClip;
    public AudioClip accessDeniedClip;  //不能开门的音频
    public AudioClip winGameClip;

    private Animator animator;
    private AudioSource audioSource;
    private int cout;

    private GameObject tipsText;


    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
        tipsText = this.transform.root.Find("EasyTouchControlsCanvas/tipsText").gameObject;
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
                    tipsText.GetComponent<Text>().text = "You must find a key to open the door!";
                    tipsText.SetActive(true);
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

