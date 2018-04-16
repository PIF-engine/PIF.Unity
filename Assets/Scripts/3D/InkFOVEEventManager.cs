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
    public bool usingLSL;
    private bool activeLSLConnection = false;

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


        //if we have a working LSL connection
        if (usingLSL)
        {

            // and if we're currently waiting for a choice
            if (waitForChoice)
            {
                //Get the last choice from the input          
                choice = choiceInput.GetLastChoice();

                //If the choice is invalid
                if (choice < 0)
                {
                    //request a responce from the director
                    choiceOutlet.RequestResponce();
                }
                else
                {
                    clear = true;
                    advance = true;
                }
            }
            else //otherwise we're not waiting for a choice
            {
                choiceInput.ClearLastChoice();
                choiceOutlet.ResponceRecieved();
            }
        }

        //Manual inputs for debugging
        if (Input.GetKeyDown("1"))
        {

            choice = 0;
            clear = true;
            advance = true;

        }
        else if (Input.GetKeyDown("2"))
        {

            choice = 1;
            clear = true;
            advance = true;

        }
        else if (Input.GetKeyDown("3"))
        {

            choice = 2;
            clear = true;
            advance = true;
        }
        else if (Input.GetKeyDown("space"))
        {
            advance = true;
        }


        if (choice >= 0)
        {
            //attempt to make a choice. If its invalid, ignore the advance and clear flags
            if (!MakeChoice(choice)) return;
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
        //if the story can continue, continue it
        if (story.canContinue)
        {
            string text = story.Continue().Trim();
            //Newline filler word for a break
            if(text == @"NEWLINE")
            {
                targetDisplay.NewLine();
                AdvanceStory();
                return;
            //Newpage filler word to change the page
            } else if(text == @"NEWPAGE")
            {
                targetDisplay.RemoveText();
                AdvanceStory();
                return;
            }
            targetDisplay.CreateText(text);
            Debug.Log(text);
            targetDisplay.NewLine();
            return;
        } //otherwise, we're either done or waiting for a choice
        //if we're at a choice, but havent started waiting yet
        else if (story.currentChoices.Count > 0)
        {             
            //if We're waiting for a choice
            if (!waitForChoice)
            {
                //start waiting, and display the choices
                waitForChoice = true;
                bool logging = targetDisplay.logging;
                targetDisplay.logging = false;
                for (int i = 0; i < story.currentChoices.Count; i++)
                {
                    string ct = story.currentChoices[i].text.Trim();
                    //Debug.Log(ct);
                    targetDisplay.NewLine();
                    targetDisplay.CreateText(ct);
                }
                targetDisplay.logging = logging;
            }
        } else
        {
            //TODO: Add logic for end of story
            Debug.Log("End of Story!");
            return;
            //End of story
        }

    }

    public bool IsWaitingForChoice()
    {
        return waitForChoice;
    }

    //returns true if we made a successful choice
    public bool MakeChoice(int i)
    {
        Debug.Log("Num Choice: " + story.currentChoices.Count + ", choosing: " + i);
        //If the director sends a bad message
        if (i >= story.currentChoices.Count)
        {
            //Log the error and ignore the message received
            //TODO: Implement proper communication for this case
            Debug.Log("Invalid choice recieved!");
            choiceInput.ClearLastChoice();
            return false;
        }
        story.ChooseChoiceIndex(i);
        targetDisplay.storyChoiceLog += "->" + i;
        waitForChoice = false;
        return true;
    }


}
