using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Bestiole Bestiole;
    public ProjectileSettings ProjectileSettings;

    private float timer = 2.0f;
    private void Update()
    {
        if (Bestiole.targetList.Count == 0) return; 
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
        Vector2 direction = Bestiole.BulletSpawnPoint.position - Bestiole.transform.position;
        var damage = ProjectileSettings.DefaultDamage; // TODO : GeneProjectileDamage
        var speed = ProjectileSettings.DefaultSpeed + Bestiole.Genome.GetGene<GeneProjectileSpeed>().Value * ProjectileSettings.DefaultSpeed;
        var size = ProjectileSettings.DefaultSize + Bestiole.Genome.GetGene<GeneProjectileSize>().Value * ProjectileSettings.DefaultSize;
        var lifetime = ProjectileSettings.DefaultLifespan + Bestiole.Genome.GetGene<GeneProjectileLifespan>().Value * ProjectileSettings.DefaultLifespan;
        bullet.SetupBullet(damage, speed, size, lifetime, direction.normalized, Bestiole);
    }
}
