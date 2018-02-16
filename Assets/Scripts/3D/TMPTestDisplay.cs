using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPTestDisplay : MonoBehaviour {

    public string text;
    public GameObject textPrefab;

    private List<GameObject> activeBounds;


    // Use this for initialization
    void Start () {

        activeBounds = new List<GameObject>();

        text = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void UpdateBounds()
    {
        //remove current bounds

        if (activeBounds.Count > 0)
        {
            activeBounds.ForEach(o => Destroy(o));
        }


        TextMeshPro m_Text = this.GetComponent<TextMeshPro>();

        m_Text.text = text;
        //m_Text.autoSizeTextContainer = true;

        m_Text.ForceMeshUpdate();

        

        for (int i = 0; i < m_Text.textInfo.wordCount; i++)
        {

            var wordInfo = m_Text.textInfo.wordInfo[i];
            var charInfoArr = m_Text.textInfo.characterInfo;

            var start = charInfoArr[wordInfo.firstCharacterIndex];
            var end = charInfoArr[wordInfo.lastCharacterIndex];

            float maxy = float.MinValue;
            for(int j = wordInfo.firstCharacterIndex; j <= wordInfo.lastCharacterIndex; j++)
            {
                if(maxy < charInfoArr[j].topRight.y)
                {
                    maxy = charInfoArr[j].topRight.y;
                }
            }
            

            Vector3 min = this.transform.TransformPoint(start.bottomLeft);

            Vector3 max = end.topRight;
            max.y = maxy;
            max = this.transform.TransformPoint(max);


            string word = wordInfo.GetWord();
            Debug.Log(word + " min: " + min);
            Debug.Log(word + " max: " + max);

            var center = (max + min) / 2;
            Debug.Log(word + " center: " + center);

            var wordBounds = new GameObject();
            activeBounds.Add(wordBounds);
            wordBounds.name = "Bounds_" + word;
            var coll = wordBounds.AddComponent<BoxCollider>();
            coll.transform.SetParent(this.transform);
            coll.center = center;

            coll.size = max - min;


        }
    }

    public void RemoveText()
    {
        text = "";
        UpdateBounds();
    }

    public void NewLine()
    {
        text += "\n";
    }

    public void CreateText(string newtext)
    {
        text += newtext;
        UpdateBounds();
    }
}
