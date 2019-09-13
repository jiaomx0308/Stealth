using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public AudioClip shoutAudio;

    public float speedDampTime = 0.5f;  //进行阻尼过渡的总时间
    public float rotationDampTime = 15f;

    private Rigidbody rigidbody;
    private Animator animator;
    private AudioSource audioSource;

    private int sneakingBool;


    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        rigidbody = this.GetComponent<Rigidbody>();
        audioSource = this.GetComponent<AudioSource>();
        animator.SetLayerWeight(1, 1f);

        sneakingBool = Animator.StringToHash("Sneaking");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool sneak = Input.GetButton("Sneak");   //如果要判断按键是否按下，GetButton就可，要按按下的值要用GetAxis
        bool shout = Input.GetButtonDown("Attract");

        animator.SetBool("Shouting", shout);

        if (shout)
        {
            AudioSource.PlayClipAtPoint(shoutAudio, this.transform.position);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion"))
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        //潜行
        //animator.SetBool("Sneaking", sneak);
        animator.SetBool(sneakingBool, sneak);

        //移动
        if (h != 0 || v != 0)
        {
            animator.SetFloat("Speed", 5.6f, speedDampTime, Time.deltaTime);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

        //转向
        if (h != 0 || v != 0)
        {
            Vector3 targetDir = new Vector3(h , 0, v);
            Quaternion targetQuaternion = Quaternion.LookRotation(targetDir);

            Quaternion currentQuaternion = Quaternion.Lerp(rigidbody.rotation, targetQuaternion, rotationDampTime * Time.fixedDeltaTime);

            rigidbody.MoveRotation(currentQuaternion);

        }
    }
}
