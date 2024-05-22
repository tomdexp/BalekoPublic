using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BestioleManager : MonoBehaviour
{
    public Button StartButton;
    public TMP_Text GenerationCounter;
    
    private BestioleManager _bestioleManager;

    private void Awake()
    {
        _bestioleManager = FindAnyObjectByType<BestioleManager>();
        if (_bestioleManager == null)
        {
            Debug.LogError("No BestioleManager found in the scene");
        }
        
        StartButton.onClick.AddListener(StartLoop);
    }

    private void OnDestroy()
    {
        StartButton.onClick.RemoveListener(StartLoop);
    }

    private void StartLoop()
    {
        StartButton.gameObject.SetActive(false);
        _bestioleManager.StartLoop();
    }

    private void Update()
    {
        GenerationCounter.text = "Generation n°" + _bestioleManager.CurrentGeneration;
    }
}
