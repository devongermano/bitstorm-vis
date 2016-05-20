using UnityEngine;
using System.Collections;

public class ZoneTrigger : MonoBehaviour {

	private Light myLight;
	private MeshRenderer myText;
    private TextMesh myTextMesh;
    private UnisonAPI api;

	void Awake() {
		myLight = GetComponentInChildren<Light> ();
		myText = GetComponentInChildren<MeshRenderer> ();
        myTextMesh = GetComponentInChildren<TextMesh>();
        api = GetComponent<UnisonAPI>();
	}

	void OnTriggerEnter(Collider other) {
		myLight.enabled = true;
		myText.enabled = true;
        if (api != null)
            api.MoveInventory();
	}

	void OnTriggerExit(Collider other) {
		myLight.enabled = false;
		myText.enabled = false;
	}

    void Build(string label) {
        myTextMesh.text = label;
    }
}
