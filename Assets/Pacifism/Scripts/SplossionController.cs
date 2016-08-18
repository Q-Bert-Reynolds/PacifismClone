using UnityEngine;
using System.Collections;

public class SplossionController : MonoBehaviour {

	public float startSize = 1;
	public float endSize = 10;
	public float splosionLength = 1;

	void OnEnable () {
		transform.localScale = Vector3.one * 0.1f;
		StartCoroutine("ExpandAndDieCoroutine");
	}

	IEnumerator ExpandAndDieCoroutine () {
		float t = 0;
		while (t <= splosionLength) {
			t += Time.deltaTime;
			float fraction = t / splosionLength;
			transform.localScale = Vector3.one * Mathf.Lerp(startSize, endSize, fraction);
			yield return new WaitForEndOfFrame();
		}
		gameObject.SetActive(false);
	}
}
