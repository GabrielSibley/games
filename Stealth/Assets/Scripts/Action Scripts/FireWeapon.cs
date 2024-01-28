using UnityEngine;
using System.Collections;

public class FireWeapon : Action {
	private Firearm weapon;
	private float timeToComplete;
	
	public static float GetShotDeviation(Entity actor, Firearm weapon){
		float deviation = weapon.unaimedDeviation;
		float[] actorMods = actor.GetShotDeviationModifiers(weapon);
		deviation = deviation * actorMods[0] + actorMods[1];
		return deviation;
	}
	
	public FireWeapon(Entity actor, ItemHolder source, ItemHolder destination, Item item) : base(actor, source, destination, item){
		name = "Fire";
		weapon = item as Firearm;
		timeToComplete = weapon.fireTime;
		
		//Check that action is actually valid
		if(!validAssignment)
			return;
		
		//FIRE!
		if(!weapon.isSemiAuto)
			weapon.isChambered = false;
		else{
			if(weapon.magazine != null && weapon.magazine.count > 0)
				weapon.magazine.count -= 1;
			else
				weapon.isChambered = false;
		}
		//Player marksmanship ability
		Player player = Player.m_instance;
		if(actor == player && player.TargetGuard != null && player.TargetGuard.m_accuracyDisplay.Power >= 1){
			player.TargetGuard.ModifyHealth(-weapon.damage * player.TargetGuard.m_accuracyDisplay.Power);
			WeaponEffect (actor.WeaponOrigin(), player.TargetGuard.transform.position, actor.transform.rotation);
		}
		else{
			for(int i = 0; i < weapon.numAttacks; i++){
				//Roll some dice!
				float deviation = GetShotDeviation(actor, weapon);
				
				RaycastResult shot = CastShot(deviation);
				if(shot.hit){
					Entity entityHit = shot.hitInfo.collider.GetComponent<Entity>();
					if(entityHit){
						entityHit.ModifyHealth(-weapon.damage);
					}
					//Launch weapon effect
					WeaponEffect (actor.WeaponOrigin(), shot.hitInfo.point, actor.transform.rotation);
				}
			}
		}
		SoundManager.WeaponFired(weapon);
		
		
		if(actor == player){
			//Make "noise" if appropriate
			LevelLogic.EmitSound(actor.Location, weapon.noise);
		}
	}
	
	public override void Update(){
		//Process cooldown
		timeToComplete -= Time.deltaTime;
		if(timeToComplete <= 0){
			OnCompletion();
		}
	}
	
	public override float TimeUntilCompletion(){
		return timeToComplete;
	}
	
	public override void Cancel (){
		//Can't cancel a shot, it's already done.
	}
	
	protected RaycastResult CastShot(float deviation){
		Vector3 firingFrom = actor.WeaponOrigin();
		Vector3 firingDirection = actor.FiringDirection();
		float actualDeviation = Random.Range(-deviation, deviation);
		firingDirection = Quaternion.Euler(0, actualDeviation, 0) * firingDirection;
		RaycastResult result;
		result.hit = Physics.Raycast(firingFrom, firingDirection, out result.hitInfo, 2000, Physics.kDefaultRaycastLayers);
		return result;
	}
	
	protected void WeaponEffect(Vector3 start, Vector3 end, Quaternion direction){
		Tracer t = Object.Instantiate(EffectsPrefabs.instance.tracerPrefab, start, Quaternion.identity) as Tracer;
		t.Fire(start, end);
		GameObject obj = (GameObject)Object.Instantiate(EffectsPrefabs.instance.shellCasing, start, direction);
		obj.rigidbody.AddRelativeForce(Random.Range(0.05f, 0.12f), Random.Range(0.05f, 0.12f), Random.Range(-0.01f, 0.01f), ForceMode.Impulse);
		obj.rigidbody.AddTorque(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), ForceMode.Impulse);
	}
}

public struct RaycastResult{
	public bool hit;
	public RaycastHit hitInfo;
}
