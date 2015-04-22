using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour, IInputReceiver{

	public int x, y;

	public List<TileFeature> features = new List<TileFeature>();

	public void OnInputDown(){
		if(GameMode.Current == GameMode.Mode.RemoveBelt) {
			var featuresToRemove = features.ToArray ();
			foreach(TileFeature f in featuresToRemove){
				f.Remove();
			}
			UpdateConveyorGraph();
		}
		if(GameMode.Current == GameMode.Mode.AddBelt){
			if(features.Count == 0){
				var conveyor = Instantiate(PrefabManager.Conveyor, Vector3.zero, Quaternion.identity) as Conveyor;
				conveyor.AddToTile(this);
				//conveyor.transform.position = transform.position - Vector3.forward;
				//conveyor.tile = this;
			}
			else{
				features[0].OnInputDown();
			}
			UpdateConveyorGraph();
		}
		if(GameMode.Current == GameMode.Mode.AddCrate && features.Count > 0){
			features[0].OnInputDown();
			//CrateManager.CreateCrate(conveyor);
		}
		if(GameMode.Current == GameMode.Mode.RemoveCrate && features.Count > 0){
			features[0].OnInputDown();;
		}
	}

	private void UpdateConveyorGraph(){
		/*
		foreach(Tile t in FloorLayout.GetTiles()){
			if(t.conveyor){
				t.conveyor.ClearEdges();
			}
		}
		foreach(Tile t in FloorLayout.GetTiles()){
			if(t.conveyor){
				t.conveyor.UpdateEdges();
			}
		}
		*/
	}
}
