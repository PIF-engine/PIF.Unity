using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InkTesting : MonoBehaviour {

    //[SerializeField]
    public TextAsset storyJSON;
    private Story story;

    //This is the same start point as the TMPDisplayer
    public GameObject startPoint;
    private TMPDisplayer display;
    


    // Use this for initialization
    void Start () {

        display = gameObject.GetComponent<TMPDisplayer>();

       // m_Text = canvas.GetComponentInChildren<TextMeshProUGUI>(); 
        StartStory();
        
	}
	
	// Update is called once per frame
	void Update () {
        bool clear = false;
        bool advance = false;

        if (Input.GetKeyDown("1"))
        {
            if(story.currentChoices.Count >= 1)
            {
                story.ChooseChoiceIndex(0);
                clear = true;
                advance = true;
            }
        } else if (Input.GetKeyDown("2"))
        {
            if (story.currentChoices.Count >= 2)
            {
                story.ChooseChoiceIndex(1);
                clear = true;
                advance = true;
            }
        } else if (Input.GetKeyDown("3"))
        {
            if (story.currentChoices.Count >= 3)
            {
                story.ChooseChoiceIndex(2);
                clear = true;
                advance = true;
            }
        } else if (Input.GetKeyDown("space"))
        {
            if(story.canContinue)
            {
                advance = true;
            }
        }

        if(clear)
        {
            display.RemoveText();
            AdvanceStory();
        }
        if (advance)
        {
            AdvanceStory();
        }


    }


    void StartStory()
    {
        story = new Story(storyJSON.text);
        AdvanceStory();
    }


    void AdvanceStory()
    {

        if (story.canContinue)
        {
            string text = story.Continue().Trim();
            display.text = text;
            display.CreateText();
            Debug.Log(text);
            display.NewLine();
        }

        if(story.currentChoices.Count > 0)
        {
            for(int i = 0; i < story.currentChoices.Count; i++)
            {
                string ct = story.currentChoices[i].text.Trim();
                Debug.Log(ct);
                display.text = ct;
                display.NewLine();
                display.CreateText();
            }
        }
    
    }



}
