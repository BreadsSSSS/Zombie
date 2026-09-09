using UnityEngine;
using UnityEngine.UI;

public class SetingUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Button setBtn;
    void Start()
    {
        setBtn = GetComponent<Button>();
        setBtn.onClick.AddListener(GameManager.Instance.Pause);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
