using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

	public int maxSpawn = 100;
	public int spawnCount = 5;
	public float spawnSpacing = 2f;
	public float spawnInterval = 1;
	public GameObject enemyPrefab;
	public Transform[] spawnPoints;

	private List<GameObject> spawnedEnemies = new List<GameObject>();

	void OnEnable () {
		StartCoroutine("SpawnCoroutine");
	}

	void OnDisable () {
		StopCoroutine("SpawnCoroutine");
	}

	IEnumerator SpawnCoroutine () {
		while (enabled) {
			int activeCount = spawnedEnemies.FindAll(IsActiveEnemy).Count;
			if (activeCount < maxSpawn) {
				Vector3 pos = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
				Spawn(Mathf.Min(spawnCount, maxSpawn - activeCount), pos);
			}
			yield return new WaitForSeconds(spawnInterval);
		}
	}

	void Spawn (int num, Vector3 pos) {
		List<GameObject> enemies = spawnedEnemies.FindAll(IsInactiveEnemy);
		for (int i = 0; i < num; i++) {
			GameObject enemy = null;
			if (i >= enemies.Count) {
				enemy = Object.Instantiate(enemyPrefab) as GameObject;
				enemy.transform.parent = transform;
				spawnedEnemies.Add(enemy);
			}
			else {
				enemy = enemies[i];
				enemy.SetActive(true);
			}

			enemy.transform.position = pos + (Vector3)Random.insideUnitCircle * spawnSpacing;
		}
	}

	bool IsActiveEnemy (GameObject enemy) {
		return enemy.activeSelf;
	}

	bool IsInactiveEnemy (GameObject enemy) {
		return !enemy.activeSelf;
	}

	public void Reset () {
		foreach (GameObject enemy in spawnedEnemies) {
			enemy.SetActive(false);
		}
		gameObject.SetActive(true);
	}
}
