using System.Collections;
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
    private string currentAnimState = ""; // 记录当前播放的动画名，避免重复Play
    public float DamageRate = 0.8f;
    public int Damame = 10;
    public float LastDamgeTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

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

        UpdateWalkAnimation();
    }

    void UpdateWalkAnimation()
    {
        Vector2 velocity = agent.velocity;

        // 速度太小视为静止，不切换动画(这里默认继续播放最后一帧朝向的走路动画
        // 如果你有单独的 Idle 动画，可以在这里 Play Idle)
        if (velocity.sqrMagnitude < 0.01f) return;

        string newState;

        // 比较x和y方向哪个分量更大，决定用左右还是上下的动画
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
        if (currentAnimState == stateName) return; // 已经在播这个动画了，不重复调用

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
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        //StopAllCoroutines();
        StartCoroutine(FlashRed());
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
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
        if(collision.gameObject.tag == "Player")
        {
            if(Time.time - LastDamgeTime >= DamageRate)
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