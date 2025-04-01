using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        //data.space.matrices.Sort((a, b) => a.ToUnityMatrix().GetPosition().magnitude.CompareTo(b.ToUnityMatrix().GetPosition().magnitude)); 
        float startTime = Time.unscaledTime;

        Debug.Log("Data sorted by magnitude of position");





        // Поиск данных
        Debug.Log("Data searched started");

        float offsetMin = 1f;
        float offsetMax = 2f;
        resultMatrixDataArray.matrices = new List<MatrixData>();
        for (int i = 0; i < data.model.matrices.Count; i++)
        {
            MatrixData matrix = data.model.matrices[i];
            MatrixData offsetedMatrix = matrix.DoOffset(offsetMin);

            yield return SearchInSpace(offsetedMatrix, data, offsetMin, offsetMax);


            float percent = (float)(i / data.model.matrices.Count);
            Debug.Log($"Data searched {percent * 100}%");
        }

        yield return null;


        string result = JsonUtility.ToJson(resultMatrixDataArray);

        System.IO.File.WriteAllText("Assets/Streaming Assets/result.json", result);

        float endTime = Time.unscaledTime;

        Debug.Log($"Data searched finished in {endTime - startTime} seconds");

    }

    [SerializeField] MatrixDataArray resultMatrixDataArray = new MatrixDataArray();
    private IEnumerator SearchInSpace(MatrixData targetMatrix, Data data, float offsetMin, float offsetMax)
    {
        MatrixData closestMatrix = null;

        foreach (var spaceMatrix in data.space.matrices)
        {
            float distance = Vector3.Distance(targetMatrix.ToUnityMatrix().GetPosition(), spaceMatrix.ToUnityMatrix().GetPosition()); // ну тут все печально просто срравниниваю по дистанции



            // compare two matrix 

            bool distanceCondition = distance >= offsetMin && distance <= offsetMax;
            if (distanceCondition)
            {
                closestMatrix = spaceMatrix;
                closestMatrix.linkedCube.GetComponent<Renderer>().material.color = Color.green;

                Debug.DrawLine(targetMatrix.ToUnityMatrix().GetPosition(), closestMatrix.ToUnityMatrix().GetPosition(), Color.yellow, 1f);

                resultMatrixDataArray.matrices.Add(closestMatrix);
            }
            else
            {
                continue;
            }
        }


        yield return null;
    }
}
