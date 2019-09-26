using UnityEngine;

public class TerrainEditor : MonoBehaviour
{
    private bool addTerrain = true;
    public float Force = 5f;
    public float Range = 1f;

    private float maxReachDistance = 100f;

    private AnimationCurve forceOverDistance = AnimationCurve.Constant(0, 1, 1);

    public MeshGenerator world;
    private Transform playerCamera;
    private float pointSpacing;
    private int numPoints;
    Vector3 worldBounds;



    public DensityModifier Modifier;




    Chunk[] _initChunks;
    ComputeBuffer pointsBuffer;

    private void Start()
    {
        _initChunks = new Chunk[8];
        numPoints = 50 * 50 * 50;
        pointSpacing = world.boundsSize / (world.numPointsPerAxis - 1);
        pointsBuffer = new ComputeBuffer(numPoints, sizeof(float) * 4);

        worldBounds = new Vector3(world.numChunks.x, world.numChunks.y, world.numChunks.z) * world.boundsSize;
    }

    private void Update()
    {
        TryEditTerrain();
    }

    private void TryEditTerrain()
    {
        if (Force <= 0 || Range <= 0)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastToTerrain(addTerrain);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RaycastToTerrain(!addTerrain);
        }
    }

    private void RaycastToTerrain(bool addTerrain)
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 hitPoint = hit.point;
            Collider[] hits = Physics.OverlapSphere(hitPoint, Range);
            for (int i = 0; i < hits.Length; i++)
            {
                Chunk c = hits[i].gameObject.GetComponent<Chunk>();
                if (c != null)
                {
                    InitializeComputeShader(c, hitPoint, addTerrain ? Force : -Force);
                    // c.SetDencity(hitPoint, Range, addTerrain ? 5 : -5);
                }
            }

        }
    }
    private void InitializeComputeShader(Chunk c, Vector3 hPoint,float density)
    {
        Modifier.HitPoint = hPoint;
        Modifier.Force = density;
        Modifier.Range = Range;
        pointsBuffer.SetData(c.Points);
        Vector3 centre = world.CentreFromCoord(c.coord);
        Modifier.Generate(pointsBuffer, world.numPointsPerAxis, world.boundsSize, worldBounds, centre, world.offset, pointSpacing);
        pointsBuffer.GetData(c.Points);
        c.InvokeUpdate();
    }
    void OnDestroy()
    {
        if (Application.isPlaying)
        {
            pointsBuffer.Release();
        }
    }
}

