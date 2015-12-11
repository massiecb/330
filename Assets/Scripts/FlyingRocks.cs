using UnityEngine;
using System.Collections;
using System.Linq;

public class FlyingRocks : MonoBehaviour {

	// Use this for initialization
	public GameObject rock;
	public GameObject OriginRock;
	public Vector3 rockThrow;
	public Transform[] trackA;
	public Transform[] trackC;
	public Transform[] trackB;
	public Transform[] rocks;
	public GameObject[] rockObjects;
	public Terrain ground;
	int num_rocks = 80;
	void Start () {
		OriginRock = GameObject.FindWithTag ("OriginRock");
		trackA = GameObject.FindWithTag ("TrackA").GetComponentsInChildren<Transform> ();
		trackC = GameObject.FindWithTag ("TrackC").GetComponentsInChildren<Transform> ();
		trackB = GameObject.FindWithTag ("TrackB").GetComponentsInChildren<Transform> ();
		ground = Terrain.activeTerrain;
		rocks = new Transform[num_rocks];
		rockObjects = new GameObject[num_rocks];
		for (int i = 0; i < num_rocks; i++) {
			int coinFlip = Random.Range (0, 3);
			if (coinFlip == 0){/*
				int objPosition = Random.Range (0, trackA.Length - 1);
				rocks[i] = trackA[objPosition];*/
				rocks[i] = NewRock (rocks, trackA);
			}
			else if (coinFlip == 1){
				/*
				int objPosition = Random.Range (0, trackC.Length - 1);
				rocks[i] = trackC[objPosition];*/
				rocks[i] = NewRock(rocks, trackB);
			}
			else if (coinFlip == 2){/*
				int objPosition = Random.Range (0, trackB.Length - 1);
				rocks[i] = trackC[objPosition];*/
				rocks[i] = NewRock(rocks, trackC);
			}
			rockObjects[i] = (GameObject) Instantiate(OriginRock, new Vector3 (rocks[i].position.x, rocks[i].position.y + 25f, rocks[i].position.z), Quaternion.identity);
			rockObjects[i].GetComponent<Renderer>().enabled = true;
			rockObjects[i].GetComponent<SphereCollider>().enabled = true;
			rockObjects[i].transform.position = new Vector3 (rocks[i].position.x, rocks[i].position.y + 25f, rocks[i].position.z);
			rockObjects[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
		}

	}
	
	// Update is called once per frame
	void Update () {

	}

	Transform NewRock (Transform [] rocks, Transform[] track){
		Transform transform1;
		int position = Random.Range (0, track.Length - 1);
		if (position < 46 || position > 113) {
			if (rocks.Contains (track [position]))
				transform1 = NewRock (rocks, track);
			else 
				transform1 = track [position];

		} else 
			transform1 = NewRock (rocks, track);
		return transform1;

	}

}


