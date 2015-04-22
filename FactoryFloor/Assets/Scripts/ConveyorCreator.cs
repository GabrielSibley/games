using UnityEngine;
using System.Collections;

public class ConveyorCreator : MonoBehaviour {

	public ConveyorCreatorOverlay editingOverlay;

	private static ConveyorCreator instance;

	private Tile targetTile;

	public static void StartEditing(Tile target) {
		instance.Edit(target);
	}

	private void Awake(){
		instance = this;
	}

	private void Edit(Tile target){
		targetTile = target;
		editingOverlay.Show(target);
	}
}
