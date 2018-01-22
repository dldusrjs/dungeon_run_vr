using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySkeletonControl : MonoBehaviour {

    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent nvAgent;
    private Animator animator;
    public bool isDie = false;
    private float hitActionLimit = 2.5f;

    private bool isWalkingSoundPlaying = false;

    public float traceDist = 10.0f;
    public float attackDist = 2.1f;
    public float monsterHealth = 100f;
    public float damageAmplifier = 8f;

    public enum MonsterState { idle, trace, attack, defence, die};
    public MonsterState monsterState = MonsterState.idle;
    public AudioClip HitClip1;
    public AudioClip DeadClip1;


	// Use this for initialization
	void Start () {
        monsterTr = this.gameObject.GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = this.gameObject.GetComponent<Animator>();

        nvAgent.destination = playerTr.position;

        StartCoroutine(this.CheckMonsterState());
        StartCoroutine(this.MonsterAction());
	}

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.2f);
            float dist = Vector3.Distance(playerTr.position, monsterTr.position);
            if (dist <= attackDist)
            {
                monsterState = MonsterState.attack;
            }
            else if (dist <= traceDist)
            {
                monsterState = MonsterState.trace;

            }
            else
            {
                monsterState = MonsterState.idle;
            }
        }
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (monsterState)
            {
                case MonsterState.idle:
                    nvAgent.isStopped = true;
                    animator.SetBool("IsTrace", false);
                    break;

                case MonsterState.trace:
                    nvAgent.destination = playerTr.position;
                    nvAgent.updateRotation = true;
                    nvAgent.isStopped = false;
                    animator.SetBool("IsTrace", true);
                    animator.SetBool("IsAttack", false);

                    break;

                case MonsterState.attack:
                    animator.SetBool("IsAttack", true);
                    nvAgent.updateRotation = false;

                    break;
            }
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            
            if (Vector3.Magnitude(collision.impulse) > hitActionLimit)
            {
                animator.SetTrigger("IsHit");
                GetComponent<AudioSource>().PlayOneShot(HitClip1, 0.5f);
            }
            monsterHealth -= Vector3.Magnitude(collision.impulse) * damageAmplifier; // 대미지 주는 부분
            Debug.Log(monsterHealth);
        }
    }
    /// <summary>
    /// //////////////////////////몬스터 애니메이션
    /// </summary>


    // Update is called once per frame
    void Update () {
        MonsterHealthCheck();
        MonsterWalkingSoundCheck();
        if (monsterState == MonsterState.attack)
        {
            Vector3 lookTrackingDirection = Vector3.Normalize(playerTr.position - transform.position);
            Quaternion lookTrackingRotation = Quaternion.LookRotation(new Vector3(lookTrackingDirection.x, 0, lookTrackingDirection.z), Vector3.up);
            transform.rotation = Quaternion.Slerp(lookTrackingRotation, transform.rotation, Time.deltaTime * 1f);
        }
	}

    void MonsterHealthCheck()
    {
        if (monsterHealth < 0 && !isDie)
        {
            MonsterDie();
        }
    }

    void MonsterDie()
    {
        StopAllCoroutines();
        monsterState = MonsterState.die;
        animator.SetTrigger("IsDie");
        nvAgent.isStopped = true;
        isDie = true;
        GetComponent<AudioSource>().Stop(); // 걷는소리 중지
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;
        gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
        GetComponent<AudioSource>().PlayOneShot(DeadClip1, 0.5f); // 적 죽는 소리
        Destroy(this.gameObject, 4f);


    }

    void MonsterWalkingSoundCheck()
    {
        if(!nvAgent.isStopped && !isWalkingSoundPlaying)
        {
            GetComponent<AudioSource>().Play();
            //FindObjectOfType<AudioManager>().Play("WoodStep", 0.3f);
            isWalkingSoundPlaying = true;
        }
        if(animator.GetBool("IsAttack") && isWalkingSoundPlaying)
        {
            //FindObjectOfType<AudioManager>().Stop("WoodStep");
            isWalkingSoundPlaying = false;
        }
    }

}

