using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.LSL4Unity.Scripts.AbstractInlets;
using Assets.LSL4Unity.Scripts;

public class LSLChoiceInlet : InletIntSamples {

    public GameObject EventSystem;
    private InkFOVEEventManager manager;

    private int lastChoice = -1;

    private bool connected = false;

    protected override void Process(int[] newSample, double timeStamp)
    {
        if(newSample == null)
        {
            Debug.Log("Null Sample");

            return;
        }
        lastChoice = newSample[0];
    }


    public int GetLastChoice()
    {
        return lastChoice;
    }


    public void ClearLastChoice()
    {
        lastChoice = -1;
    }

    protected override void OnStreamAvailable()
    {
        connected = true;
    }

    protected override void OnStreamLost()
    {
        connected = false;
    }

    // Use this for initialization
    void Start () {
        manager = EventSystem.GetComponent<InkFOVEEventManager>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if(manager.IsWaitingForChoice() && connected)
            pullSamples();
	}
}
