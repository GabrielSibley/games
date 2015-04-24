using UnityEngine;
using System.Collections;

public abstract class TileFeature : MonoBehaviour, IInputReceiver {

	public Tile CurrentTile;

	public abstract void MoveToTile(Tile t);

	public abstract void Remove();

	public virtual void OnInputDown(){
	}
}
