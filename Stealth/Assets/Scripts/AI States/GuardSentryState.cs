using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardSentryState : GuardState {

	public const float kViewWorthyness = 0.3f; //How good a view has to be, compared to the best view, to take a look at it.
	public const float kBaseTotalDuration = 8; //Time spread over all views
	public const float kPerViewDuration = 2; //Plus this * number of views
	public const float kDurationVariation = 4; // plus or minus this

	public GridSquare m_sentrySquare;
	
	protected List<SentryView> m_views = new List<SentryView>();
	
	protected bool m_rotateClockwise;
	protected bool m_pingpong; //Ping-pong instead of wrapping around
	protected int m_startViewIndex; //Ping-pong endpoints
	protected int m_endViewIndex;
	protected int m_currentViewIndex;
	protected float m_remainingTimeInCurrentView;
	
	//For gameplay, squares that result in a non-turning sentry have 0 value.
	//Otherwise, sum of sentry values
	public static float CalcSquareSentryExcellence(GridSquare sentrySquare){
		//pick a couple of good directions
		float[] scores = new float[sentrySquare.AdjustedSentryValues.Length];
		int[] directions = new int[scores.Length];
		int bestDirection = 0;
		float bestScore = 0;
		float totalViewScore = 0;
		int viewCount = 0;
		
		for(int i = 0; i < scores.Length; i++){
			scores[i] = sentrySquare.AdjustedSentryValues[i];
			directions[i] = i;
			bestScore = Mathf.Max(bestScore, scores[i]);
		}
		
		float threshold = bestScore * kViewWorthyness;
		
		while(bestScore >= threshold){
			System.Array.Sort(scores, directions); //Sort by score ascending
			bestDirection = directions[directions.Length - 1];
			System.Array.Sort(directions, scores); //Sort by direction ascending
		
			viewCount++;
			totalViewScore += scores[bestDirection];
			//Diminish scores of nearby directions
			bestScore = 0;
			for(int i = 0; i < scores.Length; i++){
				float theta = (i - bestDirection) * 2 * Mathf.PI / scores.Length;
				scores[i] *= 1 - Mathf.Cos(theta);
				bestScore = Mathf.Max(bestScore, scores[i]);
			}
		}
		if(viewCount <= 1){
			return 0;
		}
		else{
			return totalViewScore;
		}
	}
	
	public GuardSentryState(GridSquare sentrySquare){
		m_sentrySquare = sentrySquare;

		//pick a couple of good directions
		float[] scores = new float[m_sentrySquare.AdjustedSentryValues.Length];
		int[] directions = new int[scores.Length];
		int bestDirection = 0;
		float bestScore = 0;
		float totalViewScore = 0;
		
		for(int i = 0; i < scores.Length; i++){
			scores[i] = m_sentrySquare.AdjustedSentryValues[i];
			directions[i] = i;
			bestScore = Mathf.Max(bestScore, scores[i]);
		}
		
		float threshold = bestScore * kViewWorthyness;
		
		while(bestScore >= threshold){
			System.Array.Sort(scores, directions); //Sort by score ascending
			bestDirection = directions[directions.Length - 1];
			System.Array.Sort(directions, scores); //Sort by direction ascending
		
			m_views.Add(new SentryView(bestDirection, scores[bestDirection]));
			totalViewScore += scores[bestDirection];
			//Diminish scores of nearby directions
			bestScore = 0;
			for(int i = 0; i < scores.Length; i++){
				float theta = (i - bestDirection) * 2 * Mathf.PI / scores.Length;
				scores[i] *= 1 - Mathf.Cos(theta);
				bestScore = Mathf.Max(bestScore, scores[i]);
			}
		}
		
		//Normalize durations
		float totalDuration = kBaseTotalDuration + kPerViewDuration * m_views.Count + Random.Range(-kDurationVariation, kDurationVariation);
		for(int i = 0; i < m_views.Count; i++){
			SentryView sv = m_views[i];
			sv.duration = sv.duration / totalViewScore * totalDuration;
			m_views[i] = sv;
		}
		//Sort views radially
		m_views.Sort(delegate(SentryView a, SentryView b){return a.direction.CompareTo(b.direction);});
		//Find if there is a view-to-view angle of >= 180 degrees
		for(int i = 0; i < m_views.Count; i++){
			int next = Mathfx.Repeat(i+1, m_views.Count);
			if(Vector3.Angle(m_views[i].directionVector, m_views[next].directionVector) >= 180){
				m_pingpong = true;
				m_startViewIndex = i;
				m_endViewIndex = next;
				break;
			}
		}
		//Set up some initial state
		m_rotateClockwise = Random.value > 0.5f;
		m_currentViewIndex = Random.Range(0, m_views.Count);
		m_remainingTimeInCurrentView = Random.Range(0, m_views[m_currentViewIndex].duration);
	}
	
	public override void OnStateEntered(Guard guard){
		guard.DisplayMode = guard.normalMaterial;
	}
	
	public override void Update (Guard guard) {
		if(LevelLogic.alert == AlertMode.Alert){
			guard.PushState(new GuardAgroState());
			return;
		}
		if(LevelLogic.alert == AlertMode.Evasion){
			guard.PushState(new GuardSearchState());
			return;
		}
		
		if(guard.suspicion > 0){
			guard.PushState(new GuardSearchState());
			LevelLogic.SetAlertMode(AlertMode.Warning);
			return;
		}		
		
		//Switch between views
		m_remainingTimeInCurrentView -= Time.deltaTime;
		if(m_remainingTimeInCurrentView <= 0){
			if(m_pingpong && ((m_rotateClockwise && m_currentViewIndex == m_endViewIndex) || (!m_rotateClockwise && m_currentViewIndex == m_startViewIndex))){
				m_rotateClockwise = !m_rotateClockwise;
			}
			if(m_rotateClockwise){
				m_currentViewIndex = Mathfx.Repeat(m_currentViewIndex - 1, m_views.Count);
			}
			else{
				m_currentViewIndex = Mathfx.Repeat(m_currentViewIndex + 1, m_views.Count);
			}
			m_remainingTimeInCurrentView = m_views[m_currentViewIndex].duration;
		}
		
		Look(guard, m_views[m_currentViewIndex].directionVector);
		
		foreach(SentryView sv in m_views){
			if(m_pingpong){
				Debug.DrawRay(guard.transform.position, sv.directionVector, new Color(1, 0, 1));
			}
			else{
				Debug.DrawRay(guard.transform.position, sv.directionVector, new Color(0, 1, 1));
			}
		}
		
	}
	
	protected struct SentryView{
		public int direction;
		public Vector3 directionVector;
		public float duration;
		
		public SentryView(int direction, float duration){
			this.direction = direction;
			this.duration = duration;
			float theta = Mathf.PI * 2 * direction / GridSquare.kSentryDivisions;
			directionVector = new Vector3(Mathf.Cos(theta), 0, Mathf.Sin(theta));
		}
	}
}
