using System.Collections.Generic;
using UnityEngine;


struct RayPoint
{
    public Vector2 position;
    public float angle;
};
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CreateFOVCone : MonoBehaviour
{
    [SerializeField] LayerMask obstaclesLayer;
    [SerializeField] bool debug = false;
    List<PolygonCollider2D> obstacles = new List<PolygonCollider2D>();
    List<RayPoint> rayPoints = new List<RayPoint>();

    // TODO: ustawic prawidlowo kierunek widzenia gracza, zasieg i kat widzenia
    Vector2 playerViewDir = Vector2.right;
    [Range(0.1f, 20f)]
    [SerializeField] float viewRange = 8f;
    float rayCastRange;
    [Range(0.1f, 120f)]
    [SerializeField] float playerFOV = 90f;
    [Range(0.001f, 0.1f)]
    [SerializeField] float epsilon = 0.01f;

    // MeshVariables
    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;

    Renderer rend;
    MaterialPropertyBlock mpb;

    private void Start()
    {
        playerViewDir = -transform.up;

        rayCastRange = viewRange / Mathf.Cos(playerFOV * 0.5f * Mathf.Deg2Rad);
        GetComponent<CircleCollider2D>().radius = rayCastRange;
        vertices = new List<Vector3>();
        triangles = new List<int>();

        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            rayCastRange = viewRange / Mathf.Cos(playerFOV * 0.5f * Mathf.Deg2Rad);
            GetComponent<CircleCollider2D>().radius = rayCastRange;
        }
    }

    // Detect obstacles entering and exiting the player's view range
    // For this obstacles we calculate ConeMesh
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            PolygonCollider2D obstacle = collision.GetComponent<PolygonCollider2D>();
            if (obstacle != null && !obstacles.Contains(obstacle))
            {
                obstacles.Add(obstacle);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            PolygonCollider2D obstacle = collision.GetComponent<PolygonCollider2D>();
            if (obstacle != null && obstacles.Contains(obstacle))
            {
                obstacles.Remove(obstacle);
            }
        }
    }


    private void LateUpdate()
    {
        playerViewDir = -transform.up;
        rayPoints.Clear();

        foreach (PolygonCollider2D obstacle in obstacles)
        {
            Vector2[] points = obstacle.points;
            for (int i = 0; i < points.Length; i++)
            {
                // Calculate world pos and angle to each obstacle point
                Vector2 worldPoint = obstacle.transform.TransformPoint(points[i]);
                Vector2 fromPlayerToObstacle = worldPoint - (Vector2)transform.position;
                float angle = VectorMath.AngleBetween(playerViewDir, fromPlayerToObstacle);
                if (Mathf.Abs(angle) > playerFOV * 0.5f || Vector2.Distance(transform.position, worldPoint) > rayCastRange)
                    continue;
                RayPoint rp = new RayPoint();
                rp.position = worldPoint;
                rp.angle = angle;
                rayPoints.Add(rp);


                // Create two additional rays slightly offset to the left and right
                RayPoint rp1 = new RayPoint();
                Vector2 rp1Dir = VectorMath.Rotate(fromPlayerToObstacle, epsilon);
                float rp1Angle = VectorMath.AngleBetween(playerViewDir, rp1Dir);
                rp1.position = (Vector2)transform.position + rp1Dir;
                rp1.angle = rp1Angle;
                rayPoints.Add(rp1);

                RayPoint rp2 = new RayPoint();
                Vector2 rp2Dir = VectorMath.Rotate(fromPlayerToObstacle, -epsilon);
                float rp2Angle = VectorMath.AngleBetween(playerViewDir, rp2Dir);
                rp2.position = (Vector2)transform.position + rp2Dir;
                rp2.angle = rp2Angle;
                rayPoints.Add(rp2);

            }
        }

        // Add cone edge points
        Vector2 coneLeftDir = VectorMath.Rotate(playerViewDir, playerFOV * 0.5f);
        Vector2 coneLeftPoint = (Vector2)transform.position + coneLeftDir.normalized * rayCastRange;
        float coneLeftAngle = playerFOV * 0.5f;
        RayPoint coneLeftRayPoint = new RayPoint();
        coneLeftRayPoint.position = coneLeftPoint;
        coneLeftRayPoint.angle = coneLeftAngle;
        rayPoints.Add(coneLeftRayPoint);

        Vector2 coneRightDir = VectorMath.Rotate(playerViewDir, -playerFOV * 0.5f);
        Vector2 coneRightPoint = (Vector2)transform.position + coneRightDir.normalized * rayCastRange;
        float coneRightAngle = -playerFOV * 0.5f;
        RayPoint coneRightRayPoint = new RayPoint();
        coneRightRayPoint.position = coneRightPoint;
        coneRightRayPoint.angle = coneRightAngle;
        rayPoints.Add(coneRightRayPoint);

        // Sort ray points by angle
        rayPoints.Sort((a, b) => a.angle.CompareTo(b.angle));


        if (mesh)
            mesh.Clear();

        mesh = new Mesh();
        vertices.Clear();
        triangles.Clear();

        // player position is the first vertex
        vertices.Add(Vector3.zero);
        int vertCount = 0;

        foreach (RayPoint rp in rayPoints)
        {
            if (debug)
                Debug.DrawLine(transform.position, rp.position, UnityEngine.Color.green);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, (rp.position - (Vector2)transform.position).normalized, rayCastRange, obstaclesLayer);
            Vector3 vert = hit ? hit.point :
                transform.position + (((Vector3)rp.position - transform.position).normalized * rayCastRange);
            vert = transform.InverseTransformPoint(vert);
            vertices.Add(vert);
            //print(hit.point);
            vertCount++;
            triangles.Add(0);
            triangles.Add(vertCount);
            triangles.Add(vertCount - 1);
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);

        //mesh.RecalculateBounds();
        mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 1000f);
        GetComponent<MeshFilter>().mesh = mesh;

        mpb.Clear();
        mpb.SetFloat("_Fov", playerFOV);
        mpb.SetFloat("_ViewRange", viewRange);
        rend.SetPropertyBlock(mpb);
    }



}
