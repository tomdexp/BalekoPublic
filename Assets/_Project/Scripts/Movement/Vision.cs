using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
    public GameObject model;
    public float Fov;
    public float VisionRange;
    public int VerticeNum = 3;
    public LineRenderer Line;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Line.positionCount = VerticeNum;
        int iter = 15;
        float baseAngle = Mathf.Deg2Rad * model.transform.eulerAngles.z;
        Vector3[] points = new Vector3[iter + 3];

        points[0] = Vector3.zero;
        for (int i = 0; i <= iter; i++)
        {
            float angle = Mathf.Deg2Rad * (Fov / iter * i - Fov / 2);
            Vector3 angleVect = new Vector3(Mathf.Cos(baseAngle + angle), Mathf.Sin(baseAngle + angle)) * VisionRange;
            points[i + 1] = angleVect;
        }
        points[iter + 2] = Vector3.zero;

        Line.SetPositions(points);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        int iter = 15;
        float baseAngle = Mathf.Deg2Rad * model.transform.eulerAngles.z;
        Vector3[] points = new Vector3[iter + 3];

        points[0] = transform.position;
        for (int i = 0; i <= iter; i++)
        {
            float angle = Mathf.Deg2Rad * (Fov/iter*i - Fov/2);
            Vector3 angleVect = new Vector3(Mathf.Cos(baseAngle+ angle), Mathf.Sin(baseAngle+ angle)) * VisionRange;
            points[i+1]= transform.position + angleVect;
        }
        points[iter+2] = transform.position;

        Gizmos.DrawLineStrip(points, true);
    }

}