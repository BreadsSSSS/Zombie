using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public int health;
    public Transform target;
    public float updateRate = 0.5f;
    private NavMeshAgent agent;
    private float timer;
    public EnemyType enemyType;

    private Animator animator;
    private string currentAnimState = "";
    public float DamageRate = 0.8f;
    public int Damame = 10;
    public float LastDamgeTime;
    public GameObject BossBullet;

    public float bossAttackRange;
    public float bossFireRate = 2f;
    private float lastBossShotTime;

    public GameObject[] Boxs;
    public GameObject[] BodyParts;
    public float BodyPartDropRate = 0.5f;

    void Start()
    {
        target = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = 4f;
        if (enemyType == EnemyType.Boss)
        {
            agent.speed = 4.5f; // Boss moves slower
        }

        animator = GetComponent<Animator>();

        Debug.Log("是否在NavMesh上: " + agent.isOnNavMesh);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > updateRate)
        {
            timer = 0f;
            agent.SetDestination(target.position);
        }

        if (enemyType == EnemyType.Boss)
        {
            HandleBossAttack();
        }

        UpdateWalkAnimation();
    }

    void HandleBossAttack()
    {
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);
        if (distance > bossAttackRange) return;

        if (Time.time - lastBossShotTime < bossFireRate) return;

        lastBossShotTime = Time.time;
        FireBossBullet();
    }

    void FireBossBullet()
    {
        if (BossBullet == null || target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;

        GameObject bulletObj = Instantiate(BossBullet, transform.position, Quaternion.identity);
        bulletObj.GetComponent<BossBullet>().Fire(direction);
    }

    void UpdateWalkAnimation()
    {
        Vector2 velocity = agent.velocity;
        if (velocity.sqrMagnitude < 0.01f) return;

        string newState;
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            newState = velocity.x > 0 ? "walk_west" : "walk_east";
        }
        else
        {
            newState = velocity.y > 0 ? "walk_north" : "walk_south";
        }

        PlayAnimation(newState);
    }

    void PlayAnimation(string stateName)
    {
        if (currentAnimState == stateName) return;
        animator.Play(stateName);
        currentAnimState = stateName;
    }

    void OnDrawGizmos()
    {
        if (agent == null || !agent.hasPath) return;

        Gizmos.color = Color.red;
        var path = agent.path;
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
        }

        if (enemyType == EnemyType.Boss)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, bossAttackRange);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(FlashRed());

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (enemyType == EnemyType.Normal)
        {
            DropBox();
            DropBodyPart();
        }

        if (enemyType == EnemyType.Boss)
        {
            GameManager.Instance.WinGame();
        }

        GameManager.Instance.EnemyContro();
        Destroy(gameObject);
    }

    private void DropBox()
    {
        double num = Random.value;

        if (num <= 0.9f && Boxs != null && Boxs.Length >= 2)
        {
            double num2 = Random.value;

            if (num2 >= 0.5f)
            {
                Instantiate(Boxs[0], transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(Boxs[1], transform.position, Quaternion.identity);
            }
        }
    }

    private void DropBodyPart()
    {
        if (BodyParts == null || BodyParts.Length == 0)
            return;

        if (Random.value > BodyPartDropRate)
            return;

        int index = Random.Range(0, BodyParts.Length);

        if (BodyParts[index] == null)
            return;

        GameObject bodyPart = Instantiate(
            BodyParts[index],
            transform.position,
            Quaternion.identity
        );

        BodyPart bodyPartScript = bodyPart.GetComponent<BodyPart>();

        if (bodyPartScript != null)
        {
            bodyPartScript.Launch();
        }
    }

    IEnumerator FlashRed()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.color = Color.white;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Time.time - LastDamgeTime >= DamageRate)
            {
                LastDamgeTime = Time.time;
                GameObject.Find("Player").GetComponent<PlayerController>().Hurt(Damame);
            }
        }
    }
}

public enum EnemyType
{
    Normal,
    Boss,
}