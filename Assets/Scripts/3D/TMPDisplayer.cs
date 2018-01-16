using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TMPDisplayer : MonoBehaviour
{



    public GameObject parentObj;
    public GameObject textPrefab;


    //Debug String
    string wordString;

    // Use this for initialization
    void Start()
    {

        wordString = "This is a test of Text Mesh Pro!";

        CreateText();

    }

    // Update is called once per frame
    void Update()
    {

    }


    void CreateText()
    {
        List<string> words = new List<string>(wordString.Split(' '));
        Transform parentTrans = parentObj.transform;
        Vector3 parentPos = parentTrans.position;

        //Mesh mesh = parentObj.GetComponent<MeshFilter>().mesh;
        /*Debug.Log("size: x=" + mesh.bounds.size.x
            + ", y=" + mesh.bounds.size.y + ", z=" + mesh.bounds.size.z);*/

        //var deltax = mesh.bounds.size.x;
        //var deltay = mesh.bounds.size.z; //3D to 2D transformation

        Vector3 offset = parentPos - new Vector3(0, 0, 0.1F);

        /*
         *  Setup space character
         */

        var space = Instantiate(textPrefab);
        space.transform.SetParent(parentTrans);
        space.transform.localScale = new Vector3(1, 1, 1);

        TextMeshPro m_Space = space.GetComponent<TextMeshPro>();
        m_Space.text = "␣"; //space
        m_Space.ForceMeshUpdate();
        RectTransform r_Space = space.GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(r_Space); //rebuild to get exact size of space
        Vector3 spaceMove = new Vector3(r_Space.sizeDelta.x / 2, 0, 0);
        Destroy(space); //remove it now that we're done with it

        //bool firstWord = true;
        foreach (string s in words)
        {
            var word = Instantiate(textPrefab);
            word.name = textPrefab.name + "_" + s;
            word.transform.SetParent(parentTrans); //change this to starting point if we want one
            word.transform.localScale = new Vector3(1, 1, 1);

            //Word Setup
            TextMeshPro m_Text = word.GetComponent<TextMeshPro>();

            m_Text.text = s;
            m_Text.ForceMeshUpdate();

            //Refresh Fitter so we can use it in the script
            RectTransform rect = word.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

            //Update collider size so we can raycast later
            BoxCollider coll = word.GetComponent<BoxCollider>();
            Vector3 collSize = new Vector3(rect.sizeDelta.x, rect.sizeDelta.y, 0);
            coll.size = collSize;


            //TODO Put check for overflow here later


            //Move offset half the required amount
            Vector3 moveX = new Vector3(rect.sizeDelta.x / 2, 0, 0);
            offset += moveX;


            //Move word
            word.transform.position = offset;


            //Move offset to end of word, then past the space
            offset += moveX;
            offset += spaceMove;

            Vector3 camRelative = Camera.main.transform.InverseTransformPoint(word.transform.position);
            Debug.Log(word.name + " pos: " + Camera.main.WorldToScreenPoint(camRelative));


        }

    }
}