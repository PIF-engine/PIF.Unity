using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToFOVEEye : MonoBehaviour
{

    public Canvas intersectionCanvas;

	// Use this for initialization
	void Start () {
		Invoke("Attach",.1F);
	}


    void Attach()
    {
        var cam = GameObject.Find("FOVE Eye (Left)");
        if (cam == null) return;
        var leftCamera = cam.GetComponent<Camera>();
        intersectionCanvas.worldCamera = leftCamera;
    }
}
