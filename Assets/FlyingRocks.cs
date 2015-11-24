using UnityEngine;
using System.Collections;

public class FlyingRocks : MonoBehaviour {

	// Use this for initialization
	public Rigidbody rock;
	public Transform OriginRock;
	public Transform[][] points;
	public Transform[] collection1;
	public Transform[] collection2;
	void Start () {
		OriginRock = GameObject.FindWithTag ("OriginRock").transform;


	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
