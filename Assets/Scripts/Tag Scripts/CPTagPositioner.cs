using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CPTagPositioner : MonoBehaviour {

    //Flags to enable features
    bool lerpBetweenPoints = true;
    bool calculateAverage = true;

    //Unfiltered positions 
    Queue<Vector3> rawPositions;

    //Positions the tag should move towards.
    //If this grows too large the tag is lagging 
    //behind where it should be
    Queue<Vector3> moveToPositions;

    //Lerping values
    Vector3 lerpingFrom;
    Vector3 lerpingTo;
    //float startLerping;
    float currentLerpTime;
    float lerpTime = 0.04f;
    bool stillLerping = false;

    //Number of consecutive positions which are average together
    //if the "calculateAverage" flag is true
    int positionsToAverage = 5;



	// Use this for initialization
	void Start () {
        rawPositions = new Queue<Vector3>();
        moveToPositions = new Queue<Vector3>();
        transform.position = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        //FindNextPosition();
	}

    private void FindNextPosition() {
        if(calculateAverage) {
            FindPositionByAverage();
        } else {
            moveToPositions.Enqueue(rawPositions.Dequeue());
        }
        MoveToNextPosition();
    }

    private void MoveToNextPosition() {
        if (moveToPositions.Count == 0)
            return;

        if(lerpBetweenPoints) {
            Lerp();
            return;
        } else {
            moveToPositions.Enqueue(rawPositions.Dequeue());
        }
        
    }

    private void Lerp() {
//        print("lerp");
        if(!stillLerping) {
            lerpingFrom = transform.position;
            lerpingTo = moveToPositions.Dequeue();
            //startLerping = Time.time;
            currentLerpTime = 0.0f;
            stillLerping = true;
        }
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime) {
            currentLerpTime = lerpTime;
            stillLerping = false;
        }

//        print("currentLerpTime = " + currentLerpTime.ToString());
//        print("lerpTime = " + lerpTime.ToString());
        float percComplete = currentLerpTime / lerpTime;
        transform.position = Vector3.Lerp(lerpingFrom, lerpingTo, percComplete);
    }

    public void AddPosition(Vector3 nextPosition) {
//        print(name);
        rawPositions.Enqueue(nextPosition);
    }

    private void FindPositionByAverage() {
        if (rawPositions.Count < positionsToAverage)
            return;

        List<Vector3> toAverage = rawPositions.Take<Vector3>(positionsToAverage).ToList<Vector3>();
        rawPositions.Dequeue();

        float xSum = 0;
        float ySum = 0;
        float zSum = 0;
        foreach(Vector3 pos in toAverage) {
            xSum += pos.x;
            ySum += pos.y;
            zSum += pos.z;
        }
        
        moveToPositions.Enqueue(new Vector3(xSum/toAverage.Count, ySum/toAverage.Count, zSum/toAverage.Count));
    }
}

