using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ���콺 ��Ŭ�� ���� �ϸ� �׿� ���� �ִϸ��̼��� ������ �ϴ� ��ũ��Ʈ

public class WizardMecanim : MonoBehaviour
{
    public Animator animator; // �ִϸ����� ������Ʈ
    void Start()
    {
        animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ�� ������ 
    }

    
    void Update()
    {                           // ��ī�� ��Ŀ��� Down���� ���� 
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư�� Ŭ���ϸ�
        {
            animator.SetTrigger("Attack"); // �ִϸ������� Ʈ���Ÿ� Attack���� ����
        }
        // ���콺 �� ��ư 
        else if (Input.GetMouseButtonDown(2))
        {
            animator.SetBool("IsSkill", true); // �ִϸ������� Ʈ���Ÿ� Skill���� ����

        }
        //else if (Input.GetMouseButtonDown(1))
        //{
        //    animator.
        //}
    }
}
