using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 20;
    public float lifeTime = 5f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Destroy(gameObject, lifeTime); // 存活时间到了自动销毁，避免飞出地图后一直占内存
    }

    public void Fire(Vector2 direction)
    {
        rb.linearVelocity = direction * speed;

        // 让子弹贴图朝向飞行方向
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Hurt(damage);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall") || other.CompareTag("AirWall"))
        {
            Destroy(gameObject);
        }
    }
}