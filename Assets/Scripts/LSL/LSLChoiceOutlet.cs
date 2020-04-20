using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;
using Assets.LSL4Unity.Scripts.Common;
using Assets.LSL4Unity.Scripts;

public class LSLChoiceOutlet : LSLMarkerStream
{


    private const string unique_source_id = "A256CFBDAA314B8D8CFA64140A219D31";

    private string storyName = "";

    private bool requestSent = false;

    public void SetStoryName(string sname)
    {
        storyName = sname;
    }

    /// <summary>
    /// Sends a marker to the director requesting it to make a decision on what story tree to go down
    /// </summary>
    public void RequestResponceMarker()
    {
        if (requestSent) return;
        Write("request");
        requestSent = true;
    }

    public void ResponceRecieved()
    {
        Write("recieved");
        requestSent = false;
    }

    /// <summary>
    /// Writes a marker at the begining of the stream to say what the story name is
    /// </summary>
    /// <param name="storyName">Name of the story</param>
    public void WriteStoryNameMarkerStart()
    {
        Write("STORY_" + storyName + "_START", liblsl.local_clock());
    }
    public void WriteStoryNameMarkerEnd()
    {
        Write("STORY_" + storyName + "_END", liblsl.local_clock());
    }

    /// <summary>
    /// Writes a marker for the current decision tree of the story. Should be called every time this tree changes
    /// </summary>
    /// <param name="storyChoiceLog">The string representing the story choice log</param>
    public void WriteStoryTreeMarker(string storyChoiceLog)
    {
        Write("CHOICELOG_" + storyName + ": " + storyChoiceLog, liblsl.local_clock());
    }

    /// <summary>
    /// Writes a marker for the current page number. Should be called when the page changes
    /// </summary>
    /// <param name="pageNum">The page number that has has begun to be displayed</param>
    public void WriteStoryPageMarker(int pageNum)
    {
        Write("ENDPAGE_" + storyName + ": " + (pageNum - 1), liblsl.local_clock());
        Write("NEXTPAGE_" + storyName + ": " + pageNum, liblsl.local_clock());
    }

    public void WriteCustomMarker(string marker)
    {
        Write(marker, liblsl.local_clock());
    }

    public void WriteVariableMarker(string varname, System.Type type, object value)
    {
        Write("VARIABLEUPDATE:" + varname + ":" + type + ":" + value);
    }
}
