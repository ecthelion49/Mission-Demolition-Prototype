using UnityEngine;
using System.Collections;

public class CloudCrafter : MonoBehaviour {
	// Fields set in the Unity Inspector pane

	public int				numClouds = 40;			// The number of clouds to make
	public GameObject[]		cloudPrefabs;			// The prefabs for the clouds
	public Vector3			cloudPosMin;			// Min position of each cloud
	public Vector3			cloudPosMax;			// Max position of each cloud
	public float			cloudScaleMin = 1;		// Min scale of each cloud
	public float			cloudScaleMax = 5;		// Max scale of each clouf
	public float			cloudSpeedMult = 10f;	// Adjusts speed of clouds

	public bool           __________________________________;

	// Fields set dynamically
	public GameObject[]		cloudInstances;

	void Awake() {
		// Make an array large enough to hold all the Cloud_ instances
		cloudInstances = new GameObject[numClouds];

		// Find the CloundAnchor parent GameObject
		GameObject anchor = GameObject.Find ("CloudAnchor");

		// Iterate through and make Cloud_s
		GameObject cloud;

		for (int i = 0; i < numClouds; i++) {
			// Pick an int between 0 and cloudPreFabs.Length - 1
			// Random.Range will not over picl as high as the top number
			int prefabNum = Random.Range (0, cloudPrefabs.Length);

			// Make an instance
			cloud = Instantiate (cloudPrefabs [prefabNum]) as GameObject;

			// Position cloud
			Vector3 cPos = Vector3.zero;

			cPos.x = Random.Range (cloudPosMin.x, cloudPosMax.x);
			cPos.y = Random.Range (cloudPosMin.y, cloudPosMax.y);

			// Scale cloud
			float scaleU = Random.value;
			float scaleVal = Mathf.Lerp (cloudScaleMin, cloudScaleMax, scaleU);

			// Smaller clouds (with Smaller Scale U) should be nearer the ground
			cPos.y = Mathf.Lerp (cloudPosMin.y, cPos.y, scaleU);

			// Smaller clouds should be further away
			cPos.z = 100 - 90 * scaleU;

			// Apply these transforms to the cloud
			cloud.transform.position = cPos;
			cloud.transform.localScale = Vector3.one * scaleVal;

			// Make cloud a child of the anchor
			cloud.transform.parent = anchor.transform;

			// Add the cloud to clouInstances
			cloudInstances [i] = cloud;
		}
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Iterate over each cloud that was created
		foreach (GameObject cloud in cloudInstances) {
			// Get the cloud scale and position
			float scaleVal = cloud.transform.localScale.x;
			Vector3 cPos = cloud.transform.position;

			// Move larger clouds faster
			cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;

			// If a cloud has moved too far to the left ...
			if (cPos.x <= cloudPosMin.x) {
				// Move it to the far right
				cPos.x = cloudPosMax.x;
			}

			// Apply the new position to cloud
			cloud.transform.position = cPos;
		}
	}
}
