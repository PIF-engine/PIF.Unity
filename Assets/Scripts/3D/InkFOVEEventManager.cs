using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InkFOVEEventManager : MonoBehaviour
{

    //[SerializeField]
    public TextAsset storyJSON;
    private Story story;

    public GameObject TargetTMPDisplayPrefab;
    public GameObject LSLChoiceOutlet;
    public GameObject LSLChoiceInput;
    private TMPDisplayer targetDisplay;
    private LSLChoiceOutlet choiceOutlet;
    private LSLChoiceInput choiceInput;

    private bool waitForChoice;

    // Use this for initialization
    void Start()
    {

        targetDisplay = TargetTMPDisplayPrefab.GetComponent<TMPDisplayer>();
        if (LSLChoiceOutlet != null)
            choiceOutlet = LSLChoiceOutlet.GetComponent<LSLChoiceOutlet>();
        if (LSLChoiceInput != null)
            choiceInput = LSLChoiceInput.GetComponent<LSLChoiceInput>();


        waitForChoice = false;

        // m_Text = canvas.GetComponentInChildren<TextMeshProUGUI>(); 
        StartStory();

    }

    // Update is called once per frame
    void Update()
    {
        bool clear = false;
        bool advance = false;
        int choice = -1;

        if(choiceOutlet != null && story.currentChoices.Count >= 1)
        {
            if (waitForChoice == false)
            {
                AdvanceStory();
                choiceOutlet.RequestResponce();
                return;
            }
            else
            {
                if (choiceInput == null) return;
                //logic for getting the choice remotely

                choice = choiceInput.GetLastChoice();
                if (choice == -1) return;
                clear = true;
                advance = true;
                choiceInput.ClearLastChoice();
                choiceOutlet.StopRequest();
            }
        }


        if (Input.GetKeyDown("1"))
        {
            if (story.currentChoices.Count >= 1)
            {
                choice = 0;
                clear = true;
                advance = true;
            }
        }
        else if (Input.GetKeyDown("2"))
        {
            if (story.currentChoices.Count >= 2)
            {
                choice = 1;
                clear = true;
                advance = true;
            }
        }
        else if (Input.GetKeyDown("3"))
        {
            if (story.currentChoices.Count >= 3)
            {
                choice = 2;
                clear = true;
                advance = true;
            }
        }
        else if (Input.GetKeyDown("space"))
        {
            advance = true;
        }
        if(choice >= 0)
        {
            MakeChoice(choice);
        }
        if (clear)
        {
            targetDisplay.RemoveText();
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
            targetDisplay.CreateText(text);
            //Debug.Log(text);
            targetDisplay.NewLine();
            return;
        }
        else if (story.currentChoices.Count > 0 && !waitForChoice)
        {
            waitForChoice = true;
            targetDisplay.logging = false;
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                string ct = story.currentChoices[i].text.Trim();
                //Debug.Log(ct);
                targetDisplay.NewLine();
                targetDisplay.CreateText(ct);
            }
            targetDisplay.logging = true;
        }

    }

    public bool IsWaitingForChoice()
    {
        return waitForChoice;
    }


    public void MakeChoice(int i)
    {
        story.ChooseChoiceIndex(i);
        targetDisplay.storyChoiceLog += "->" + i;
        waitForChoice = false;
    }


}
