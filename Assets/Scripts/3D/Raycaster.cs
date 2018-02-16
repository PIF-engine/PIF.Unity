using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour {

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
            doRaycast(Input.mousePosition);
        }
    }

    //Takes a pos in screen coordinates
    public void doRaycast(Vector3 pos)
    {

        pos.y = Screen.currentResolution.height - pos.y;
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
}
