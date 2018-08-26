using System;
using System.Collections;
using Ink.Runtime;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class InkFOVEEventManager : MonoBehaviour
{

    //[SerializeField]
    public List<TextAsset> storyJSON;
    public TextAsset startingStory;
    private Story story;

    public GameObject TargetTMPDisplayPrefab;
    public GameObject LSLChoiceOutlet;
    public GameObject LSLChoiceInput;
    private TMPDisplayer targetDisplay;
    private LSLChoiceOutlet choiceOutlet;
    private LSLChoiceInlet choiceInput;

    private bool waitForChoice;
    private bool endOfCurrentStory;
    private bool endOfExperiment = false;

    public bool usingLSL;

    private bool onAdvanceCooldown;
    private bool setup = true;

    // Use this for initialization
    void Start()
    {

        targetDisplay = TargetTMPDisplayPrefab.GetComponent<TMPDisplayer>();
        if (LSLChoiceOutlet != null)
            choiceOutlet = LSLChoiceOutlet.GetComponent<LSLChoiceOutlet>();
        if (LSLChoiceInput != null)
            choiceInput = LSLChoiceInput.GetComponent<LSLChoiceInlet>();


        //Randomize the order of the stories
        var rand = new System.Random();
        storyJSON = storyJSON.OrderBy( (x) => (rand.Next())).ToList();
        if(startingStory != null) storyJSON.Insert(0,startingStory);//start with the starting story


        waitForChoice = false;

        // m_Text = canvas.GetComponentInChildren<TextMeshProUGUI>(); 
        StartStory();

    }

    private bool DoSetupCheck()
    {
        try
        { //fove connected
            bool inSet = FoveInterface.IsEyeTrackingCalibrating();
            if (inSet == false)
            {//setup of headset is done!
                setup = false;
                choiceOutlet.WriteCustomMarker("xp start");
                return false;
            }
        }
        catch (Exception)
        { //no FOVE connected
            setup = false;
            return false;
        }
        return true;
    }
    // Update is called once per frame
    void Update()
    {
        if (endOfExperiment) return;
        //if we're in setup, dont allow the story to advance until we're out of it
        if (setup)
        {
            if (DoSetupCheck()) return;
        }
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
                    choiceOutlet.RequestResponceMarker();
                }
                else
                {
                    clear = true;
                    advance = true;
                }
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
        else if (Input.GetKeyDown("space") || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            if (onAdvanceCooldown) return;
            onAdvanceCooldown = true;
            advance = true;
            StartCoroutine("ResetCooldown");
        }


        if (choice >= 0)
        {
            //attempt to make a choice. If its invalid, ignore the advance and clear flags
            if (!MakeChoice(choice)) return;
        }
        if (clear)
        {
            targetDisplay.RemoveText();
            // a valid choice has been made, reset LSL input if any, send back acknowledgment
            if (usingLSL)
            {
                choiceInput.ClearLastChoice();
                // NB: reding a "recieved" on keyboard input could confuse the Director?
                choiceOutlet.ResponceRecieved();
            }
            
        }

        if (!advance) return;
        if(endOfCurrentStory) { targetDisplay.RemoveText(); StartStory();}
        else AdvanceStory();


    }


    void StartStory()
    {
        
        if (storyJSON.Count == 0)
        {
            Debug.Log("End of story collection!");
            choiceOutlet.WriteCustomMarker("xp end");
            endOfExperiment = true;
            return;
        }

        endOfCurrentStory = false; //new story!
        story = new Story(storyJSON[0].text);
        targetDisplay.StartNewStory(storyJSON[0].name);
        choiceOutlet.WriteStoryNameMarkerStart();    
        AdvanceStory();
    }


    void AdvanceStory()
    {
        if (endOfExperiment) return;
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
            }
            if (text == @"NEWPAGE")
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

        if (story.currentChoices.Count > 0)
        {             
            //if We're waiting for a choice
            if (waitForChoice) return;
            //start waiting, and display the choices
            waitForChoice = true;
            bool logging = targetDisplay.logging;
            targetDisplay.logging = false;
            foreach (var t in story.currentChoices)
            {
                string ct = t.text.Trim();
                //Debug.Log(ct);
                targetDisplay.NewLine();
                targetDisplay.CreateText(ct);
            }
            targetDisplay.logging = logging;
        } else
        {
            targetDisplay.RemoveText();
            targetDisplay.CreateText("End of Story!");
            choiceOutlet.WriteStoryNameMarkerEnd();
            endOfCurrentStory = true;
            storyJSON.RemoveAt(0); //remove this story from the list
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
        choiceOutlet.WriteStoryTreeMarker(targetDisplay.storyChoiceLog);
        waitForChoice = false;
        return true;
    }


    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSecondsRealtime(.2F);
        onAdvanceCooldown = false;
    }

}
