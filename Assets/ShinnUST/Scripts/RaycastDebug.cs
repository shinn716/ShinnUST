using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastDebug : MonoBehaviour {

	public bool enable = false;
	public Transform mouse;

	void Update () {

		if (enable) {

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				if (hit.transform.tag == "trigger") {
					hit.transform.gameObject.SendMessage ("CubeStart");
				}
			}
			mouse.position = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));
		}
	}
}
