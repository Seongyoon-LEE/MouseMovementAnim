using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable] //Serializable : 인스펙터에서 보이게 해주는 어트리뷰트
public class WizardAni //필요한 애니메이션 클립을 모아두는 클래스
{ 
    public AnimationClip idle;
    public AnimationClip walk;
    public AnimationClip run;
    public AnimationClip attack;
    public AnimationClip skill;
}

public class WizardCtrl : MonoBehaviour
{
    public WizardAni wizardAni; // 애니메이션 클립을 담고 있는 클래스
    public Animation ani;
    void Start()
    {                           //.name : 스트링으로 가져옴 
        ani.Play(wizardAni.idle.name); // idle 애니메이션 재생
        
    }

    
    void Update()
    {
        
        //Attack //마우스 클릭시 공격 애니메이션 재생
        if (Input.GetMouseButton(0)) // 마우스 좌클릭 : 0 , 마우스 우클릭 : 1, 마우스 휠 : 2
        {     //CrossFade : 앞전 애니메이션과 뒤에 애니메이션을 부드럽게 만듬
            ani.CrossFade(wizardAni.attack.name,0.2f); // 공격 애니메이션 전환
        }
        else if (Input.GetMouseButton(1))
        {
            ani.CrossFade(wizardAni.skill.name,0.2f); // 스킬 애니메이션 전환
        }
        else
        {     
            ani.CrossFade(wizardAni.idle.name,0.2f); //아이들 애니메이션 전환

        }

    }
}
