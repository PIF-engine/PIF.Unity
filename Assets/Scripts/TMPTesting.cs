using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TMPTesting : MonoBehaviour {


    //Public Serializables
    public Camera cam;

    //public TextMeshProUGUI m_Test;

    public Canvas parent;
    public GameObject startPoint;

    public GameObject textPrefab;



    //Private
    string sentance;


	// Use this for initialization
	void Start () {
        sentance = "test words in the sentance with text mesh pro! I am going to write a run on sentance to test out how well the EOL works!";
        splitTextRect();
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    
    void splitTextRect()
    {
        Vector3 parentPos = parent.transform.position;
        List<string> words = new List<string>(sentance.Split(' '));

        Vector3 offset = new Vector3(0,0,0);

        Transform trans = startPoint.transform;

        //Debug vars and assignments
        var pX = parent.GetComponent<RectTransform>().sizeDelta.x; //Get width of canvas
        var sX = startPoint.transform.localPosition.x; // figure out where the start point is from the center
        sX += pX / 2; // This number gives us our right margin
        sX *= 2; //margin test
        var EOLoffset = pX - sX; //Subtract them to get our EOL number
        Debug.Log(pX + " - " + sX + " = " + EOLoffset);


        bool firstWord = true;
        foreach (string s in words)
        {
            var word = Instantiate(textPrefab);
            word.transform.SetParent(trans);
            word.transform.localScale = new Vector3(1, 1, 1);
            
            word.transform.localPosition = offset;



            //Debug.Log(word.transform.parent);
            //Debug.Log(parent.transform);





            //TextMeshProUGUI m_Text = word.AddComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
            //HorizontalLayoutGroup layout = word.AddComponent(typeof(HorizontalLayoutGroup)) as HorizontalLayoutGroup;
            //ContentSizeFitter fitter = word.AddComponent(typeof(ContentSizeFitter)) as ContentSizeFitter;

            //fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            //fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            TextMeshProUGUI m_Text = word.GetComponent<TextMeshProUGUI>();

            m_Text.text = firstWord ? s : " " + s;
            if (firstWord) firstWord = false;
            
            m_Text.ForceMeshUpdate();

            //fitter.
            RectTransform rect = word.GetComponent<RectTransform>();
            


            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

            //debug print of offset
             //Debug.Log("Offset for word: " + m_Text.text + " is " +offset.x);

            if (offset.x+ rect.sizeDelta.x >= EOLoffset)
            {
                Debug.Log("This word should be on a new line");
                offset.x = 0;
                offset.y -= 50; //magic number, calc later
                word.transform.localPosition = offset;
                m_Text.text = s;
                LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

            }





            // move offset (Probably)
            // find way to set x pivot to zero before offset update with width!
            //rect.pivot.Set(0, rect.pivot.y);
            offset.x += rect.sizeDelta.x;
            
            

            //Debug.Log(rect.sizeDelta.x);
            //offset.x += word.GetComponent<RectTransform>().rect.width * parent.scaleFactor / 2;

            // wordPos.y += (wordPos.y - oldPos.y);

            //parent test
            //trans = word.transform;

            //word
        }
    }
}
