using System.Collections;
using System.Collections.Generic;
using Assets.LSL4Unity.Scripts.AbstractInlets;
using UnityEngine;

public class LSLVarInlet : InletStringSamples {

    bool pull = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	    if(pull) pullSamples();	
	}

    protected override void OnStreamAvailable()
    {
        pull = true;
    }

    protected override void OnStreamLost()
    {
        pull = false;
    }

    protected override void Process(string[] newSample, double timeStamp)
    {
        if (newSample == null)
        {
            Debug.Log("Null Sample");

            return;
        }
        Debug.Log("VAR UPDATE! " + newSample[0]);
        InkFOVEEventManager.DoDirectorUpdate(newSample[0]);
    }
}
