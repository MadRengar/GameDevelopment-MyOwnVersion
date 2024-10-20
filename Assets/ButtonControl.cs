using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ButtonControl : MonoBehaviour
{
    private Animator animator;
    private NavMeshObstacle navMeshObstacle;
    private bool isPressed = false; // �����ж�̤���Ƿ��ѱ�����
    //private DoorControl doorControl;
    public DoorControl doorControl;//ֱ����Inspector��������ק��

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //GameObject goDoor = GameObject.Find("DoorPivot");//����ʹ��Find������������ϵͳ����
    }

    // ��Ҳ���̤��ʱ����
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Push the Button!");
            animator.SetBool("isPressed", true);
            doorControl.OpenDoor();
        }
    }
    // ����뿪̤��ʱ����
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isPressed", false);
        }
    }
}
