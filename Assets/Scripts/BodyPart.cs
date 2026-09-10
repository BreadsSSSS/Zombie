using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public float launchForce = 6f;
    public float randomForce = 2f;
    public float rotationForce = 500f;
    public float lifeTime = 10f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch()
    {
        if (rb == null)
            return;

        Vector2 direction = Random.insideUnitCircle.normalized;

        float force = launchForce + Random.Range(-randomForce, randomForce);

        rb.AddForce(direction * force, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-rotationForce, rotationForce));

        transform.rotation = Quaternion.Euler(
            0,
            0,
            Random.Range(0f, 360f)
        );
    }

    void Start()
    {
        //Destroy(gameObject, lifeTime);
    }
}