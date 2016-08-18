using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public GameObject multiplierPrefab;
	public int multipliersPerEnemy = 5;
	public Text scoreText;
	public Text highScoreText;
	public Text multiplierText;
	public Text startText;

	private List<GameObject> spawnedMultipliers = new List<GameObject>();
	private int score = 0;
	private int multiplier = 1;
	private PlayerController _player;
	private GateSpawner gateSpawner;
	private EnemySpawner enemySpawner;

	public static PlayerController player {
		get { return instance._player; }
	}

	public enum State {
		Waiting,
		Playing,
		GameOver
	}
	private State gameState = State.Waiting;

	void Awake () {
		if (instance == null) {
			instance = this;
			
			_player = Object.FindObjectOfType(typeof(PlayerController)) as PlayerController;
			_player.gameObject.SetActive(false);
			
			gateSpawner = Object.FindObjectOfType(typeof(GateSpawner)) as GateSpawner;
			gateSpawner.gameObject.SetActive(false);
			
			enemySpawner = Object.FindObjectOfType(typeof(EnemySpawner)) as EnemySpawner;
			enemySpawner.gameObject.SetActive(false);
		}
		else {
			Debug.LogError("A GameManager instance already exists.");
		}
	}

	void Start () {
		Cursor.visible = false;
		SetScoreText();
		SetMultiplierText();
		SetHighScoreText();
	}

	void Update () {
		switch (gameState) {
			case State.Waiting:
				if (Input.GetButtonUp("Fire1")) {
					gameState = State.Playing;
					_player.gameObject.SetActive(true);
					gateSpawner.Reset();
					enemySpawner.Reset();
					ResetMultipliers();
					startText.gameObject.SetActive(false);
					score = 0;
					multiplier = 1;
					SetScoreText();
					SetMultiplierText();
				}
				else if (Input.GetButtonUp("Fire2")) {
					Application.Quit();
				}
				break;
			case State.Playing:
				if (!_player.gameObject.activeSelf) {
					gameState = State.GameOver;
					_player.gameObject.SetActive(false);
					gateSpawner.gameObject.SetActive(false);
					enemySpawner.gameObject.SetActive(false);
					startText.gameObject.SetActive(true);
					int currentHighScore = PlayerPrefs.GetInt("highScore", 0);
					if (score > currentHighScore) {
						PlayerPrefs.SetInt("highScore", score);
						PlayerPrefs.Save();
						startText.text = "You Ded\nNew High Score";
						SetHighScoreText();
					}
					else {
						startText.text = "You Ded";
					}
				}
				break;

			case State.GameOver:
				if (Input.anyKeyDown) {
					gameState = State.Waiting;
					startText.text = "Green Button to Play\nRed Button to Leave";
				}
				break;

			default:
				break;
		}
	}

	void SetScoreText () {
		scoreText.text = "Score:\n" + score;
	}

	void SetMultiplierText () {
		multiplierText.text = "x" + multiplier;
	}

	void SetHighScoreText () {
		highScoreText.text = "High Score:\n" + PlayerPrefs.GetInt("highScore", 0);
	}

	public static void AddToScore (int points) {
		instance.score += points * instance.multiplier;
		instance.SetScoreText();
	}

	public static void AddToMultiplier (int amount) {
		instance.multiplier += amount;
		instance.SetMultiplierText();
	}

	public static void SpawnMultiplier (Vector3 position) {
		List<GameObject> inactiveMultipliers = instance.spawnedMultipliers.FindAll(IsInactiveMultiplier);
		for (int i = 0; i < instance.multipliersPerEnemy; i++) {
			GameObject multiplier = null;
			if (i >= inactiveMultipliers.Count) {
				multiplier = Object.Instantiate(instance.multiplierPrefab) as GameObject;
				instance.spawnedMultipliers.Add(multiplier);
			}
			else {
				multiplier = inactiveMultipliers[i];
				multiplier.SetActive(true);
			}
			multiplier.transform.position = position + (Vector3)Random.insideUnitCircle;
		}
	}

	void ResetMultipliers() {
		foreach (GameObject multiplier in spawnedMultipliers) {
			multiplier.SetActive(false);
		}
	}

	static bool IsInactiveMultiplier (GameObject multiplier) {
		return !multiplier.activeSelf;
	}
}
