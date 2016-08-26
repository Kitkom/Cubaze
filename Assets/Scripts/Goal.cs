//	Goal
//		Reach here to get to next level

using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour 
{

	[SerializeField] Generator generator;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col)
	{
        generator.mapSize++;
		generator.New();
	}
}
