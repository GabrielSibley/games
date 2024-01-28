using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class BoardStateTests  {


    [MenuItem("Tests/BoardState Unit Test")]
	public static void RunUnitTest()
    {
        Debug.Log("Running boardstate unit tests...");
        //Empty board should have no match
        if(new BoardState().CheckForMatches())
        {
            Debug.LogError("[FAIL] New board has match");
        }
        //Test all expected match setups
        for(int type = 0; type < BoardState.typeCount; type++)
        {
            for(int x = 0; x < BoardState.width - 2; x++)
            {
                for(int y = 0; y < BoardState.height - 2; y++)
                {
                    //Horiz
                    BoardState bs = new BoardState();
                    bs.SetSquare(x, y, type);
                    bs.SetSquare(x + 1, y, type);
                    bs.SetSquare(x + 2, y, type);
                    if(!bs.CheckForMatches())
                    {
                        Debug.LogError("[FAIL] Did not detect horiz match t=" + type + " x=" + x + " y=" + y);
                        Debug.Log(bs.ToString());
                    }
                    //Vert
                    bs = new BoardState();
                    bs.SetSquare(x, y, type);
                    bs.SetSquare(x, y+1, type);
                    bs.SetSquare(x, y+2, type);
                    if (!bs.CheckForMatches())
                    {
                        Debug.LogError("[FAIL] Did not detect vert match t=" + type + " x=" + x + " y=" + y);
                        Debug.Log(bs.ToString());
                    }
                }
            }
        }
    }

    [MenuItem("Tests/BoardState Check Perf")]
    public static void RunPerfTest()
    {
        //Generate random boards
        int testCount = 100000;
        BoardState[] states = new BoardState[testCount];
        for(int i = 0; i < testCount; i++)
        {
            states[i] = new BoardState();
            for(int x = 0; x < BoardState.width; x++)
            {
                for(int y = 0; y < BoardState.height; y++)
                {
                    states[i].SetSquare(x, y, Random.Range(0, BoardState.typeCount));
                }
            }            
        }

        double startTime = EditorApplication.timeSinceStartup;
        for(int i = 0; i < testCount; i++)
        {
            states[i].CheckForMatches();
        }
        double endTime = EditorApplication.timeSinceStartup;
        Debug.Log(testCount + " boards evaluated in " + (endTime - startTime) + " (" + (endTime - startTime) / testCount * 1000 + " ms/board)");
    }
}
