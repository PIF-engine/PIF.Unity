using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;
using Assets.LSL4Unity.Scripts;
using Assets.LSL4Unity.Scripts.Common;

public class LSLGazeOutlet : MonoBehaviour {


    private const string unique_source_id = "D256CFBDBA3144878CFA64140A219D31";

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


    //do we have someone listening?
    public bool HasConsumer()
    {
        if (outlet != null)
            return outlet.have_consumers();

        return false;
    }

    public string StreamName = "Unity.Gaze.VectorName";
    public string StreamType = "Unity.VectorName";
    public int ChannelCount = 4; // { Name , x , y , z }

    public MomentForSampling sampling;


    //Public fields for prefabs
    public GameObject TargetTMPDisplayPrefab;
    public GameObject FOVERig;


    private Raycaster raycast;

    public bool useFOVEGazeCast;


    //Set up our array for the current samples
    void Start () {
        // initialize the array once
        currentSample = new string[ChannelCount];

        dataRate = LSLUtils.GetSamplingRateFor(sampling);

        streamInfo = new liblsl.StreamInfo(StreamName, StreamType, ChannelCount, dataRate, liblsl.channel_format_t.cf_string, unique_source_id);

        outlet = new liblsl.StreamOutlet(streamInfo);

        gameObject.AddComponent<Raycaster>();
        raycast = gameObject.GetComponent<Raycaster>();


        Time.fixedDeltaTime = 0.00833333333333F; //update 120 times a second, to sync with the FOVE eyetracking
    }


    //Push the current mouse position and if it intersects with a collider
    private void pushSample()
    {
        if (outlet == null)
            return;

        Raycaster.CASTRET cASTRET;

        //Will be using the FOVEGazeCast?
        if(useFOVEGazeCast)
        {
            cASTRET = raycast.DoFOVECast(FOVERig, TargetTMPDisplayPrefab);
        }
        else
        {
            cASTRET = raycast.DoScreencast(Input.mousePosition);
        }

        currentSample[0] = cASTRET.Name;
        currentSample[1] = "" + cASTRET.x;
        currentSample[2] = "" + cASTRET.y;
        currentSample[3] = "" + cASTRET.z;

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
