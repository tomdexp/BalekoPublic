using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Collectable : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent == null) return;
        Bestiole bestiole = other.transform.parent.GetComponent<Bestiole>();
        if (bestiole != null)
        {
            bestiole.Hungerable.Substract(-10);
            _spriteRenderer.transform.DOScale(0.75f, .1f).OnComplete(() =>
            {
                _spriteRenderer.transform.DOScale(0.5f, .1f).OnComplete(() =>
                {
                    transform.DOKill();
                    Destroy(gameObject);
                });
            });
        }
    }
}
