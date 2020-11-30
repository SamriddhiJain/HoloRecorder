using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static PointCloudSubscriber;

public class PointCloudRenderer : MonoBehaviour
{
    //public PointCloudSubscriber subscriber;

    // Mesh stores the positions and colours of every point in the cloud
    // The renderer and filter are used to display it
    Mesh mesh;
    public MeshRenderer meshRenderer;
    MeshFilter mf;

    // The size, positions and colours of each of the pointcloud
    public float pointSize = 5f;

    [Header("MAKE SURE THESE LISTS ARE MINIMISED OR EDITOR WILL CRASH")]
    private Vector3[] positions = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0) };
    private Color[] colours = new Color[] { new Color(1f, 0f, 0f), new Color(0f, 1f, 0f) };
    //private static PointCloudSubscriber subscriber;

    public Transform offset; // Put any gameobject that faciliatates adjusting the origin of the pointcloud in VR. 
    private static bool showRecording = false;
    private int runningFrame = -1;

    void Start()
    {
        // Give all the required components to the gameObject
        //meshRenderer = gameObject.AddComponent<MeshRenderer>();
        mf = gameObject.AddComponent<MeshFilter>();
        //meshRenderer.material = new Material(Shader.Find("Custom/PointCloudShader"));
        mesh = new Mesh
        {
            // Use 32 bit integer values for the mesh, allows for stupid amount of vertices (2,147,483,647 I think?)
            indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
        };
        mf.mesh = mesh;

        transform.position = offset.position;
        transform.rotation = offset.rotation;
        //subscriber = new PointCloudSubscriber();
    }

    void UpdateMesh()
    {
        runningFrame++;

        positions = PointCloudSubscriber.GetPCL(runningFrame);

        Debug.Log("Renderer: points received");
        if (positions == null || positions.Length == 0)
        {
            Debug.Log("Empty array");
            return;
        }
        int size = positions.Length;

        colours = new Color[size];
        Debug.Log(size);

        for (int n = 0; n < size; n++)
        {
            //pcl[n] = new Vector3(points[n * 3], points[n * 3 + 1], points[n * 3 + 2]);
            colours[n] = new Color(1, 1, 1, 1);
        }

        mf.mesh.Clear();

        Mesh mesh2 = new Mesh
        {
            // Use 32 bit integer values for the mesh, allows for stupid amount of vertices (2,147,483,647 I think?)
            indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
        };
        mesh2.vertices = positions;
        mesh2.colors = colours;
        int[] indices = new int[positions.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            indices[i] = i;
        }
       
        mesh2.SetIndices(indices, MeshTopology.Points, 0);
        mesh2.RecalculateNormals();
        mf.mesh = mesh2;
        Debug.Log("Renderer: points updated");
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = offset.position;
        //transform.rotation = offset.rotation;
        if (showRecording)
        {
            UpdateMesh();
        }
    }

    public static void toggleView()
    {
        showRecording = !showRecording;
    }
}