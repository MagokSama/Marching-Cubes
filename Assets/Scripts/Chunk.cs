using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Vector3Int coord;

    public int ID;
    [HideInInspector]
    public Vector4[] Points;


    private bool _saved = false;
    public bool Saved
    {
        get => _saved;
        set
        {

            _saved = value;
        }
    }



    public int PointsCount;
    [HideInInspector]
    public Mesh mesh;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    MeshCollider meshCollider;
    bool generateCollider;


    public float PointsBoundsRatio;
    public void DestroyOrDisable()
    {
        if (Application.isPlaying)
        {
            mesh.Clear();
            gameObject.SetActive(false);
        }
        else
        {
            DestroyImmediate(gameObject, false);
        }
    }

    // Add components/get references in case lost (references can be lost when working in the editor)
    public void SetUp(Material mat, bool generateCollider)
    {
        this.generateCollider = generateCollider;

        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        if (meshCollider == null && generateCollider)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }
        if (meshCollider != null && !generateCollider)
        {
            DestroyImmediate(meshCollider);
        }

        mesh = meshFilter.sharedMesh;
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            meshFilter.sharedMesh = mesh;
        }

        if (generateCollider)
        {
            if (meshCollider.sharedMesh == null)
            {
                meshCollider.sharedMesh = mesh;

            }
            // force update
            meshCollider.enabled = false;
            meshCollider.enabled = true;
        }

        meshRenderer.material = mat;
    }


    public void SetDencity(Vector3 position, float range, float den)
    {
        for (int i = 0; i < PointsCount; i++)
        {
            if (Vector3.Distance(Points[i], position) <= range)
            {
                Points[i].w += den;
                UpdatedCells++;
            }

        }
        InvokeUpdate();
        //OnDensityUpdate.Invoke(this);
    }



    public void InvokeUpdate()
    {
        OnDensityUpdate.Invoke(this);
    }
    public int UpdatedCells;
    public delegate void NewMesh(Chunk owner);
    public event NewMesh OnDensityUpdate;

}