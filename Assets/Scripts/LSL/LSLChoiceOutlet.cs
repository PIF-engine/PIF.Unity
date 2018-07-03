using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;
using Assets.LSL4Unity.Scripts.Common;
using Assets.LSL4Unity.Scripts;

public class LSLChoiceOutlet : LSLMarkerStream {


    private const string unique_source_id = "A256CFBDAA314B8D8CFA64140A219D31";

    /// <summary>
    /// Sends a marker to the director requesting it to make a decision on what story tree to go down
    /// </summary>
    public void RequestResponceMarker()
    {
        Write("request");
    }

    public void ResponceRecieved()
    {
        Write("recieved");
    }

    /// <summary>
    /// Writes a marker at the begining of the stream to say what the story name is
    /// </summary>
    /// <param name="storyName">Name of the story</param>
    public void WriteStoryNameMarker(string storyName)
    {
        Write("STORY: " + storyName);
    }

    /// <summary>
    /// Writes a marker for the current decision tree of the story. Should be called every time this tree changes
    /// </summary>
    /// <param name="storyChoiceLog">The string representing the story choice log</param>
    public void WriteStoryTreeMarker(string storyChoiceLog)
    {
        Write("CHOICELOG: " + storyChoiceLog);
    }

    /// <summary>
    /// Writes a marker for the current page number. Should be called when the page changes
    /// </summary>
    /// <param name="pageNum">The page number that has has begun to be displayed</param>
    public void WriteStoryPageMarker(int pageNum)
    {
        Write("ENDPAGE: " + (pageNum-1));
        Write("NEXTPAGE: " + pageNum);
    }
}
