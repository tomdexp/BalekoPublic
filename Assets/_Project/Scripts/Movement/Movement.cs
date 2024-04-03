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
        if (Bestiole.Hungerable.CurrentValue / Bestiole.Hungerable.MaxValue > 0.2f && Bestiole.targetList.Count > 0 && Vector2.Distance(Bestiole.targetList[0].transform.position, Bestiole.transform.position) > 2.0f)
            EnemyInfluence();

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
        _currentDirection = (Bestiole.targetList[0].transform.position - transform.position).normalized;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + _currentDirection.normalized);
    }
}
