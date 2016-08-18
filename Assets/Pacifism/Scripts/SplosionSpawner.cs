using UnityEngine;
using System.Collections;

public class SplosionSpawner : MonoBehaviour {

	public GameObject splosionPrefab;

	private GameObject splosion;

	void Start () {
		splosion = Object.Instantiate(splosionPrefab) as GameObject;
		splosion.SetActive(false);
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.tag == "Player") {
			splosion.transform.position = transform.position;
			splosion.SetActive(true);
			gameObject.SetActive(false);
		}
	}
}
