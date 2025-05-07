using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))] // attribute
// NavMeshAgent 가 없으면 경고를 보여줌 

[System.Serializable] // Serializable : 인스펙터에서 보이게 해주는 어트리뷰트(속성)
public class PlayerAni // 필요한 애니메이션 클립을 모아두는 클래스
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
    public PlayerAni playerAni = new PlayerAni();//애니메이션 클립을 담고 있는 클래스 
    [SerializeField]//private라고 했지만 인스펙터에서 보이게 해줌
    private Animation playerAnimation; // 애니메이션 컴포넌트
    Ray ray; //광선 // 앞에 아무것도 안쓰면 자동으로 private
    RaycastHit hit; // 광선이 닿은 물체의 위치 방향 거리를 알수 있음 
    Vector3 target = Vector3.zero;
    NavMeshAgent agent; // 네비메쉬 에이전트
    Transform tr;
    [Header("마우스 더블클릭관련 변수")]
    public float M_doubleClickSecond = 0.25f; // 더블클릭 시간 
    public bool IsOnClick = false; // 원클릭 더블클릭 여부 
    public double m_Timer = 0; // 더블클릭 타이머 
    
    void Start()
    {
        tr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>(); 
        playerAnimation = GetComponent<Animation>();

    }

   
    void Update()
    {
        if(IsOnClick&&((Time.time - m_Timer) > M_doubleClickSecond)) // 더블클릭이 아니고 더블클릭 타이머가 더블클릭 시간보다 크면
        {
            Debug.Log("OneClick!.");// 더블클릭이아님
            IsOnClick = false; // 더블클릭 여부를 false로 설정
            
        }
        if(Input.GetMouseButtonDown(1)) // 마우스 오른쪽 버튼을 클릭하면
        {
            if(IsOnClick == false) // 더블클릭이면
            {
                m_Timer = Time.time; // 더블클릭 타이머를 현재 시간으로 설정
                IsOnClick = true; // 더블클릭 여부를 true로 설정

            }
            else if(IsOnClick = true&& (Time.time - m_Timer)< M_doubleClickSecond)
            {
                Debug.Log("DoubleClick!"); // 더블클릭
                IsOnClick = false; // 더블클릭 여부를 false로 설정 
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
            playerAnimation.CrossFade(playerAni.skill.name, 0.2f); // 아이들 애니메이션 재생
            agent.speed = 0f; //속도설정
            agent.velocity = Vector3.zero; // 속도 초기화
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            playerAnimation.CrossFade(playerAni.idle.name, 0.2f); // 아이들 애니메이션 재생
        }
    }

    private void Attack()
    {
        if (Input.GetMouseButton(0))
        {
            playerAnimation.CrossFade(playerAni.attack.name, 0.2f); // 공격 애니메이션 재생
            agent.speed = 0f; //속도설정
            agent.velocity = Vector3.zero; // 속도 초기화
        }
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift)) // 쉬프트 누르면
        {
            playerAnimation.CrossFade(playerAni.run.name, 0.2f); // 런 애니메이션
            agent.speed = 10f; //속도설정    
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) // 쉬프트 떼면
        {
            playerAnimation.CrossFade(playerAni.idle.name, 0.2f); // 런 애니메이션 재생
            agent.speed = 0f; //속도설정
            agent.velocity = Vector3.zero; // 속도 초기화
        }
    }

    private void MouseMovement()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);// 마우스 위치를 월드 좌표로 변환
                                                                //카메라에서 마우스 위치로 광선을 쏜다.
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow); // 광선 시각화 
        //씬 화면에서만 보인다(레이위치,레이방향,길이,색상)

        if (Input.GetMouseButtonDown(1))
        {
            //광선,맞은위치,거리는100,레이어마스크(8번 레이어만 체크)
            if (Physics.Raycast(ray, out hit, 100, 1 << 8)) // 광선이 물체에 닿으면
            {
                if(IsOnClick==true)//더블클릭이면
                {
                    agent.speed = 3f;
                    playerAnimation.CrossFade(playerAni.walk.name, 0.3f);//걷는 애니메이션 구현

                }
                else
                {
                    agent.speed = 10f;
                    playerAnimation.CrossFade(playerAni.run.name, 0.3f);//뛰는 애니메이션 구현
                    
                }

                target = hit.point; // 타켓 위치를 광선이 닿은 위치로 설정
                agent.destination = target; // 네비 메쉬 에이전트의 목적지를 타겟으로 설정
                agent.isStopped = false; // 내비메쉬 에이전트를 멈추지 않음 추적을 계속함 
            }
        }
        else
        {
            #region 거리를 구하는 첫번째 방법
            //3차원좌표에서 거리를 구하는 함수
            //if (Vector3.Distance(tr.position, target) <= 0.5f) // 플레이어와 타겟의 거리
            //{
            //    playerAnimation.CrossFade(playerAni.idle.name, 0.2f); // 아이들 애니메이션 재생
            //}
            #endregion
            #region 거리를 구하는 두번째 방법 : 네비메쉬에이전트를 이용한 방법 -> 주로씀
            if (agent.remainingDistance <= 0.5f) // 네비메쉬 에이전트의 남은 거리
            {
                playerAnimation.CrossFade(playerAni.idle.name, 0.2f); // 아이들 애니메이션 재생
            }
            #endregion
        }
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space)) // 스페이스바를 누른만큼
        {
            playerAnimation.CrossFade(playerAni.Jump.name, 0.2f); // 점프 애니메이션 재생
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            playerAnimation.CrossFade(playerAni.walk.name, 0.2f); // 아이들 애니메이션 재생
        }
    }
}

