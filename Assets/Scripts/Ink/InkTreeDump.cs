using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InkTreeDump : MonoBehaviour {

    public TextAsset storyJSON;
    

	// Use this for initialization
	void Start () {
        DumpTree();
	}
	
	// Update is called once per frame
	void Update () {
		
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
            Debug.Log("Done with branch: " + choiceList.Select(x => ""+x).Aggregate((i,j) => i + "-" + j));
            Debug.Log(textsofar);
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
        Story ret = new Story(storyJSON.text);

        for(int i = 0; i < choiceList.Count;i++)
        {
            ret.ContinueMaximally();
            ret.ChooseChoiceIndex(choiceList[i]);
        }
        return ret;
    }
}
