using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Entity{
	
	public static Player m_instance;
	
	protected static List<ItemObjective> m_objectives = new List<ItemObjective>();
	
	public List<ItemHolder> Inventory{get{return m_inventory;}}
	public List<ItemHolder> Hands{get{return m_hands;}}
	
	public int m_numInventorySlots = 12;
	public List<Item> m_debugStartingItems;
	public LineRenderer[] m_accuracyRenderers;
	
	protected List<ItemHolder> m_hands;
	protected List<ItemHolder> m_inventory;
	
	public static List<ItemObjective> Objectives{
		get{return m_objectives;}
	}
	
	public Guard TargetGuard{
		get; set;
	}

	public void Awake(){
		Player.m_instance = this;
		m_inventory = new List<ItemHolder>();
		for(int i = 0; i < m_numInventorySlots; i++)
			m_inventory.Add(new ItemHolder());
		if(MainMenu.playerLoadout == null){
			foreach(Item i in m_debugStartingItems){
				ItemHolder ih = GetInventorySlot();
				if(ih != null)
					ih.Hold(i.Spawn());
			}
		}
		else{
			foreach(Item i in MainMenu.playerLoadout){
				ItemHolder ih = GetInventorySlot();
				if(ih != null)
					ih.Hold(i.Spawn());
			}
		}
		m_hands = new List<ItemHolder>();
		m_hands.Add(new ItemHolder());
		m_hands.Add(new ItemHolder());
	}
	
	public void Update(){
		
		//Movement
		Vector3 moveDirection = Vector3.zero;
		
		if(Input.GetKey(KeyCode.W)){
			moveDirection.z = 1;
		}
		if(Input.GetKey(KeyCode.S)){
			moveDirection.z = -1;
		}
		if(Input.GetKey(KeyCode.A)){
			moveDirection.x = -1;
		}
		if(Input.GetKey(KeyCode.D)){
			moveDirection.x = 1;
		}
		
		moveDirection = moveDirection.normalized * moveSpeed * Time.deltaTime;
		
		GetComponent<CharacterController>().Move(moveDirection);
		
		//Facing
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
		Vector3 offset = worldPoint - transform.position;
		offset.y = 0;
		transform.rotation = Quaternion.LookRotation(offset);
		
		//Actions
		foreach(ItemHolder ih in m_hands){
			ih.Update();
		}
		
		UpdateAccuracyRenderers();
	}
	
	public ItemHolder FreeHand(){
		return(m_hands.Find(delegate (ItemHolder ih){return ih.IsFree;}));
	}
	
	public int TotalItemWeight(){
		int weight = 0;
		foreach(ItemHolder ih in Inventory){
			if(ih.held != null)
				weight += ih.held.Weight;
		}
		foreach(ItemHolder ih in Hands){
			if(ih.held != null)
				weight += ih.held.Weight;
		}
		return weight;
	}
	
	public ItemHolder GetInventorySlot(){
		return Inventory.Find(delegate(ItemHolder ih){return ih.IsFree;});
	}
	
	public override float[] GetShotDeviationModifiers(Firearm weapon){
		float[] result = new float[2];
		result[0] = 1;
		result[1] = FreeHand() != null ? 0 : weapon.oneHandedDeviationModifier;
		return result;
	}
	
	protected void UpdateAccuracyRenderers(){
		//Weapon arcs
		Firearm gunLeft = Hands[0].held as Firearm;
		if(gunLeft != null){
			float acc = FireWeapon.GetShotDeviation(this, gunLeft);
			SetDisplayedDeviation(m_accuracyRenderers[0], acc);
		}
		else{
			SetDisplayedDeviation(m_accuracyRenderers[0], 0);
		}
		Firearm gunRight = Hands[1].held as Firearm;
		if(gunRight != null){
			float acc = FireWeapon.GetShotDeviation(this, gunRight);
			SetDisplayedDeviation(m_accuracyRenderers[1], acc);
		}
		else{
			SetDisplayedDeviation(m_accuracyRenderers[1], 0);
		}
		//Two guns -> no aiming
		if(gunLeft && gunRight){
			foreach(Guard g in LevelLogic.guards){
				g.m_accuracyDisplay.Power = 0;
			}
		}
		else{
			Firearm gun = gunLeft ?? gunRight;
			if(gun != null){
				//Increase aiming power on visible guards
				foreach(Guard g in LevelLogic.guards){
					float range = Vector3.Distance (g.transform.position, transform.position);
					g.m_accuracyDisplay.Multiplier = gun.damage;
					g.m_accuracyDisplay.KillDamage = g.health;
					g.m_accuracyDisplay.MaxPower = gun.maxAimPower + 1 - Mathf.Pow (2, range / gun.maxPowerHalfRange);
					if(!gun.holder.InUse && !Utility.RaisedLineCast(transform.position, g.transform.position, LayerSets.terrain)){
						g.m_accuracyDisplay.Power += Time.deltaTime * gun.powerGain / Mathf.Pow (2, range / gun.powerGainHalfRange);
					}
					else{
						g.m_accuracyDisplay.Power = 0;
					}
				}
				//Determine Target Guard
				TargetGuard = null;
				float minRange = float.MaxValue;
				Vector3 mouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
				mouse.y = 0;
				foreach(Guard g in LevelLogic.guards){
					float d = (g.transform.position - mouse).sqrMagnitude;
					if(d < minRange){
						minRange = d;
						TargetGuard = g;
					}
				}
			}
		}
	}
	
	protected void SetDisplayedDeviation(LineRenderer lr, float deviation){
		Vector3 firingFrom = WeaponOrigin();
		Vector3 toMouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1)) - firingFrom;
		toMouse.y = 0;
		int count = 17;
		lr.SetVertexCount(count);
		for(int i = 0; i < count; i++){
			float deviationFraction = (i - count/2)/(float)(count/2);
			lr.SetPosition(i, firingFrom + Quaternion.Euler(0, deviation * deviationFraction, 0) * toMouse + Vector3.up * 5f);
		}
	}
}
