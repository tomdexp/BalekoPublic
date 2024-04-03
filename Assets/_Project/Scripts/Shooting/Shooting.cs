using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Bestiole Bestiole;
    public ProjectileSettings ProjectileSettings;

    private float timer;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 2.0f)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        timer = 0;
        var bulletFlyweight = FlyweightFactory.Spawn(ProjectileSettings);
        var bullet = bulletFlyweight.GetComponent<Bullet>();
        bullet.transform.position = Bestiole.BulletSpawnPoint.position;
        var damage = ProjectileSettings.DefaultDamage; // TODO : GeneProjectileDamage
        var speed = ProjectileSettings.DefaultSpeed + Bestiole.Genome.GetGene<GeneProjectileSpeed>().Value * ProjectileSettings.DefaultSpeed;
        var size = ProjectileSettings.DefaultSize + Bestiole.Genome.GetGene<GeneProjectileSize>().Value * ProjectileSettings.DefaultSize;
        bullet.SetupBullet(damage, speed, size, new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized, Bestiole);
    }
}
