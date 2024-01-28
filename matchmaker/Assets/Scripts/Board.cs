using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PieceType
{
    Heart,
    Gem0,
    Gem1,
    Gem2,
    Gem3,
    Gem4
}

public class Board : MonoBehaviour {

    public static float[] weights = new float[]
    {
        2, 3, 3, 3, 3, 3
    };

    public static PieceType GetRandomType()
    {
        return (PieceType)(RandomEx.RandomIndexWeighted(weights));
    }

    public static List<PieceType> GetRandomTypeOrder()
    {
        List<PieceType> bag = new List<PieceType>() { PieceType.Heart, PieceType.Gem0, PieceType.Gem1, PieceType.Gem2, PieceType.Gem3, PieceType.Gem4 };
        List<float> weightBag = new List<float>(weights);
        for(int i = 0; i < bag.Count - 1; i++)
        {
            int nextIndex = RandomEx.RandomIndexWeighted(weightBag);
            if(nextIndex != 0)
            {
                //Swap chosen element to position i
                var swap = bag[nextIndex+i];
                bag[nextIndex + i] = bag[i];
                bag[i] = swap;
            }
            weightBag[nextIndex] = weightBag[0];
            weightBag.RemoveAt(0);
        }
        
        return bag;
    }

    public PieceButton pieceButtonPrefab;

    public PieceButton[] pieces;

    [ContextMenu("Populate")]
    private void Populate()
    {
        pieces = new PieceButton[BoardState.width * BoardState.height];
        for(int y = 0; y < BoardState.height; y++)
        {
            for (int x = 0; x < BoardState.width; x++)
            {
                PieceButton pb = Instantiate(pieceButtonPrefab);
                pb.transform.SetParent(transform, false);
                pb.x = x;
                pb.y = y;
                pieces[x + y * BoardState.width] = pb;
            }
        }
        Canvas.ForceUpdateCanvases();
    }

    private void Start()
    {
        ResetBoard();
        FillBoard(false);
    }

    private void ResetBoard()
    {
        //TODO
    }

    //TODO: This resets the board state -- want it to fill gaps instead
    private void FillBoard(bool allowInstantMatches)
    {
        BoardState bs = new BoardState();
        float start = Time.realtimeSinceStartup;
        FillBoardRecursive(0, 0, bs, allowInstantMatches);
        float end = Time.realtimeSinceStartup;
        Debug.Log("Filled board in " + (end - start) + "sec");
        //copy to graphics
        for (int y = 0; y < BoardState.height; y++)
        {
            for (int x = 0; x < BoardState.width; x++)
            {
                pieces[x + y * BoardState.width].SetType((PieceType)(bs.GetSquare(x, y)));
            }
        }
    }

    private bool FillBoardRecursive(int x, int y, BoardState state, bool allowInstantMatches)
    {
        if(x >= BoardState.width)
        {
            x = 0;
            y++;
        }
        if(y >= BoardState.height)
        {
            return true; //Board filled successfully
        }
        if(!allowInstantMatches)
        {
            //WIP: Cycles through types deterministically instead of picking random order
            IList<PieceType> types = GetRandomTypeOrder();
            for(int t = 0; t < types.Count; t++)
            {
                //Provisionally set type
                state.SetSquare(x, y, (int)types[t]);
                if(state.CheckForMatches())
                {
                    continue; //Setting this square to this type causes a match. Try next type.
                }
                //Check if rest of board can be filled
                if(FillBoardRecursive(x+1, y, state, allowInstantMatches))
                {                    
                    return true; //Great!
                }                
            }
            //Board cannot be filled without allowing a match: Clear this square and backtrack
            state.ClearSquare(x, y);
            return false;
        }
        else
        {
            state.SetSquare(x, y, (int)GetRandomType()); //TODO: Still disallow heart matches
            return FillBoardRecursive(x+1, y, state, allowInstantMatches);
        }
    }
}
