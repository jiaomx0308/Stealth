using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private EnemySight enemySight;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private float smoothTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        enemySight = this.GetComponent<EnemySight>();
        animator = this.GetComponent<Animator>();
        navMeshAgent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 0f;
        float turn = 0f;

        if (enemySight.isPlayerInSight)  //玩家被看到
        {
            Vector3 targetPositon = LastPlayerSighting.Instance.position - this.transform.position;

            turn = Vector3.SignedAngle(this.transform.forward, targetPositon, this.transform.up);
        }
        else
        {
            //求速度，desiredVelocity和我们前方向上的投影，desiredVelocity是目标速度（既包含方向，也有角度）
            speed = Vector3.Project(navMeshAgent.desiredVelocity, transform.forward).magnitude;

            //求角度，desiredVelocity和forward的夹角
            turn = Vector3.SignedAngle(transform.forward, navMeshAgent.desiredVelocity, transform.up);
        }
        animator.SetFloat("Speed", speed, smoothTime, Time.deltaTime);
        animator.SetFloat("AngularSpeed", turn * Mathf.Deg2Rad);
        animator.SetBool("PlayerInSight", enemySight.isPlayerInSight);
    }

    private void OnAnimatorMove()
    {
        navMeshAgent.velocity = animator.deltaPosition / Time.deltaTime;
        this.transform.rotation = animator.rootRotation;
    }
}
