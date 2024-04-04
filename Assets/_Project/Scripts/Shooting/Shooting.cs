using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shooting : MonoBehaviour
{
    public Bestiole Bestiole;
    public ProjectileSettings ProjectileSettings;

    public float AttackSpeed = 2.0f;
    public float Timer;
    public float Angle = 30f;
    public int ProjectileCount = 1;

    private void OnEnable()
    {
        Timer = 0;
    }

    private void OnDisable()
    {
        Timer = 0;
    }

    private void Update()
    {
        if (Bestiole.targetList.Count == 0) return; 
        Timer += Time.deltaTime;
        if (Timer > AttackSpeed)
        {
            for (int i = 0; i < ProjectileCount; i++)
            {
                Shoot();
            }
        }
    }

    public void Shoot()
    {
        Angle = Mathf.Clamp(Angle, 0, 360);
        Timer = 0;
        var bulletFlyweight = FlyweightFactory.Spawn(ProjectileSettings);
        var bullet = bulletFlyweight.GetComponent<Bullet>();
        bullet.transform.position = Bestiole.BulletSpawnPoint.position;
    
        // Calculate the base direction of the bullet
        Vector2 direction = Bestiole.targetList[0].transform.position - Bestiole.BulletSpawnPoint.position;

        // Randomize the shooting angle within the specified precision range
        float angleOffset = Random.Range(-Angle / 2, Angle / 2);
        Quaternion rotation = Quaternion.Euler(0, 0, angleOffset); // Rotate around Z axis

        // Apply the randomized angle offset to the direction
        Vector2 randomizedDirection = rotation * direction.normalized;

        var damage = ProjectileSettings.DefaultDamage; // TODO : GeneProjectileDamage
        var speed = ProjectileSettings.DefaultSpeed + Bestiole.Genome.GetGene<GeneProjectileSpeed>().Value * ProjectileSettings.DefaultSpeed;
        var size = ProjectileSettings.DefaultSize + Bestiole.Genome.GetGene<GeneProjectileSize>().Value * ProjectileSettings.DefaultSize;
        var lifetime = ProjectileSettings.DefaultLifespan + Bestiole.Genome.GetGene<GeneProjectileLifespan>().Value * ProjectileSettings.DefaultLifespan;
        bullet.SetupBullet(damage, speed, size, lifetime, randomizedDirection, Bestiole);
    }

}
