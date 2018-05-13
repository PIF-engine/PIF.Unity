using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;

public class InkTreeDump : MonoBehaviour {

    public TextAsset storyJSON;

    private string storyEnc;

    public string fileLoc;

	// Use this for initialization
	void Start () {
        storyEnc = storyJSON.text;
        //DumpTree();
	}
	
    public void OnFileLocChanged(string str)
    {
        fileLoc = str;
    }

    public void DumpTreeFromFile()
    {
        if(!Uri.IsWellFormedUriString(fileLoc, UriKind.RelativeOrAbsolute))
        {
            Debug.LogError("Invalid file location!");
            return;
        }

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(fileLoc);
        
        storyEnc= reader.ReadToEnd();     
        reader.Close();

        DumpTree();

    }

    void DumpTree()
    {
        DumpTreeRecurse(new List<int>(), "");
    }

    void DumpTreeRecurse(List<int> choiceList, string textsofar)
    {
        Story story = ChoiceListToStory(choiceList);
        textsofar += story.ContinueMaximally();
        int count = story.currentChoices.Count;
        if(count == 0)
        {
            string id = "0";
            if(choiceList.Count > 0)
                id = Path.GetFileName(fileLoc) + choiceList.Select(x => "" + x).Aggregate((i, j) => i + "-" + j);

            //Debug.Log("Done with branch: " + id);
            //Debug.Log(textsofar);
            string fileloc = Application.dataPath + "/" + id + ".txt";
            Debug.Log("Outputting to: " + fileloc);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileloc, false))
            {
                file.WriteLine(textsofar);
            }

            return;
        } 
        for(int i = 0; i < count;i++)
        {
            List<int> list = new List<int>(choiceList);
            list.Add(i);
            DumpTreeRecurse(list, textsofar);
        }
    }


    private Story ChoiceListToStory(List<int> choiceList)
    {
        Story ret = new Story(storyEnc);

        for(int i = 0; i < choiceList.Count;i++)
        {
            ret.ContinueMaximally();
            ret.ChooseChoiceIndex(choiceList[i]);
        }
        return ret;
    }
}
