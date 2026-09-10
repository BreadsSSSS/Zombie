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
    public Sprite[] gunIcons;
    public int GunBulletCount;
    public GameObject BulletCountTxt;
    public bool isPaused = false;
    private bool isGameStarted = true;
    public GameObject PauseUI;
    public Image GunIconUI;
    public Image HpBarUI;
    private int KillCount;
    public AudioClip BGM;
    public AudioClip SFX;
    public AudioSource audioSource;
    public AudioSource sfxSource;
    private bool isBoss;
    public GameObject Boss;
    public GameObject[] EenmyS;
    public Transform[] BornPoints;
    public GameObject LoginUI;
    public GameObject LoseUI;
    public GameObject WinUI;

    private int enemyCount;
    private int currentWave;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.volume = 0.5f;
        audioSource.loop = true;
        audioSource.clip = BGM;
        audioSource.Play();

        Time.timeScale = 0f;
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
        currentGunType = Pistol;

        GunType ShoutGun = new GunType()
        {
            Type = Gun.Shoutgun,
            Name = "ShoutGun",
            Damage = 20,
            Icon = "",
            shotSpeed = 30,
            fireRate = 0.8f,
            BulletCount = 90
        };

        AddGunType(ShoutGun.Type, ShoutGun);

        GunType Rifle = new GunType()
        {
            Type = Gun.Rifle,
            Name = "Rifle",
            Damage = 10,
            Icon = "",
            shotSpeed = 20,
            fireRate = 0.05f,
            BulletCount = 100
        };

        AddGunType(Rifle.Type, Rifle);
        isPaused = true;
        //RebornEnemy();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape) && isGameStarted)
            //Pause();

        transform.position = new Vector3(audioSource.transform.position.x, audioSource.transform.position.y, 0);
    }

    public void Pause()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;
            PauseUI.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            isPaused = true;
            PauseUI.SetActive(true);
        }
    }

    public void GameStart()
    {
        isGameStarted = true;
        isGameOver = false;
        score = 0;
        timeScore = 1000;
        Round = 1;
        KillCount = 0;
        isBoss = false;
        currentWave = 0;
        enemyCount = 0;

        LoginUI.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;

        RebornEnemy();
    }

    public void DoSomething()
    {
        Debug.Log("Doing something in GameManager");
    }

    public void AddGunType(Gun gun, GunType gunType)
    {
        gunTypes.Add(gun, gunType);
        Debug.Log(gunTypes.Count);
    }

    public GunType GetCurrentGunType()
    {
        return currentGunType;
    }

    public void ChangeGunType(Gun type, Sprite icon)
    {
        currentGunType = gunTypes[type];
        GunBulletCount = currentGunType.BulletCount;
        GameObject.Find("Player/gun").GetComponent<SpriteRenderer>().sprite = icon;
        GunIconUI.sprite = icon;
        UpdateGunBulletCount(GunBulletCount);
    }

    public void RemoveGunType(Gun type)
    {
        if (gunTypes.ContainsKey(type))
        {
            gunTypes.Remove(type);
        }
    }

    public void UpdateGunBulletCount(int count)
    {
        GunBulletCount = count;
        BulletCountTxt.GetComponent<Text>().text = "残弾" + GunBulletCount.ToString();
    }

    public void UpdateHpBar()
    {
        HpBarUI.fillAmount = GameObject.Find("Player").GetComponent<PlayerController>().HP / 100f;
    }

    public void PlayGunSound()
    {
        sfxSource.clip = SFX;
        sfxSource.Play();
    }

    public void GameOver()
    {
        isGameOver = true;
        isGameStarted = false;
        LoseUI.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void WinGame()
    {
        WinUI.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void EnemyContro()
    {
        KillCount++;
        enemyCount--;

        Debug.Log("KillCount: " + KillCount + " EnemyCount: " + enemyCount);

        if (enemyCount <= 0)
        {
            if (currentWave < 4)
            {
                RebornEnemy();
            }
            else if (currentWave == 4 && !isBoss)
            {
                RebornBoss();
            }
        }
    }

    public void RebornEnemy()
    {
        currentWave++;

        if (currentWave == 1)
            enemyCount = 5;
        else if (currentWave == 2)
            enemyCount = 10;
        else if (currentWave == 3)
            enemyCount = 15;
        else if (currentWave == 4)
            enemyCount = 15;
        else
            return;

        Round = currentWave;

        for (int i = 0; i < enemyCount; i++)
        {
            int enemyIndex = Random.Range(0, EenmyS.Length);
            int bornIndex = Random.Range(0, BornPoints.Length);

            Instantiate(
                EenmyS[enemyIndex],
                BornPoints[bornIndex].position,
                Quaternion.identity
            );
        }

        Debug.Log("第 " + currentWave + " 波生成 " + enemyCount + " 只敌人");
    }

    public void RebornBoss()
    {
        if (isBoss)
            return;

        int bornIndex = Random.Range(0, BornPoints.Length);

        Instantiate(
            Boss,
            BornPoints[bornIndex].position,
            Quaternion.identity
        );

        isBoss = true;

        Debug.Log("Boss 生成");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}