using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Diagnostics;

public class Bullet : Flyweight
{
    public new ProjectileSettings Settings
    {
        get => (ProjectileSettings)base.Settings;
        set => base.Settings = value;
    }

    [SerializeField] private Rigidbody2D Rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public Bestiole Sender;
    public float Damage;
    public float Speed;
    public Vector2 Direction;
    public float Lifetime;
    public bool bulletCanBounce;
    public LayerMask WallsLayerMask;


    public void SetupBullet(float damage, float speed, float size, float lifetime,Vector2 direction, Bestiole sender)
    {
        Damage = damage;
        Speed = speed;
        transform.localScale = new Vector2(size, size);
        Lifetime = lifetime;
        Direction = direction;
        Sender = sender;

        StartCoroutine(LifetimeCoroutine());
    }

    private IEnumerator LifetimeCoroutine()
    {
        yield return new WaitForSeconds(Lifetime);
        if (gameObject.activeSelf) DestroyBullet();
    }


    private void Update()
    {
        Rigidbody.velocity = Direction * Speed;
    }

    public void DestroyBullet()
    {
        _spriteRenderer.transform.DOScale(0.5f, .1f).OnComplete(() =>
        {
            _spriteRenderer.transform.DOScale(0.3f, .1f).OnComplete(() =>
            {
            transform.DOKill();
            FlyweightFactory.ReturnToPool(this);
            }).SetUpdate(true);
        }).SetUpdate(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((WallsLayerMask & (1 << collision.gameObject.layer)) != 0)
        {
            UnityEngine.Debug.Log("11");
            DestroyBullet();
            return;
        }
        if (collision.transform.parent == null) return;
        Bestiole bestiole = collision.transform.parent.GetComponent<Bestiole>();
        if (bestiole != null)
        {
            if (bestiole == Sender && !bulletCanBounce)
                return;
            bestiole.Damageable.Substract(Damage);
            Sender.killNumber++;
            if (_spriteRenderer != null)
                DestroyBullet();
        }
    }
}
