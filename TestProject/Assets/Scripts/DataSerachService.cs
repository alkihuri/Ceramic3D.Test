﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class DataSerachService : MonoBehaviour
{

    [SerializeField] DataVisualiserService dataVisualiserService;

    private void Awake()
    {
        dataVisualiserService.OnDataVisualised += SearchData;
    }

    private void SearchData(Data data)
    {

        StartCoroutine(SearchDataRoutine(data));
    }

    IEnumerator SearchDataRoutine(Data data)
    {

        // sort data by magnitude of position 
        data.space.matrices.Sort((a, b) => a.ToUnityMatrix().GetPosition().magnitude.CompareTo(b.ToUnityMatrix().GetPosition().magnitude));
        yield return null;

        Debug.Log("Data sorted by magnitude of position");





        // Поиск данных
        Debug.Log("Data searched started");

        float offset = 0.1f;

        foreach (var matrix in data.space.matrices)
        {
            var offsetedMatrix = matrix.DoOffset(offset);

            yield return SearchInSpace(offsetedMatrix, data);
            Debug.DrawLine(matrix.ToUnityMatrix().GetPosition(), offsetedMatrix.ToUnityMatrix().GetPosition(), Color.yellow, 1f);
            yield return null;
        }


    }

    private IEnumerator SearchInSpace(MatrixData matrix, Data data)
    {
        // search for the closest matrix in the model
        float minDistance = float.MaxValue;
        MatrixData closestMatrix = null;

        foreach (var modelMatrix in data.model.matrices)
        {
            float distance = Vector3.Distance(matrix.ToUnityMatrix().GetPosition(), modelMatrix.ToUnityMatrix().GetPosition());
            if (distance < minDistance)
            {
                minDistance = distance;
                closestMatrix = modelMatrix;
                modelMatrix.linkedCube.GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                modelMatrix.linkedCube.SetActive(false);    
                yield return null; 
            }
        }

        Debug.Log($"Closest matrix to {matrix} is {closestMatrix} with distance {minDistance}");

    }
}
