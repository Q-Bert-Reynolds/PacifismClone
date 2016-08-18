using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float speed = 3;
	public Bounds bounds;
	private PlayerController player;
	private Camera cam;

	void Start () {
		cam = GetComponent<Camera>();
	}
	
	void FixedUpdate () {
		if (GameManager.player.gameObject.activeSelf) {
			Vector3 pos = Vector3.Lerp(
				transform.position, 
				GameManager.player.transform.position, 
				Time.deltaTime * speed
			);
			
			Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
			Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
			Vector3 topRightDiff = topRight - bounds.max;
			Vector3 bottomLeftDiff = bottomLeft - bounds.min;
			Vector3 size = topRight - bottomLeft;
			
			if (size.x > bounds.size.x) pos.x = bounds.center.x;
			else if (topRightDiff.x > 0) pos.x -= topRightDiff.x;
			else if (bottomLeftDiff.x < 0) pos.x -= bottomLeftDiff.x;

			if (size.y > bounds.size.y) pos.y = bounds.center.y;
			else if (topRightDiff.y > 0) pos.y -= topRightDiff.y;
			else if (bottomLeftDiff.y < 0) pos.y -= bottomLeftDiff.y;

			pos.z = transform.position.z;

			transform.position = pos;
		}
	}

	void OnDrawGizmos () {
		Gizmos.DrawWireCube(bounds.center, bounds.size);
	}
}
