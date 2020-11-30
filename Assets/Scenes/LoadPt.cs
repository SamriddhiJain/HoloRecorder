using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System.IO;

public class LoadPt : MonoBehaviour
{
    //Mesh mesh;
    public MeshRenderer meshRenderer;
    MeshFilter mf;
    Vector3[] pcl;
    Color[] pcl_color;
    // The size, positions and colours of each of the pointcloud
    public float pointSize = 1f;
    private int active = 0;
    private bool check = true;

    public Transform offset;

    // Start is called before the first frame update
    void Start()
    {
        //FileStream file = File.Open(Application.dataPath + "/pointCloud_102.dat", FileMode.Open);
        //BinaryFormatter binary = new BinaryFormatter();
        byte[] bHex = File.ReadAllBytes(Application.streamingAssetsPath + "/pointCloud_102.dat");
        //byte[] bHex = Resources.Load("pointCloud_102.dat") as byte[];
        //var points = bHex.Select(b => (float)Convert.ToDouble(b)).ToArray();
        //float[] points = (float [])binary.Deserialize(file);
        //file.Close();
        float[] points = new float[bHex.Length/4];
        Buffer.BlockCopy(bHex, 0, points, 0, points.Length);
        
        int size = points.Length / 3;
        Debug.Log(size);
        pcl = new Vector3[size];
        pcl_color = new Color[size];

        for (int n = 0; n < size; n++)
        {
            pcl[n] = new Vector3(points[n * 3], points[n * 3 + 1], points[n * 3 + 2]);
            pcl_color[n] = new Color(1, 0, 0);
        }

        //meshRenderer = gameObject.AddComponent<MeshRenderer>();
        mf = gameObject.AddComponent<MeshFilter>();
        //meshRenderer.material = new Material(Shader.Find("Custom/PointCloudShader"));
        Mesh mesh = new Mesh();
        //{
        //    // Use 32 bit integer values for the mesh, allows for stupid amount of vertices (2,147,483,647 I think?)
        //    indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
        //};
        mesh.vertices = pcl;
        mesh.colors = pcl_color;
        int[] indices = new int[pcl.Length];

        for (int i = 0; i < pcl.Length; i++)
        {
            indices[i] = i;
        }

        mesh.SetIndices(indices, MeshTopology.Points, 0);
        //mesh.RecalculateNormals();
        mf.mesh = mesh;

        transform.position = offset.position;
        transform.rotation = offset.rotation;

        var normal = -Camera.main.transform.forward;
        var position = transform.position;
        UnityEngine.XR.WSA.HolographicSettings.SetFocusPointForFrame(position, normal);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    MeshFilter mf = gameObject.GetComponent<MeshFilter>();
    //    if (active % 10 == 0)
    //    {
    //        mf.mesh.Clear();
    //        if (check)
    //        {
    //            byte[] bHex = File.ReadAllBytes(Application.streamingAssetsPath + "/pointCloud_102.dat");
    //            //var points = bHex.Select(b => (float)Convert.ToDouble(b)).ToArray();
    //            //float[] points = (float [])binary.Deserialize(file);
    //            //file.Close();
    //            float[] points = new float[bHex.Length / 4];
    //            Buffer.BlockCopy(bHex, 0, points, 0, points.Length);

    //            int size = points.Length / 3;
    //            Debug.Log(size);
    //            pcl = new Vector3[size];
    //            pcl_color = new Color[size];

    //            for (int n = 0; n < size; n++)
    //            {
    //                pcl[n] = new Vector3(points[n * 3], points[n * 3 + 1], points[n * 3 + 2]);
    //                pcl_color[n] = new Color(1, 1, 1);
    //            }
    //        }
    //        else
    //        {
    //            byte[] bHex = File.ReadAllBytes(Application.streamingAssetsPath + "/pointCloud_158.dat");
    //            //var points = bHex.Select(b => (float)Convert.ToDouble(b)).ToArray();
    //            //float[] points = (float [])binary.Deserialize(file);
    //            //file.Close();
    //            float[] points = new float[bHex.Length / 4];
    //            Buffer.BlockCopy(bHex, 0, points, 0, points.Length);

    //            int size = points.Length / 3;
    //            Debug.Log(size);
    //            pcl = new Vector3[size];
    //            pcl_color = new Color[size];

    //            for (int n = 0; n < size; n++)
    //            {
    //                pcl[n] = new Vector3(points[n * 3], points[n * 3 + 1], points[n * 3 + 2]);
    //                pcl_color[n] = new Color(1, 1, 1);
    //            }
    //        }
    //        Mesh mesh2 = new Mesh
    //        {
    //            // Use 32 bit integer values for the mesh, allows for stupid amount of vertices (2,147,483,647 I think?)
    //            indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
    //        };
    //        mesh2.vertices = pcl;
    //        mesh2.colors = pcl_color;
    //        int[] indices = new int[pcl.Length];

    //        for (int i = 0; i < pcl.Length; i++)
    //        {
    //            indices[i] = i;
    //        }

    //        mesh2.SetIndices(indices, MeshTopology.Points, 0);
    //        //mesh.RecalculateNormals();
    //        mf.mesh = mesh2;

    //        check = !check;
    //    }
    //    active++;
    //}
}
