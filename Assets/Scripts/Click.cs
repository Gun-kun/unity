using UnityEngine;
using System.Collections;

public class Click : MonoBehaviour {

	public string sceneselect;

	public void ClickNam () {
		Application.LoadLevel (sceneselect);
	}

}
