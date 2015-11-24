using UnityEngine;
using System;
using UnityEditor;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CC1 : MonoBehaviour {
    //force commit
    private GameObject ghost;
	public float u, velocity, first_gear, second_gear, third_gear, fourth_gear, top_speed, drag, weight, lane_change, shift;
//	public BSpline[] BS;
//	public Transform[][] array; 
	private BSpline b2;
	public int whatTrack;
	private int trackMin = -1, trackMax = 1, lane_change_pause = 0;
//	private Vector3 original, prev_track;

	// Use this for initialization
	void Start () {
        GameObject track = GameObject.FindGameObjectWithTag("TrackB");
        b2 = new BSpline (track.GetComponentsInChildren<Transform>());
        whatTrack = 0;
		u = 1f;
        ghost = new GameObject();
		top_speed = 80f;
		velocity = 0f;
        shift = 4.0f;
        drag = 2; weight = 10; drag *= weight;
        lane_change = 0f;
        first_gear = 20; second_gear = 18; third_gear = 16; fourth_gear = 12;
        for (int i = 0; i < track.transform.childCount; i++)
            track.transform.GetChild(i).GetComponent<Renderer>().enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        velocity = deltaVelocity(velocity);
        if (lane_change_pause < 15)
            lane_change_pause++;
        float point_smooth = 1 / (b2.Evaluate((float)Math.Truncate(u + 1)) - b2.Evaluate((float)Math.Truncate(u))).magnitude;
		u += Time.fixedDeltaTime * velocity / 5 * point_smooth;
        if (CrossPlatformInputManager.GetAxis("Horizontal") != 0 && lane_change_pause == 15)
            changeLanes();
        if (u >= b2.Length)
            u -= b2.Length;
        Vector3 next_pos = b2.Evaluate(u);
        ghost.transform.LookAt(next_pos);
        ghost.transform.position = next_pos;
        if (whatTrack == -1)
            next_pos = ghost.transform.position - ghost.transform.right * shift;
        else if (whatTrack == 1)
            next_pos = ghost.transform.position + ghost.transform.right * shift;
        if (lane_change != 1) {
            if (lane_change + 0.01 > 1f)
                lane_change = 1;
            else
                lane_change += 0.01f;
        }
        transform.localRotation = ghost.transform.localRotation;
        transform.position = transform.position * (1 - lane_change) + next_pos * lane_change;
    }

    private float deltaVelocity(float velocity) {
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
        return velocity;
	}
	
	public void changeLanes() {
        lane_change = 0;
        lane_change_pause = 0;
        if (CrossPlatformInputManager.GetAxis ("Horizontal") < 0 && whatTrack > trackMin)
			whatTrack -= 1;
		else if (CrossPlatformInputManager.GetAxis ("Horizontal") > 0 && whatTrack < trackMax)
            whatTrack +=1;
	}
}