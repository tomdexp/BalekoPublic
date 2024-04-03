using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Storm : MonoBehaviour
{
    public StormData Data;

    private float _startScale;
    private Vector3 _startPosition;
    private bool _isStormStarted;


    // Start is called before the first frame update
    void Start()
    {
        _startScale = transform.localScale.x;
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isStormStarted)
        {
            return;
        }
        transform.localScale -= Vector3.one * Data.SizeDecreaseSpeed * Time.deltaTime;
        transform.localScale = Vector3.one * Mathf.Clamp(transform.localScale.x, Data.MinScale, _startScale);
    }

    private void OnDrawGizmosSelected()
    {
    }

    [Button("Start Storm")]
    public void StartStorm()
    {
        _isStormStarted = true;
    }

    [Button("Stop Storm")]
    public void StopStorm()
    {
        _isStormStarted = false;
    }


    [Button("Reset Storm")]
    public void ResetStorm()
    {
        transform.localScale = Vector3.one * _startScale;
        transform.position = _startPosition;
    }

    public void ResetStorm(Vector3 newPosition)
    {
        transform.localScale = Vector3.one * _startScale;
        ChangePosition(newPosition);
    }

    [Button("Change Position Rand")]
    public void ChangePositionRandom()
    {
        Vector3 randomInCircle = Random.insideUnitCircle;
        transform.position = transform.position + randomInCircle * transform.localScale.x/2;
    }

    public void ChangePosition(Vector3 position)
    {
        transform.position = position;
    }

}
