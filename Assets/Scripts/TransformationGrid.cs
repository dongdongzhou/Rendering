﻿using System.Collections.Generic;
using UnityEngine;

public class TransformationGrid : MonoBehaviour
{
    private List<Transformation> transformations;
    public Transform prefab;
    public int gridResolution = 10;
    private Transform[] grid;
    private Matrix4x4 transformation;

    private void Awake()
    {
        transformations = new List<Transformation>();
        grid = new Transform[gridResolution * gridResolution * gridResolution];
        for (int i = 0, z = 0; z < gridResolution; z++)
        {
            for (int y = 0; y < gridResolution; y++)
            {
                for (int x = 0; x < gridResolution; x++, i++)
                    grid[i] = CreateGridPoint(x, y, z);
            }
        }
    }

    private void Update()
    {
        UpdateTransformation();
        for (int i = 0, z = 0; z < gridResolution; z++)
        {
            for (int y = 0; y < gridResolution; y++)
            {
                for (int x = 0; x < gridResolution; x++, i++)
                    grid[i].localPosition = TransformPoint(x, y, z);
            }
        }
    }

    private void UpdateTransformation()
    {
        GetComponents(transformations);
        if (transformations.Count > 0)
        {
            transformation = transformations[0].Matrix;
            for (int i = 1; i < transformations.Count; i++)
                transformation = transformations[i].Matrix * transformation;
        }
    }

    private Vector3 TransformPoint(int x, int y, int z)
    {
        Vector3 coordinates = GetCoordinates(x, y, z);
      
        return transformation.MultiplyPoint(coordinates);
    }

    private Transform CreateGridPoint(int x, int y, int z)
    {
        Transform point = Instantiate(prefab, transform);
        point.localPosition = GetCoordinates(x, y, z);
        point.GetComponent<MeshRenderer>().material.color = new Color((float) x / gridResolution,
                                                                      (float) y / gridResolution,
                                                                      (float) z / gridResolution);
        return point;
    }

    private Vector3 GetCoordinates(int x, int y, int z) =>
        new Vector3(x - (gridResolution - 1) * 0.5f,
                    y - (gridResolution - 1) * 0.5f,
                    z - (gridResolution - 1) * 0.5f);
}