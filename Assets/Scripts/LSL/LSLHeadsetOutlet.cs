using System;
using Assets.LSL4Unity.Scripts;
using Assets.LSL4Unity.Scripts.Common;
using LSL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSLHeadsetOutlet : MonoBehaviour {

    private const string unique_source_id = "D256CFBDBA3BDBD2321236729873248";

    private liblsl.StreamOutlet outlet;
    private liblsl.StreamInfo streamInfo;
    public liblsl.StreamInfo GetStreamInfo()
    {
        return streamInfo;
    }
    /// <summary>
    /// Use a array to reduce allocation costs
    /// </summary>
    private float[] currentSample;

    private double dataRate;

    public double GetDataRate()
    {
        return dataRate;
    }


    //do we have someone listening?
    public bool HasConsumer()
    {
        if (outlet != null)
            return outlet.have_consumers();

        return false;
    }

    public string StreamName = "Unity.PIF.HeadsetPosRot";
    public string StreamType = "Unity.HeadsetPosRot";
    private const int ChannelCount = 7; // {  x , y , z }

    public MomentForSampling sampling;

    // Use this for initialization
    void Start()
    {
        bool connected;
        try
        {
            connected = FoveInterface.IsHardwareConnected();
        }
        catch (Exception)
        {
            connected = false;
        }
        if(!connected) { Destroy(this);}

        // initialize the array once
        currentSample = new float[ChannelCount];

        dataRate = LSLUtils.GetSamplingRateFor(sampling, false);

        streamInfo = new liblsl.StreamInfo(StreamName, StreamType, ChannelCount, dataRate, liblsl.channel_format_t.cf_float32, unique_source_id);

        outlet = new liblsl.StreamOutlet(streamInfo);
    }

    /// <summary>
    /// This method pushes our EyeConv data sample. It is called according to the sampling rate.
    /// Our sample is six floats. The first 3 are the x,y,z vector for eyeDirection
    /// The second 3 are the x,y,z location of the point of covergance
    /// </summary>
    private void pushSample()
    {
        if (FoveInterface.IsEyeTrackingCalibrating())
            return;


        Vector3 headsetPos = FoveInterface.GetHMDPosition();
        Quaternion headsetRot = FoveInterface.GetHMDRotation();

        //Head Position Vector
        currentSample[0] = headsetPos.x;
        currentSample[1] = headsetPos.y;
        currentSample[2] = headsetPos.z;

        //Converg Point
        currentSample[3] = headsetRot.w;
        currentSample[4] = headsetRot.x;
        currentSample[5] = headsetRot.y;
        currentSample[6] = headsetRot.z;

        outlet.push_sample(currentSample, liblsl.local_clock());
    }

    /*
    * Do our sampling 
    */
    void FixedUpdate()
    {
        if (sampling == MomentForSampling.FixedUpdate)
            pushSample();
    }

    void Update()
    {
        if (sampling == MomentForSampling.Update)
            pushSample();
    }

    void LateUpdate()
    {
        if (sampling == MomentForSampling.LateUpdate)
            pushSample();
    }
}
