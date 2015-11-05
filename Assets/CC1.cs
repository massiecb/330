using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CC1 : MonoBehaviour {
	
	public Vector3 lane;
	public float u, velocity, first_gear, second_gear, third_gear, fourth_gear, top_speed, drag, weight, angle, road_smooth;
	public Transform[] ctrlA;
	public Transform[] ctrlB;
	public BSpline[] BS;
	public Transform[][] array; 
	//private BSpline b;
	//private BSpline b2;
	private int whatTrack;
	private int trackMin = 0;
	private int trackMax = 1;
	// Use this for initialization
	void Start () {
		//b = new BSpline(ctrlA);
		//b2 = new BSpline (ctrlB);
		BS = new BSpline[] {
			new BSpline(ctrlA) , 
			new BSpline(ctrlB)};
		array = new Transform[][] {
			 new Transform[ctrlA.Length],
			new Transform[ctrlB.Length]
		};
		//b = BS [0];
		//b2 = BS [1];
		array [0] = ctrlA;
		array [1] = ctrlB;
		whatTrack = 0;
		u = 0f;
		top_speed = 40f;
		//        road_smooth = 0.1f;
		velocity = 0;
		lane = Vector3.zero;
		drag *= weight;
			
	}

	
	// Update is called once per frame
	void FixedUpdate () {
		//changeLanes(); // after net_pos
		deltaVelocity();
		u += Time.deltaTime * velocity / 5;
		if (u >= BS[whatTrack].Length)
			u -= BS[whatTrack].Length;
//		Vector3 next_pos = BS[whatTrack].Evaluate(u);
		Vector3 next_pos = changeLanes ();
		//changeLanes (next_pos);
		transform.LookAt(next_pos);
		transform.position = next_pos + lane;        
	}
	
	void deltaVelocity() {
		if (CrossPlatformInputManager.GetAxis("Vertical") == 0 && velocity > 0)
			velocity -= drag;
		else if (velocity <= 10)
			velocity += CrossPlatformInputManager.GetAxis("Vertical")/first_gear;
		else if (velocity <= 20)
			velocity += CrossPlatformInputManager.GetAxis("Vertical") / second_gear;
		else if (velocity <= 30)
			velocity += CrossPlatformInputManager.GetAxis("Vertical") / third_gear;
		else if (velocity < top_speed)
			velocity += CrossPlatformInputManager.GetAxis("Vertical") / fourth_gear;
		else
			velocity = top_speed;
		if (velocity < 0)
			velocity = 0;
	}
	
	Vector3 changeLanes() {
		// in this the size is 2
		Vector3 nextPosition = BS [whatTrack].Evaluate (u);
		//float searchX;
		//float searchZ;
		//int lowestIndex = 0;
		if (CrossPlatformInputManager.GetAxis ("Horizontal") > 0 && whatTrack != trackMax) {
			whatTrack += 1;
			//searchX = transform.position.x / 2f;
			//searchZ = transform.position.z / 2f;
			for (int i = 0; i < array[whatTrack].Length; i++){ 
				//Debug.Log ("bob");
				if ((Mathf.Abs(array[whatTrack][i+1].position.x - nextPosition.x)) > (Mathf.Abs (array[whatTrack][i].position.x - nextPosition.x))){
					return array[whatTrack][i];
				}

			}
		} 
		else if (CrossPlatformInputManager.GetAxis ("Horizontal") < 0 && whatTrack != trackMin) {
			whatTrack -=1;
			//searchX = transform.position.x / 2f;
			//searchZ = transform.position.z /2f;
			for (int i = 0; i < array[whatTrack].Length; i++){
				Debug.Log ("AntiBob");
			}
		}
		return nextPosition;
	}
}