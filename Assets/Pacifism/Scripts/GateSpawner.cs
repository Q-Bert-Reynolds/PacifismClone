using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GateSpawner : MonoBehaviour {

	public int maxSpawn = 50;
	public int spawnCount = 5;
	public float spawnSpacing = 10;
	public float spawnInterval = 1;
	public GameObject gatePrefab;

	private List<GameObject> spawnedGates = new List<GameObject>();

	void OnEnable () {
		StartCoroutine("SpawnCoroutine");
	}

	void OnDisable () {
		StopCoroutine("SpawnCoroutine");
	}

	IEnumerator SpawnCoroutine () {
		while (enabled) {
			int activeCount = spawnedGates.FindAll(IsActiveGate).Count;
			if (activeCount < maxSpawn) {
				Spawn(Mathf.Min(spawnCount, maxSpawn - activeCount));
			}
			yield return new WaitForSeconds(spawnInterval);
		}
	}

	void Spawn (int num) {
		List<GameObject> gates = spawnedGates.FindAll(IsInactiveGate);
		for (int i = 0; i < num; i++) {
			GameObject gate = null;
			if (i >= gates.Count) {
				gate = Object.Instantiate(gatePrefab) as GameObject;
				spawnedGates.Add(gate);
				Rigidbody2D gateBody = gate.GetComponent<Rigidbody2D>();
				gateBody.velocity = Random.insideUnitCircle;
				gateBody.angularVelocity = Random.Range(-1, 1);
				gate.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
				gate.transform.parent = transform;
			}
			else {
				gate = gates[i];
				gate.SetActive(true);
			}
			gate.transform.localPosition = (Vector3)Random.insideUnitCircle * spawnSpacing;
		}
	}

	bool IsActiveGate (GameObject gate) {
		return gate.activeSelf;
	}

	bool IsInactiveGate (GameObject gate) {
		return !gate.activeSelf;
	}

	public void Reset () {
		foreach (GameObject gate in spawnedGates) {
			gate.SetActive(false);
		}
		gameObject.SetActive(true);
	}
}
