using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;
using Assets.LSL4Unity.Scripts;
using Assets.LSL4Unity.Scripts.Common;

public class LSLEyeVectorOutlet : MonoBehaviour
{
    private const string unique_source_id = "D256CFBDBA3BDBD232123";

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

    public string StreamName = "Unity.PIF.EyeConvergance";
    public string StreamType = "Unity.EyeConvergance";
    public int ChannelCount = 6; // { Name , x , y , z }

    public MomentForSampling sampling;

    public GameObject FOVERig;
    private FoveInterface foveHeadset;

    public TMPDisplayer targetTMPDisplay;

    private bool isInvalid = false;

    // Use this for initialization
    void Start()
    {
        // initialize the array once
        currentSample = new float[ChannelCount];

        dataRate = LSLUtils.GetSamplingRateFor(sampling, false);

        streamInfo = new liblsl.StreamInfo(StreamName, StreamType, ChannelCount, dataRate, liblsl.channel_format_t.cf_float32, unique_source_id);

        outlet = new liblsl.StreamOutlet(streamInfo);
        foveHeadset = FOVERig.GetComponentInChildren<FoveInterface>();

        if (targetTMPDisplay == null)
        {
            targetTMPDisplay = FindObjectOfType<TMPDisplayer>();
            if (targetTMPDisplay == null)
            {
                Debug.LogError("Cant Find Valid TMP Displayer!");
                isInvalid = true;
            }
        }
    }

    /// <summary>
    /// This method pushes our EyeConv data sample. It is called according to the sampling rate.
    /// Our sample is six floats. The first 3 are the x,y,z vector for eyeDirection
    /// The second 3 are the x,y,z location of the point of covergance
    /// </summary>
    private void pushSample()
    {
        if (outlet == null || isInvalid)
            return;

        Vector3 eyeDir = FoveInterface.GetLeftEyeVector();


        //Convergance Calc. We check for the current bound object first, as reading throws off the convergence number

        TMPDisplayer display = targetTMPDisplay.GetComponent<TMPDisplayer>();
        List<GameObject> objects = display.GetActiveBounds();

        Collider coll = Raycaster.GetFOVEGazeRaycastCollider(FOVERig, objects);

        Vector3 endpoint;
        if(coll != null)
        {
            endpoint = coll.transform.position;
        }
        else
        {
            var dat = foveHeadset.GetWorldGazeConvergence();
            endpoint = dat.ray.origin + (dat.ray.direction * dat.distance);
        }

        //Eye Vector (left eye)
        currentSample[0] = eyeDir.x;
        currentSample[0] = eyeDir.y;
        currentSample[0] = eyeDir.z;

        //Converg Point
        currentSample[0] = endpoint.x;
        currentSample[0] = endpoint.y;
        currentSample[0] = endpoint.z;

        outlet.push_sample(currentSample, liblsl.local_clock());
        //Debug.Log("Pushed EyeConv Sample");
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
