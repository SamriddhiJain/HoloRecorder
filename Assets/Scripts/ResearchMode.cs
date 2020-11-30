using UnityEngine;
using System;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System.IO;
using static PointCloudSubscriber;

#if ENABLE_WINMD_SUPPORT
using HL2UnityPlugin;
#endif

public class ResearchMode : MonoBehaviour
{
#if ENABLE_WINMD_SUPPORT
    HL2ResearchMode researchMode;
#endif

    [SerializeField]
    GameObject previewPlane = null;
    [SerializeField]
    Text text;
    //public Button yourButton;
    private Material mediaMaterial = null;
    private Texture2D mediaTexture = null;
    private byte[] frameData = null;
    BinaryWriter bw;
    //private static PointCloudSubscriber subscriber;

    void Start()
    {
#if ENABLE_WINMD_SUPPORT
        researchMode = new HL2ResearchMode();
        researchMode.InitializeDepthSensor();
        
#endif
        //StartPreviewEvent();
        //mediaMaterial = previewPlane.GetComponent<MeshRenderer>().material;
        //Button btn = yourButton.GetComponent<Button>();
        //btn.onClick.AddListener(StartDepthSensingLoopEvent);
        //subscriber = new PointCloudSubscriber();
        StartDepthSensingLoopEvent();
        //subscriber = new PointCloudSubscriber();
    }

    #region Button Events
    public void PrintDepthEvent()
    {
#if ENABLE_WINMD_SUPPORT
        text.text = researchMode.GetCenterDepth().ToString();
#endif
    }

    public void PrintDepthExtrinsicsEvent()
    {
#if ENABLE_WINMD_SUPPORT
        text.text = researchMode.PrintDepthExtrinsics();
#endif
    }

    public void StartDepthSensingLoopEvent()
    {
#if ENABLE_WINMD_SUPPORT
        Debug.Log("depth sensor loop start signal sent");
        researchMode.StartDepthSensorLoop();
#endif
    }

    public void StopSensorLoopEvent()
    {
#if ENABLE_WINMD_SUPPORT
        researchMode.StopAllSensorDevice();
#endif
    }

    static bool startRealtimePreview = false;
    public static void StartPreviewEvent()
    {
        startRealtimePreview = !startRealtimePreview;
    }
    #endregion

    private void LateUpdate()
    {
#if ENABLE_WINMD_SUPPORT
        // update depth map texture
        Debug.Log("last update");
        if (startRealtimePreview && researchMode.DepthMapTextureUpdated())
        {
            if (!mediaTexture)
            {
                mediaTexture = new Texture2D(512, 512, TextureFormat.Alpha8, false);
                mediaMaterial.mainTexture = mediaTexture;
            }

            // byte[] frameTexture = researchMode.GetDepthMapTextureBuffer();
            // ushort[] depth = researchMode.GetDepthMapBuffer();
            float[] pointCloud = researchMode.GetPointCloudBuffer();
            //if (frameTexture.Length > 0)
            //{
            //    if (frameData == null)
            //    {
            //        frameData = frameTexture;
            //    }
            //    else
            //    {
            //        System.Buffer.BlockCopy(frameTexture, 0, frameData, 0, frameData.Length);
            //    }

            //    if (frameData != null)
            //    {
            //        Debug.Log("data updated");
            //        //File.WriteAllBytes(Application.persistentDataPath + "/depthData.dat", frameData);
            //        mediaTexture.LoadRawTextureData(frameData);
            //        mediaTexture.Apply();
            //    }
            //}

            //if (depth.Length > 0)
            //{
            //    var byteArray = new byte[depth.Length * 2];
            //    Buffer.BlockCopy(depth, 0, byteArray, 0, byteArray.Length);
            //    File.WriteAllBytes(String.Format(Application.persistentDataPath+"/"+"depth_{0}.dat", frame), byteArray);
            //    Debug.Log("depthData written");
            //}

            if (pointCloud.Length > 0)
            {
                PointCloudSubscriber.ReceiveMessage(pointCloud);
                //var byteArray2 = new byte[pointCloud.Length * 4];
                //Buffer.BlockCopy(pointCloud, 0, byteArray2, 0, byteArray2.Length);
                //File.WriteAllBytes(String.Format(Application.persistentDataPath+"/"+"pointCloud_{0}.dat", frame), byteArray2);
                //Debug.Log(pointCloud[0]);
                //Debug.Log(pointCloud[1]);
                //Debug.Log(pointCloud[2]);
                Debug.Log("research mode: point cloud data written");
            }
        }
#endif
    }
}