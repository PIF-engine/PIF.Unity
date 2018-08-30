using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;
using Assets.LSL4Unity.Scripts;
using System;
using Assets.LSL4Unity.Scripts.Common;

public class LSLScreenProjectionOutlet : MonoBehaviour {
    private const string unique_source_id = "D256CFBDBA3BDBD2321434343423";

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
    //private RectTransform planeTransform;

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

    public string StreamName = "Unity.PIF.ScreenProjection";
    public string StreamType = "Unity.ScreenCoordinates";
    private const int ChannelCount = 2; // {  x val, y val }

    public MomentForSampling sampling;


    public GameObject FOVERig;
    private FoveInterface foveHeadset;

    public RectTransform projectionCanvasTransform;


    // Use this for initialization
    void Start () {
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


        intersectionplane = new Plane();
        intersectionplane.SetNormalAndPosition(CalcNormalFromRotation(projectionCanvasTransform), projectionCanvasTransform.position);
    }



    private void pushSample()
    {

        if (FoveInterface.IsEyeTrackingCalibrating())
            return;

        intersectionplane.SetNormalAndPosition(CalcNormalFromRotation(projectionCanvasTransform), projectionCanvasTransform.position);

        var dat = foveHeadset.GetWorldGazeConvergence();

        float dist = 0; //distance from headset to plane will be negative if the user is not looking at the plane
        //Otherwise, we project our gaze convergence onto a 2D plane inline with the display
        intersectionplane.Raycast(dat.ray, out dist);

        var endpoint = dat.ray.origin + (dat.ray.direction * dist); //the endpoint on the plane

        endpoint = projectionCanvasTransform.InverseTransformPoint(endpoint);

        //offsets for center of canvas to top left point
        float woff = projectionCanvasTransform.rect.width / 2;
        float hoff = -projectionCanvasTransform.rect.height / 2;

        endpoint = endpoint + new Vector3(woff, hoff,0);//convert from center of screen to top left coords

        //normalized screen coords.
        //Thus, 0,0 will be the top left, and 1,1 will be the bottom right of the tablet. NOTE: values can be outside this range, and will be negative or greater than 1!
        float xNorm = endpoint.x / projectionCanvasTransform.rect.width; //(origin - position) / scale. Origin is 0,0, so it is ignored
        float yNorm = endpoint.y / projectionCanvasTransform.rect.height; 

        endpoint = new Vector3(xNorm,-yNorm);// update to the normalized version, and push the sample

        currentSample[0] = endpoint.x; 
        currentSample[1] = endpoint.y; 


        outlet.push_sample(currentSample, liblsl.local_clock());
        Debug.Log("Pushed ScreenPlane Sample: " + endpoint);
        //Debug.Log("Dist to ScreenPlane: " + dist);

    }


    private static Vector3 CalcNormalFromRotation(Transform t)
    {
        return t.rotation * Vector3.back; //returns the multiplication of the quarternion in world space time the backward vector. This should create a normal facing the camera
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
