using UnityEngine;
using System.Collections.Generic;
using System;

public class AnchorScript : MonoBehaviour {

	public class AnchorDistance
	{
		public AnchorScript anchor = null;
		public float distance;
	}

	public bool IsSurveyed = false;
	public GameObject SurveyMarker = null;
	public bool DisplayRange = false;
	public bool IsRanging = false;
	public float RangeOffset = 0.0f;
	public GameObject RangeText;

    public GameObject ProximityPrefab;

	public int sampleSize = 5;

	[HideInInspector]
	public List<AnchorDistance> AnchorDistances = new List<AnchorDistance> ();

	private Queue<float> samples = new Queue<float>();

    private AnchorInfoPacket lastPacket;

    //Experimentally the natural log function was found to provide the best representation of the
    //tag's actual position.
    private Func<float, float> averageFunc = (x => (float)Math.Log(x, Math.E));

    public void Update() {
		RangeText.SetActive (DisplayRange);
	}




    /**
     * To generate a weighted average a function is applied over the range [1,n]. Each of
     * these range values is divided by the sum of the range. This provides set of n numbers
     * less than 1 whose sum is 1.
     */
    private List<float> GenerateAverageRange(int n, Func<float, float> func) {
        List<float> range = new List<float>();

        float sum = 0;
        for(int i = 1; i <= n; i++) {
            sum += func(i);
        }
        for(int i = 1; i <= n; i++) {
            range.Add(func(i) / sum);
        }

        return range;
    }


    /**
     * Modifies packet by reference to add its distance to the current samples
     * and calculate the filtered distance by weighted average. 
     */
	public void ProcessPacket(AnchorInfoPacket packet) {
		// Tune value based on offset
		packet.distance += RangeOffset;

        //Maintain reference to most recent packet
		lastPacket = packet;
		samples.Enqueue(packet.distance);
        packet.distance_filtered = FindWeightedAverage(packet.distance);
	}

    /**
     * Calculates a weighted average based on the current averageFunction. This smooths
     * the movement of the tag by averaging the distance to the anchors.
     */
    private float FindWeightedAverage(float defaultValue) {
		if (samples.Count >= sampleSize) {
			float average = 0;
			float[] samplesArray = samples.ToArray();

            List<float> averageScaleNumbers = GenerateAverageRange(sampleSize, averageFunc);
            for (int i = 0; i < samplesArray.Length; i++) {
                average += samplesArray[i] * averageScaleNumbers[i];
            }
			samples.Dequeue();
            return average;
        } else {
            return defaultValue;
		}
    }
	
	public AnchorInfoPacket GetLastAnchorInfoPacket() {
		return lastPacket;
	}
}
