using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : StateMachine<EnemyAI>
{
    public float patrolWaitTime;
    public float patrolSpeed;
    public float chaseWaitTime;
    public float chaseSpeed;
    public Transform[] wayPoints;

    private float patrolWaitTimer;
    private float chaseWaitTimer;
    private int wayPointIndex;
    private EnemySight enemySight;
    private NavMeshAgent navMeshAgent;

    class PatrolState : State<EnemyAI>
    {
        public override void Enter(EnemyAI e)
        {
            e.navMeshAgent.speed = e.patrolSpeed;
        }

        public override void Update(EnemyAI e)
        {
            //如果看见玩家进入射击状态
            if (e.enemySight.isPlayerInSight)
            {
                e.ChangeState(new ShotState());
                return;
            }


            // 如果听见玩家,或玩家被其他监事器发现，进入追踪状态
            if (e.enemySight.personalLastSighting != LastPlayerSighting.Instance.resetPosition || e.enemySight.isPlayerFoundOut)
            {
                e.ChangeState(new ChaseState());
                return;
            }


            //自己状态更新，路点模式
            if (e.navMeshAgent.remainingDistance < e.navMeshAgent.stoppingDistance)
            {
                e.patrolWaitTimer += Time.deltaTime;

                if (e.patrolWaitTimer >= e.patrolWaitTime)
                {
                    e.wayPointIndex = (e.wayPointIndex + 1) % e.wayPoints.Length;
                    e.patrolWaitTimer = 0;
                    e.navMeshAgent.destination = e.wayPoints[e.wayPointIndex].position;
                }
            }
        }

        public override void Exit(EnemyAI e)
        {
            e.patrolWaitTimer = 0;
        }
    }

    class ChaseState : State<EnemyAI>
    {
        public override void Enter(EnemyAI e)
        {
            e.navMeshAgent.speed = e.chaseSpeed;
            e.navMeshAgent.SetDestination(LastPlayerSighting.Instance.position);
        }

        public override void Update(EnemyAI e)
        {
            if (e.enemySight.isPlayerInSight)
            {
                e.ChangeState(new ShotState());
                return;
            }

            if (e.navMeshAgent.remainingDistance < e.navMeshAgent.stoppingDistance)
            {
                e.chaseWaitTimer += Time.deltaTime;

                if (e.chaseWaitTimer >= e.chaseWaitTime)
                {
                    e.ChangeState(new PatrolState());
                }
            }
        }

        public override void Exit(EnemyAI e)
        {
            e.chaseWaitTimer = 0;
            if (!e.enemySight.isPlayerInSight && e.enemySight.personalLastSighting == LastPlayerSighting.Instance.resetPosition)
            {
                LastPlayerSighting.Instance.position = LastPlayerSighting.Instance.resetPosition;
            }
        }
    }

    class ShotState : State<EnemyAI>
    {
        public override void Enter(EnemyAI e)
        {
            e.navMeshAgent.destination = e.transform.position;
        }

        public override void Update(EnemyAI e)
        {
            if (!e.enemySight.isPlayerInSight)
            {
                if (e.enemySight.personalLastSighting != LastPlayerSighting.Instance.resetPosition)
                    e.ChangeState(new ChaseState());
                else
                    e.ChangeState(new PatrolState());
            }
        }

        public override void Exit(EnemyAI e)
        {
 
        }
    }

    private void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        enemySight = this.GetComponent<EnemySight>();
        base.Init(this, new PatrolState());
    }
}
