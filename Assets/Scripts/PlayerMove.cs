using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;



public class PlayerController : MonoBehaviour
{
    private NavMeshAgent playerAgent; // ��ҽ�ɫ�� NavMesh Agent
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        playerAgent = GetComponent<NavMeshAgent>(); //���NavMeshAgent���
        animator = GetComponent<Animator>();
        animator.enabled = false; // ���� Animator ��� ��Ϊ�Ῠ��Entry��normal��״̬������NavMesh Agent�����Player�����ƶ���
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))// ������������
        {
            Camera currentCamera = CamerasControl.Instance.GetCurrentCamera();// ��ȡ��ǰ�������ʵ��
            Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; // ����һ�� RaycastHit ������׼���洢���߽��
            bool isCollide = Physics.Raycast(ray, out hit);
            if (isCollide)
            {
                if(hit.collider.tag == Tag.GROUND)
                {
                    playerAgent.SetDestination(hit.point);//����SetDestination��������������ƶ�Ŀ�ĵ�
                }else if(hit.collider.tag == Tag.INTERACTABLE)
                {
                    hit.collider.GetComponent<InteractableObject>().OnClick(playerAgent);
                }                
            }
            // ����Ƿ���� NavMesh Link ����
            if (playerAgent.isOnOffMeshLink)
            {
                Debug.Log("������Ծ����");
                animator.enabled = true; // ���� Animator ���
                // ��ʼ��Ծ����
                StartCoroutine(JumpToTarget(playerAgent.currentOffMeshLinkData.endPos));
            }
        }
    }
    // Э�̴�����Ծ����
    IEnumerator JumpToTarget(Vector3 targetPosition)
    {
        
        Debug.Log("���Ŷ�����");
        /*���½�ɫ�ĳ���*/
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;

        
        playerAgent.enabled = false;//��ֹNavMesh�������ֹ���ֺͶ������������
        animator.Play("JumpAnimation", 0, 0); // ����¼�ƶ�������㲥�Ŷ��� ��ֹ��Ϊ���Ŷ�������˲�Ƶ�����
        animator.SetTrigger("Jump"); // ������Ծ����
        yield return new WaitForSeconds(1.8f); // ��Ծ����ʱ��Ϊ1.8��
        playerAgent.enabled = true;//�ָ�NavMesh���

        playerAgent.CompleteOffMeshLink();
    }
}
