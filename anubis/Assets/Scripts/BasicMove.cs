using UnityEngine;
using System.Collections;

public class BasicMove : MonoBehaviour {
		
	public int playerIndex;

	public new SpriteRenderer renderer;
	public SpriteRenderer carriedRenderer;

	public Sprite normalSprite;
	public Sprite carryingSprite;
	public float moveSpeed = 16;
	public Altar altar;
	public GameObject otherPlayer;
	
	public int CarriedGlyph
	{
		get
		{
			return carriedGlyph;
		}
		set
		{
			carriedGlyph = value;
			carriedRenderer.sprite = Glyphs.GetGlyph(value);
			if(value >= 0)
			{
				renderer.sprite = carryingSprite;
			}
			else
			{
				renderer.sprite = normalSprite;
			}
		}
	}
	private int carriedGlyph;

	public bool isAlive = true;

	private bool endgame;
	private bool canCollectOtherPlayer;

	public void Endgame(bool winner)
	{
		CarriedGlyph = -1;
		endgame = true;
		canCollectOtherPlayer = winner;
	}

	void Start() {
		Application.targetFrameRate = 60;
		CarriedGlyph = -1;
	}

	void Update () {
		rigidbody.velocity = Vector3.zero;

		if(isAlive){
			if(Winnput.ADown[playerIndex])
			{
				if(!endgame)
				{
					//Collect / swap glyph
					float xRange = 0.08f;
					float yRange = 0.12f;
					for(int i = 0; i < Vase.All.Count; i++)
					{
						Vase v = Vase.All[i];
						if(Mathf.Abs (v.transform.position.x - transform.position.x) <= xRange
						   && Mathf.Abs (v.transform.position.y - transform.position.y) <= yRange
						   )
						{
							int temp = CarriedGlyph;
							CarriedGlyph = v.glyph;
							v.glyph = temp;
						}
					}
				}
				else if(canCollectOtherPlayer && otherPlayer != null)
				{
					float xRange = 0.12f;
					float yRange = 0.18f;
					if(Mathf.Abs(otherPlayer.transform.position.x - transform.position.x) <= xRange
					   && Mathf.Abs (otherPlayer.transform.position.y - transform.position.y) <= yRange
					   )
					{
						CarriedGlyph = 17 - playerIndex;
						Destroy(otherPlayer);
					}
				}

				//Sacrifice
				if(CarriedGlyph == Game.Instance.Demand){
					float xRange = 0.16f;
					float yRange = 0.14f;
					if(Mathf.Abs (altar.transform.position.x - transform.position.x) <= xRange
					   && Mathf.Abs (altar.transform.position.y - transform.position.y) <= yRange)
					{
						altar.OnSacrifice();
						Game.Instance.MadeSacrifice(CarriedGlyph, playerIndex);
						CarriedGlyph = -1;
					}
				}
			}

			//Movement
			if(Winnput.Up[playerIndex]){
				rigidbody.velocity += new Vector3(0, moveSpeed, 0);
			}
			else if(Winnput.Down[playerIndex]){
				rigidbody.velocity += new Vector3(0, -moveSpeed, 0);
			}
			else if(Winnput.Right[playerIndex]){
				rigidbody.velocity += new Vector3(moveSpeed, 0 , 0);
			}
			else if(Winnput.Left[playerIndex]){
				rigidbody.velocity += new Vector3(-moveSpeed, 0 , 0);
			}
		}
	}
}
