using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
// 마우스 좌클릭 공격 하면 그에 따른 애니메이션이 나오게 하는 스크립트

public class WizardMecanim : MonoBehaviour
{
    public Animator animator; // 애니메이터 컴포넌트
    public string attack = "Attack2"; // "Attack"을 그대로 넣으면 동적할당이 되어서 무거워짐 
    public string skill = "IsSkill"; // 애니메이터의 트리거 이름
    public string IdleAndMove = "IdleAndMove";
    //public string isjump = "IsJump";
    
    Ray ray;
    RaycastHit hit;
    Vector3 target = Vector3.zero;
    NavMeshAgent navi;
    public float M_doubleClickSecond = 0.25f; // 더블클릭 시간 
    public bool IsOnClick = false; // 원클릭 더블클릭 여부 
    public double m_Timer = 0; // 더블클릭 타이머 

    void Start()
    {
        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트를 가져옴 
        navi = GetComponent<NavMeshAgent>();
    }


    // 애니메이션 속도와 반응은 레거시가 빠름.메카님은 부드럽다.
    // Trigger와 Bool의 차이점은 트리거는 콤보어택을 사용 Bool은 중간에 다른 애니메이션으로 빠르게 전환할때 사용 
    // 레거시와 메카님은 캐릭터 밑 하이라키 최적화가 다르다. 무기를 쥐는 bone만 노출 시킬 수 있따.
    void Update()
    {
        OnClick();
        MouseMovement();
        AttackAndSkill();
        //Jump();
    }

    private void OnClick()
    {
        if (IsOnClick && ((Time.time - m_Timer) > M_doubleClickSecond)) // 더블클릭이 아니고 더블클릭 타이머가 더블클릭 시간보다 크면
        {
            Debug.Log("OneClick!.");// 더블클릭이아님
            IsOnClick = false; // 더블클릭 여부를 false로 설정
        }
        if (Input.GetMouseButtonDown(1)) // 마우스 오른쪽 버튼을 클릭하면
        {
            if (IsOnClick == false) // 더블클릭이면
            {
                m_Timer = Time.time; // 더블클릭 타이머를 현재 시간으로 설정
                IsOnClick = true; // 더블클릭 여부를 true로 설정

            }
            else if (IsOnClick = true && (Time.time - m_Timer) < M_doubleClickSecond)
            {
                Debug.Log("DoubleClick!"); // 더블클릭
                IsOnClick = false; // 더블클릭 여부를 false로 설정 
            }
        }
    }

    private void MouseMovement()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 메인카메라에서 마우스 포지션으로 광선을 쏜다.
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow); // 광선의 시작점과 방향*100 색깔로 시각화 

       

        if (Input.GetMouseButtonDown(1))
        {
            
                if (Physics.Raycast(ray, out hit, 100, 1 << 8)) // 광선이 충돌하면
                {
                if(IsOnClick == true) // 원클릭이면
                {
                    navi.speed = 3f;
                    animator.SetFloat(IdleAndMove, navi.speed);
                }
                else
                {
                    navi.speed = 10f; 
                    animator.SetFloat(IdleAndMove, navi.speed); // 뛰어라
                }
                    target = hit.point; // 타켓 위치를 광선이 닿은 위치로 설정
                    navi.destination = target; // 네비 메쉬 에이전트의 목적지를 타겟으로 설정
                    navi.isStopped = false; // 내비메쉬 에이전트를 멈추지 않음 추적을 계속함 
                }
            
            
        }
        else
        {
            #region 거리를 구하는 첫번째 방법
            //3차원좌표에서 거리를 구하는 함수
            //if (Vector3.Distance(tr.position, target) <= 0.5f) // 플레이어와 타겟의 거리
            //{
            //}
            #endregion
            #region 거리를 구하는 두번째 방법 : 네비메쉬에이전트를 이용한 방법 -> 주로씀
            if (navi.remainingDistance <= 0.5f) // 네비메쉬 에이전트의 남은 거리
            {

                navi.isStopped = true;
                animator.SetFloat(IdleAndMove, 0, 0.2f, Time.deltaTime);
            }
            #endregion
        }
        

    }


    private void AttackAndSkill()
    {
        // 메카님 방식에서 Down으로 설정 
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼을 클릭하면
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
    //        animator.SetBool(isjump, true); // 애니메이터 트리거를 jump로 설정
    //        GetComponent<Rigidbody>().velocity = Vector3.up * 50f; // 리지드바디 속도 설정
    //    }
    //    else if (Input.GetKeyUp(KeyCode.Space)) // 스페이스바를 때면
    //    {
    //        animator.SetBool(isjump, false);
    //    }
    //}

}

