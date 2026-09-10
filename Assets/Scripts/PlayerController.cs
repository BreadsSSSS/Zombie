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

    private Animator animator;
    private string currentAnimState = "";

    void Start()
    {
        HP = 100;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

        UpdateWalkAnimation(moveX, moveY);
    }

    void UpdateWalkAnimation(float moveX, float moveY)
    {
        Vector2 input = new Vector2(moveX, moveY);

        // 没有输入，保持静止，不切换动画(如果有Idle动画可以在这里Play)
        if (input.sqrMagnitude < 0.01f) return;

        string newState;

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            newState = input.x > 0 ? "walk_east" : "walk_west";
        }
        else
        {
            newState = input.y > 0 ? "walk_north" : "walk_south";
        }

        PlayAnimation(newState);
    }

    void PlayAnimation(string stateName)
    {
        if (currentAnimState == stateName) return;

        animator.Play(stateName);
        currentAnimState = stateName;
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
        GameManager.Instance.UpdateHpBar();
        if (HP <= 0)
        {
            HP = 0;
            GameManager.Instance.UpdateHpBar();
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