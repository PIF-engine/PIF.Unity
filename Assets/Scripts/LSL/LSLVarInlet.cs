using System.Collections;
using System.Collections.Generic;
using Assets.LSL4Unity.Scripts.AbstractInlets;
using UnityEngine;

public class LSLVarInlet : InletStringSamples {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    pullSamples();	
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
