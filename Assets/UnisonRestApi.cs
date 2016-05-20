using UnityEngine;
using System.Collections;

/***************************/
/* TESTING TESTING TESTING */
/* DO NOT USE DO NOT USE   */
/***************************/

public class UnisonRestApi : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public void TestSubmitClick() {
		StartCoroutine (TestSubmit());
	}

	IEnumerator TestSubmit() {
		string url = "http://unison-dev.cphandheld.com/api/Inventory/NadaMoveInventory";

		WWWForm form = new WWWForm ();
		form.headers.Add ("Content-Type", "application/x-www-form-urlencoded");
		form.AddField("VIN", "2HKRM4H7XEH679791");
		form.AddField("BinId", 2);

		WWW w = new WWW(url, form);
		yield return w;

		if (!string.IsNullOrEmpty (w.error)) {
			print ("NadaMoveInventory fail ... " + w.error);
		} else {
			print ("NadaMoveInventory SUCCESS!");
		}

	}

}
