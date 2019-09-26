using System.Collections.Generic;
using UnityEngine;

public class DensityModifier : DensityGenerator
{
    [HideInInspector]
    public float Force;
    [HideInInspector]
    public float Range;
    public Vector3 HitPoint;
    public override ComputeBuffer Generate(ComputeBuffer pointsBuffer, int numPointsPerAxis, float boundsSize, Vector3 worldBounds, Vector3 centre, Vector3 offset, float spacing)
    {
        buffersToRelease = new List<ComputeBuffer>();

        // Noise parameters

        densityShader.SetFloat("force", Force);
        densityShader.SetFloat("range", Range);
        densityShader.SetVector("hitPoint", HitPoint);

        return base.Generate(pointsBuffer, numPointsPerAxis, boundsSize, worldBounds, centre, offset, spacing);
    }
}

