using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 마우스 좌클릭 공격 하면 그에 따른 애니메이션이 나오게 하는 스크립트

public class WizardMecanim : MonoBehaviour
{
    public Animator animator; // 애니메이터 컴포넌트
    void Start()
    {
        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트를 가져옴 
    }

    
    void Update()
    {                           // 메카님 방식에서 Down으로 설정 
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼을 클릭하면
        {
            animator.SetTrigger("Attack"); // 애니메이터의 트리거를 Attack으로 설정
        }
        // 마우스 휠 버튼 
        else if (Input.GetMouseButtonDown(2))
        {
            animator.SetBool("IsSkill", true); // 애니메이터의 트리거를 Skill으로 설정

        }
        //else if (Input.GetMouseButtonDown(1))
        //{
        //    animator.
        //}
    }
}
