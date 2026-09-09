using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    Rigidbody2D bulletRb;
    void Awake()
    {
        bulletRb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        //Destroy(gameObject,2f);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void Fire(Vector2 direction)
    {
        bulletRb.linearVelocity = direction * GameManager.Instance.GetCurrentGunType().shotSpeed;
        if (GameManager.Instance.GetCurrentGunType().Type != Gun.Pistol) {
            GameManager.Instance.GetCurrentGunType().BulletCount--;
            GameManager.Instance.UpdateGunBulletCount(GameManager.Instance.GetCurrentGunType().BulletCount);
            if (GameManager.Instance.GetCurrentGunType().BulletCount <= 0) {
                GameManager.Instance.RemoveGunType(GameManager.Instance.GetCurrentGunType().Type);
                GameManager.Instance.ChangeGunType(Gun.Pistol);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")||collision.gameObject.CompareTag("AirWall")) {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy")) {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(GameManager.Instance.GetCurrentGunType().Damage);
            Destroy(gameObject);
        }
    }
}
