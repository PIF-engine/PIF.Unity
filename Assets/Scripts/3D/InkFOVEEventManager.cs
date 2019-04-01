using System;
using System.Collections;
using Ink.Runtime;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Random = System.Random;

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

    private Dictionary<string, object> variableStates;

    private bool waitForChoice;
    private bool endOfCurrentStory;
    private bool endOfExperiment = false;

    public bool usingLSL;

    private bool onAdvanceCooldown;
    private bool setup = true;


    private static bool varUpdateReady = false;
    private static string varUpdateCode = "";

    // Use this for initialization
    private void Start()
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

        //Sends the initial vars to the director as soon as its connected
        StartCoroutine(SendInitialVarsToDirector());
        //And start listening for updates
        StartCoroutine(UpdateVars());

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
    private void Update()
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

        //If our vars have changed here, it means that the change did not originate from LSL
        if (HasVarsChanged())
        {
            Debug.Log("VARS CHANGED!");
            UpdateVarDictionary(true);
        }

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


    private void StartStory()
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
        choiceOutlet.SetStoryName(storyJSON[0].name);
        choiceOutlet.WriteStoryNameMarkerStart();    
        UpdateVarDictionary(true);
        AdvanceStory();
    }

    /// <summary>
    /// Worker function for updating our variable dictionary
    /// </summary>
    /// <param name="sendToDirector">Do we push this update to the director?</param>
    private void UpdateVarDictionary(bool sendToDirector)
    {
        variableStates = new Dictionary<string, object>();
        VariablesState state = story.variablesState;
        foreach (string v in state)
        {
            Debug.Log("name: " + v+ ", type: " + state[v].GetType() + ", value: " + state[v]);
            if(sendToDirector) choiceOutlet.WriteVariableMarker(v,state[v].GetType(),state[v]);
            variableStates.Add(v,state[v]);
        }
    }

    /// <summary>
    /// Simple function to tell us if a variable has changed state
    /// </summary>
    /// <returns>returns true if our variabledictionary is out of sysnc with the story</returns>
    public bool HasVarsChanged()
    {
        VariablesState state = story.variablesState;
        foreach (string v in state)
        {
            //Debug.Log("Var is " + v + ", " + variableStates[v] + " : " + state[v]);
            if (variableStates[v].Equals( state[v])) continue;
            Debug.Log("Vars have changed");
            Debug.Log("Var is " + v + ", " + variableStates[v] + " : " + state[v]);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Sets the story state to the variable dictionary (AKA overwrites it)
    /// </summary>
    public void SetStateToDict()
    {
        VariablesState state = story.variablesState;
        foreach(string v in state)
        {
            state[v] = variableStates[v];
        }
    }

    /// <summary>
    /// Sets the variable dictionary to the story's state (AKA Updates it)
    /// </summary>
    public void SetDictToState()
    {
        UpdateVarDictionary(false);
    }

    /// <summary>
    /// Makes a change to the variabledictionary. Call SetStateToDict() afterward to update the story's state
    /// </summary>
    /// <param name="varname">Name of the variable to be changed</param>
    /// <param name="value">Value it is changed to</param>
    public void UpdateDict(string varname, object value)
    {
        story.variablesState[varname] = value;
        UpdateVarDictionary(true);
    }




    private void AdvanceStory()
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
            if (text.StartsWith("##") )
            {
                choiceOutlet.WriteCustomMarker(text);
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
            if (waitForChoice)
            {
                //We pick a random branch to go down
                int choiceCount = story.currentChoices.Count;
                var r = new Random();
                MakeChoice(r.Next(0, choiceCount));
                targetDisplay.RemoveText(); //Clear screen for the test runs
                return;
            }
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


    public void ProcessUpdateFromDirector(string message)
    {
        string[] updates = message.Split(';');
        foreach (string s in updates)
        {
            if (s.Equals("")) continue;
            var comp = s.Split(':');
            UpdateDict(comp[0],comp[1]);
            Debug.Log("Processed update from director: " + comp[0] + "," + comp[1]);
        }
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSecondsRealtime(.2F);
        onAdvanceCooldown = false;
    }


    private IEnumerator SendInitialVarsToDirector()
    {
        yield return new WaitUntil(() => choiceInput.IsConnected());
        yield return new WaitForSecondsRealtime(1F);
        Debug.Log("UPDATED VAR DICT ON STARTUP");
        UpdateVarDictionary(true);
    }

    private IEnumerator UpdateVars()
    {
        while (true)
        {
            yield return new WaitUntil(() => varUpdateReady);
            varUpdateReady = false;
            ProcessUpdateFromDirector(varUpdateCode);
            varUpdateCode = "";
        }
    }

    public static void DoDirectorUpdate(string message)
    {
        
        varUpdateCode = message;
        varUpdateReady = true;
    }

}
