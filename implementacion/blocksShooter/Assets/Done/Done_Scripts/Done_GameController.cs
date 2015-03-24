using UnityEngine;
using System.Collections;
using Ligabo;

public class Done_GameController : MonoBehaviour
{
	public GameObject[] hazards;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	
	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameOverText;

	public bool started = false;
	private bool gameOver;
	private bool restart;
	private int score;
	public bool paused = false;
	
	void Start ()
	{
		gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		score = 0;
		UpdateScore ();
		Time.timeScale=0;
		//StartCoroutine (SpawnWaves ());
	}
	
	//void Update ()
	//{
	//	if (restart)
	//	{
	//		if (Input.GetKeyDown (KeyCode.R))
	//		{
	//			Application.LoadLevel (Application.loadedLevel);
	//		}
	//	}
	//}
	void Update () {
		if (!started && LigaboState.statusTracking==LigaboState.StatusTracking.Tracking) {
			StartGame();
		}
		if (started && LigaboState.statusTracking!=LigaboState.StatusTracking.Tracking && !paused)
			PauseGame();
		if (started && paused && LigaboState.statusTracking==LigaboState.StatusTracking.Tracking )
			ResumeGame();
	}

	void PauseGame(){
		Time.timeScale=0;
		paused=true;
	}
	
	void ResumeGame(){
		Time.timeScale=1;
		paused=false;
	}

	void StartGame(){
		Time.timeScale=1;
		StartCoroutine (SpawnWaves ());
		LigaboState.status=LigaboState.Status.Running;
		started=true; 
	}

	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		while (true)
		{
			for (int i = 0; i < hazardCount; i++)
			{
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
			
			if (gameOver)
			{
				restartText.text = "Press 'R' for Restart";
				restart = true;
				break;
			}
		}
	}

	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}
	
	void UpdateScore ()
	{
		scoreText.text = "Score: " + score;
	}
	
	public void GameOver ()
	{
		gameOverText.text = "Game Over!";
		gameOver = true;
	}

	void OnGUI(){
		float butWidth=300f;
		LigaboUtils.GUIScaleBegin();
		
		if (!started && LigaboState.status!=LigaboState.Status.Running){
			if (GUI.Button(new Rect(LigaboUtils.screenWidth/2-butWidth/2, LigaboUtils.screenHeight/2-butWidth/5/2,butWidth,butWidth/5),"Start Game"))
				LigaboState.status=LigaboState.Status.Running;
		}
		
		LigaboUtils.GUIScaleEnd();
	}

}