using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;
using Assets.LSL4Unity.Scripts.Common;
using Assets.LSL4Unity.Scripts;

public class LSLChoiceOutlet : MonoBehaviour {


    private const string unique_source_id = "A256CFBDAA314B8D8CFA64140A219D31";

    private liblsl.StreamOutlet outlet;
    private liblsl.StreamInfo streamInfo;
    public liblsl.StreamInfo GetStreamInfo()
    {
        return streamInfo;
    }
    /// <summary>
    /// Use a array to reduce allocation costs
    /// </summary>
    private string[] currentSample;

    private double dataRate;

    public double GetDataRate()
    {
        return dataRate;
    }

    public MomentForSampling sampling;

    //do we have someone listening?
    public bool HasConsumer()
    {
        if (outlet != null)
            return outlet.have_consumers();

        return false;
    }

    public string StreamName = "Unity.Choice.Request";
    public string StreamType = "Choice.Request";
    public int ChannelCount = 1; // { True/False }
    private bool requesting = false;


    


    // Use this for initialization
    void Start () {
        currentSample = new string[ChannelCount];

        dataRate = LSLUtils.GetSamplingRateFor(sampling, false);

        streamInfo = new liblsl.StreamInfo(StreamName, StreamType, ChannelCount, dataRate, liblsl.channel_format_t.cf_string, unique_source_id);

        outlet = new liblsl.StreamOutlet(streamInfo);
    }
	

    public void RequestResponce()
    {
        requesting = true;
    }

    public void StopRequest()
    {
        requesting = false;
        if (outlet == null)
            return;

        currentSample[0] = "recieved";

        outlet.push_sample(currentSample, liblsl.local_clock());
    }

    private void pushSample()
    {
        //if the outlet is not connected or we're not requesting, dont push a sample
        if (!requesting || outlet == null)
            return;

        currentSample[0] = "request";

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
