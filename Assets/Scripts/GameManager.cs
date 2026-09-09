using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public bool isGameOver = false;
    public int score;
    public int timeScore;
    public int Round;
    public int MaxRound;
    private Dictionary<Gun, GunType> gunTypes = new Dictionary<Gun, GunType>();
    private GunType currentGunType;
    public Image[] gunIcons;
    public int GunBulletCount;
    public GameObject BulletCountTxt;
    private bool isPaused = false;
    private bool isGameStarted = true;
    public GameObject PauseUI;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null) {
                GameObject singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<GameManager>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        GunType Pistol = new GunType()
        {
            Type = Gun.Pistol,
            Name = "Pistol",
            Damage = 10,
            Icon = "pistol_icon",
            shotSpeed = 10,
            fireRate = 0.5f,
            BulletCount = 1,
        };
        GunBulletCount = Pistol.BulletCount;
        UpdateGunBulletCount(GunBulletCount);
        AddGunType(Pistol.Type, Pistol);


    }

    void Update()
    {
        Pause();
    }

    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGameStarted) {
            if (isPaused) {
                Time.timeScale = 1f;
                isPaused = false;
                PauseUI.SetActive(false);
            }

            else {
                Time.timeScale = 0f;
                isPaused = true;
                PauseUI.SetActive(true);
            }
        }
    }

    public void GameStart()
    {
        isGameStarted = true;
        isGameOver = false;
        score = 0;
        timeScore = 1000;
        Round = 1;
    }

    public void DoSomething()
    {
        Debug.Log("Doing something in GameManager");
    }

    public void AddGunType(Gun gun, GunType gunType)
    {
        gunTypes.Add(gun, gunType);
        currentGunType = gunType;
        Debug.Log(gunTypes.Count);
    }

    public GunType GetCurrentGunType()
    {
        return currentGunType;
    }

    public void ChangeGunType(Gun type)
    {
        currentGunType = gunTypes[type];
    }

    public void RemoveGunType(Gun type)
    {
        if (gunTypes.ContainsKey(type)) {
            gunTypes.Remove(type);
        }
    }

    public void UpdateGunBulletCount(int count)
    {
        GunBulletCount = count;
        BulletCountTxt.GetComponent<Text>().text = "残弾" + GunBulletCount.ToString();
    }
}
