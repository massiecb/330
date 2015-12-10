using UnityEngine;
using System.Collections;

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
	private int rockNumber = 0;
	void Start () {
		OriginRock = GameObject.FindWithTag ("OriginRock");
		trackA = GameObject.FindWithTag ("TrackA").GetComponentsInChildren<Transform> ();
		trackC = GameObject.FindWithTag ("TrackC").GetComponentsInChildren<Transform> ();
		trackB = GameObject.FindWithTag ("TrackB").GetComponentsInChildren<Transform> ();
		ground = Terrain.activeTerrain;
		rocks = new Transform[20];
		rockObjects = new GameObject[20];
		for (int i = 0; i < 20; i++) {
			int coinFlip = Random.Range (0, 3);
			if (coinFlip == 0){
				int objPosition = Random.Range (0, trackA.Length);
				rocks[i] = trackA[objPosition];
			}
			else if (coinFlip == 1){
				int objPosition = Random.Range (0, trackC.Length);
				rocks[i] = trackC[objPosition];
			}
			else {
				int objPosition = Random.Range (0, trackB.Length);
				rocks[i] = trackC[objPosition];
			}
			rockObjects[i] = Instantiate(OriginRock);
			rockObjects[i].GetComponent<Renderer>().enabled = true;
			rockObjects[i].GetComponent<SphereCollider>().enabled = true;
			rockObjects[i].transform.position = new Vector3 (rocks[i].position.x, rocks[i].position.y + 25f, rocks[i].position.z);
			rockObjects[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
		}


	}
	
	// Update is called once per frame
	void FixedUpdate () {
		/*
		for (int i = 0; i < rockObjects.Length; i++)
			if (rockObjects [i].transform.position.y + 4 <= rocks [i].position.y) {
				rockObjects [i].GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
			}*/
	}

		/*
		if (rockNumber < 1){//rockObjects.Length) {
			rockObjects[rockNumber].GetComponent<Renderer>().enabled = true;
			rockObjects[rockNumber].GetComponent<SphereCollider>().enabled = true;
			rockThrow = ThrowRock (OriginRock.transform, rocks[rockNumber].transform);
			Debug.Log (rockThrow);
			rockObjects[rockNumber].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			rockObjects[rockNumber].GetComponent<Rigidbody>().AddForce (rockThrow, ForceMode.VelocityChange);
			rockNumber +=1;
		}
		if (rockNumber == rockObjects.Length) {
			for (int i = 0; i < rockObjects.Length; i++){
				rockObjects[i].GetComponent<Renderer>().enabled = false;
				rockObjects[i].GetComponent<SphereCollider>().enabled = true;
				rockObjects[i].transform.position = new Vector3 (rocks[i].position.x, rocks[i].position.y + 25f, rocks[i].position.z);
				rockObjects[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
				rockObjects[i].GetComponent<Rigidbody>().AddForce(Vector3.down * 10, ForceMode.VelocityChange);
				rockObjects[i].GetComponent<Renderer>().enabled = true;
			}
			for (int i= 0; i < rockObjects.Length; i++){
				Debug.Log (rockObjects[i].transform.position.y +":: " + rocks[i].position.y);
				if (rockObjects [i].transform.position.y < rocks[i].position.y)
					rockObjects [i].GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
			}

		}

	}

	private Vector3 ThrowRock (Transform origin, Transform target){

		Vector3 apply = new Vector3 (1, 1, 1);
		float zAngle = Vector3.Angle (origin.position, target.position);
		apply.z = apply.z * zAngle;
		float yAngle = Mathf.Asin ((Vector3.Cross (origin.position, target.position).magnitude) / (origin.position.magnitude * target.position.magnitude));
		apply.y = apply.y * yAngle * 10f *  Mathf.Rad2Deg;
		return apply;
	}

	private void OnCollisionEnter (Collision other){
		Debug.Log (other.gameObject.name);
	}
	*/
}


