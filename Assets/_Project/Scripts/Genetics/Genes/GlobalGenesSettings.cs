using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;


[CreateAssetMenu(fileName = nameof(GlobalGenesSettings), menuName = "ScriptableData/" + nameof(GlobalGenesSettings))]
public class GlobalGenesSettings : SerializedScriptableObject
{
    [Serializable]
    public class GeneSetting
    {
        [ReadOnly] public string GeneTypeName;
        public float MinValue = -1.0f;
        public float MaxValue = 1.0f;
        public float MutationRate = 0.1f;
        public float MinMutationRate = 0.01f;
        public float MaxMutationRate = 0.5f;
        public float MutationChance = 0.5f;
    }

    [TableList] public List<GeneSetting> GeneSettings = new List<GeneSetting>();

    [Button(ButtonSizes.Large)]
    private void RefreshGeneList()
    {
        var geneTypes = Assembly.GetAssembly(typeof(Gene)).GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Gene)))
            .Select(t => t.Name).ToHashSet(); // Convert to HashSet for efficient lookup

        // Add new gene types to the settings
        foreach (var geneTypeName in geneTypes)
        {
            if (GeneSettings.All(s => s.GeneTypeName != geneTypeName))
            {
                var newSetting = new GeneSetting
                {
                    GeneTypeName = geneTypeName
                };
                GeneSettings.Add(newSetting);
                Debug.Log($"Added settings for new gene type: {geneTypeName}");
            }
        }

        // Detect and remove settings for gene types that no longer exist
        for (int i = GeneSettings.Count - 1; i >= 0; i--)
        {
            var setting = GeneSettings[i];
            if (!geneTypes.Contains(setting.GeneTypeName))
            {
                GeneSettings.RemoveAt(i);
                Debug.LogWarning($"Removed settings for gene type '{setting.GeneTypeName}' which no longer exists.");
            }
        }
        
        Debug.Log("Genes Settings List is up to date.");
    }

    
    public GeneSetting GetGeneSettings<T>() where T : Gene
    {
        var geneTypeName = typeof(T).Name;
        var setting = GeneSettings.FirstOrDefault(s => s.GeneTypeName.Equals(geneTypeName));
        if (setting == null)
        {
            Debug.LogError("Gene settings not found for " + geneTypeName + ", maybe you need to refresh the gene list on the scriptable object ?");
        }
        return setting;
    }
}
