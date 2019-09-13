using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public AudioClip fireClip;

    private Animator animator;
    private bool isShoting = false;
    private Transform player;
    private float harm = 50f;
    private Transform gun;
    private LineRenderer gunLaser;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER).transform;
        gun = this.transform.Find("char_robotGuard_skeleton/char_robotGuard_Hips/char_robotGuard_Spine/char_robotGuard_RightShoulder/char_robotGuard_RightArm/char_robotGuard_RightForeArm/char_robotGuard_RightHand/prop_sciFiGun_low");
        gunLaser = gun.Find("fx_laserShot").GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float shot = animator.GetFloat("Shot");
        if (shot > 0.5 && !isShoting)
        {
            isShoting = true;

            //伤害计算
            float r = this.GetComponent<SphereCollider>().radius;
            float d = (player.position - this.transform.position).magnitude;
            float factor = 1 - d / r;
            player.GetComponent<PlayerHealth>().TakeDamage(harm * factor);

            //实现射击效果
            gunLaser.enabled = true;
            gunLaser.SetPosition(0, gun.transform.position);
            gunLaser.SetPosition(1, player.position + Vector3.up * 1.5f); //玩家的position是在脚底的位置，所以这里加了一个1.5f的高度

            gun.GetComponent<Light>().intensity = 1f;
            AudioSource.PlayClipAtPoint(fireClip, this.transform.position);
        }
        else
        {
            isShoting = false;
            gunLaser.enabled = false;

            gun.GetComponent<Light>().intensity = Mathf.Lerp(gun.GetComponent<Light>().intensity, 0f, Time.deltaTime);
        }
    }

    void OnShotting()
    {
        Debug.Log("Shotting!");
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (isShoting)
        {
            animator.SetIKPosition(AvatarIKGoal.RightHand, player.transform.position + 1.5f * Vector3.up);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        }
    }
}
