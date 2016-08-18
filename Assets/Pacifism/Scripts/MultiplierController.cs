using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class MultiplierController : MonoBehaviour {

	public float speed = 3;
	public float flickerDelay = 1;
	public float flickerDuration = 1;
	public float flickerInterval = 0.1f;
	public float moveRange = 1;

	private Rigidbody2D body;
	private SpriteRenderer sprite;
	private Color initialColor;
	private Color offColor;
	private Vector2 heading;

	void Awake () {
		sprite = GetComponent<SpriteRenderer>();
		initialColor = sprite.color;
		offColor = initialColor;
		offColor.a = 0;

		body = GetComponent<Rigidbody2D>();
	}

	void OnEnable () {
		sprite.color = initialColor;
		body.velocity = Random.insideUnitCircle;
		StartCoroutine("Fade");
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.tag == "Player") {
			GameManager.AddToMultiplier(1);
			gameObject.SetActive(false);
		}
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
		if (heading.magnitude < moveRange) {
			body.velocity = heading.normalized * speed;
		}
	}

	IEnumerator Fade () {
		yield return new WaitForSeconds(flickerDelay);
		bool isVisible = true;
		for (float t = 0; t < flickerDuration; t += flickerInterval) {
			if (isVisible) {
				sprite.color = initialColor;
			}
			else {
				sprite.color = offColor;
			}

			isVisible = !isVisible;
			yield return new WaitForSeconds(flickerInterval);
		}
		gameObject.SetActive(false);
	}
}
