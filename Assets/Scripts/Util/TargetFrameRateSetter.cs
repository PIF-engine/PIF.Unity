using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFrameRateSetter : MonoBehaviour {


    public int targetFrameRate = 90;

	[ExecuteInEditMode]
	void Awake () {
        Application.targetFrameRate = targetFrameRate;

    }
}
