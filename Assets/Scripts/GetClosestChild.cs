using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetClosestChild : MonoBehaviour {

    public GameObject parent;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        if(Input.GetMouseButton(0))
        {
            Vector3 mousePos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Mouse Pos: " + mousePos.x + ", " + mousePos.y);

            List<Transform> list = new List<Transform>(parent.GetComponentsInChildren<Transform>());
            Debug.Log(parent.name);
            list.Remove(parent.transform);
            
            GameObject closest = GetClosest(mousePos, list).gameObject;
            

            TextMeshProUGUI m_Text = closest.GetComponent<TextMeshProUGUI>();

            Debug.Log("Closest word is: " + m_Text.text);

            
        }
		
	}

    Transform GetClosest(Vector3 currentPosition, List<Transform> transforms)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;


        foreach (Transform potentialTarget in transforms)
        {

            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}
