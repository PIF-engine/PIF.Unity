using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;
using Assets.LSL4Unity.Scripts.Common;
using Assets.LSL4Unity.Scripts;

public class LSLChoiceOutlet : LSLMarkerStream {


    private const string unique_source_id = "A256CFBDAA314B8D8CFA64140A219D31";


    public void RequestResponce()
    {
        Write("request");
    }

    public void ResponceRecieved()
    {
        Write("recieved");
    }

}
