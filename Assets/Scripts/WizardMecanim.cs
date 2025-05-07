using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
// ���콺 ��Ŭ�� ���� �ϸ� �׿� ���� �ִϸ��̼��� ������ �ϴ� ��ũ��Ʈ

public class WizardMecanim : MonoBehaviour
{
    public Animator animator; // �ִϸ����� ������Ʈ
    public string attack = "Attack2"; // "Attack"�� �״�� ������ �����Ҵ��� �Ǿ ���ſ��� 
    public string skill = "IsSkill"; // �ִϸ������� Ʈ���� �̸�
    public string IdleAndMove = "IdleAndMove";
    //public string isjump = "IsJump";
    
    Ray ray;
    RaycastHit hit;
    Vector3 target = Vector3.zero;
    NavMeshAgent navi;
    public float M_doubleClickSecond = 0.25f; // ����Ŭ�� �ð� 
    public bool IsOnClick = false; // ��Ŭ�� ����Ŭ�� ���� 
    public double m_Timer = 0; // ����Ŭ�� Ÿ�̸� 

    void Start()
    {
        animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ�� ������ 
        navi = GetComponent<NavMeshAgent>();
    }


    // �ִϸ��̼� �ӵ��� ������ ���Žð� ����.��ī���� �ε巴��.
    // Trigger�� Bool�� �������� Ʈ���Ŵ� �޺������� ��� Bool�� �߰��� �ٸ� �ִϸ��̼����� ������ ��ȯ�Ҷ� ��� 
    // ���Žÿ� ��ī���� ĳ���� �� ���̶�Ű ����ȭ�� �ٸ���. ���⸦ ��� bone�� ���� ��ų �� �ֵ�.
    void Update()
    {
        OnClick();
        MouseMovement();
        AttackAndSkill();
        //Jump();
    }

    private void OnClick()
    {
        if (IsOnClick && ((Time.time - m_Timer) > M_doubleClickSecond)) // ����Ŭ���� �ƴϰ� ����Ŭ�� Ÿ�̸Ӱ� ����Ŭ�� �ð����� ũ��
        {
            Debug.Log("OneClick!.");// ����Ŭ���̾ƴ�
            IsOnClick = false; // ����Ŭ�� ���θ� false�� ����
        }
        if (Input.GetMouseButtonDown(1)) // ���콺 ������ ��ư�� Ŭ���ϸ�
        {
            if (IsOnClick == false) // ����Ŭ���̸�
            {
                m_Timer = Time.time; // ����Ŭ�� Ÿ�̸Ӹ� ���� �ð����� ����
                IsOnClick = true; // ����Ŭ�� ���θ� true�� ����

            }
            else if (IsOnClick = true && (Time.time - m_Timer) < M_doubleClickSecond)
            {
                Debug.Log("DoubleClick!"); // ����Ŭ��
                IsOnClick = false; // ����Ŭ�� ���θ� false�� ���� 
            }
        }
    }

    private void MouseMovement()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ����ī�޶󿡼� ���콺 ���������� ������ ���.
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow); // ������ �������� ����*100 ����� �ð�ȭ 

       

        if (Input.GetMouseButtonDown(1))
        {
            
                if (Physics.Raycast(ray, out hit, 100, 1 << 8)) // ������ �浹�ϸ�
                {
                if(IsOnClick == true) // ��Ŭ���̸�
                {
                    navi.speed = 3f;
                    animator.SetFloat(IdleAndMove, navi.speed);
                }
                else
                {
                    navi.speed = 10f; 
                    animator.SetFloat(IdleAndMove, navi.speed); // �پ��
                }
                    target = hit.point; // Ÿ�� ��ġ�� ������ ���� ��ġ�� ����
                    navi.destination = target; // �׺� �޽� ������Ʈ�� �������� Ÿ������ ����
                    navi.isStopped = false; // ����޽� ������Ʈ�� ������ ���� ������ ����� 
                }
            
            
        }
        else
        {
            #region �Ÿ��� ���ϴ� ù��° ���
            //3������ǥ���� �Ÿ��� ���ϴ� �Լ�
            //if (Vector3.Distance(tr.position, target) <= 0.5f) // �÷��̾�� Ÿ���� �Ÿ�
            //{
            //}
            #endregion
            #region �Ÿ��� ���ϴ� �ι�° ��� : �׺�޽�������Ʈ�� �̿��� ��� -> �ַξ�
            if (navi.remainingDistance <= 0.5f) // �׺�޽� ������Ʈ�� ���� �Ÿ�
            {

                navi.isStopped = true;
                animator.SetFloat(IdleAndMove, 0, 0.2f, Time.deltaTime);
            }
            #endregion
        }
        

    }


    private void AttackAndSkill()
    {
        // ��ī�� ��Ŀ��� Down���� ���� 
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư�� Ŭ���ϸ�
        {
            
            animator.SetTrigger(attack);
            navi.speed = 0f;
            navi.velocity = Vector3.zero;
            
        }
        else if(Input.GetMouseButtonUp(0))
        {
            animator.SetFloat(IdleAndMove, 0 ,0.000002f,Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetBool(skill, true);
            
        }
        else
        {
            animator.SetBool(skill, false);
        }
       
       
    }

    //private void Jump()
    //{
    //    if (Input.GetKey(KeyCode.Space))
    //    {
    //        animator.SetBool(isjump, true); // �ִϸ����� Ʈ���Ÿ� jump�� ����
    //        GetComponent<Rigidbody>().velocity = Vector3.up * 50f; // ������ٵ� �ӵ� ����
    //    }
    //    else if (Input.GetKeyUp(KeyCode.Space)) // �����̽��ٸ� ����
    //    {
    //        animator.SetBool(isjump, false);
    //    }
    //}

}

