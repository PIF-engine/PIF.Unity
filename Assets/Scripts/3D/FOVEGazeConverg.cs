using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVEGazeConverg : MonoBehaviour {


    public GameObject FOVERig;
    private FoveInterface rig;
    public bool active;



	// Use this for initialization
	void Start () {
        if (!active || FOVERig == null)
            //if we're not active, destroy the parent
            Destroy(gameObject);

        rig = FOVERig.GetComponentInChildren<FoveInterface>();
	}
	
	// Update is called once per frame
	void Update () {
        FoveInterfaceBase.GazeConvergenceData dat;
        try
        {
            dat = rig.GetWorldGazeConvergence();
        } catch (Exception e)
        {
            Debug.Log("Error getting Gaze data! Destroying the debugger...");
            Destroy(gameObject);
            return;
        }
        Vector3 endpoint = dat.ray.origin + (dat.ray.direction * dat.distance);
        gameObject.transform.position = endpoint;

	}
}
