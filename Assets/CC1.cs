using UnityEngine;
using System;
using UnityEditor;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CC1 : MonoBehaviour {
	//force commit
	public float u, velocity, first_gear, second_gear, third_gear, fourth_gear, top_speed, drag, weight, lane_change;
	public Transform[] ctrlA, ctrlB;
	public BSpline[] BS;
	public Transform[][] array; 
	private BSpline b, b2;
	private int whatTrack;
	private int trackMin = 0, trackMax = 1;
	private Vector3 original, prev_track;

	// Use this for initialization
	void Start () {
//		Debug.Log (b);
		b = new BSpline(ctrlA);
		b2 = new BSpline (ctrlB);
		BS = new BSpline[] {b, b2};
		array = new Transform[][] {new Transform[ctrlA.Length], new Transform[ctrlB.Length]};
		array [0] = ctrlA;
		array [1] = ctrlB;
		whatTrack = 0;
		u = 0f;
		top_speed = 40f;
		velocity = 0;
		drag *= weight;
        lane_change = 0f;
        for (int i = 0; i < ctrlA.Length; i++)
            ctrlA[i].GetComponent<Renderer>().enabled = false;
        for (int i = 0; i < ctrlB.Length; i++)
            ctrlB[i].GetComponent<Renderer>().enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		velocity = 5;
		u += Time.fixedDeltaTime * velocity / 5;
        if (CrossPlatformInputManager.GetAxis("Horizontal") != 0)
            u += changeLanes(u) - (float)Math.Truncate(u);
        if (u >= BS[whatTrack].Length)
			u -= BS[whatTrack].Length;
        Vector3 next_pos = BS[whatTrack].Evaluate(u);
        if (lane_change > 0) {
            if (lane_change - 0.05f < 0f)
                lane_change = 0;
            else
                lane_change -= 0.05f;
            transform.position = transform.position + (next_pos - transform.position).normalized * velocity * Time.deltaTime;
        } else {
            transform.LookAt(next_pos);
            transform.position = next_pos;
        }
    }

    void deltaVelocity() {
		//Debug.Log ("DV: " + drag + " " + first_gear + " " + second_gear + " " + third_gear + " " + fourth_gear + " " + top_speed);
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
	
	public int changeLanes(float temp) {
		Vector3 nextPosition = BS [whatTrack].Evaluate (u);
		if (CrossPlatformInputManager.GetAxis ("Horizontal") > 0 && whatTrack > trackMin) {
			whatTrack -= 1;
//			for (int i = 0; i < array[whatTrack].Length - 1; i++){ 
//				if (Mathf.Abs (original.x - array[whatTrack][i].position.x) < Mathf.Abs (original.x  - array[whatTrack][i+1].position.x)){
//					if (Mathf.Abs (original.z - array[whatTrack][i].position.z) < Mathf.Abs(original.z - array[whatTrack][i+1].position.z)){
//                        return i;
//                    }
//				}
//			}
		} 
		else if (CrossPlatformInputManager.GetAxis ("Horizontal") < 0 && whatTrack < trackMax) {
            whatTrack +=1;
//			for (int i = 0; i < array[whatTrack].Length - 1; i++){
//				if (Mathf.Abs (original.x - array[whatTrack][i].position.x) < Mathf.Abs (original.x  - array[whatTrack][i+1].position.x)){
//					if (Mathf.Abs (original.z - array[whatTrack][i].position.z) < Mathf.Abs(original.z - array[whatTrack][i+1].position.z)){
//                        return i;
//                    }
//				}
//			}
		}
        return (int) Math.Truncate(temp);
	}
}