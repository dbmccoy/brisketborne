using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    Grid grid;

    public Vector2 pos;
    public Vector2 Pos()
    {
        return grid.Get(transform.position.x, transform.position.y).Pos;
    }

    public Vector2 

	// Use this for initialization
	void Start () {
        grid = Camera.main.GetComponent<Grid>();
    }
	
	// Update is called once per frame
	void Update () {
        pos = Pos();	
	}
}
