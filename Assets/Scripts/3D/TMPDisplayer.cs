using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Ink.Runtime;
using System.Text.RegularExpressions;
using System.Globalization;

public class TMPDisplayer : MonoBehaviour
{


    private bool USESCREEN = true;


    public GameObject parentObj;
    public GameObject textPrefab;
    public string storyChoiceLog;
    private List<GameObject> activeWords = new List<GameObject>();


    //Debug String
    public string text;

    private Vector3 offset;
    private Vector3 spaceMove;
    private Vector3 nlMove;
    private float rightMargin = float.MaxValue;

    private Regex rgx;
    private int pageNum;
    private int sentenceNum;
    private int wordNum;

    public bool logging;

    // Use this for initialization
    void Start()
    {
        activeWords = new List<GameObject>();
        storyChoiceLog = "S";
        rgx = new Regex("[^a-zA-Z0-9 \\.-]");

        pageNum = 0;
        sentenceNum = 0;
        wordNum = 0;

        logging = true;


        //setup log

        if (logging)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.dataPath + "/log.csv", true))
            {
                file.WriteLine("Res: " + Screen.width + "x" + Screen.height);
                file.WriteLine("");
                file.WriteLine("");
                file.WriteLine("Time, Story, Word, Word Num, Sentence Num, Page Num, BoundMin, BoundMax");
            }
        }

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
            Vector3 camRelative = word.transform.TransformPoint(word.transform.localPosition);
            Debug.Log(word.name + " screen pos: " + Camera.main.WorldToScreenPoint(camRelative));

            logWordToFile(word);
            wordNum++;
        }
        logBlankLine();
        sentenceNum++;

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
        pageNum++;
        wordNum = 0;
        sentenceNum = 0;
        
    }

    void resetOffset()
    {
        offset = parentObj.transform.position + new Vector3(0, 0, -0.1F);
    }

    void logWordToFile(GameObject word)
    {
        if (!logging) return;
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.dataPath + "/log.csv", true))
        {
            string text = word.GetComponent<TextMeshPro>().text;

            BoxCollider coll = word.GetComponent<BoxCollider>();
            Vector3 min = coll.bounds.min;
            Vector3 max = coll.bounds.max;
            //min = word.transform.TransformPoint(word.transform.localPosition + min);
            //max = word.transform.TransformPoint(word.transform.localPosition + max);

            string minScreen = "(x:";
            minScreen += Camera.main.WorldToScreenPoint(min).x;
            minScreen += "  y:";
            minScreen += Camera.main.WorldToScreenPoint(min).y;
            minScreen += ")";

            string maxScreen = "(x:";
            maxScreen += Camera.main.WorldToScreenPoint(max).x;
            maxScreen += "  y:";
            maxScreen += Camera.main.WorldToScreenPoint(max).y;
            maxScreen += ")";

            string output = DateTime.UtcNow.ToString("HH:mm:ss")+":" + DateTime.UtcNow.Millisecond + ", ";
            output += storyChoiceLog + ", ";
            output += rgx.Replace(text, "") + ", ";
            output += wordNum + ", ";
            output += sentenceNum + ", ";
            output += pageNum + ", ";
            output += minScreen + ", ";
            output += maxScreen + ", ";
            file.WriteLine(output);
        }

        //Debug.Log(Application.dataPath + "/log.csv");     
    }

    void logBlankLine()
    {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.dataPath + "/log.csv", true))
        {
            file.WriteLine("");
        }
    }
}