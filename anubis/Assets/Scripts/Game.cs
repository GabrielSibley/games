using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Game : MonoBehaviour {

	public static Game Instance;

	public SpriteRenderer demandRenderer;
	public Scales scales;
	public BasicMove[] players;
	public GameObject[] winnerScreens;

	public int Demand
	{
		get
		{
			return demand;
		}
		set
		{
			demand = value;
			demandRenderer.sprite = Glyphs.GetGlyph(value);
		}
	}
	private int demand;

	private int[] scores = new int[2];

	void Start () {
		Instance = this;
		List<int> glyphChoice = new List<int>();
		for(int i = 0; i < 16; i++)
		{
			glyphChoice.Add (i);
		}
		//Populate vases
		for(int i = 0; i < Vase.All.Count; i++)
		{
			int choiceIndex = Random.Range (0, glyphChoice.Count);
			Vase.All[i].glyph = glyphChoice[choiceIndex];
			glyphChoice.RemoveAt (choiceIndex);
		}
		//Set demand
		NewRandomDemand();
	}

	private void NewRandomDemand()
	{
		Demand = Random.Range (0, 16);
	}

	private void Update()
	{
		if(Winnput.HomeDown)
		{
			Application.LoadLevel("title");
		}
	}

	public void MadeSacrifice(int glyph, int player)
	{
		//If sacrifice was Player Glyph
		if(glyph >= 16)
		{
			//Game over / winner screen
			StartCoroutine(GameOver(player));
		}
		else
		{
			//Score point
			scores[player]++;
			//Update scales
			scales.ShowScores(scores);
			//If one player ahead by 3
			if(scores[0] - scores[1] >= 3)
			{
				//Graham winning ENDGAME
				players[0].Endgame(true);
				players[1].Endgame(false);
				Demand = 17;
				players[1].moveSpeed = players[1].moveSpeed / 2;
			}
			else if(scores[1] - scores[0] >= 3)
			{
				//Tom winning ENDGAME
				players[0].Endgame(false);
				players[1].Endgame(true);
				Demand = 16;
				players[0].moveSpeed = players[0].moveSpeed / 2;
			}
			else
			{
				//new demand
				NewRandomDemand();
				//restock vase
				List<Vase> emptyVases = Vase.All.FindAll (x => x.glyph < 0);
				emptyVases[Random.Range (0, emptyVases.Count)].glyph = glyph;
			}
		}
	}

	private IEnumerator GameOver(int winner)
	{
		Demand = -1;
		winnerScreens[winner].SetActive(true);
		yield return new WaitForSeconds(4);
		Application.LoadLevel("title");
	}
}
