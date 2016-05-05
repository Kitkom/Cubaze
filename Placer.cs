using UnityEngine;
using System.Collections;

public class Placer : MonoBehaviour {

    public int x, y, z;
	public bool corner;
	[SerializeField] Player player;
    [SerializeField] RotatingCamera camera;
	// Use this for initialization
	void Start () {
        Check();

	}
	
	// Update is called once per frame
	void Update () {
        
		if (Input.GetButtonDown("Rotate Vertical") ||  Input.GetButtonDown("Rotate Horizonal") || Input.GetButtonDown("View") || Input.GetButtonUp("View"))
			Check();
	}

    public void Ref()
    {
		corner = false;
        transform.position = new Vector3(x, y, z);
		corner |= (x % 2 + y % 2 + z % 2) % 2 == 1;

    }

    void Check()
    {
        gameObject.layer = 8;
		if ((camera.status == 0) && (Mathf.Abs(camera.normal.x - 0) > 0.1) && (x == player.target.x))
            gameObject.layer = 0;
		if ((camera.status == 0) && (Mathf.Abs(camera.normal.y - 0) > 0.1) && (y == player.target.y))
            gameObject.layer = 0;
		if ((camera.status == 0) && (Mathf.Abs(camera.normal.z - 0) > 0.1) && (z == player.target.z))
            gameObject.layer = 0;
    }


}
