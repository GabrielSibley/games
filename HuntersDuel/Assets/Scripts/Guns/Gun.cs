using UnityEngine;
using System.Collections;

public enum BulletState{
	Empty,
	Live,
	Fired
}

public enum GunType {
	Rifle,
	Pistol,
	Shotgun
};

public abstract class Gun : MonoBehaviour {

	public abstract int Damage { get; }
	public abstract float WalkSpeed { get; }
	public abstract GunType GunType { get; }
	public abstract bool Fire(BasicMove player, float angle); //Returns true if gun actually fires

	public abstract void ManipulateUp();
	public abstract void ManipulateDown();
	public abstract void ManipulateLeft();
	public abstract void ManipulateRight();

	public virtual void SpawnBullet(GameObject bulletPrefab, float bulletSpeed, BasicMove player, float angle){
		Quaternion dir = Quaternion.Euler(0, 0, angle);
		GameObject bullet = Instantiate(bulletPrefab, player.transform.position + dir * new Vector3(Bullet.bulletSpawnDistance, 0, 0), Quaternion.identity) as GameObject;
		bullet.rigidbody.velocity = dir * new Vector3(bulletSpeed, 0, 0);
		bullet.GetComponent<Bullet>().damage = Damage;
		bullet.GetComponent<Bullet>().owner = player.m_player;
		Physics.IgnoreCollision(bullet.collider, player.collider);
	}

}
