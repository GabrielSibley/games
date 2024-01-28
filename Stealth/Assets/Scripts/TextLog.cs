using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextLog : MonoBehaviour {

	public static List<string> log;
	
	void Awake(){
		log = new List<string>();
		AddEntry("--Mission Start--");
	}
	
	public static void AddEntry(string message){
		log.Insert(0, Utility.FormatTime(Time.time) +": " +message);
	}
	
	/*
	void OnGUI(){
		GUILayout.BeginArea(new Rect(240, Screen.height - 240, 400, 240));
		for(int i = 0; i < 8 && i < log.Count; i++){
			GUILayout.Label(log[i]);
		}
		GUILayout.EndArea();
	}
	*/
}
