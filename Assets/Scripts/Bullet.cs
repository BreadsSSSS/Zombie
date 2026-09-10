using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    Rigidbody2D bulletRb;
    public Sprite gunSp;
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
            GameManager.Instance.GunBulletCount--;
            GameManager.Instance.UpdateGunBulletCount(GameManager.Instance.GunBulletCount);
            if (GameManager.Instance.GunBulletCount <= 0) {
                //GameManager.Instance.RemoveGunType(GameManager.Instance.GetCurrentGunType().Type);
                GameManager.Instance.ChangeGunType(Gun.Pistol, GameManager.Instance.gunIcons[0]);
            }
        }
        GameManager.Instance.UpdateGunBulletCount(GameManager.Instance.GunBulletCount);
        GameManager.Instance.PlayGunSound();
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
