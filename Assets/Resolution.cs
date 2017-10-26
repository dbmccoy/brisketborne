using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolution : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Screen.SetResolution(640, 480, true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
