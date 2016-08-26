using UnityEngine;
using System.Collections;

public class CubazeSystem : MonoBehaviour 
{
	[SerializeField] Generator generator;
	[SerializeField] Player player;
	[SerializeField] RotatingCamera camera;
	[SerializeField] GameObject ui;
	[SerializeField] UnityEngine.UI.Text text;
	// Use this for initialization
	void Start ()
	{
		Call();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButton("Cancel") && (!ui.activeSelf))
			Call();
	}

	public void SetUp()
	{
		int t = GetInitialLevel();
		if ((t >= 2) && (t <= 10))
		{
			generator.New(t);
			player.controling = true;
			camera.controling = true;
			ui.SetActive(false);
		}
	}

	public void Call ()
	{
		StopControl();
		ui.SetActive(true);
	}

	public void StopControl()
	{
		player.controling = false;
		camera.controling = false;
	}

	int GetInitialLevel()
	{
		System.Int32 k = 0;
		System.Int32.TryParse(text.text, out k);
		return k;
	}
}
