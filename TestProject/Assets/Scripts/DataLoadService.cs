using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Data
{
    public MatrixDataArray space;
    public MatrixDataArray model;

    public Data(MatrixDataArray spaceMatrix, MatrixDataArray modelMatrix)
    {
        space = spaceMatrix;
        model = modelMatrix;
    }
}

public class DataLoadService : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private MatrixDataArray spaceDebug;
    [SerializeField] private MatrixDataArray modelDebug;


    public event Action<Data> OnDataLoaded;



    private void Awake()
    {
        LoadData();
    }


    [ContextMenu("Load")]
    public void LoadData()
    {
        string spacepath = "Assets/Streaming Assets/space.json";
        string modelpath = "Assets/Streaming Assets/model.json";


        if (!System.IO.File.Exists(spacepath) || !System.IO.File.Exists(modelpath))
        {
            Debug.LogError("Data not found");
            return;
        }

        string spacejson = System.IO.File.ReadAllText(spacepath);
        string modeljson = System.IO.File.ReadAllText(modelpath);


        Data data = new Data(MatrixDataArray.FromJson(spacejson), MatrixDataArray.FromJson(modeljson));

        spaceDebug = data.space;
        modelDebug = data.model;


        OnDataLoaded?.Invoke(data);

        Debug.Log("Data loaded");

    }
}
