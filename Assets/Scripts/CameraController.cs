using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 pos = GameObject.Find("Player").transform.position;
        pos.x = pos.x < -17.5 ? -17.5f : pos.x;
        pos.x = pos.x > 17.5 ? 17.5f : pos.x;
        transform.position = new Vector3(pos.x, pos.y, -10);
    }
}
