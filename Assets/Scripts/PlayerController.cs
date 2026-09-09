using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    private float lastShotTime;
    private bool wantsToFire;
    public int HP;
    void Start()
    {
        HP = 100;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        HandleShooting();
    }

    void HandleShooting()
    {
        GunType gun = GameManager.Instance.GetCurrentGunType();

        if (Mouse.current.leftButton.isPressed && Time.time - lastShotTime >= gun.fireRate) {
            lastShotTime = Time.time;
            Shoot();
        }
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        rigidbody.linearVelocity = new Vector2(moveX * moveSpeed, moveY * moveSpeed);
    }

    void Shoot()
    {
        GameManager.Instance.DoSomething();

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        worldMousePos.z = 0f;

        Vector2 fireDirection = (worldMousePos - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Fire(fireDirection);

    }

    public void Hurt(int damage)
    {
        HP -= damage;
        if(HP <= 0)
        {
            HP = 0;
            // todo
        }
        StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.color = Color.white;
    }
}
