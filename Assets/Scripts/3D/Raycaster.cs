using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Raycaster : MonoBehaviour {


    //wtf windows
#if UNITY_EDITOR

    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern IntPtr FindWindow(System.String className, System.String windowName);


    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

#endif

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
    }




    public struct CASTRET
    {
        public string Name;
        public float x;
        public float y;
        public float z;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Takes a pos in screen coordinates (Unity Window)
    public CASTRET DoScreencast(Vector3 pos)
    {

        CASTRET ret;

        //pos.y = Screen.currentResolution.height - pos.y;
        Ray ray = Camera.main.ScreenPointToRay(pos);

        ray.origin = (ray.origin - ray.direction * 100);

        //Debug.Log("Raycasting from " + pos.ToString());
        
        RaycastHit hit;

        //Raycast to look for direct hit (For 3D work)
        if(Physics.Raycast(ray, out hit, 10000F))
        {
            //Debug.Log("Direct Hit object: " + hit.collider.gameObject.name);
            ret = ParseCast(hit.collider.gameObject.name, pos);
        }
        //Sphere cast to simulate closest! not 100% accurate in 3D
        else if (Physics.SphereCast(ray, .025F, out hit, 10000F))
        {
            //Debug.Log("Hit object: " + hit.collider.gameObject.name);
            ret = ParseCast(hit.collider.gameObject.name, pos);
        } else
        {
            //Debug.Log("Missed!");
            ret = ParseCast("MISSED CAST!", new Vector3(-1, -1, -1));
        }


        return ret;
    }

    //takes a pos in absolute screen coordinates (Real screen as opposed to unity window)
    public void DoAbsoluteRaycast(Vector3 pos)
    {
        if(Screen.fullScreen)
        {
            DoScreencast(pos);
            return;
        }

#if UNITY_EDITOR
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
        DoScreencast(pos);
#endif   
    }

    //TODO TEST THIS WITH THE FOVE!
    public CASTRET DoFOVECast(GameObject FOVERig, GameObject TargetTMPDisplay)
    {
        CASTRET ret;
        TMPDisplayer display = TargetTMPDisplay.GetComponent<TMPDisplayer>();
        List<GameObject> objects = display.GetActiveBounds();

        Collider coll = GetFOVEGazeRaycastCollider(FOVERig, objects);
        

        if(coll == null)
        {
            ret = ParseCast("MISSED CAST!", new Vector3(-1, -1, -1));
        }
        else
        {
            
            ret = ParseCast(coll.gameObject.name, coll.gameObject.transform.position);
        }

        return ret;
    }

    public Collider GetFOVEGazeRaycastCollider(GameObject FOVERig, List<GameObject> objects)
    {

        FoveInterface fove = FOVERig.GetComponentInChildren<FoveInterface>();

        List<Collider> activeColliders = objects.Select(x => x.GetComponent<Collider>()).ToList();
        Collider coll;
        fove.Gazecast(activeColliders, out coll);


        return coll;
    }

    private CASTRET ParseCast(string name, Vector3 pos)
    {
        CASTRET ret = new CASTRET
        {
            Name = name,
            x = pos.x,
            y = pos.y,
            z = pos.z
        };

        return ret;
    }
}
