using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataVisualiserService : MonoBehaviour
{

    [SerializeField] DataLoadService dataLoadService;

    public event System.Action<Data> OnDataVisualised;

    private void Awake()
    {
        dataLoadService.OnDataLoaded += VisualiseData;
    }

    private void VisualiseData(Data data)
    {
        StartCoroutine(VisualiseDataRoutine(data));
    }


    int fps;
    IEnumerator VisualiseDataRoutine(Data data)
    {
        // Визуализация данных
        Debug.Log("Data visualised");
        // instantiate a cube per matrix

        GameObject spaceRoot = new GameObject("SpaceRoot");
        foreach (var matrix in data.space.matrices)
        {
            fps = (int)(1f / Time.deltaTime);

            VisualiseCube(matrix, Color.blue).transform.SetParent(spaceRoot.transform);


            if (fps < 3)
            {
                yield return null;
            }

        }

        GameObject modelRoot = new GameObject("ModelRoot");

        foreach (var matrix in data.model.matrices)
        {
            fps = (int)(1f / Time.deltaTime);
            VisualiseCube(matrix, Color.red).transform.SetParent(modelRoot.transform);
            if (fps < 3)
            {
                yield return null;
            }
        }


        OnDataVisualised?.Invoke(data);

    }


    public static GameObject VisualiseCube(MatrixData matrix, Color color)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = matrix.ToUnityMatrix().GetPosition();
        cube.transform.rotation = matrix.ToUnityMatrix().rotation;
        cube.GetComponent<Renderer>().material.color = color;
        cube.name = "Cube_" + matrix.ToString();
        matrix.linkedCube = cube;
        return cube;
    }
}
