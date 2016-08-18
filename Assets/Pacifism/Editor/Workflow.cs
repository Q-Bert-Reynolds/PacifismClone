// A collection of workflow scripts from http://wiki.unity3d.com/index.php/Scripts/Editor
// Shortcut keys and Undo functionality added by Nolan Baker
	
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Workflow : Editor {
 

 	//CTRL+D to duplicate
    [MenuItem("Edit/_Duplicate %d")]
    static void Duplicate () {
        EditorApplication.ExecuteMenuItem("Edit/Duplicate");
    }

	// From: http://wiki.unity3d.com/index.php/AddChild
	// Author: Neil Carter (NCarter)
	[MenuItem ("GameObject/Add Child &c")]
	static void AddChild() {
		Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
		Undo.RecordObjects(transforms, "adds children to selected transforms");
		foreach(Transform transform in transforms) {
			GameObject newChild = new GameObject("NewChild");
			Undo.RegisterCreatedObjectUndo(newChild, "create child");
			newChild.transform.parent = transform;
			newChild.transform.localPosition = Vector3.zero;
		}
	}

	// From: http://wiki.unity3d.com/index.php/AddParent
	// Author: Neil Carter (NCarter)
	// added center param - Nolan Baker
	static void AddParent(Vector3 center) {
		Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
 		if (transforms.Length == 0) return;
		
		GameObject newParent = new GameObject("NewParent");
		Undo.RegisterCreatedObjectUndo(newParent, "create parent");
		Transform newParentTransform = newParent.transform;
		newParentTransform.position = center;
		Undo.SetTransformParent(newParentTransform, transforms[0].parent, "sets parent of created obj");
 
		foreach (Transform t in transforms) {
			 Undo.SetTransformParent(t, newParentTransform, "sets parent of selected");
		}
	}

	[MenuItem ("GameObject/Add Parent at Origin %g")]
	static void AddParentAtOrigin () {
		AddParent(Vector3.zero);
	}

	[MenuItem ("GameObject/Add Parent at Center &g")]
	static void AddParentAtCenter () {
		Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
		if (transforms.Length == 0) return;
		Vector3[] positions = System.Array.ConvertAll(transforms, t => t.position);
		Vector3 center = Vector3.zero;
		for (int i = 0; i < positions.Length; i++) {
			center += positions[i];
		}
		center /= (float)positions.Length;
 		AddParent(center);
	}
 
	// From: http://wiki.unity3d.com/index.php/InvertSelection
	// Author: Mift (mift)
	[MenuItem ("Edit/Invert Selection &i")]
	static void InvertSelection() { 
		List< GameObject > oldSelection = new List< GameObject >();
		List< GameObject > newSelection = new List< GameObject >();
 
		foreach(GameObject obj in Selection.GetFiltered(typeof(GameObject), SelectionMode.ExcludePrefab)) {
			oldSelection.Add(obj);
		}
 
		foreach(GameObject obj in FindObjectsOfType(typeof(GameObject))) {
			if (!oldSelection.Contains(obj)) {
				newSelection.Add(obj);
			}
		}
 
		Undo.RecordObjects(Selection.objects, "inverts selection");
		Selection.objects = newSelection.ToArray();
	}

	[MenuItem ("Edit/Select Children %&c")]
	static void SelectChildren() { 
		List< GameObject > oldSelection = new List< GameObject >();
		List< GameObject > newSelection = new List< GameObject >();
 
		foreach (GameObject obj in Selection.GetFiltered(typeof(GameObject), SelectionMode.ExcludePrefab)) {
			oldSelection.Add(obj);
		}
 
		foreach (GameObject obj in oldSelection) {
			foreach (Transform t in obj.transform) {
				newSelection.Add(t.gameObject);
			}
		}
 
		Undo.RecordObjects(Selection.objects, "selects children");
		Selection.objects = newSelection.ToArray();
	}

	// From: http://wiki.unity3d.com/index.php/MoveToOrigin
	// Author: Matthew Miner (matthew@matthewminer.com)
	[MenuItem ("GameObject/Move To Origin &m")]
	static void MenuMoveToOrigin () {
		foreach (Transform t in Selection.transforms) {
			Undo.RecordObject(t, "Move " + t.name);
			t.position = Vector3.zero;
			Debug.Log("Moving " + t.name + " to origin");
		}
	}

	 static UnityEditor.SceneView sceneView {
    	get { return UnityEditor.SceneView.lastActiveSceneView; }
    }

    static Vector3 pivot {
    	get { return sceneView.pivot; }
    }

	[MenuItem("View/Look Right &1")]
    static void LookRight() {
		sceneView.LookAt(pivot, Quaternion.LookRotation(Vector3.right));
	}

	[MenuItem("View/Look Left &#1")]
    static void LookLeft() {
		sceneView.LookAt(pivot, Quaternion.LookRotation(-Vector3.right));
	}
 
 	[MenuItem("View/Look Forward &2")]
    static void LookForward() {
		sceneView.LookAt(pivot, Quaternion.LookRotation(Vector3.forward));
	}

	[MenuItem("View/Look Back &#2")]
    static void LookBack() {
		sceneView.LookAt(pivot, Quaternion.LookRotation(-Vector3.forward));
	}

	[MenuItem("View/Look Down &3")]
    static void LookDown() {
		sceneView.LookAt(pivot, Quaternion.LookRotation(-Vector3.up));
	}

	[MenuItem("View/Look Up &#3")]
    static void LookUp() {
		sceneView.LookAt(pivot, Quaternion.LookRotation(Vector3.up));
	}

	[MenuItem("View/Perspective &5")]
    static void SetPerspective() {
		sceneView.orthographic = false;
    }

    [MenuItem("View/Orthographic &#5")]
    static void SetOrthographic() {
		sceneView.orthographic = true;
    }
}