using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFrameRateSetter : MonoBehaviour {


    public int targetFrameRate = -1;

	[ExecuteInEditMode]
	void Update () {
        Application.targetFrameRate = targetFrameRate;

    }
}
