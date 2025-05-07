using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))] // attribute
// NavMeshAgent �� ������ ��� ������ 

[System.Serializable] // Serializable : �ν����Ϳ��� ���̰� ���ִ� ��Ʈ����Ʈ(�Ӽ�)
public class PlayerAni // �ʿ��� �ִϸ��̼� Ŭ���� ��Ƶδ� Ŭ����
{
    public AnimationClip idle;
    public AnimationClip walk;
    public AnimationClip run;
    public AnimationClip attack;
    public AnimationClip skill;
    public AnimationClip Jump;
}
public class Player : MonoBehaviour
{
    public PlayerAni playerAni = new PlayerAni();//�ִϸ��̼� Ŭ���� ��� �ִ� Ŭ���� 
    [SerializeField]//private��� ������ �ν����Ϳ��� ���̰� ����
    private Animation playerAnimation; // �ִϸ��̼� ������Ʈ
    Ray ray; //���� // �տ� �ƹ��͵� �Ⱦ��� �ڵ����� private
    RaycastHit hit; // ������ ���� ��ü�� ��ġ ���� �Ÿ��� �˼� ���� 
    Vector3 target = Vector3.zero;
    NavMeshAgent agent; // �׺�޽� ������Ʈ
    Transform tr;
    [Header("���콺 ����Ŭ������ ����")]
    public float M_doubleClickSecond = 0.25f; // ����Ŭ�� �ð� 
    public bool IsOnClick = false; // ��Ŭ�� ����Ŭ�� ���� 
    public double m_Timer = 0; // ����Ŭ�� Ÿ�̸� 
    
    void Start()
    {
        tr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>(); 
        playerAnimation = GetComponent<Animation>();

    }

   
    void Update()
    {
        if(IsOnClick&&((Time.time - m_Timer) > M_doubleClickSecond)) // ����Ŭ���� �ƴϰ� ����Ŭ�� Ÿ�̸Ӱ� ����Ŭ�� �ð����� ũ��
        {
            Debug.Log("OneClick!.");// ����Ŭ���̾ƴ�
            IsOnClick = false; // ����Ŭ�� ���θ� false�� ����
            
        }
        if(Input.GetMouseButtonDown(1)) // ���콺 ������ ��ư�� Ŭ���ϸ�
        {
            if(IsOnClick == false) // ����Ŭ���̸�
            {
                m_Timer = Time.time; // ����Ŭ�� Ÿ�̸Ӹ� ���� �ð����� ����
                IsOnClick = true; // ����Ŭ�� ���θ� true�� ����

            }
            else if(IsOnClick = true&& (Time.time - m_Timer)< M_doubleClickSecond)
            {
                Debug.Log("DoubleClick!"); // ����Ŭ��
                IsOnClick = false; // ����Ŭ�� ���θ� false�� ���� 
            }
        }
        MouseMovement();
        Jump();
        Attack();
        Run();
        SkillQ();

    }

    private void SkillQ()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            playerAnimation.CrossFade(playerAni.skill.name, 0.2f); // ���̵� �ִϸ��̼� ���
            agent.speed = 0f; //�ӵ�����
            agent.velocity = Vector3.zero; // �ӵ� �ʱ�ȭ
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            playerAnimation.CrossFade(playerAni.idle.name, 0.2f); // ���̵� �ִϸ��̼� ���
        }
    }

    private void Attack()
    {
        if (Input.GetMouseButton(0))
        {
            playerAnimation.CrossFade(playerAni.attack.name, 0.2f); // ���� �ִϸ��̼� ���
            agent.speed = 0f; //�ӵ�����
            agent.velocity = Vector3.zero; // �ӵ� �ʱ�ȭ
        }
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift)) // ����Ʈ ������
        {
            playerAnimation.CrossFade(playerAni.run.name, 0.2f); // �� �ִϸ��̼�
            agent.speed = 10f; //�ӵ�����    
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) // ����Ʈ ����
        {
            playerAnimation.CrossFade(playerAni.idle.name, 0.2f); // �� �ִϸ��̼� ���
            agent.speed = 0f; //�ӵ�����
            agent.velocity = Vector3.zero; // �ӵ� �ʱ�ȭ
        }
    }

    private void MouseMovement()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);// ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
                                                                //ī�޶󿡼� ���콺 ��ġ�� ������ ���.
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow); // ���� �ð�ȭ 
        //�� ȭ�鿡���� ���δ�(������ġ,���̹���,����,����)

        if (Input.GetMouseButtonDown(1))
        {
            //����,������ġ,�Ÿ���100,���̾��ũ(8�� ���̾ üũ)
            if (Physics.Raycast(ray, out hit, 100, 1 << 8)) // ������ ��ü�� ������
            {
                if(IsOnClick==true)//����Ŭ���̸�
                {
                    agent.speed = 3f;
                    playerAnimation.CrossFade(playerAni.walk.name, 0.3f);//�ȴ� �ִϸ��̼� ����

                }
                else
                {
                    agent.speed = 10f;
                    playerAnimation.CrossFade(playerAni.run.name, 0.3f);//�ٴ� �ִϸ��̼� ����
                    
                }

                target = hit.point; // Ÿ�� ��ġ�� ������ ���� ��ġ�� ����
                agent.destination = target; // �׺� �޽� ������Ʈ�� �������� Ÿ������ ����
                agent.isStopped = false; // ����޽� ������Ʈ�� ������ ���� ������ ����� 
            }
        }
        else
        {
            #region �Ÿ��� ���ϴ� ù��° ���
            //3������ǥ���� �Ÿ��� ���ϴ� �Լ�
            //if (Vector3.Distance(tr.position, target) <= 0.5f) // �÷��̾�� Ÿ���� �Ÿ�
            //{
            //    playerAnimation.CrossFade(playerAni.idle.name, 0.2f); // ���̵� �ִϸ��̼� ���
            //}
            #endregion
            #region �Ÿ��� ���ϴ� �ι�° ��� : �׺�޽�������Ʈ�� �̿��� ��� -> �ַξ�
            if (agent.remainingDistance <= 0.5f) // �׺�޽� ������Ʈ�� ���� �Ÿ�
            {
                playerAnimation.CrossFade(playerAni.idle.name, 0.2f); // ���̵� �ִϸ��̼� ���
            }
            #endregion
        }
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space)) // �����̽��ٸ� ������ŭ
        {
            playerAnimation.CrossFade(playerAni.Jump.name, 0.2f); // ���� �ִϸ��̼� ���
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            playerAnimation.CrossFade(playerAni.walk.name, 0.2f); // ���̵� �ִϸ��̼� ���
        }
    }
}

