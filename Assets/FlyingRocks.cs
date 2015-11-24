using UnityEngine;
using System.Collections;

public class FlyingRocks : MonoBehaviour {

	// Use this for initialization
	public Rigidbody rock;
	public Transform OriginRock;
	public Transform[] trackA;
	public Transform[] trackC;
	public Transform[] rocks;
	void Start () {
		OriginRock = GameObject.FindWithTag ("OriginRock").transform;
		//GameObject trackA1 = GameObject.FindWithTag ("TrackA");
		trackA = GameObject.FindWithTag ("TrackA").GetComponentsInChildren<Transform> ();
		trackC = GameObject.FindWithTag ("TrackC").GetComponentsInChildren<Transform> ();
		for (int i = 0; i < 20; i++) {
			int coinFlip = Random.Range (0, 1);
			if (coinFlip > 0){
				int position = Random.Range (0, trackA.Length);
				rocks[i] = trackA[position];
			}
			else{
				int position = Random.Range (0, trackA.Length);
				rocks[i] = trackC[position];
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
