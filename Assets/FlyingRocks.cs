using UnityEngine;
using System.Collections;

public class FlyingRocks : MonoBehaviour {

	// Use this for initialization
	public GameObject rock;
	public GameObject OriginRock;
	public Vector3 rockThrow;
	public Transform[] trackA;
	public Transform[] trackC;
	public Transform[] rocks;
	public GameObject[] rockObject;
	private int rockNumber = 0;
	void Start () {
		OriginRock = GameObject.FindWithTag ("OriginRock");
		trackA = GameObject.FindWithTag ("TrackA").GetComponentsInChildren<Transform> ();
		trackC = GameObject.FindWithTag ("TrackC").GetComponentsInChildren<Transform> ();
		rocks = new Transform[20];
		for (int i = 0; i < 20; i++) {
			int coinFlip = Random.Range (0, 1);
			if (coinFlip > 0){
				int objPosition = Random.Range (0, trackA.Length);
				rocks[i] = trackA[objPosition];
			}
			else{
				int objPosition = Random.Range (0, trackA.Length);
				rocks[i] = trackC[objPosition];
			}
		}
		rockObject = new GameObject[20];

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (rockNumber < rocks.Length) {
			rock = Instantiate (OriginRock);
			rockObject[rockNumber] = rock;
			rock.GetComponent<Renderer>().enabled = true;
			rock.GetComponent<SphereCollider>().enabled = true;
			rockThrow = ThrowRock (OriginRock.transform, rocks [rockNumber]);
			rock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			//rock.GetComponent<Transform>().LookAt(rocks[rockNumber]);
			rock.GetComponent<Rigidbody>().AddForce (rockThrow, ForceMode.VelocityChange);
			//rock.transform.position = rocks[rockNumber].position;
			if (rock.transform.position == rocks[rockNumber].position)
				rock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			rockNumber += 1;
		}
		if (rockNumber == rocks.Length - 1) {
			for (int i = 0; i < rockObject.Length; i++){
				rockObject[i].GetComponent<Renderer>().enabled = false;
				rockObject[i].GetComponent<Transform>().position = rocks[i].position;
				rockObject[i].GetComponent<Transform>().position = new Vector3 (rocks[i].position.x, rocks[i].position.y + 25f, rocks[i].position.z);
				rockObject[i].GetComponent<Renderer>().enabled = true;
				rockObject[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
			}
		}


	}

	private Vector3 ThrowRock (Transform origin, Transform target){
		/*
		//use basic phyiscs formuals to calculate
		float yInitialVelocity;
		float xInitialVelocity;
		float zInitialVelocity;
		Vector3 setTarget;
		float airTime = 10f;
		Vector3 toTarget = target.position - origin.position;//origin.position - target.position;
		//initial y speed = distance/ 1/2 gt^2
		//yInitialVelocity = toTarget.y / airTime + 0.5f * Physics.gravity.magnitude * airTime * airTime;
		yInitialVelocity = (toTarget.y - 0.5f * Physics.gravity.magnitude * airTime * airTime) / airTime;
		//horizontal have no acceleration v = d/t
		xInitialVelocity = toTarget.x / airTime;
		zInitialVelocity = toTarget.z / airTime;
		//setTarget = toTarget.normalized; // set direction?
		setTarget.x = xInitialVelocity;
		setTarget.z = zInitialVelocity;
		setTarget.y = yInitialVelocity;
		return setTarget;


		float distance = (target.position - origin.position).sqrMagnitude;
		Vector3 apply = new Vector3 (1, 0, 0) * distance/10f; //v = d/t
		float zAngle = Mathf.Acos ( (Vector3.Cross (origin.position, target.position).magnitude) / (origin.position.magnitude * target.position.magnitude)); 
		Debug.Log ("target:" + target.position.z + " origin:" + origin.position.z);
		Debug.Log ("zangle: " + zAngle);
		apply.z = zAngle * 2f;// Mathf.Deg2Rad;
		Debug.Log (apply.z);
		float yAngle = Mathf.Asin ( (Vector3.Cross (origin.position, target.position).magnitude) / (origin.position.magnitude * target.position.magnitude));
		apply.y = yAngle * 2f;
		return apply;
		*/
		Vector3 apply = new Vector3 (1, 1, 1);
		float zAngle = Vector3.Angle (origin.position, target.position);
		apply.z = apply.z * zAngle;
		float yAngle = Mathf.Asin ((Vector3.Cross (origin.position, target.position).magnitude) / (origin.position.magnitude * target.position.magnitude));
		apply.y = apply.y * yAngle * 1.5f *  Mathf.Rad2Deg;
		Debug.Log (apply);
		return apply;
	}
}
