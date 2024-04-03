using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D Rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public Bestiole Sender;
    public float Damage;
    public float Speed;
    public Vector2 Direction;

    public void SetupBullet(float damage, float speed, Vector2 direction, Bestiole sender)
    {
        Damage = damage;
        Speed = speed;
        Direction = direction;
        Sender = sender;
    }

    private void Update()
    {
        Rigidbody.velocity = Direction * Speed;
    }

    public void DestroyBullet()
    {
        _spriteRenderer.transform.DOScale(0.75f, .1f).OnComplete(() =>
        {
            _spriteRenderer.transform.DOScale(0.5f, .1f).OnComplete(() =>
            {
                transform.DOKill();
                Destroy(gameObject);
            });
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bestiole bestiole = collision.GetComponentInParent<Bestiole>();
        if (bestiole != null)
        {
            bestiole.Damageable.Damage(Damage);
            Sender.killNumber++;
            DestroyBullet();
        }
    }
}
