using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour {

	public int points = 1;
	public float speed = 3;
	private Rigidbody2D body;
    private Vector2 heading;

	void Start () {
		body = gameObject.GetComponent<Rigidbody2D>();
	}

	void Update () {
		if (GameManager.player.gameObject.activeSelf) {
			heading = (Vector2)(GameManager.player.transform.position - transform.position);
			float angle = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
	        transform.rotation = Quaternion.Euler(0, 0, angle);
	    }
	    else {
	    	gameObject.SetActive(false);
	    }
	}
	
	void FixedUpdate () {
		body.MovePosition(body.position + heading.normalized * speed * Time.deltaTime);
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag == "KillEnemy") {
			gameObject.SetActive(false);
			GameManager.AddToScore(points);
			GameManager.SpawnMultiplier(transform.position);
		}
	}
}
