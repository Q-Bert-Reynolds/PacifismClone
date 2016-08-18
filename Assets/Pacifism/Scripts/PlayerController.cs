using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

	public float speed = 5;
	public float turnSpeed = 10;
	private Rigidbody2D _body;
	public Rigidbody2D body {
		get {
			return _body;
		}
	}

	private Vector2 heading;

	void Awake () {
		_body = gameObject.GetComponent<Rigidbody2D>();
	}

	void Update () {
		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");
		heading = new Vector2(x, y);

		if (heading.sqrMagnitude < 0.1f) {
			return;
		}

		float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(
        	transform.rotation, 
        	Quaternion.Euler(0, 0, angle), 
        	turnSpeed * Time.deltaTime
        );
	}

	void FixedUpdate () {
		body.velocity = heading.normalized * speed;
	}

	void OnCollisionEnter2D(Collision2D c) {
		if (c.gameObject.tag == "Enemy") {
			gameObject.SetActive(false);
		}
	}
}
