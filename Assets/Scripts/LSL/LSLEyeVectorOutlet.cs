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
    private const int ChannelCount = 10; // {  Vector3 EyeLeft, Vector3 EyeRight, Vector3 ConvergPnt, float pupilSize }

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
        intersectionplane.SetNormalAndPosition(Vector3.forward,planeTransform.transform.position);
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

        Vector3 eyeDirLeft = FoveInterface.GetLeftEyeVector();
        Vector3 eyeDirRight = FoveInterface.GetRightEyeVector();

        //Convergance Calc. We check for the current bound object first, as reading throws off the convergence number

        TMPDisplayer display = targetTMPDisplay.GetComponent<TMPDisplayer>();
        List<GameObject> objects = display.GetActiveBounds();

        Collider coll = Raycaster.GetFOVEGazeRaycastCollider(FOVERig, objects);

        Vector3 endpoint;

        var dat = foveHeadset.GetWorldGazeConvergence();
        if (coll != null)
        {
            endpoint = coll.transform.position; //if we found a collider (A word), send that position
        }
        else
        {
            //endpoint = dat.ray.origin + (dat.ray.direction * dat.distance); original, not relevant now


            float dist = 0; //distance from headset to plane will be negative if the user is not looking at the plane
            //Otherwise, we project our gaze convergence onto a 2D plane inline with the display
            intersectionplane.Raycast(dat.ray, out dist);

            if (dist <= 0) dist = 0; //location to 0,0,0 if we miss our cast
            endpoint = dat.ray.origin + (dat.ray.direction * dist); //the endpoint on the plane
        }

        //Debug.Log("Attension Value: " + FoveInterfaceBase.GetAttentionValue());

        //Eye Vector (left eye)
        currentSample[0] = eyeDirLeft.x;
        currentSample[1] = eyeDirLeft.y;
        currentSample[2] = eyeDirLeft.z;

        //Eye Vector (Right eye)
        currentSample[3] = eyeDirRight.x;
        currentSample[4] = eyeDirRight.y;
        currentSample[5] = eyeDirRight.z;

        //Converg Point
        currentSample[6] = endpoint.x;
        currentSample[7] = endpoint.y;
        currentSample[8] = endpoint.z;

        //pupilSize
        currentSample[9] = dat.pupilDilation;

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
