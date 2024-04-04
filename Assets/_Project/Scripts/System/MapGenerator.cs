using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor.Rendering;

[Serializable]
public class MapProp
{
    public GameObject prefab;
    public float minSizeAdd;
    public float maxSizeAdd;
    public List<float> Rotations = new List<float>();
}

public class MapGenerator : MonoBehaviour
{
    [MinMaxSlider(0,500)]
    public Vector2 GeneratedObjects = Vector2.zero;
    public List<MapProp> mapProps = new List<MapProp>();
    [Required]
    public GameObject MapObject;

    private Storm _storm;


    // Start is called before the first frame update
    void Start()
    {
        _storm = GameObject.FindFirstObjectByType<Storm>();
        GenerateMap();
    }


    [Button("Regenerate map")]
    public void GenerateMap()
    {
        foreach (Transform child in MapObject.transform)
        {
            Destroy(child.gameObject);
        }

        int generatedObjects = (int)Mathf.Floor(UnityEngine.Random.Range(GeneratedObjects.x, GeneratedObjects.y));

        for (int i = 0; i < generatedObjects; i++)
        {
            int objectChosen = UnityEngine.Random.Range(0,mapProps.Count);
            GameObject obj = Instantiate(mapProps[objectChosen].prefab, MapObject.transform);
            obj.transform.localScale = obj.transform.localScale + Vector3.one * UnityEngine.Random.Range(mapProps[objectChosen].minSizeAdd*100, mapProps[objectChosen].maxSizeAdd*100)/100;
            Vector3 randomInCircle = UnityEngine.Random.insideUnitCircle;
            obj.transform.position = _storm.transform.position + randomInCircle * _storm.transform.localScale.x / 2;
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, -1);
            obj.transform.eulerAngles = new Vector3(0,0, mapProps[objectChosen].Rotations[UnityEngine.Random.Range(0, mapProps[objectChosen].Rotations.Count)]);
        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
