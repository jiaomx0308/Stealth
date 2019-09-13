using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float smoothTime = 1.5f;
    private Transform player;

    private Vector3 relCameraPos;  //从player指向camera的向量
    private float relCameraPosMag;  //我们尽量保持的Camera和玩家之间的距离
    private float stepCoefficient = 0.2f; //步长系数
    private Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER).transform;
        relCameraPos = this.transform.position - player.position;
        relCameraPosMag = relCameraPos.magnitude;
    }

    private void LateUpdate()
    {
        Vector3 positionCamera = player.transform.position + relCameraPos;  //Camera理应在的位置

        //当我们移动角色时，角色和Camera之间可能被墙或者其他障碍物遮挡，此时我们需要向上调整Camer的视角，使其能观察到玩家
        Vector3 playerUp = player.position + player.up * relCameraPosMag;  //玩家上方90度方向距离relCameraPosMag的位置，//这个是Camer的极限位置，在玩家头顶正上面，此时如果还有物体遮挡，那么保持这个玩家和Camera之间的距离就找不到一个合适的角度进行观察

        RaycastHit hitInfo;
        for (float i = 0; i <= 1; i += stepCoefficient)
        {
            targetPos = Vector3.Lerp(positionCamera, playerUp, i);
            Physics.Raycast(targetPos, player.position - targetPos, out hitInfo);
            if (hitInfo.transform.tag == Tags.PLAYER)
                break;
        }


        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, smoothTime * Time.deltaTime);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(player.position - targetPos), smoothTime * Time.deltaTime);
    }
}
