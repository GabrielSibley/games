using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

public class TriviaMaster : MonoBehaviour {
	
	private static string[] roundText = {
		"ROUND 1\nGET PSYCHED",
		"LIGHTNING ROUND\nTRIPLE SCORE",
		"ROUND 2\nDOUBLE SCORE"
	};
	
	public enum GameMode{
		Title,
		Playing,
		Paused,
		Scores
	}
	
	public enum QuestionType{
		CityToPop = 0,
		PopToCity = 1,
		CityToCountry = 2,
		CountryToCity = 3
	}
	
	public GUISkin skin;
	public TextAsset cityDataFile;
	public Texture2D[] playerIcons;
	public GameMode currentMode;
	public float introTime;	
	public float timeLeft;
	public int currentQuestionNumber;
	public int currentRound;
	public int[] scores;
	public int[] selections;
	public Rect[] titleRects;
	public Rect[] playRects;
	public Rect fullScreen = new Rect(0, 0, 256, 192);
	public Texture2D blackTexture, tomSelect, grahamSelect;
	public AudioClip sfxSelectDown, sfxSelectUp, sfxCorrect, sfxIncorrect, sfxGameOver;
	
	protected float introTimer;
	protected List<City> cities = new List<City>();
	protected CultureInfo en_US = CultureInfo.CreateSpecificCulture("en-US");
	protected bool IsLightningRound{
		get{return currentRound == 2;}
	}
	protected GUIStyle LargeLabel {
		get {
			return GUI.skin.GetStyle("LargeLabel");
		}
	}
	protected GUIStyle MediumLabel {
		get {
			return GUI.skin.GetStyle("MediumLabel");
		}
	}
	protected GUIStyle SmallLabel {
		get {
			return GUI.skin.GetStyle("SmallLabel");
		}
	}
	protected GUIStyle FancyBox{
		get {
			return GUI.skin.GetStyle("FancyBox");
		}
	}
	protected GUIStyle QuestionStyle{
		get {
			return GUI.skin.GetStyle("QuestionStyle");
		}
	}
	protected GUIStyle AnswerStyle{
		get {
			return GUI.skin.GetStyle("AnswerStyle");
		}
	}
	protected GUIStyle ScoreStyle{
		get {
			return GUI.skin.GetStyle ("ScoreStyle");
		}
	}
	protected int QuestionValue{
		get{
			if(currentRound == 1){
				return 5;
			}
			else if(IsLightningRound){
				return 15;
			}
			else return 10;
		}
	}
	
	protected string[] questionStrings;
	protected string[] answerStrings;
	protected int correctAnswerIndex;
	protected bool[] repeatFlag = new bool[2];
	protected bool[] lockout = new bool[2];
	protected bool showCorrectAnswer, waitingForNextQuestion, showRoundIntro;
	protected float questionTimer;
	
	void Awake(){
		StringReader sr = new StringReader(cityDataFile.text);
		string line;
		while((line = sr.ReadLine()) != null){
			string[] parts = line.Split (new char[]{'\t'});
			cities.Add(new City(parts));
		}
		Debug.Log("Loaded " + cities.Count + " cities");
	}
	
	void LoadQuestion(){
		//pick question type
		QuestionType q = (QuestionType)Random.Range (0, 3);
		//Pick city
		City c = cities[Random.Range(0, cities.Count - 1)];
		//Build question string
		questionStrings = new string[]{
			"WHAT IS THE",
			"",
			"OF",
			""
		};
		switch(q){
			case QuestionType.CityToCountry:
				questionStrings[1] = "COUNTRY";
				questionStrings[3] = c.name.ToUpper();
				break;
			case QuestionType.CityToPop:
				questionStrings[1] = "POPULATION";
				questionStrings[3] = c.name.ToUpper();
				break;
			case QuestionType.CountryToCity:
				questionStrings[1] = "CITY";
				questionStrings[3] = c.country.ToUpper();
				break;
			case QuestionType.PopToCity:
				questionStrings[1] = "CITY";
				questionStrings[3] = "POP " + c.population.ToString("N0", en_US);
				break;
		}
		
		//Build answers
		correctAnswerIndex = Random.Range (0, 3);
		answerStrings = new string[4];
		List<City> chosenCities = new List<City>();
		chosenCities.Add(c);
		for(int i = 0; i < answerStrings.Length; i++){
			City src;
			if(i == correctAnswerIndex){
				src = c;
			}
			else{
				src = GetGoodCity(q, chosenCities);
				chosenCities.Add(src);
			}
			if(q == QuestionType.CityToCountry){
				answerStrings[i] = src.country.ToUpper();
			}
			else if(q == QuestionType.CityToPop){
				answerStrings[i] = src.population.ToString("N0", en_US);
			}
			else{
				answerStrings[i] = src.name.ToUpper();
			}
		}
	}
	
