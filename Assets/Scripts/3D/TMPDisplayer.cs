using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TMPDisplayer : MonoBehaviour
{


    private bool USESCREEN = true;


    public GameObject parentObj;
    public GameObject textPrefab;
    private List<GameObject> activeWords = new List<GameObject>();


    //Debug String
    public string text;

    private Vector3 offset;
    private Vector3 spaceMove;
    private Vector3 nlMove;
    private float rightMargin = float.MaxValue;

    // Use this for initialization
    void Start()
    {
        activeWords = new List<GameObject>();

        var space = Instantiate(textPrefab);
        space.transform.SetParent(parentObj.transform);
        space.transform.localScale = new Vector3(1, 1, 1);

        // Space Character vector and new line vector setup
        TextMeshPro m_Space = space.GetComponent<TextMeshPro>();
        m_Space.text = "␣"; //space
        m_Space.ForceMeshUpdate();
        RectTransform r_Space = space.GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(r_Space); //rebuild to get exact size of space
        spaceMove = new Vector3(r_Space.sizeDelta.x / 2, 0, 0);
        nlMove = new Vector3(0, -r_Space.sizeDelta.y * 1.5F); //multiplier for line spacing
        
        Destroy(space); //remove it now that we're done with it


        resetOffset();

        createMargin();

    }

    private void createMargin()
    {
        if (USESCREEN)
        {
            Vector3 worldSpace = transform.TransformPoint(parentObj.transform.position);
            Vector3 margin = Camera.main.WorldToScreenPoint(worldSpace);
            Debug.Log("X LEFT MARGIN IS " + margin.x);
            Debug.Log("X RIGHT MARGIN IS " + (Screen.width - margin.x));
            margin.x = Screen.width - margin.x;
            worldSpace = Camera.main.ScreenToWorldPoint(margin);
            rightMargin = worldSpace.x;
         }
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void CreateText()
    {
        List<string> words = new List<string>(text.Split(' '));
        Transform parentTrans = parentObj.transform;
        Vector3 parentPos = parentTrans.position;
        
        //Check for overflow here




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


            //Move offset to end of word
            offset += moveX;

            //if we pass the margin (Overflow), newline
            Vector3 offsetWorldPos = transform.TransformPoint(offset);
            if(offsetWorldPos.x >= rightMargin)
            {
                NewLine();
                offset += moveX;
                word.transform.position = offset;
            } else
            { //other wise we just move a space
                offset += spaceMove;
            }
            

            //Add the new word to the word list
            activeWords.Add(word);


            //Debug Positions
            Vector3 camRelative = Camera.main.transform.InverseTransformPoint(word.transform.position);
            Debug.Log(word.name + " pos: " + Camera.main.WorldToScreenPoint(camRelative));


        }

    }

    public void NewLine()
    {
        offset.x = parentObj.transform.position.x;
        offset += nlMove;
    }

    public void RemoveText()
    {
        if (activeWords.Count <= 0)
            return;
        foreach(GameObject o in activeWords)
        {
            Destroy(o);
        }
        resetOffset();
    }

    void resetOffset()
    {
        offset = parentObj.transform.position + new Vector3(0, 0, -0.1F);
    }
}