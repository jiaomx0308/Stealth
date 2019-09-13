using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySight : MonoBehaviour
{
    public float fieldOfViewAngle = 110f;  //Fov角度，是一个全视角，我们在用的时候要使用半视角去判断； (一般人的视角大概就是120度左右) 
    public bool isPlayerInSight { get; private set; } //玩家是否被发现
    public Vector3 personalLastSighting { get; private set; } //记录玩家被本敌人发现（听见）的位置，如果时看见，直接更新LastPlayerSighting中的position
    public bool isPlayerFoundOut { get; private set; } //玩家被别人发现

    private Vector3 previousSighting; //记录上一帧是玩家被发现的位置，主要用于检测玩家是否被"外部"发现
    private NavMeshAgent navMeshAgent;
    private SphereCollider sphereCollider;
    private Animator animator;
    private GameObject player;
    private Animator playerAniamtor;
    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
        playerAniamtor = player.GetComponent<Animator>();
        sphereCollider = GetComponent<SphereCollider>();
        playerHealth = player.GetComponent<PlayerHealth>();

        previousSighting = LastPlayerSighting.Instance.resetPosition;
        personalLastSighting = LastPlayerSighting.Instance.resetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        previousSighting = LastPlayerSighting.Instance.position;
        if (previousSighting != LastPlayerSighting.Instance.resetPosition)
        {
            isPlayerFoundOut = true;
        }
        else
            isPlayerFoundOut = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == Tags.PLAYER)
        {
            isPlayerInSight = false;

            //判断玩家是否在敌人前方视野范围内
            float angle = Vector3.Angle(this.transform.forward, other.transform.position - this.transform.position); //注意Angle的返回值总是小于180度非负度  //如果要取得带符号的角度值，使用SignedAngle方法

            if (angle < fieldOfViewAngle / 2)
            {
                RaycastHit hitInfo;
                Physics.Linecast(this.transform.position, other.transform.position, out hitInfo); //如果玩家和敌人之间有障碍物遮挡，那么敌人是看不到玩家的
                if (hitInfo.transform.tag == Tags.PLAYER && !playerHealth.isPlayerDead)
                {
                    LastPlayerSighting.Instance.position = other.transform.position;
                        isPlayerInSight = true;
                }
            }

            //判断玩家的脚本声和呼叫声能否被敌人听见；
            if (playerAniamtor.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")  
                || playerAniamtor.GetCurrentAnimatorStateInfo(1).IsName("Shout"))  //如果玩家在移动或者喊叫，这里使用Animator进行状态的判断，也可以从玩家的脚本处获取玩家的状态，但是这个状态体现在Animator上
            {
                //计算敌人和玩家之间的最短路径：
                NavMeshPath path = new NavMeshPath();
                navMeshAgent.CalculatePath(other.transform.position, path);  //计算指定点的路径并存储生成的路径 //此功能可用于提前规划路径，以避免在需要路径时延迟游戏玩法。另一个用途是在移动代理之前检查目标位置是否可达

                float pathDistance = 0f;
                if (path.status == NavMeshPathStatus.PathComplete) //可以找到这条路径
                {
                    for (int i = 1; i < path.corners.Length; i++)  // path.corners至少包含起点和终点两个位置
                    {
                        pathDistance += Vector3.Distance(path.corners[i], path.corners[i - 1]);
                    }
                }

             //   print(pathDistance);
                if (pathDistance != 0 && pathDistance < sphereCollider.radius) //如果nav的距离小于我的collider的警戒距离，我也不关心
                {
                    personalLastSighting = other.transform.position;  //表示听见的位置，需要去确认下那是否有人啊
                 //   navMeshAgent.SetDestination(other.transform.position); //这个现在交给AI状态机去做
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == Tags.PLAYER)
        {
            isPlayerInSight = false;
            personalLastSighting = LastPlayerSighting.Instance.resetPosition;
        }
    }
}
