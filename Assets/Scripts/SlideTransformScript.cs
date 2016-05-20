using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlideTransformScript : MonoBehaviour {

    public bool SlideX = false;
    public bool SlideY = false;
    public bool SlideZ = false;
    public GameObject Target;
    public float Factor = 5f;

    private Vector3 startPos;
    private Slider mySlider;

    public void Start() {
        mySlider = gameObject.GetComponent<Slider>();
        UpdatePosition();
    }

    public void UpdatePosition() {
        if (Target != null)
            startPos = Target.transform.position;
        
        if (mySlider != null)
            mySlider.value = 0;
    }

    public void Slide(Slider slider) {
        float d = Factor * slider.value;
        Vector3 pos = startPos;
        if (SlideX) pos.x += d;
        if (SlideY) pos.y += d;
        if (SlideZ) pos.z += d;
        //TODO: Lerp this
        Target.transform.position = pos;
    }
}
