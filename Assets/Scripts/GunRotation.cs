using UnityEngine;

public class GunRotation : MonoBehaviour
{
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        AimAtMouse();
    }

    void AimAtMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector2 direction = (mouseWorldPos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 鼠标在左侧时，翻转Y轴，避免枪贴图倒过来
        if (angle > 90 || angle < -90) {
            sr.flipY = true;
        }
        else {
            sr.flipY = false;
        }
    }
}
