//  Generator
//  	Gnerating the maze with random-Kruskal and set up the environment.

using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {
    [SerializeField] public GameObject wallSource, transparentWallSource, goal, camera_, path;
	[SerializeField] public RotatingCamera camera;
	[SerializeField] public Placer placer;
	[SerializeField] public PathPlacer pathPlacer;
	[SerializeField] public Player player;
    public int cubeSize = 1;
    public float moveingSpeed = 1f;
    public int mapSize = 2;
    public int seed;
	public GameObject[] list;
	private int listCnt;
    //public GameObject[] wall;
    public bool[] pass;
	// Use this for initialization
	void Start () {
        placer = wallSource.GetComponent<Placer>();
        Generate();
	}
	
	// Update is called once per frame
	void Update () {
	}
    
    void Generate()
	{
		player.Reset();
		camera.gameObject.transform.position = new Vector3(mapSize, mapSize, mapSize);
		camera_.transform.localPosition = new Vector3(0, 0, -mapSize * 3);
        System.Random r = new System.Random();
        UnionFind uf = new UnionFind(mapSize);
        GameObject wall = (GameObject)Resources.Load("Wall");
		list = new GameObject[(2 * mapSize + 1) * (2 * mapSize + 1) * (2 * mapSize + 1) * 3];
		listCnt = 0;
        Vector4[] road = new Vector4[mapSize * mapSize * mapSize * 3];
        for (int x = 0; x < mapSize; ++x)
            for (int y = 0; y < mapSize; ++y)
                for (int z = 0; z < mapSize; ++z)
                    for (int k = 0; k < 3; ++k)
                        road[(x * mapSize * mapSize + y * mapSize + z) * 3 + k] = new Vector4(x, y, z, k);
        pass = new bool[mapSize * mapSize * mapSize * 3];
        Vector4[] kruskal = (Vector4[])road.Clone();
        for (int ii = 0; ii < mapSize * mapSize * mapSize * 3; ++ii)
        {
            int j = r.Next(mapSize * mapSize * mapSize * 3);
            Vector4 t = kruskal[ii];
            kruskal[ii] = kruskal[j];
            kruskal[j] = t;
        }
        int cnt = 1;
        int i= -1;
        while (cnt < mapSize * mapSize * mapSize)
        {
            ++i;
            Vector3 point1 = v4tov3(kruskal[i]);
            Vector3 point2 = point1;
            if (kruskal[i].w == 0)
                point2.x++;
            if (point2.x == mapSize)
                continue;
            if (kruskal[i].w == 1)
                point2.y++;
            if (point2.y == mapSize)
                continue;
            if (kruskal[i].w == 2)
                point2.z++;
            if (point2.z == mapSize)
                continue;
            if (!uf.Union(point1, point2))
            {
                ++cnt;
                pass[uf.v3toI(point1) * 3 + (int)kruskal[i].w] = true;
            }
        }
        //absolute wall

        for (int x = 0; x <= mapSize; ++x)
            for (int y = 0; y <= mapSize; ++y)
                for (int z = 0; z <= mapSize; ++z)
				{
					placeWall(x * 2, y * 2, z * 2);
					if (x != mapSize)
						placeWall(x * 2 + 1, y * 2, z * 2);

					if (y != mapSize)
						placeWall(x * 2, y * 2 + 1, z * 2);
					if (z != mapSize)
						placeWall(x * 2, y * 2, z * 2 + 1);
				}
        //border
	
        for (int m = 0; m < mapSize; ++m)
            for (int k = 0; k < mapSize; ++k)
            {
                placeWall(m * 2 + 1, k * 2 + 1, 0);
                placeWall(k * 2 + 1, m * 2 + 1, 0);
                placeWall(0, m * 2 + 1, k * 2 + 1);
                placeWall(0, k * 2 + 1, m * 2 + 1);
                placeWall(m * 2 + 1, 0, k * 2 + 1);
                placeWall(k * 2 + 1, 0, m * 2 + 1);

            }

        //dynamic wall
        for (int x = 0; x < mapSize; ++x)
            for (int y = 0; y < mapSize; ++y)
                for (int z = 0; z < mapSize; ++z)
                {
					if (x + y + z < 3 * mapSize - 3)
						placePath(2 * x + 1, 2 * y + 1, 2 * z + 1);
					int index = x * mapSize * mapSize + y * mapSize + z;
                    if (!pass[index * 3])
                        placeWall(2 * x + 2, 2 * y + 1, 2 * z + 1);
					else
						placePath(2 * x + 2, 2 * y + 1, 2 * z + 1);
                    if (!pass[index * 3 + 1])
                        placeWall(2 * x + 1, 2 * y + 2, 2 * z + 1);
					else
						placePath(2 * x + 1, 2 * y + 2, 2 * z + 1);
                    if (!pass[index * 3 + 2])
						placeWall(2 * x + 1, 2 * y + 1, 2 * z + 2);
					else
						placePath(2 * x + 1, 2 * y + 1, 2 * z + 2);
                }

		placer.z = 500;
		placer.Ref();
		goal.transform.position = 2 * mapSize * Vector3.one - Vector3.one;
    }

	public bool passCheck(Vector3 position, Vector3 movement)
	{
		if (OutOfBound(position + movement))
			return false;
		int k = 0, index = 0;
		if (Mathf.Abs(movement.x - 0) > 0.1)
			k = 0;
		if (Mathf.Abs(movement.y - 0) > 0.1)
			k = 1;
		if (Mathf.Abs(movement.z - 0) > 0.1)
			k = 2;
		if (movement.x + movement.y + movement.z < 0)
			position += movement;
		index = ((int)(position.x + 0.2) * mapSize * mapSize + (int)(position.y + 0.2) * mapSize + (int)(position.z + 0.2)) * 3 + k;
		return pass[index];
	}

	public void New()
	{
		foreach(GameObject temp in list)
			Destroy(temp);
		Generate();

	}

	bool OutOfBound(Vector3 position)
	{
		bool result = false;
		result |= position.x < -0.1;
		result |= position.y < -0.1;
		result |= position.z < -0.1;
		result |= position.x >= mapSize;
		result |= position.y >= mapSize;
		result |= position.z >= mapSize;
		return result;
	}

    Vector3 v4tov3(Vector4 a)
    {
        return new Vector3(a.x, a.y, a.z);
    }

    void placeWall(int x, int y, int z)
    {
        placer.x = x;
        placer.y = y;
        placer.z = z;
        placer.Ref();
        GameObject solid = Instantiate<GameObject>(wallSource);
        GameObject temp = Instantiate<GameObject>(transparentWallSource);
		temp.transform.position = solid.transform.position;
		list[listCnt++] = solid;
		list[listCnt++] = temp;
    }
	void placePath(int x, int y, int z)
	{
		pathPlacer.x = x;
		pathPlacer.y = y;
		pathPlacer.z = z;
		pathPlacer.Ref();
		GameObject temp = Instantiate<GameObject>(path);
		list[listCnt++] = temp;
	}

}

public class UnionFind
{
    public int[] p;
    private int size;
    public UnionFind(int s)
    {
        size = s;
        p = new int[size * size * size];
        for (int i = 0; i < size * size * size; ++i)
            p[i] = -1;
    }
    public bool Union(Vector3 a, Vector3 b)
    {
        int aa = Find(a);
        int bb = Find(b);
        if (aa == bb)
            return true;
        if (p[aa] > p[bb])
        {
            p[aa] += p[bb];
            p[bb] = aa;
        }
        else
        {
            p[bb] += p[aa];
            p[aa] = bb;
        }
        return false;

    }
    public int Find(Vector3 aa)
    {
        int a = v3toI(aa);
        int q = a;
        while (p[a] >= 0)
        {
            a = p[a];
        }
        int s = p[q];
        if (s < 0)
            return a;
        p[q] = a;
        while (p[s] >= 0)
        {
            q = s;
            s = p[q];
            p[q] = a;
        }
        return a;
    }
    public int v3toI(Vector3 a)
    {
        return (int)(a.x * size * size + a.y * size + a.z);
    }
}
