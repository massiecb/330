using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * B Spline
 * 
 * Used to evaluate a smooth curve using a B Spline
 * 
 * Author:	B. Brookwell
 * Date:	2015-02-12
 * 
 * Constant		Description
 * 
 * OneSixth		1/6 (Common numerical factor in basis functions for B Spline)
 * TwoThirds	2/3 (Common numerical factor in basis functions for B Spline)
 * 
 * Private		Description
 * 
 * ctlPoint		Array of target control points
 * 
 * Public		Description
 * 
 * Length		Number of control points
 */

public class BSpline {
	private const float OneSixth = 1f / 6f;
	private const float TwoThirds = 2f / 3f;
	private Transform[] ctlPoint;
	
	public int Length;

/**
 * Creator for B Spline.  It saves the control points and sets the Length
 * 
 * Parameter	Description
 * 
 * v			Array of control points defining the B Spline
 */
	public BSpline (Transform[] v) {
		ctlPoint = v;
		Length = v.Length;
	}

/**
 * Evaluates the B Spline function at parameter value 'u'
 * 
 * Parameter	Description
 * 
 * u			Value used to evaluate the B Spline (0 .. Length of ctlPoint)
 */

	public Vector3 Evaluate (float u) {
		int start = Mathf.FloorToInt (u);
		u -= start;
		float oneMinusU = 1f - u;
		float u2 = u * u; 			// u squared
		float u3 = u2 * u;			// u cubed

		float b0 = oneMinusU * oneMinusU * oneMinusU * OneSixth; // Basis functions
		float b1 = 0.5f * u3 - u2 + TwoThirds;
		float b2 = -0.5f * u3 + 0.5f * u2 + 0.5f * u + OneSixth;
		float b3 = u * u * u * OneSixth;

		return b0 * ctlPoint[start].position + 				// Calculate point on curve
			b1 * ctlPoint[(start + 1) % ctlPoint.Length].position + 
			b2 * ctlPoint[(start + 2) % ctlPoint.Length].position + 
			b3 * ctlPoint[(start + 3) % ctlPoint.Length].position;
	}
}
