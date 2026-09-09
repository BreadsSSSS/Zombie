using UnityEngine;

public class GunType
{
    public Gun Type;
    public string Name;
    public int Damage;
    public string Icon;
    public int shotSpeed;
    public float fireRate;
    public int BulletCount;
}

public enum Gun
{
    Shoutgun,
    Rifle,
    Pistol,
    GrenadeLauncher,
    Sniper,
}
