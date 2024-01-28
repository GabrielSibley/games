using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Guard : Entity {
	
	public Firearm spawnWeapon;
	public Magazine ammoSupply;
	public bool isOnTarget;
	public float visionArc = 100;
	public bool CanSeePlayer{
		get{return canSeePlayer;}
	}
	
	public Material normalMaterial;
	public Material suspiciousMaterial;
	public Material searchingMaterial;
	public Material alertMaterial;
	
		
	public float suspicion;
	
	public Firearm Gun{
		get{
			return gun;
		}
	}
	
	public AIState<Guard> CurrentAIState{
		get{
			if(aiStates.Count > 0){
				return aiStates[aiStates.Count-1];
			}
			else{
				return null;
			}
		}
	}
	
	public AccuracyDisplay m_accuracyDisplay;
	
	public Material DisplayMode{
		get{return m_mainRenderer.material;}
		set{m_mainRenderer.material = value;}
	}
		
	public Renderer m_mainRenderer;
	
	protected bool canSeePlayer;
	protected ItemHolder gunHand;
	protected Firearm gun;
	protected List<AIState<Guard>> aiStates;	
		
	public void Awake(){
		actions = new List<Action>();
		gunHand = new ItemHolder();
		gun = spawnWeapon.Spawn() as Firearm;
		gun.magazine = ammoSupply.Spawn() as Magazine;
		gunHand.Hold(gun);
		aiStates = new List<AIState<Guard>>();
	}
	
	public void ChangeState(AIState<Guard> newState){
		if(CurrentAIState != null){
			CurrentAIState.OnStateExited(this);
			aiStates[aiStates.Count - 1] = newState;
			CurrentAIState.OnStateEntered(this);
		}
		else{
			PushState(newState);			
		}
	}
	
	public void PushState(AIState<Guard> newState){
		if(CurrentAIState != null){
			CurrentAIState.OnStateExited(this);
		}
		aiStates.Add(newState);
		CurrentAIState.OnStateEntered(this);
	}
	
	public void PopState(){
		if(CurrentAIState != null){
			CurrentAIState.OnStateExited(this);
			aiStates.RemoveAt(aiStates.Count - 1);
			if(CurrentAIState != null){
				CurrentAIState.OnStateEntered(this);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		//LoS
		if(LookForPlayer()){
			if(!canSeePlayer){
				LevelLogic.guardsSeeingPlayer += 1;
			}
			canSeePlayer = true;
		}
		else{
			if(canSeePlayer){
				LevelLogic.guardsSeeingPlayer -= 1;
				if(LevelLogic.guardsSeeingPlayer == 0){
					LevelLogic.PlayerBrokeLoS();
				}
			}
			canSeePlayer = false;
		}
		//Standard actions
		if(CurrentAIState != null){
			CurrentAIState.Update(this);
		}
		else{
			Debug.LogWarning("Guard went gone brain-dead", this);
			PushState(new GuardWanderState());
		}

		//Process misc actions
		foreach(Action act in actions){
			act.Update();
		}
		foreach(Action act in actions.FindAll(delegate(Action a){return a.completed || a.cancelled;})){
			actions.Remove(act);
		}
		
		//Weapon manangement
		WeaponUpdate();
	}
	
	private bool LookForPlayer(){
		if(Player.m_instance == null || Time.time < 10){
			return false;
		}
		Vector3 playerPos = Player.m_instance.transform.position;
		Vector3 guardToPlayer = playerPos - transform.position;
		return Vector3.Angle(transform.forward, guardToPlayer) <= visionArc/2 && !Utility.RaisedLineCast(transform.position, playerPos, LayerSets.terrain);
		
	}
	
	public bool InVisionArc(GridSquare sq){
		Vector3 guardToPoint = sq.transform.position - transform.position;
		return Vector3.Angle(transform.forward, guardToPoint) <= visionArc / 2;
	}
	
	public bool HasLineOfSight(GridSquare sq){
		return InVisionArc(sq) && !Utility.RaisedLineCast(transform.position, sq.transform.position, LayerSets.terrain);
	}
	
	/* Guards don't like moving along walls and don't like cutting across corners
		Therefore, the distance from one square to the next is augmented by the number of neighbours
		the destination square has (+5% per neighbour below 8)
		There is a +300% penalty if a corner is cut (the two squares have less than 2 neighbours in common)
	*/
	public static float GuardPathfindingWeight(GridSquare a, GridSquare b){
		float nearWallPenalty = 0.05f * (8-a.neighbours.Count);
		float cornerCutPenalty = a.neighbours.Where(sq => b.neighbours.Contains(sq)).Count() < 2 ? 3 : 0;
		return AStar.EuclideanPathfindingWeight(a, b) * (1 + nearWallPenalty + cornerCutPenalty);
	}
	
	public override void OnDie(){
		LevelLogic.guards.Remove(this);
		if(canSeePlayer)
			LevelLogic.guardsSeeingPlayer -= 1;
		base.OnDie();
	}
	
	protected void WeaponUpdate(){
		if(!gunHand.InUse){ //Already doing something, leave alone
			if(gun.magazine != null && gun.magazine.count > 0){
				//Proper care and feeding
				if(!gun.isChambered){
					gunHand.SetAction(new ChamberWeapon(this, gunHand, gun));
				}
				//There he is!
				else if(canSeePlayer && isOnTarget){
					gunHand.SetAction(new FireWeapon(this, gunHand, null, gun));
				}
			}
			//Out of ammo, reload
			else{
				//Take old magazine out of gun
				if(gun.magazine != null){
					gunHand.SetAction(new UnloadWeapon(this, gunHand, null, gun));
				}
				//Load new magazine
				else{
					ItemHolder tempHand = new ItemHolder();
					tempHand.Hold(ammoSupply.Spawn() as Magazine);
					gunHand.SetAction(new LoadWeapon(this, gunHand, tempHand, gun));
				}
			}
		}
		gunHand.Update();
	}
}
