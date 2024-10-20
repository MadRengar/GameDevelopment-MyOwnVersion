using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ButtonControl : MonoBehaviour
{
    private Animator animator;
    private NavMeshObstacle navMeshObstacle;
    private bool isPressed = false; // 用于判断踏板是否已被按下
    //private DoorControl doorControl;
    public DoorControl doorControl;//直接在Inspector窗口中拖拽绑定

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //GameObject goDoor = GameObject.Find("DoorPivot");//避免使用Find函数，会增大系统开销
    }

    // 玩家踩下踏板时触发
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Push the Button!");
            animator.SetBool("isPressed", true);
            doorControl.OpenDoor();
        }
    }
    // 玩家离开踏板时触发
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isPressed", false);
        }
    }
}
