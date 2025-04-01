using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MatrixData
{
    // Все 16 полей матрицы 4x4
    public float m00; public float m10; public float m20; public float m30;
    public float m01; public float m11; public float m21; public float m31;
    public float m02; public float m12; public float m22; public float m32;
    public float m03; public float m13; public float m23; public float m33;

    // Конвертация в Unity Matrix4x4
    public Matrix4x4 ToUnityMatrix()
    {
        return new Matrix4x4(
            new Vector4(m00, m10, m20, m30),
            new Vector4(m01, m11, m21, m31),
            new Vector4(m02, m12, m22, m32),
            new Vector4(m03, m13, m23, m33)
        );
    }

    // Создание из Unity Matrix4x4
    public static MatrixData FromUnityMatrix(Matrix4x4 matrix)
    {
        return new MatrixData
        {
            m00 = matrix.m00,
            m10 = matrix.m10,
            m20 = matrix.m20,
            m30 = matrix.m30,
            m01 = matrix.m01,
            m11 = matrix.m11,
            m21 = matrix.m21,
            m31 = matrix.m31,
            m02 = matrix.m02,
            m12 = matrix.m12,
            m22 = matrix.m22,
            m32 = matrix.m32,
            m03 = matrix.m03,
            m13 = matrix.m13,
            m23 = matrix.m23,
            m33 = matrix.m33
        };
    }
}

// Класс-обертка для десериализации массива матриц
[System.Serializable]
public class MatrixDataArray
{
    public List<MatrixData> matrices;

    // Метод для удобной загрузки из JSON
    public static MatrixDataArray FromJson(string json)
    {
        // Обработка случая, когда JSON является массивом
        if (json.Trim().StartsWith("["))
        {
            json = "{\"matrices\":" + json + "}";
        }

        return JsonUtility.FromJson<MatrixDataArray>(json);
    }

    // Метод для получения массива Matrix4x4
    public Matrix4x4[] ToUnityMatrixArray()
    {
        Matrix4x4[] result = new Matrix4x4[matrices.Count];
        for (int i = 0; i < matrices.Count; i++)
        {
            result[i] = matrices[i].ToUnityMatrix();
        }
        return result;
    }
}