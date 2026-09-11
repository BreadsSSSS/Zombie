using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 30;
    public float lifeTime = 5f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Destroy(gameObject, lifeTime); // ���ʱ�䵽���Զ����٣�����ɳ���ͼ��һֱռ�ڴ�
    }

    public void Fire(Vector2 direction)
    {
        rb.linearVelocity = direction * speed;

        // ���ӵ���ͼ������з���
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