using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;


public class PointCloudSubscriber: MonoBehaviour
{
    //private byte[] byteArray;
    //private bool isMessageReceived = false;
    //bool readyToProcessMessage = true;
    //private int size;

    private static Vector3[] pcl;
    private static Color[] pcl_color;
    private static int count = -1;

    private static Dictionary<int, Vector3[]> Pairs = new Dictionary<int, Vector3[]>();
    //int width;
    //int height;
    //int row_step;
    //int point_step;

    public static void ReceiveMessage(int frame, float[] points)
    {
        int size = points.Length / 3;
        pcl = new Vector3[size];
        //pcl_color = new Color[size];
        Debug.Log(size);

        for (int n = 0; n < size; n++)
        {
            pcl[n] = new Vector3(points[n*3], points[n * 3 +1], points[n * 3 +2]);
            //pcl_color[n] = new Color(1, 1, 1);
        }
        //isMessageReceived = true;
        Pairs[frame] = pcl;
        Debug.Log("Subscriber: point cloud data received");
    }

    //void PointCloudRendering()
    //{
    //    pcl = new Vector3[size];
    //    pcl_color = new Color[size];

    //    int x_posi;
    //    int y_posi;
    //    int z_posi;

    //    float x;
    //    float y;
    //    float z;

    //    int rgb_posi;
    //    int rgb_max = 255;

    //    float r;
    //    float g;
    //    float b;

    //    //この部分でbyte型をfloatに変換         
    //    for (int n = 0; n < size; n++)
    //    {
    //        x_posi = n * point_step + 0;
    //        y_posi = n * point_step + 4;
    //        z_posi = n * point_step + 8;

    //        x = BitConverter.ToSingle(byteArray, x_posi);
    //        y = BitConverter.ToSingle(byteArray, y_posi);
    //        z = BitConverter.ToSingle(byteArray, z_posi);


    //        rgb_posi = n * point_step + 16;

    //        b = byteArray[rgb_posi + 0];
    //        g = byteArray[rgb_posi + 1];
    //        r = byteArray[rgb_posi + 2];

    //        r = r / rgb_max;
    //        g = g / rgb_max;
    //        b = b / rgb_max;

    //        pcl[n] = new Vector3(x, z, y);
    //        pcl_color[n] = new Color(r, g, b);


    //    }
    //}

    public static Vector3[] GetPCL()
    {
        if(count < Pairs.Count)
        {
            count++;
            return Pairs[count];
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

    public static bool Ready()
    {
        return Pairs.Count >= 100;
    }
}