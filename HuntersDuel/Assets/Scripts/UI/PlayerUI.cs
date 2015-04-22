using UnityEngine;
using System.Collections;

public class PlayerUI : MonoBehaviour {

	public RifleUI rifleUI;
	public ShotgunUI shotgunUI;
	public PistolUI pistolUI;
	public HealthUI healthUI;

	public Color BrightPlayerColor,
		DarkPlayerColor,
		LiveBulletColor,
		DeadBulletColor;

	public void Start(){
		rifleUI.SetColors(this);
		shotgunUI.SetColors(this);
		pistolUI.SetColors(this);
	}
}
