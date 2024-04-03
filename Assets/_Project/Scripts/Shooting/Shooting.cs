using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Bestiole Bestiole;
    public Bullet BulletPrefab;

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
        Bullet bullet = Instantiate(BulletPrefab);
        bullet.transform.position = Bestiole.BulletSpawnPoint.position;
        Vector2 direction = Bestiole.BulletSpawnPoint.position - Bestiole.transform.position;
        bullet.SetupBullet(10, 10, direction.normalized, Bestiole);
    }
}
