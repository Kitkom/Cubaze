//	PathPlacer
//		When dragging with mouse, show the path available on the same plane.
using UnityEngine;
using System.Collections;

public class PathPlacer : MonoBehaviour 
{

	[SerializeField]public int x, y, z;
	[SerializeField]public Player player;
	// Use this for initialization
	void Start () 
	{	
	}

	public void Ref()
	{
		transform.position = new Vector3(x, y, z);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButtonDown("View") & ((x == player.target.x) || (y == player.target.y) || (z == player.target.z)))
			gameObject.layer = 0;
		if (Input.GetButtonUp("View"))
			gameObject.layer = 8;
	}
}
