using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Ink.Runtime;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Linq;

public class TMPDisplayer : MonoBehaviour
{

    //public GameObject parentObj;
    //public GameObject textPrefab;
    [SerializeField]
    private LSLChoiceOutlet markerOutlet;


    [HideInInspector]
    public string storyChoiceLog;
    private List<GameObject> activeBounds = new List<GameObject>();


    //Debug String
    public string text;

    private Regex rgx;
    private Regex opentagrgx;
    private Regex closetagrgx;
    private int pageNum;
    private int sentenceNum;
    private int lineNum;
    private int wordNum;
    private string storyName;

    public bool logging;

    // Use this for initialization
    void Start()
    {
        if(markerOutlet == null)
        {
            markerOutlet = FindObjectOfType<LSLChoiceOutlet>();
            if (markerOutlet == null) Debug.LogError("Cant Find Choice Outlet!");
        }

        activeBounds = new List<GameObject>();
        storyChoiceLog = "S";
        rgx = new Regex("<^a-zA-Z0-9 \\.->");


        pageNum = 0;
        sentenceNum = 0;
        wordNum = 0;
        lineNum = 0;

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

    }

    public void StartNewStory(string sname)
    {
        storyName = sname;
        if(markerOutlet != null) markerOutlet.SetStoryName(sname);
        StartNewStory();
    }

    public void StartNewStory()
    {
        pageNum = 0;
        sentenceNum = 0;
        wordNum = 0;
        storyChoiceLog = "S";
    }

    void UpdateBounds()
    {
        //remove current bounds

        if (activeBounds.Count > 0)
        {
            activeBounds.ForEach(o => Destroy(o));
            activeBounds.Clear();
        }


        TextMeshPro m_Text = GetComponent<TextMeshPro>();

        m_Text.text = text;
        //m_Text.autoSizeTextContainer = true;

        m_Text.ForceMeshUpdate();



        for (int i = 0; i < m_Text.textInfo.wordCount; i++)
        {

            var wordInfo = m_Text.textInfo.wordInfo[i];
            var charInfoArr = m_Text.textInfo.characterInfo;

            var start = charInfoArr[wordInfo.firstCharacterIndex];
            var end = charInfoArr[wordInfo.lastCharacterIndex];

            float maxy = float.MinValue;
            float miny = float.MaxValue;
            for (int j = wordInfo.firstCharacterIndex; j <= wordInfo.lastCharacterIndex; j++)
            {
                if (maxy < charInfoArr[j].topRight.y)
                {
                    maxy = charInfoArr[j].topRight.y;
                }
                if(miny > charInfoArr[j].bottomLeft.y)
                {
                    miny = charInfoArr[j].bottomLeft.y;
                }
            }

            Vector3 min = start.bottomLeft;
            min.y = miny;
            min = transform.TransformPoint(min);


            Vector3 max = end.topRight;
            max.y = maxy;
            max = transform.TransformPoint(max);


            string word = wordInfo.GetWord();
            //Debug.Log(word + " min: " + min);
            //Debug.Log(word + " max: " + max);

            var center = (max + min) / 2;
            //Debug.Log(word + " center: " + center);

            var wordBounds = new GameObject();
            activeBounds.Add(wordBounds);
            wordBounds.name = String.Format("({0},{1},P{2},W{3})_", storyChoiceLog, storyName, pageNum,wordNum) + word; //(Choice, Story, Page) as unique identifier for word
            wordNum++; //Next word
            var coll = wordBounds.AddComponent<BoxCollider>();
            coll.transform.SetParent(transform);
            coll.center = center;

            Vector3 collVect = max - min;
            collVect.z = 0.0F;

            coll.size = collVect;

            

            logWordToFile(wordBounds, word,i);

        }
    }

    public void DebugTestColliders()
    {      
        List<Collider> activeColliders = activeBounds.Select(x => x.GetComponent<Collider>()).ToList();
        activeColliders.ForEach(x => Debug.Log(x.name));    
    }

    public List<GameObject> GetActiveBounds()
    {
        return activeBounds;
    }

    public void RemoveText()
    {
        text = "";
        pageNum++;
        sentenceNum = 0; //reset our counters again
        wordNum = 0;
        lineNum = 0;
        UpdateBounds();
        markerOutlet.WriteStoryPageMarker(pageNum);
        Debug.Log("page++");
    }

    public void NewLine()
    {
        text += "\n";
        lineNum++;
        logBlankLine();
    }

    public void CreateText(string newtext)
    {
        text += newtext;
        wordNum = 0; //reset word counter
        UpdateBounds();
        sentenceNum++; //incriment for the next sentence
    }

    void logWordToFile(GameObject bounds, string word, int wordnum)
    {
        if (!logging) return;
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.dataPath + "/log.csv", true))
        {
            string text = word;

            BoxCollider coll = bounds.GetComponent<BoxCollider>();
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
            output += wordnum + ", ";
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