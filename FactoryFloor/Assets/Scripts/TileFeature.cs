using UnityEngine;
using System.Collections;

public abstract class TileFeature : MonoBehaviour {

	public Tile CurrentTile;

	public abstract void AddToTile(Tile t);

	public abstract void Remove();

	public virtual void OnInputDown(){
	}
}
