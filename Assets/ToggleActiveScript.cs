using UnityEngine;
using System.Collections;

public class ToggleActiveScript : MonoBehaviour {

    public GameObject TargetObject;

    public void Toggle() {
        if (TargetObject.activeSelf)
            TargetObject.SetActive(false);
        else
            TargetObject.SetActive(true);
    }
}
