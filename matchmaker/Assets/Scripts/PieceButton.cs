using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PieceButton : MonoBehaviour {

    public int x;
    public int y;
    public PieceType type;

    public void SetType(PieceType type)
    {
        this.type = type;
        switch (type)
        {
            case PieceType.Heart:
                GetComponent<Image>().color = Color.red;
                break;
            case PieceType.Gem0:
                GetComponent<Image>().color = Color.yellow;
                break;
            case PieceType.Gem1:
                GetComponent<Image>().color = Color.green;
                break;
            case PieceType.Gem2:
                GetComponent<Image>().color = Color.blue;
                break;
            case PieceType.Gem3:
                GetComponent<Image>().color = Color.cyan;
                break;
            case PieceType.Gem4:
                GetComponent<Image>().color = Color.white;
                break;
        }
    }
}
