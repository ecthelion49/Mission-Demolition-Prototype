using UnityEngine;
using System.Collections;
// Remember the following line is needed to use Lists
using System.Collections.Generic;


public class ProjectileLine : MonoBehaviour {
	static public ProjectileLine S;			// Singleton

	// Fields set in the Unity Inspector pane
	public float			minDist = 0.1f;
	public bool				_____________________________________;

	// FIelds set dynamically
	public LineRenderer		line;
	private GameObject		_poi;
	public List<Vector3>	points;

	void Awake() {
		S = this;		// Set the singleton

		// Get a reference to the LineRenderer
		line = GetComponent<LineRenderer> ();

		// Disable the LineRenderer until it's needed
		line.enabled = false;

		// Initialise the points list
		points = new List<Vector3> ();
	}

	// This is a property (that is, a method masquerading as a field)
	public GameObject poi {
		get {
			return (_poi);
		}
		set {
			_poi = value;
			if (_poi != null) {
				// When _poi is set to something new, it resets everything
				line.enabled = false;
				points = new List<Vector3> ();
				AddPoint ();
			}
		}
	}

	// This can be used to clear the line directly
	public void Clear() {
		_poi = null;
		line.enabled = false;
		points = new List<Vector3> ();
	}

	public void AddPoint () {
		// This is called to add a point to the line
		Vector3 pt = _poi.transform.position;
		if (points.Count > 0 && (pt - lastPoint).magnitude < minDist) {
			// If the point isn't far enough from the last point it returns
			return;
		}

		if (points.Count == 0) {
			// If this is the launch point ...
			Vector3 launchPos = SlingShot.S.launchPoint.transform.position;
			Vector3 launchPosDiff = pt - launchPos;

			// ... it adds an extra bit of line to aid aiming later
			points.Add (pt + launchPosDiff);
			points.Add (pt);
			line.SetVertexCount (2);

			// Set the first two points
			line.SetPosition (0, points [0]);
			line.SetPosition (1, points [1]);

			// Enables the line rendered
			line.enabled = true;
		} else {
			// Normal behaviour of adding a point
			points.Add (pt);
			line.SetVertexCount (points.Count);
			line.SetPosition ( points.Count - 1, lastPoint);
			line.enabled = true;
		}
	}

	// Returns the location of the most recently added point
	public Vector3 lastPoint {
		get {
			if (points == null) {
				// If there are no points, returns the Vector3 zero
				return (Vector3.zero);
			}

			return (points [points.Count - 1]);
		}
	}

	void FixedUpdate () {
		if (poi == null) {
			// If there is no poi, search for one
			if (FollowCam.S.poi != null) {
				if (FollowCam.S.poi.tag == "Projectile") {
					poi = FollowCam.S.poi;
				} else {
					return;		// Return if we didn't find a poi
				}
			} else {
				return;		// Return if we didn't find a poi
			}
		}

		// If there is a poi, it's loc is added every Fixed update
		AddPoint ();
		if (poi.rigidbody.IsSleeping ()) {
			// Once the poi is sleeping, it is cleared
			poi = null;
		}
	}
}
