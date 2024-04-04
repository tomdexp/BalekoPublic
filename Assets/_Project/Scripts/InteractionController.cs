using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionController : MonoBehaviour
{
    public UnityEvent<Bestiole> OnBestioleSelected;
    public LayerMask selectionLayerMask;
    private Bestiole formerSelectedBestiole = null;

    #region Singleton
    public static InteractionController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    private void Start()
    {
        OnBestioleSelected.AddListener(ShowBestioleInfo);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.position, Mathf.Infinity, selectionLayerMask);
            if (hit)
            {
                Bestiole bestiole = hit.transform.GetComponent<Bestiole>();
                if (bestiole != null)
                {
                    OnBestioleSelected?.Invoke(bestiole);
                }
            }
        }
    }

    public void ShowBestioleInfo(Bestiole bestiole)
    {
        Debug.Log(bestiole.name);

        if (formerSelectedBestiole != null)
        {
            formerSelectedBestiole.OutlineSpriteRenderer.gameObject.SetActive(false);
        }
        formerSelectedBestiole = bestiole;
        bestiole.OutlineSpriteRenderer.gameObject.SetActive(true);
    }
}
