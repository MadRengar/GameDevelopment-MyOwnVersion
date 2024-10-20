using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;



public class PlayerController : MonoBehaviour
{
    private NavMeshAgent playerAgent; // 玩家角色的 NavMesh Agent
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        playerAgent = GetComponent<NavMeshAgent>(); //获得NavMeshAgent组件
        animator = GetComponent<Animator>();
        animator.enabled = false; // 禁用 Animator 组件 因为会卡在Entry到normal的状态，导致NavMesh Agent组件的Player不能移动！
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))// 检测鼠标左键点击
        {
            Camera currentCamera = CamerasControl.Instance.GetCurrentCamera();// 获取当前的摄像机实例
            Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; // 声明一个 RaycastHit 变量，准备存储射线结果
            bool isCollide = Physics.Raycast(ray, out hit);
            if (isCollide)
            {
                if(hit.collider.tag == Tag.GROUND)
                {
                    playerAgent.SetDestination(hit.point);//调用SetDestination方法，设置玩家移动目的地
                }else if(hit.collider.tag == Tag.INTERACTABLE)
                {
                    hit.collider.GetComponent<InteractableObject>().OnClick(playerAgent);
                }                
            }
            // 检测是否进入 NavMesh Link 区域
            if (playerAgent.isOnOffMeshLink)
            {
                Debug.Log("到达跳跃区域");
                animator.enabled = true; // 启用 Animator 组件
                // 开始跳跃动画
                StartCoroutine(JumpToTarget(playerAgent.currentOffMeshLinkData.endPos));
            }
        }
    }
    // 协程处理跳跃动作
    IEnumerator JumpToTarget(Vector3 targetPosition)
    {
        
        Debug.Log("播放动画！");
        /*更新角色的朝向*/
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;

        
        playerAgent.enabled = false;//禁止NavMesh组件，防止出现和动画争抢的情况
        animator.Play("JumpAnimation", 0, 0); // 不在录制动画的起点播放动画 防止因为播放动画出现瞬移的现象
        animator.SetTrigger("Jump"); // 触发跳跃动画
        yield return new WaitForSeconds(1.8f); // 跳跃动画时间为1.8秒
        playerAgent.enabled = true;//恢复NavMesh组件

        playerAgent.CompleteOffMeshLink();
    }
}
