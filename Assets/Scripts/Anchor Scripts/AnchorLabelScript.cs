
using UnityEngine;
using System.Collections;

[RequireComponent (typeof (GUIText))]
public class AnchorLabelScript : MonoBehaviour {

	public Transform target;  // Object that this label should follow
	public Vector3 offset = new Vector3();    // Units in world space to offset; 1 unit above object by default
	public bool clampToScreen = false;  // If true, label will be visible even if object is off screen
	public float clampBorderSize = 0.05f;  // How much viewport space to leave at the borders when a label is being clamped
	public bool useMainCamera = true;   // Use the camera tagged MainCamera
	public Camera cameraToUse ;   // Only use this if useMainCamera is false
	Transform thisTransform;

    private MeshRenderer meshRenderer;


	void Start () 
	{
//		offset.x = 1f;
//		offset.y = 0f;
//		offset.z = 0.5f;
		thisTransform = transform;
		thisTransform.rotation = Quaternion.Euler (90, 0, 0);

        meshRenderer = GetComponent<MeshRenderer>();
	}

	void Update()
	{
		thisTransform.position = target.position + offset;
		thisTransform.rotation = Quaternion.Euler (90, 0, 0);
	}

    public void Toggle(bool newState) {
        meshRenderer.enabled = newState;
    }
}
