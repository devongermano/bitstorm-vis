using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RangeFilterAvg : MonoBehaviour {

	public int SampleSize = 5;

	private Queue<float> samples = new Queue<float>();

	private AnchorInfoPacket lastPacket;

	public void ProcessPacket(AnchorInfoPacket packet) {
		lastPacket = packet;
		samples.Enqueue (packet.distance);
		if (samples.Count >= SampleSize) {
			samples.Dequeue();
			float accum = 0;
			float[] samplesArray = samples.ToArray();
			for (int i=0;i<samplesArray.Length;i++) {
				accum += samplesArray[i];
			}
			float avg = accum / samplesArray.Length;
			packet.distance_filtered = avg;
		} else {
			packet.distance_filtered = packet.distance;
		}
	}

	public void Reset() {
	}

	public AnchorInfoPacket GetLastAnchorInfoPacket() {
		return lastPacket;
	}
}
