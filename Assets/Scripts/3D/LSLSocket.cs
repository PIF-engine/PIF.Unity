using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.LSL4Unity.Scripts.AbstractInlets;
using Assets.LSL4Unity.Scripts;

public class LSLSocket : InletFloatSamples {

    private Raycaster caster;

    private bool pullSamplesContinuously = false;





    /// <summary>
    /// Expects two floats that represent the x and y of the screen position, in order to do a raycast
    /// </summary>
    /// <param name="newSample"></param>
    /// <param name="timeStamp"></param>
    protected override void Process(float[] newSample, double timeStamp)
    {
        if (newSample == null)
        {
            Debug.Log("Null sample");

            return;
        }

        float x = newSample[0];
        float y = newSample[1];

        //y = Screen.currentResolution.height - y;

        var screenPos = new Vector3(x, y, 0);


        caster.DoRaycast(screenPos);
    }



    protected override void OnStreamAvailable()
    {
        pullSamplesContinuously = true;
        Debug.Log("Linked to Stream!");
    }

    

    protected override void OnStreamLost()
    {
        base.OnStreamLost();
        Debug.Log("Stream Lost");
        pullSamplesContinuously = false;
    }




    // Use this for initialization
    void Start () {
        gameObject.AddComponent<Raycaster>();
        caster = gameObject.GetComponent<Raycaster>();

        //registerAndLookUpStream();
	}


    private void Update()
    {
        if (pullSamplesContinuously)
            pullSamples();
    }

}
