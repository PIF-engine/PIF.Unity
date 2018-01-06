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

   // [SerializeField]
    private GameObject TMP;

    private TextMeshProUGUI m_Text;

    Camera cam;


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


        
    }


    //test snippet

    void PrintPos()
    {
        string text = textComp.text;

        if (charIndex >= text.Length)
            return;

        TextGenerator textGen = new TextGenerator(text.Length);
        Vector2 extents = textComp.gameObject.GetComponent<RectTransform>().rect.size;
        textGen.Populate(text, textComp.GetGenerationSettings(extents));

        int newLine = text.Substring(0, charIndex).Split('\n').Length - 1;
        int whiteSpace = text.Substring(0, charIndex).Split(' ').Length - 1;
        int indexOfTextQuad = (charIndex * 4) + (newLine * 4) - 4;
        if (indexOfTextQuad < textGen.vertexCount)
        {
            Vector3 avgPos = (textGen.verts[indexOfTextQuad].position +
                textGen.verts[indexOfTextQuad + 1].position +
                textGen.verts[indexOfTextQuad + 2].position +
                textGen.verts[indexOfTextQuad + 3].position) / 4f;

            print(avgPos);
            PrintWorldPos(avgPos);
        }
        else
        {
            Debug.LogError("Out of text bound");
        }
    }

    void PrintWorldPos(Vector3 testPoint)
    {
        Vector3 worldPos = textComp.transform.TransformPoint(testPoint);
        print(worldPos);
        new GameObject("point").transform.position = worldPos;
        Debug.DrawRay(worldPos, Vector3.up, Color.red, 50f);
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 80), "Test"))
        {
            PrintPos();
        }
    }

}
