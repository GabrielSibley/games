using UnityEngine;
using System.Collections;

public class TitleManager : MonoBehaviour {

	public static GunType[] PlayerGuns = new GunType[2];

	public TitlePlayer[] players;

	// Update is called once per frame
	void Update () {
		if(players[0].IsReady && players[1].IsReady){
			PlayerGuns[0] = players[0].tutorial.ActiveGun.GunType;
			PlayerGuns[1] = players[1].tutorial.ActiveGun.GunType;
			Application.LoadLevel ("game");
		}
	}
}
