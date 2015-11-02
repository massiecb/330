using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Track : MonoBehaviour {

    public Vector3 lane;
    public Vector3[] lane_display;
    public float u, velocity, first_gear, second_gear, third_gear, fourth_gear, top_speed, drag, weight, angle, road_smooth;
    public Transform[] ctrl;
    private BSpline b;
	// Use this for initialization
	void Start () {
        b = new BSpline(ctrl);
        u = 0f;
        top_speed = 40f;
//        road_smooth = 0.1f;
        velocity = 0;
        lane = Vector3.zero;
        drag *= weight;
        int x = 0;
        for (float i = 0; i < ctrl.Length; i += 0.1f, x++)
            lane_display[x] = (b.Evaluate(i) - b.Evaluate(i+0.1f));
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        changeLanes();
        deltaVelocity();
        u += Time.deltaTime * velocity / 5;
        if (u >= b.Length)
            u -= b.Length;
        Vector3 next_pos = b.Evaluate(u);
        for (int i = 0; i < lane_display.Length; i++)
            Handles.DrawLine(lane_display[i], lane_display[i + 1]);
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

    void changeLanes() {
        if (CrossPlatformInputManager.GetAxis("Horizontal") < 0 && lane.z > -2)
            lane.z -= 2;
        else if (CrossPlatformInputManager.GetAxis("Horizontal") < 0 && lane.z < 2)
            lane.z += 2;
    }
}