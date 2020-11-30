using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using PointCloudRenderer;

public class ButtonInteraction : MonoBehaviour
{
    private int frame = 0;
    private ResearchMode rm;
    //private HandJoints joints;
    void Start()
    {
        rm = new ResearchMode();
        PointCloudSubscriber.startTimer();
        ResearchMode.StartPreviewEvent();
        //PointCloudRenderer.toggleView();
        //joints = gameObject.AddComponent<HandJoints>();
        //joints.toggleRecording();
    }
    
    void Update()
    {
        ////Debug.Log("button clicked");
        frame++;
        ////joints.UpdateJoints();

        if (frame == 300)
        {
            //stop recording; play
            ResearchMode.StartPreviewEvent();
            PointCloudRenderer.toggleView();
            //joints.toggleRecording();
        }
    }
}
