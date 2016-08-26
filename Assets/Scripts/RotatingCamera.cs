//  RotatingCamera
// 		Receiving controls for rotation and hand out current state ("normal").
//  	Providing with view-mode for mouse-look.

using UnityEngine;
using System.Collections;

public class RotatingCamera : MonoBehaviour
{

    [SerializeField] public int status;   // 0-normal 1-viewing
    [SerializeField] private float movingPeriod = 1;
    [SerializeField] private float movingCount = 0;
	[SerializeField] AudioSource audioSource;
    public Quaternion previous, target, current;
	public Vector3 normal;
	public bool controling;
	private float mouseVer, mouseHor;
	// Use this for initialization
	void Start () 
	{
		controling = false;
		Reset();
	}

	void Reset()
	{
		previous = Quaternion.Euler(0, 0, 0);
		normal = new Vector3(0, 0, 1);
		target = previous;
		current = previous;
		status = 0;
	}

	// Update is called once per frame
	void Update () 
	{
		if (!controling)
			return;
		if (Input.GetButtonDown("View"))
		{
			audioSource.Play();
			status = 1;
			mouseVer = 0f;
			mouseHor = 0f;
		}
		if (Input.GetButtonUp("View"))
		{
			status = 0;
			previous = transform.rotation;
			target =  current;
			movingCount = movingPeriod;
			audioSource.Pause();
		}
		if (status == 1)
		{
			mouseVer +=  Input.GetAxis("View Ver");
			mouseHor +=  Input.GetAxis("View Hor");
			transform.rotation = current;
			transform.rotation *= Quaternion.Euler(0, mouseHor, 0);
			transform.rotation *= Quaternion.Euler( mouseVer, 0, 0);
			return;
		}

        if (movingCount > 0)
        {
			audioSource.Play();
            movingCount -= Time.deltaTime;
            if (movingCount < 0)
			{
                movingCount = 0;
				audioSource.Pause();
			}
            transform.rotation = Quaternion.Lerp(target, previous, movingCount / movingPeriod);
        }
		if (Input.GetButtonDown("Rotate Vertical") || Input.GetButtonDown("Rotate Horizonal"))
		{
			previous = transform.rotation;
		    if (Input.GetButtonDown("Rotate Vertical"))
				target = target * Quaternion.Euler(Input.GetAxis("Rotate Vertical") * 90, 0, 0);
		    if (Input.GetButtonDown("Rotate Horizonal"))
				target = target * Quaternion.Euler(0, Input.GetAxis("Rotate Horizonal") * 90, 0);
			movingCount = movingPeriod;
			normal = target * new Vector3(0, 0, 1);
			current = target;

		}

	}
}
