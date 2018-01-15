using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextMeshDisplay : MonoBehaviour
{


    //Public Serializables
    public Camera cam;

    //public TextMeshProUGUI m_Test;

    public Canvas parent;
    public GameObject startPoint;

    public GameObject textPrefab;



    //Private
    string sentance;


    // Use this for initialization
    void Start()
    {
        sentance = "test words in the sentance with text mesh pro! I am going to write a run on sentance to test out how well the EOL works!";
        splitTextRect();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void splitTextRect()
    {
        Vector3 parentPos = parent.transform.position;
        List<string> words = new List<string>(sentance.Split(' '));

        Vector3 offset = new Vector3(0, 0, 0);

        Transform trans = startPoint.transform;

        //Debug vars and assignments
        var pX = parent.GetComponent<RectTransform>().sizeDelta.x; //Get width of canvas
        var sX = startPoint.transform.localPosition.x; // figure out where the start point is from the center
        sX += pX / 2; // This number gives us our right margin
        sX *= 2; //margin test
        var EOLoffset = pX - sX; //Subtract them to get our EOL number
        Debug.Log(pX + " - " + sX + " = " + EOLoffset);

        /*
         * Space character size get
         */
        var space = Instantiate(textPrefab);
        space.transform.SetParent(trans);
        space.transform.localScale = new Vector3(1, 1, 1);

        TextMeshProUGUI m_Space = space.GetComponent<TextMeshProUGUI>();
        m_Space.text = "␣"; //space
        m_Space.ForceMeshUpdate();
        RectTransform r_Space = space.GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(r_Space); //rebuild to get exact size of space
        var spaceDelta = r_Space.sizeDelta.x;
        Destroy(space); //remove it now that we're done with it




        bool firstWord = true;
        foreach (string s in words)
        {
            var word = Instantiate(textPrefab);
            word.transform.SetParent(trans);
            word.transform.localScale = new Vector3(1, 1, 1);

            

            //Text setup
            TextMeshProUGUI m_Text = word.GetComponent<TextMeshProUGUI>();

            m_Text.text = firstWord ? s:s + " ";
            if (firstWord) firstWord = false;

            m_Text.ForceMeshUpdate();

            //Refresh Fitter so we can use it in the script
            RectTransform rect = word.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);


            offset.x += rect.sizeDelta.x / 2;

            //Debug.Log("Half offset x: " + offset.x);


            //if we pass our margin, new line
            if(offset.x + rect.sizeDelta.x >= EOLoffset)
            {
                offset.x = 0;
                offset.x += rect.sizeDelta.x / 2;
                offset.y -= rect.sizeDelta.y * 1.5F; //move down a line and a half
            }

            word.transform.localPosition = offset;


            offset.x += rect.sizeDelta.x / 2; //move second half of x over
            offset.x += spaceDelta; //move over for a space
            //Debug.Log("Offset x: " + offset.x);



        
        }
    }

   
}
