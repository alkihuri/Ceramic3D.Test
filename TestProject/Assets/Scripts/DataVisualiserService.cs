using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataVisualiserService : MonoBehaviour
{

    [SerializeField] DataLoadService dataLoadService;

    private void Awake()
    {
        dataLoadService.OnDataLoaded += VisualiseData;
    }
    
    private void VisualiseData(Data data)
    { 
        // Визуализация данных
        Debug.Log("Data visualised");
        // instantiate a cube per matrix
        foreach (var matrix in data.space.matrices)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(matrix.m03, matrix.m13, matrix.m23);
            cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }

        foreach (var matrix in data.model.matrices)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(matrix.m03, matrix.m13, matrix.m23);
            // red color 
            cube.GetComponent<Renderer>().material.color = Color.red;   
            cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
}
