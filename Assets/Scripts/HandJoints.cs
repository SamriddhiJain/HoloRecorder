using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit;

public class HandJoints : MonoBehaviour
{
    private bool startRecording = false;
    private float timeSinceStartOfRecording = 0.0f;
    private Dictionary<int, float> timesteps;
    private Dictionary<int, Vector3> leftPalmArray;
    private Dictionary<int, Vector3> rightPalmArray;
    private int frame_count = 0;

    public void toggleRecording()
    {
        startRecording = !startRecording;
        if (startRecording)
        {
            frame_count = 0;
            timeSinceStartOfRecording = Time.time;
            timesteps = new Dictionary<int, float>();
            leftPalmArray = new Dictionary<int, Vector3>();
            rightPalmArray = new Dictionary<int, Vector3>();
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (startRecording)
        {
            frame_count++;
            float delta = Time.time - timeSinceStartOfRecording;
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
            timesteps[frame_count] = delta;
            leftPalmArray[frame_count] = leftPalm;
            rightPalmArray[frame_count] = rightPalm;
        }
    }

    public Vector3 GetHandJoints(bool hand, int frame)
    {
        if(frame <=0 && frame < timesteps.Count)
        {
            if (hand)
            {
                return leftPalmArray[frame];
            }
            else
            {
                return rightPalmArray[frame];
            }
        }

        return new Vector3(0, 0, 0);
    }
}
