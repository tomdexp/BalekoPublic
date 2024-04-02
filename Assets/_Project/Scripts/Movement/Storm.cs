using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storm : MonoBehaviour
{
    public StormData Data;

    private float _startScale;


    // Start is called before the first frame update
    void Start()
    {
        _startScale = transform.localScale.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale -= Vector3.one * Data.SizeDecreaseSpeed * Time.deltaTime;
        transform.localScale = Vector3.one * Mathf.Clamp(transform.localScale.x, Data.MinScale, _startScale);
    }

    private void OnDrawGizmosSelected()
    {
    }

}
