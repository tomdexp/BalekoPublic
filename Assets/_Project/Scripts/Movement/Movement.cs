using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Bestiole Bestiole;
    public GameObject model;
    //ToReplaceLater
    public float Speed;
    public float RotateSpeed;
    public float ToEnemySpeed;
    public float StormInfluenceSpeed;
    public float EraticScale = 1;

    private float _noiseRandomizerX;
    private float _noiseRandomizerY;
    private Vector3 _currentDirection;
    private Vector3 _currentAngleVec;
    private Rigidbody2D _rigidbody2D;
    private Storm _storm;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _noiseRandomizerX = Random.Range(-100000, 100000);
        _noiseRandomizerY = Random.Range(-100000, 100000);
        _storm = GameObject.FindFirstObjectByType<Storm>();
    }

    void FixedUpdate()
    {
        _currentDirection = Vector3.zero;
        NoiseInfluence();
        StormInfluence();
        if (Bestiole.targetList.Count > 0 && Bestiole.collectableList.Count > 0)
        {
            if (Bestiole.Hungerable.CurrentValue / Bestiole.Hungerable.MaxValue > 0.2f)
                EnemyInfluence();
            else
                FoodInfluence();
        }
        else if (Bestiole.targetList.Count > 0 && Bestiole.collectableList.Count == 0)
            EnemyInfluence();
        else if (Bestiole.targetList.Count == 0 && Bestiole.collectableList.Count > 0)
            FoodInfluence();

        _rigidbody2D.velocity = _rigidbody2D.velocity * 0.999f * Time.fixedDeltaTime;
        _rigidbody2D.AddForce(_currentDirection);

        _currentAngleVec = model.transform.up;
        
        model.transform.up = Vector3.Slerp(_currentAngleVec, _rigidbody2D.velocity.normalized, RotateSpeed * Time.fixedDeltaTime);
    }

    void NoiseInfluence()
    {
        _currentDirection += new Vector3(Mathf.PerlinNoise1D((Time.time + _noiseRandomizerX) * EraticScale) - 0.45f, Mathf.PerlinNoise1D((Time.time + _noiseRandomizerY) * EraticScale + 10000) -0.46f).normalized * Speed;
    }

    void StormInfluence()
    {
        Vector3 stormDir = _storm.transform.position - transform.position;
        float delta = Mathf.Exp((stormDir.magnitude / _storm.transform.localScale.x) * 5 - 5);
        _currentDirection += stormDir * delta * StormInfluenceSpeed;
    }

    void EnemyInfluence()
    {
        if (Bestiole.targetList[0].gameObject.activeSelf)
        {
            if (Vector2.Distance(Bestiole.targetList[0].transform.position, Bestiole.transform.position) > 2.0f)
                _currentDirection = (Bestiole.targetList[0].transform.position - transform.position).normalized;
            else _currentDirection = Vector3.zero;
        } else
        {
            Bestiole.targetList.Remove(Bestiole.targetList[0]);
        }
    }

    void FoodInfluence()
    {
        Debug.Log("food influence");
        if (Bestiole.collectableList[0] != null)
        {
            _currentDirection = (Bestiole.collectableList[0].transform.position - transform.position).normalized;
        }
        else
        {
            Bestiole.collectableList.Remove(Bestiole.collectableList[0]);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + _currentDirection.normalized);
    }
}
