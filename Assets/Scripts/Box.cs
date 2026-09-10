using UnityEngine;

public class Box : MonoBehaviour
{
    public Gun gunType;
    public Sprite sprite;
    public AudioClip clip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject,5F);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            GameManager.Instance.ChangeGunType(gunType, sprite);
            GameManager.Instance.SFX = clip;
            Destroy(gameObject);
        }
    }
}
