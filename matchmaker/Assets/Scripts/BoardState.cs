using UnityEngine;
using System.Collections;

public class BoardState {

    public const int typeCount = 5;
    public const int width = 8;
    public const int height = 8;

    private int[,] rows = new int[typeCount, height];
    private int[,] cols = new int[typeCount, width];

    public void SetSquare(int x, int y, int pType)
    {
        //E.g. set 1, 3 to T
        //row 3 gets masked with 1 << 1
        //col 1 gets masked with 1 << 3
        int rowMask = 1 << x;
        int colMask = 1 << y;
        for(int i = 0; i < typeCount; i++)
        {
            if(i == pType)
            {
                //Set bit
                rows[i, y] |= rowMask;            
                cols[i, x] |= colMask;
            }
            else
            {
                //Clear bit
                rows[i, y] &= ~rowMask;
                cols[i, x] &= ~colMask;
            }
        }
    }

    public void ClearSquare(int x, int y)
    {
        SetSquare(x, y, -1);
    }

    //Returns the type in the square, or -1 if empty
    public int GetSquare(int x, int y)
    {
        for (int i = 0; i < typeCount; i++)
        {
            int mask = 1 << x;
            if ((rows[i, y] & mask) == mask)
            {
                return i;
            }
        }
        return -1;
    }

    public bool CheckForMatches()
    {
        for(int i = 0; i < typeCount; i++)
        {
            //check cols for vertical matches
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height - 2; y++)
                {
                    int mask = 7 << y; //Three raised bits in a row
                    if ((cols[i, x] & mask) == mask)
                    {
                        return true; // match detected
                    }
                }
            }
            //check rows for horizontal matches
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width - 2; x++)
                {
                    int mask = 7 << x; //Three raised bits in a row
                    if ((rows[i, y] & mask) == mask)
                    {
                        return true; // match detected
                    }
                }
            }
        }
        return false;
    }

    //Constructs human-readable string representing board
    //(0,0) is at top left of result string
    public string ToString(bool cols = false)
    {
        string result = "";        
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                int typeInSq = -1;
                for(int i = 0; i < typeCount; i++)
                {
                    int mask = 1 << x;
                    if((rows[i, y] & mask) == mask)
                    {
                        //Sanity check
                        if(typeInSq != -1)
                        {
                            Debug.LogError("Ahh multiple types in the same square help");
                        }
                        typeInSq = i;
                    }
                }
                if(typeInSq == -1)
                {
                    result += ".";
                }
                else
                {
                    result += typeInSq;
                }
            }
            result += "\n";
        }
        return result;
    }
}