	City GetGoodCity(QuestionType q, List<City> chosen){
		//Find other cities that don't share the same answer
		List<City> otherCities;
		if(q == QuestionType.CityToCountry || q == QuestionType.CountryToCity){
			otherCities = cities.FindAll(x => chosen.TrueForAll (y => x.country != y.country));
		}
		else{
			otherCities = cities.FindAll(x => chosen.TrueForAll (y => 
					x.population < y.population - 900000
				||	x.population > y.population + 900000
				));
		}
		return otherCities[Random.Range (0, otherCities.Count - 1)];
	}
	
	void OnGUI(){
		
		GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3(Screen.width / 256f, Screen.height / 192f, 1));
		
		GUI.skin = skin;
		switch(currentMode){
			case GameMode.Title:
				OnGUI_Title();
				break;
			case GameMode.Playing:
				if(showRoundIntro){
					OnGUI_Round();
				}
				else{
					OnGUI_Playing();
				}
				break;
			case GameMode.Paused:
				OnGUI_Paused();
				break;
			case GameMode.Scores:
				OnGUI_Scores();
				break;			
		}
	}
	
	void Update(){
		if(currentMode == GameMode.Title){
			if(Winnput.A[0]){
				currentMode = GameMode.Playing;
				CleanGameState();
			}
		}
		else if(currentMode == GameMode.Playing){
			if(showRoundIntro){
				return;
			}
			introTimer -= Time.deltaTime;
			if(introTimer <= 0){
				for(int player = 0; player < 2; player++){
					if(lockout[player]){
						continue;
					}
					if(Winnput.A[player]){
						if(selections[player] == correctAnswerIndex){
							audio.PlayOneShot(sfxCorrect);
							scores[player] += QuestionValue;
							lockout[0] = lockout[1] = true;							
						}
						else{
							audio.PlayOneShot(sfxIncorrect);
							scores[player] -= QuestionValue;
							lockout[player] = true;
						}
					}
					if(!repeatFlag[player]){
						if(Winnput.Down[player] && selections[player] < 3){
							selections[player] += 1;
							repeatFlag[player] = true;
							audio.PlayOneShot(sfxSelectDown);
						}
						if(Winnput.Up[player] && selections[player] > 0){
							selections[player] -= 1;
							repeatFlag[player] = true;
							audio.PlayOneShot(sfxSelectUp);
						}
					}			
					else if(!Winnput.Up[player] && !Winnput.Down[player]){
						repeatFlag[player] = false;
					}
				}
				if(!waitingForNextQuestion){
					if(lockout[0] && lockout[1]){
						StartCoroutine(ShowCorrectAnswerAndProceed());
					}
					questionTimer -= Time.deltaTime;
					if(questionTimer <= 0){
						audio.PlayOneShot(sfxIncorrect);
						lockout[0] = lockout[1] = true;
					}
				}
				
			}
		}
		else if(currentMode == GameMode.Scores){
			if(Winnput.AnyButtonDown){
				currentMode = GameMode.Title;
			}
		}
	}
	
	void CleanGameState(){
		currentRound = 1;
		currentQuestionNumber = 0;
		scores = new int[]{0, 0};		
		NextQuestion();
		StartCoroutine(NewRound());	
	}
	
	void NextQuestion(){
		//Next question
		currentQuestionNumber++;
		if((currentQuestionNumber > 10) || (currentQuestionNumber > 5 && IsLightningRound)){
			currentRound++;
			currentQuestionNumber = 1;
			//End of game
			if(currentRound > 3){
				audio.PlayOneShot(sfxGameOver);
				currentMode = GameMode.Scores;
			}
			else{
				StartCoroutine(NewRound());			
			}
		}
		
		//Reset numbers
		if(IsLightningRound){
			introTimer = 0;
			questionTimer = 3;
		}
		else{
			introTimer = introTime;
			questionTimer = 10;
		}
		selections = new int[]{0, 0};
		lockout[0] = lockout[1] = false;
		//Get next question
		LoadQuestion();
	}
	
	IEnumerator ShowCorrectAnswerAndProceed(){
		showCorrectAnswer = true;
		waitingForNextQuestion = true;
		yield return new WaitForSeconds(2);
		showCorrectAnswer = false;
		waitingForNextQuestion = false;
		NextQuestion();
	}
	
	IEnumerator NewRound(){
		showRoundIntro = true;
		yield return new WaitForSeconds(3);
		showRoundIntro = false;
	}
	
	void OnGUI_Title(){
		GUI.DrawTexture(fullScreen, blackTexture);
		GUI.color = Color.magenta;
		GUI.Label(titleRects[0], "GRAHAM", LargeLabel);
		GUI.color = Color.white;
		GUI.Label(titleRects[1], "VS", MediumLabel);
		GUI.color = Color.cyan;
		GUI.Label(titleRects[2], "TOM", LargeLabel);
		GUI.color = Color.white;
		GUI.Label(titleRects[3], "WORLD CITY", LargeLabel);
		GUI.Label(titleRects[4], "TRIVIA CHALLENGE", LargeLabel);
		GUI.Label(titleRects[5], "GRAHAM PRESS FIRE TO PLAY", SmallLabel);
	}
	
	void OnGUI_Paused(){
	}
	
	void OnGUI_Playing(){
		GUI.Box(fullScreen, "", FancyBox);
		GUI.color = Color.black;
		GUI.Label (playRects[0], questionStrings[0], QuestionStyle);
		GUI.color = Color.red;
		if(introTimer <= 3){
			GUI.Label (playRects[1], questionStrings[1], QuestionStyle);
		}
		GUI.color = Color.black;
		if(introTimer <= 2){			
			GUI.Label (playRects[2], questionStrings[2], QuestionStyle);
		}
		GUI.color = Color.red;
		if(introTimer <= 1){
			GUI.Label (playRects[3], questionStrings[3], QuestionStyle);
		}
		if(introTimer <= 0){
			//Answers
			for(int i = 0; i < 4; i++){
				if(showCorrectAnswer && i == correctAnswerIndex){
					GUI.color = Color.green;
				}
				else{
					GUI.color = Color.black;
				}
				GUI.Label (playRects[4+i], answerStrings[i], AnswerStyle);
			}
			//Player selectors
			if(lockout[0])
				GUI.color = Color.grey;
			else
				GUI.color = Color.white;
			Rect grahamRect = playRects[4+selections[0]];
			grahamRect = new Rect(8, grahamRect.y, 16, 16);
			GUI.DrawTexture(grahamRect, grahamSelect, ScaleMode.ScaleToFit);
			if(lockout[1])
				GUI.color = Color.grey;
			else
				GUI.color = Color.white;
			Rect tomRect = playRects[4+selections[1]];
			tomRect = new Rect(256 - 24, tomRect.y, 16, 16);
			GUI.DrawTexture(tomRect, tomSelect, ScaleMode.ScaleToFit);
		}
		
		//Time left window
		GUI.color = Color.white;
		GUI.Box(playRects[8], "", FancyBox);
		GUI.color = Color.black;
		GUI.Label (playRects[8], Mathf.CeilToInt (questionTimer).ToString(), ScoreStyle);
		
		//Scores
		GUI.color = Color.white;
		GUI.Box(playRects[9], "", FancyBox);
		GUI.color = Color.magenta;
		GUI.Label (playRects[9], scores[0].ToString (), ScoreStyle);
		
		GUI.color = Color.white;
		GUI.Box(playRects[10], "", FancyBox);
		GUI.color = Color.cyan;
		GUI.Label (playRects[10], scores[1].ToString(), ScoreStyle);
	}
	
	void OnGUI_Round(){
		GUI.Box (fullScreen, "", FancyBox);
		GUI.color = Color.black;
		GUI.Label (playRects[0], roundText[currentRound-1], QuestionStyle);
	}
	
	void OnGUI_Scores(){
		GUI.Box (fullScreen, "", FancyBox);
		
		if(scores[0] > scores[1]){
			GUI.color = Color.magenta;
			GUI.Label (playRects[2], "GRAHAM\nWINS", QuestionStyle);
		}
		else if(scores[1] > scores[0]){
			GUI.color = Color.cyan;
			GUI.Label (playRects[2], "TOM\nWINS", QuestionStyle);
		}
		else{
			//This is intentionally screwy
			if(Random.value > 0.5f){
				GUI.color = Color.magenta;
				GUI.Label (playRects[2], "GRAHAM\nWINS", QuestionStyle);
			}
			else{
				GUI.color = Color.cyan;
				GUI.Label (playRects[2], "TOM\nWINS", QuestionStyle);
			}
		}
		
		GUI.color = Color.white;
		GUI.Box(playRects[9], "", FancyBox);
		GUI.color = Color.magenta;
		GUI.Label (playRects[9], scores[0].ToString (), ScoreStyle);
		
		GUI.color = Color.white;
		GUI.Box(playRects[10], "", FancyBox);
		GUI.color = Color.cyan;
		GUI.Label (playRects[10], scores[1].ToString(), ScoreStyle);
	}
	
	protected class City{
		public string name;
		public string country;
		public int population;
		
		public City(string[] data){
			name = data[2];
			country = data[3];
			if(country == ""){
				Debug.LogWarning (name + " has blank country");
			}
			population = int.Parse (data[4].Replace(",",""));
		}
	}
}
