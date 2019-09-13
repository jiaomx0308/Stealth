using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;
    public float resetAfterDeathTime = 5f;
    public AudioClip deathClip;
    public bool isPlayerDead = false;

    private Animator animator;
    private float timer;
    private ScreenFadeInOut screenFadeIn;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        screenFadeIn = GameObject.FindGameObjectWithTag(Tags.FADER).GetComponent<ScreenFadeInOut>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0f)
        {
            if (isPlayerDead)
            {
                timer += Time.deltaTime;
                //场景变黑，关卡重置
                LastPlayerSighting.Instance.position = LastPlayerSighting.Instance.resetPosition;

                if (timer >= resetAfterDeathTime)
                {
                    screenFadeIn.EndScene();
                }
            }
            else
            {
                //垂死状态
                animator.SetBool("Dead", true);
                isPlayerDead = true;
                AudioSource.PlayClipAtPoint(deathClip, this.transform.position);
            }
        }
    }

    public void TakeDamage(float amout)
    {
        health -= amout;
    }
}
