using System;
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
        //data.space.matrices.Sort((a, b) => a.ToUnityMatrix().GetPosition().magnitude.CompareTo(b.ToUnityMatrix().GetPosition().magnitude)); 
        float startTime = Time.unscaledTime;

        Debug.Log("Data sorted by magnitude of position");





        // Поиск данных
        Debug.Log("Data searched started");

        float offset = 1f;
        resultMatrixDataArray.matrices = new List<MatrixData>();
        foreach (var matrix in data.space.matrices)
        {
            var offsetedMatrix = matrix.DoOffset(offset);

            yield return SearchInSpace(offsetedMatrix, data, offset);
            Debug.DrawLine(matrix.ToUnityMatrix().GetPosition(), offsetedMatrix.ToUnityMatrix().GetPosition(), Color.yellow, 1f);

            float percent = (float)data.space.matrices.IndexOf(matrix) / data.space.matrices.Count;
            Debug.Log($"Data searched {percent * 100}%");
        }

        yield return null;


        string result = JsonUtility.ToJson(resultMatrixDataArray);

        System.IO.File.WriteAllText("Assets/Streaming Assets/result.json", result);

        float endTime = Time.unscaledTime;

        Debug.Log($"Data searched finished in {endTime - startTime} seconds");

    }

    [SerializeField] MatrixDataArray resultMatrixDataArray = new MatrixDataArray();
    private IEnumerator SearchInSpace(MatrixData matrix, Data data, float offset)
    {
        // search for the closest matrix in the model
        float minDistance = offset;
        MatrixData closestMatrix = null;

        foreach (var spaceMatrix in data.space.matrices)
        {
            float distance = Vector3.Distance(matrix.ToUnityMatrix().GetPosition(), spaceMatrix.ToUnityMatrix().GetPosition()); // ну тут все печально
            if (distance < minDistance)
            {
                minDistance = distance;
                closestMatrix = spaceMatrix;
                spaceMatrix.linkedCube.GetComponent<Renderer>().material.color = Color.green;
                yield return null;
                Debug.DrawLine(matrix.ToUnityMatrix().GetPosition(), spaceMatrix.ToUnityMatrix().GetPosition(), Color.yellow, 1f);

                resultMatrixDataArray.matrices.Add(spaceMatrix);
            }
            else
            {
                //spaceMatrix.linkedCube.gameObject.SetActive(false);
            }
        }

        yield return null;
        Debug.Log($"Closest matrix to {matrix} is {closestMatrix} with distance {minDistance}");



    }
}
