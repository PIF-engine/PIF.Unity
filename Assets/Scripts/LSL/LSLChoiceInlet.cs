using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.LSL4Unity.Scripts.AbstractInlets;
using Assets.LSL4Unity.Scripts;

public class LSLChoiceInlet : InletIntSamples {

    public GameObject EventSystem;
    private InkFOVEEventManager manager;

    private int lastChoice = -2;

    [SerializeField]
    private bool connected = false;

    protected override void Process(int[] newSample, double timeStamp)
    {
        if(newSample == null)
        {
            Debug.Log("Null Sample");

            return;
        }
        if (newSample[0] == -1)
        {
            manager.AdvanceStory();
            lastChoice = -2;
        }
        else if(newSample[0] >= 0)
        {
            lastChoice = newSample[0];
        }
        
    }


    public int GetLastChoice()
    {
        return lastChoice;
    }


    public void ChoiceRecieved()
    {
        lastChoice = -2;
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
        if(connected)
            pullSamples();
	}

    public bool IsConnected()
    {
        return connected;
    }
}
