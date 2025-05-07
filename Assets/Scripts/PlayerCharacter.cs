using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMesh))]

[System.Serializable]
public class PlayerAnim
{
    public AnimationClip idle;
    public AnimationClip walk;
    public AnimationClip run;
    public AnimationClip attack;
    public AnimationClip skill;
    public AnimationClip jump;

}
public class PlayerCharacter : MonoBehaviour
{
    public PlayerAnim playeranim = new PlayerAnim();
    [SerializeField] // 프라이빗한걸 보여줌
    private Animation anim;
    Ray ray;
    RaycastHit hit;
    Vector3 target = Vector3.zero;
    NavMeshAgent agent;
    Transform tr;
    
    
    void Start()
    {
        tr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animation>();
        
    }

    
    void Update()
    {
        MouseMove();
        Attack();
        Skill();
        Run();
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.CrossFade(playeranim.run.name, 0.2f);
            agent.speed = 10f;

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.CrossFade(playeranim.idle.name, 0.2f);
            agent.speed = 0f; //속도설정
            agent.velocity = Vector3.zero; // 속도 초기화

        }
    }

    private void Skill()
    {
        
        if (Input.GetKey(KeyCode.Q))
        {
            anim.CrossFade(playeranim.skill.name, 0.2f);
            agent.speed = 0;
            agent.velocity = Vector3.zero; 
        }
        //if (Input.GetKeyUp(KeyCode.Q))
        //{
        //    anim.CrossFade(playeranim.idle.name, 0.2f);
        //}
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.CrossFade(playeranim.attack.name, 0.2f);
            agent.speed = 0f; //속도설정
            agent.velocity = Vector3.zero;
        }
    }

    private void MouseMove()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);

        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(ray, out hit, 100, 1 << 8))
            {
                target = hit.point;
                agent.destination = target;
                agent.speed = 3; // 속도 설정
                anim.CrossFade(playeranim.walk.name, 0.2f);
            }
        }
        else
        {
            if(agent.remainingDistance <= 0.5f)
            {
                anim.CrossFade(playeranim.idle.name, 0.2f);
            }
        }
    }
}
