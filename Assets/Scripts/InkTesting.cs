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

    //[SerializeField]
    public Canvas canvas;

    //private GameObject TMP;

    //private TextMeshProUGUI m_Text;

    //[SerializedField]
    public Camera cam;


    //For Snippet
    public Text textComp;
    public int charIndex;
    


    // Use this for initialization
    void Start () {

       // m_Text = canvas.GetComponentInChildren<TextMeshProUGUI>(); 
        StartStory();
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void StartStory()
    {
        story = new Story(storyJSON.text);
        AdvanceStory();
    }


    void AdvanceStory()
    {
        string text = story.Continue().Trim();
        Debug.Log(text);

        textComp.text = text;


       // m_Text.text = text;
        

        
    }

}
