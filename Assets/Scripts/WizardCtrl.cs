using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable] //Serializable : �ν����Ϳ��� ���̰� ���ִ� ��Ʈ����Ʈ
public class WizardAni //�ʿ��� �ִϸ��̼� Ŭ���� ��Ƶδ� Ŭ����
{ 
    public AnimationClip idle;
    public AnimationClip walk;
    public AnimationClip run;
    public AnimationClip attack;
    public AnimationClip skill;
}

public class WizardCtrl : MonoBehaviour
{
    public WizardAni wizardAni; // �ִϸ��̼� Ŭ���� ��� �ִ� Ŭ����
    public Animation ani;
    void Start()
    {                           //.name : ��Ʈ������ ������ 
        ani.Play(wizardAni.idle.name); // idle �ִϸ��̼� ���
        
    }

    
    void Update()
    {
        
        //Attack //���콺 Ŭ���� ���� �ִϸ��̼� ���
        if (Input.GetMouseButton(0)) // ���콺 ��Ŭ�� : 0 , ���콺 ��Ŭ�� : 1, ���콺 �� : 2
        {     //CrossFade : ���� �ִϸ��̼ǰ� �ڿ� �ִϸ��̼��� �ε巴�� ����
            ani.CrossFade(wizardAni.attack.name,0.2f); // ���� �ִϸ��̼� ��ȯ
        }
        else if (Input.GetMouseButton(1))
        {
            ani.CrossFade(wizardAni.skill.name,0.2f); // ��ų �ִϸ��̼� ��ȯ
        }
        else
        {     
            ani.CrossFade(wizardAni.idle.name,0.2f); //���̵� �ִϸ��̼� ��ȯ

        }

    }
}
