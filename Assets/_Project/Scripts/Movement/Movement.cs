using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //ToReplaceLater
    public float Speed;
    public float RotateSpeed;
    public float ToEnemySpeed;
    public float StormInfluenceSpeed;
    public float EraticScale = 1;

    private float _noiseRandomizerX;
    private float _noiseRandomizerY;
    private Vector3 _currentDirection;
    private Rigidbody2D rigidbody2D;
    private Storm _storm;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        _noiseRandomizerX = Random.Range(-100000, 100000);
        _noiseRandomizerY = Random.Range(-100000, 100000);
        _storm = GameObject.FindFirstObjectByType<Storm>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _currentDirection = Vector3.zero;
        NoiseInfluence();
        StormInfluence();
        EnemyInfluence();

        rigidbody2D.velocity = rigidbody2D.velocity * 0.999f * Time.fixedDeltaTime;
        rigidbody2D.AddForce(_currentDirection);
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
        //TODO WHEN ENEMY
        //_currentDirection += ToEnemySpeed
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + _currentDirection.normalized);
    }
}
