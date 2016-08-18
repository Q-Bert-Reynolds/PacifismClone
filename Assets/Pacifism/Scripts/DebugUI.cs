using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugUI : MonoBehaviour {

	public static DebugUI instance;

	public Text text;

	void Awake () {
		if (instance == null) {
			instance = this;
		}
	}

	public static void Log (string text) {
		instance.text.text = text;
	}
}
