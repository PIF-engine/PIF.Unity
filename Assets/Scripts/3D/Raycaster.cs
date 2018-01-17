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

        if (Input.GetMouseButtonDown(0))
        {
            doRaycast(Input.mousePosition);
        }
    }


    void doRaycast(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        ray.origin -= new Vector3(0, 0, 1000F);

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
