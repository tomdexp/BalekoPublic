﻿using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XCharts.Runtime;
using Random = UnityEngine.Random;

public class UI_LineChartGenome : MonoBehaviour
{
    public LineChart LineChart;
    
    private Genome _genome;
    private InteractionController _interactionController;

    private void Awake()
    {
        //GenerateTestData();
        _interactionController = FindAnyObjectByType<InteractionController>();
        if (_interactionController == null)
        {
            Debug.LogError("No InteractionController found in the scene", this);
            return;
        }
        _interactionController.OnBestioleSelected.AddListener(OnBestioleSelected);
    }

    private void OnBestioleSelected(Bestiole bestiole)
    {
        _genome = bestiole.Genome;
        LineChart.RemoveData();
        // TODO: Warning this is shitty, it only takes genes that have an opposing gene
        List<string> uniqueGeneNames = new List<string>();
        List<string> uniqueGenePairs = new List<string>();
        
        foreach (var (gene1, gene2) in _genome.OpposingGenes)
        {
            string namePair = gene1.Name + "/" + gene2.Name;
            LineChart.AddSerie<Line>(gene1.Name);
            uniqueGeneNames.Add(gene1.Name);
            uniqueGenePairs.Add(namePair);
        }
        
        foreach (var genome in _genome.GenomeHistory)
        {
            foreach (var gene in genome.Genes)
            {
                // add similar gene Value to the same serie
                // check if the gene is in the serie before adding it
                // round to 2 decimal places
                if (uniqueGeneNames.Contains(gene.GetType().Name))
                {
                    LineChart.AddData(gene.GetType().Name, Math.Round(gene.GetValueDeviationPercentage(), 2));
                }
            }
        }
        
        // remove every "Gene" from the uniqueGenePairs List of string
        for (int i = 0; i < uniqueGenePairs.Count; i++)
        {
            uniqueGenePairs[i] = uniqueGenePairs[i].Replace("Gene", "");
        }
        
        // rename every serie with the namePair
        for (int i = 0; i < uniqueGeneNames.Count; i++)
        {
            LineChart.GetSerie(uniqueGeneNames[i]).serieName = uniqueGenePairs[i];
        }
    }

    [Button(ButtonSizes.Large)]
    private void GenerateTestData()
    {
        _genome = new Genome();
        _genome.BuildDefaultGenome();

        for (int i = 0; i < 500; i++)
        {
            _genome.Mutate();
        }

        LineChart.RemoveData();
        // TODO: Warning this is shitty, it only takes genes that have an opposing gene
        List<string> uniqueGeneNames = new List<string>();
        List<string> uniqueGenePairs = new List<string>();
        
        foreach (var (gene1, gene2) in _genome.OpposingGenes)
        {
            string namePair = gene1.Name + "/" + gene2.Name;
            LineChart.AddSerie<Line>(gene1.Name);
            uniqueGeneNames.Add(gene1.Name);
            uniqueGenePairs.Add(namePair);
        }
        
        foreach (var genome in _genome.GenomeHistory)
        {
            foreach (var gene in genome.Genes)
            {
                // add similar gene Value to the same serie
                // check if the gene is in the serie before adding it
                // round to 2 decimal places
                if (uniqueGeneNames.Contains(gene.GetType().Name))
                {
                    LineChart.AddData(gene.GetType().Name, Math.Round(gene.GetValueDeviationPercentage(), 2));
                }
            }
        }
        
        // remove every "Gene" from the uniqueGenePairs List of string
        for (int i = 0; i < uniqueGenePairs.Count; i++)
        {
            uniqueGenePairs[i] = uniqueGenePairs[i].Replace("Gene", "");
        }
        
        // rename every serie with the namePair
        for (int i = 0; i < uniqueGeneNames.Count; i++)
        {
            LineChart.GetSerie(uniqueGeneNames[i]).serieName = uniqueGenePairs[i];
        }
    }

    private void OnDestroy()
    {
        LineChart.RemoveData();
        _interactionController.OnBestioleSelected.RemoveListener(OnBestioleSelected);
    }
}
