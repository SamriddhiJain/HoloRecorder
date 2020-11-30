using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit;


public class PointCloudSubscriber: MonoBehaviour
{
    //private byte[] byteArray;
    //private bool isMessageReceived = false;
    //bool readyToProcessMessage = true;
    //private int size;

    private static Vector3[] pcl;
    private static Color[] pcl_color;
    private static int count = -1;
    private static int frame = -1;

    private static float timeSinceStartOfRecording = 0;
    private static Dictionary<int, float> timesteps = new Dictionary<int, float>();
    private static Dictionary<int, Vector3[]> Pairs = new Dictionary<int, Vector3[]>();
    //private static Dictionary<int, Vector3> leftPalmArray;
    //private static Dictionary<int, Vector3> rightPalmArray;

    public static void startTimer()
    {
        timeSinceStartOfRecording = Time.time;
    }

    public static void ReceiveMessage(float[] points)
    {
        frame++;
        float timeElapsed = Time.time - timeSinceStartOfRecording;
        timesteps[frame] = timeElapsed;

        int size = points.Length / 3;
        Vector3[] pcl_temp = new Vector3[size];

        Debug.Log(size);

        // get mrtk joints
        Vector3 leftPalm = new Vector3();
        Vector3 rightPalm = new Vector3();
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Left, out MixedRealityPose leftPose))
        {
            leftPalm = leftPose.Position;
        }

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Right, out MixedRealityPose rightPose))
        {
            rightPalm = rightPose.Position;
        }

        // append null palm coordinates and check later
        //leftPalmArray[frame_count] = leftPalm;
        //rightPalmArray[frame_count] = rightPalm;
        if (leftPalm == null)
        {
            leftPalm = new Vector3(0, 0, 0);
        }

        if (rightPalm == null)
        {
            rightPalm = new Vector3(0, 0, 0);
        }

        float THRESH = 0.2F;
        int n_points = 0;
        bool[] filter = new bool[size];
        for (int n = 0; n < size; n++)
        {
            float min_base = float.PositiveInfinity;
            
            Vector3 pt = new Vector3(points[n*3], points[n * 3 +1], points[n * 3 +2]);
            pcl_temp[n] = pt;

            float dist1 = Vector3.Distance(pt, leftPalm);
            float dist2 = Vector3.Distance(pt, rightPalm);
            float m = Math.Min(min_base, Math.Min(dist1, dist2));

            if(m < THRESH)
            {
                n_points++;
                filter[n] = true;
            }
            else
            {
                filter[n] = false;
            }

        }

        pcl = new Vector3[n_points];
        int k = 0;
        for (int n = 0; n < size; n++)
        {
            if (filter[n])
            {
                pcl[k] = pcl_temp[n];
                k++;
            }
        }

        Pairs[frame] = pcl;
        Debug.Log("Subscriber: point cloud data received");
    }

    public static Vector3[] GetPCL(int frame)
    {
        if(frame >= 0 && frame < Pairs.Count)
        {
            if(Pairs.ContainsKey(frame))
                return Pairs[frame];
        }
        return new Vector3[0];
    }

    public static Color[] GetPCLColor(int frame)
    {
        if (frame >= 0 && frame < Pairs.Count)
        {
            int size = Pairs[frame].Length;
            //pcl = new Vector3[size];
            pcl_color = new Color[size];
            Debug.Log(size);

            for (int n = 0; n < size; n++)
            {
                //pcl[n] = new Vector3(points[n * 3], points[n * 3 + 1], points[n * 3 + 2]);
                pcl_color[n] = new Color(1, 1, 1);
            }

            return pcl_color;
        }
        return new Color[0];
    }

    public static Dictionary<int, float> GetTimeSteps()
    {
        return timesteps;
    }

    public static bool Ready()
    {
        return Pairs.Count >= 100;
    }
}