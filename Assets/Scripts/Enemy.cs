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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

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
    }

    void OnDrawGizmos()
    {
        if (agent == null || !agent.hasPath) return;

        Gizmos.color = Color.red;
        var path = agent.path;
        for (int i = 0; i < path.corners.Length - 1; i++) {
            Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        StopAllCoroutines();
        StartCoroutine(FlashRed());
        if (health <= 0) {
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
}

public enum EnemyType
{
    Normal,
    Boss,
}
