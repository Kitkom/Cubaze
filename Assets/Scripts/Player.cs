//	Player
//		Receive controls for player

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{

	[SerializeField] RotatingCamera camera;
	[SerializeField] Generator generator;
	[SerializeField] float movingCount, movingPeriod;
	[SerializeField] public Vector3 position, previous, target;
	[SerializeField] AudioClip move, goal;
	public Vector3 movement;
	public bool controling;
	// Use this for initialization
	void Start ()
	{
		controling = false;
		Reset();
	}
		
	public void Reset()
	{
		AudioSource.PlayClipAtPoint(goal, new Vector3(0, 0, 10));
		position = new Vector3(0, 0, 0);
		movingCount = 0;
		target = Vector3.one;
	}
		
	// Update is called once per frame
	void Update () 
	{
		if (movingCount > 0)
		{
			movingCount -= Time.deltaTime;
			if (movingCount < 0)
				movingCount = 0;
			transform.position = Vector3.Lerp(target, previous, movingCount / movingPeriod);
		}
		else
		{
			transform.position = target;
		}
		if (controling)
		{
			if (Input.GetButtonDown("Move Vertical"))
				movement = new Vector3 (0, Input.GetAxis("Move Vertical"), 0);
			if (Input.GetButtonDown("Move Horizonal"))
				movement = new Vector3 (Input.GetAxis("Move Horizonal"), 0, 0);
			if (Input.GetButtonDown("Move Vertical") || Input.GetButtonDown("Move Horizonal"))
			{
				if (camera.status == 1)
					return;
				movement = camera.target * movement;
				if (Mathf.Abs(movement.x) - 1 > 0.1)
					movement.x = 0;
				if (Mathf.Abs(movement.y) - 1 > 0.1)
					movement.y = 0;
				if (Mathf.Abs(movement.z) - 1 > 0.1)
					movement.z = 0;
				if (generator.passCheck(position, movement))
				{
					AudioSource.PlayClipAtPoint(move, new Vector3(0, 0, 10));
					previous = 2 * position + Vector3.one;
					position += movement;
					target = 2 * position + Vector3.one;
					movingCount = movingPeriod;
					target.x = (int)(target.x + 0.2);
					target.y = (int)(target.y + 0.2);
					target.z = (int)(target.z + 0.2);
				}

			}
		}
	}
}
