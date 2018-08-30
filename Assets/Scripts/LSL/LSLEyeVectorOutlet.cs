using System;
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


    //This is the plane we will be projecting onto for our endpoint
    private Plane intersectionplane;
    private GameObject planeTransform;

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
    private const int ChannelCount = 12; // see LSL OUTLET FORMAT.txt for layout

    public MomentForSampling sampling;

    public GameObject FOVERig;
    private FoveInterface foveHeadset;

    public TMPDisplayer targetTMPDisplay;

    private bool isInvalid = false;

    // Use this for initialization
    void Start()
    {
        //check if FOVE is connected
        bool connected;
        try
        {
            connected = FoveInterface.IsHardwareConnected();
        }
        catch (Exception)
        {
            connected = false;
        }

        if (!connected)
        {
            Debug.Log("FOVE not connected. Removing eye vector LSL outlet");
            Destroy(this);
            return;
        }


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

        planeTransform = GameObject.FindGameObjectWithTag("IntersectionPlane");
        intersectionplane = new Plane();
        intersectionplane.SetNormalAndPosition(Vector3.forward, planeTransform.transform.position);
    }

    /// <summary>
    /// This method pushes our EyeConv data sample. It is called according to the sampling rate.
    /// Our sample is six floats. The first 3 are the x,y,z vector for eyeDirection
    /// The second 3 are the x,y,z location of the point of covergance
    /// </summary>
    private void pushSample()
    {
        if (isInvalid)
            return;

        if (FoveInterface.IsEyeTrackingCalibrating())
            return;

        var eyeDirLeft = FoveInterface.GetLeftEyeVector();
        var eyeDirRight = FoveInterface.GetRightEyeVector();

        //Convergance Calc. We check for the current bound object first, as reading throws off the convergence number

        var display = targetTMPDisplay.GetComponent<TMPDisplayer>();
        List<GameObject> objects = display.GetActiveBounds();

        var dat = foveHeadset.GetWorldGazeConvergence();
        var convPoint = dat.ray.GetPoint(dat.distance); //the convergance point in world space


        //Project onto the intersection plane
        var endpoint = intersectionplane.ClosestPointOnPlane(convPoint);
        

        //normalized coordinates for endpoint. xDist -> .75, yDist -> .5
        //Thus, 0,0 will be the top left, and 1,1 will be the bottom right of the tablet. NOTE: values can be outside this range, and will be negative or greater than 1!
        float xNorm = (endpoint.x - planeTransform.transform.position.x) / .75F; //(origin - position) / scale
        float yNorm = (endpoint.y - planeTransform.transform.position.y) / -.5F; //Negative so that the negative y is positive, as the intersection plane is at the top left


        //Debug.Log("Attension Value: " + FoveInterfaceBase.GetAttentionValue());

        //Eye Vector (left eye)
        currentSample[0] = eyeDirLeft.x;
        currentSample[1] = eyeDirLeft.y;
        currentSample[2] = eyeDirLeft.z;

        //Eye Vector (Right eye)
        currentSample[3] = eyeDirRight.x;
        currentSample[4] = eyeDirRight.y;
        currentSample[5] = eyeDirRight.z;

        //Converg Point, Normalized
        currentSample[6] = xNorm;
        currentSample[7] = yNorm;

        //Real convergance point, not projected
        currentSample[8] = convPoint.x;
        currentSample[9] = convPoint.y; 
        currentSample[10] = convPoint.z; 

        //pupilSize
        currentSample[11] = dat.pupilDilation;

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
