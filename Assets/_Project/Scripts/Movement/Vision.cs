using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Vision : MonoBehaviour
{
    public GameObject model;
    public float Fov;
    public float VisionRange;
    public int VerticeNum = 3;
    public LineRenderer Line;
    [SerializeField]private LayerMask _foodLayer;

    //Events
    public UnityEvent<GameObject> OnEnemySpoted;
    public UnityEvent<GameObject> OnCollectableSpoted;


    Vector3[] points;
    private PolygonCollider2D visionCollider2D;



    // Start is called before the first frame update
    void Start()
    {
        points = new Vector3[VerticeNum + 1];
        visionCollider2D = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        VisionLineRenderer();
        VisionDetection();
    }


    private void VisionDetection()
    {
        Vector2[] newPoints = new Vector2[VerticeNum + 1];
        for (int i = 0; i < points.Length; i++)
        {
            newPoints[i] = points[i];
        }
        visionCollider2D.points = newPoints;
    }

    private void VisionLineRenderer()
    {
        Line.positionCount = VerticeNum + 1;
        int iter = VerticeNum - 1;
        float baseAngle = Mathf.Deg2Rad * (model.transform.eulerAngles.z + 90);
        points[0] = Vector3.zero;
        for (int i = 0; i < iter + 1; i++)
        {
            float angle = Mathf.Deg2Rad * (Fov / iter * i - Fov / 2);
            Vector3 angleVect = new Vector3(Mathf.Cos(baseAngle + angle), Mathf.Sin(baseAngle + angle)) * VisionRange;
            points[i + 1] = angleVect;
        }

        Line.SetPositions(points);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Collectable>(out Collectable collectable))
        {
            OnCollectableSpoted?.Invoke(collision.gameObject);
        }
        if (collision.TryGetComponent<Bestiole>(out Bestiole bestiole))
        {
            OnEnemySpoted?.Invoke(collision.gameObject);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        int iter = VerticeNum-1;
        float baseAngle = Mathf.Deg2Rad * (model.transform.eulerAngles.z+90);
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