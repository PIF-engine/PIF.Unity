using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Raycaster : MonoBehaviour {


    //wtf windows
#if UNITY_STANDALONE_WIN || UNITY_EDITOR

    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern IntPtr FindWindow(System.String className, System.String windowName);


    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
    }


#endif



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //Debug for testing if Raycaster is working
        if (Input.GetMouseButtonDown(0))
        {
            DoRaycast(Input.mousePosition);
        }

        if( Input.GetKeyDown("f"))
        {
            DoAbsoluteRaycast(Input.mousePosition);
        }
    }

    //Takes a pos in screen coordinates (Unity Window)
    public void DoRaycast(Vector3 pos)
    {

        //pos.y = Screen.currentResolution.height - pos.y;
        Ray ray = Camera.main.ScreenPointToRay(pos);
        

        Debug.Log("Raycasting from " + pos.ToString());
        
        RaycastHit hit;

        //Sphere cast to simulate closest!
        if (Physics.SphereCast(ray, 50F, out hit, 10000F))
        {
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
        } else
        {
            Debug.Log("Missed!");
        }
    }

    //takes a pos in absolute screen coordinates (Real screen as opposed to unity window)
    public void DoAbsoluteRaycast(Vector3 pos)
    {
        RECT rct;

        if (!GetWindowRect(new HandleRef(this, FindWindow(null, "Ink Sketching")), out rct))
        {
            Debug.Log("Could not find window!");
            return;
        }
        Debug.Log("Current mouse pos: " + pos.ToString());


        int left = rct.Left;
        int bottom = Screen.currentResolution.height - rct.Bottom;

        Debug.Log("Current window:\nLeft: " + left +
            ", Bottom: " + bottom);

        pos.x -= left;
        pos.y -= bottom;
        DoRaycast(pos);
        
    }
}
