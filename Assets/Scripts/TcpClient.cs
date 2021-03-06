﻿using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public class TcpClient : MonoBehaviour
{

	public delegate void PacketPositionEvent (string tagID,Vector3 pos);

	public event PacketPositionEvent OnPacketPositionSent;

	public string IP = "127.0.0.1";
	public int Port = 1337;
	public int Timeout = 5000;

	private Socket _client;
	private byte[] _buffer = new byte[8142];
	private Thread startThread;
	private bool shutdown;

	// Use this for initialization
	void Start ()
	{
		shutdown = false;
		startThread = new Thread (new ThreadStart (ClientLoop));
		startThread.IsBackground = true;
		startThread.Start (); 
	}

	void Update ()
	{
		if (shutdown) {
			Debug.Log ("Closing TcpClient");
			_client.Close ();
		}
	}

	void OnApplicationQuit ()
	{
		StopClient ();
	}

	public void StopClient ()
	{
		shutdown = true;
		if (_client != null) {
			_client.Close();
		}
	}

	private void ClientLoop ()
	{
		StringBuilder msg = new StringBuilder ();
		string pkt = string.Empty;

		while (shutdown == false) {
			try {

				if (_client == null) {
					_client = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					_client.Connect (new IPEndPoint (IPAddress.Parse (IP), Port));
					_client.ReceiveTimeout = Timeout;
				}

				while (shutdown == false) {
					if (_client.Poll(Timeout * 1000, SelectMode.SelectRead)) {
						int count = _client.Receive (_buffer);
						if (count == 0) {
							Debug.Log("TcpClient - Server closed");
							_client.Close();
							_client = null;
							break;
						}
						for (int i=0; i<count; i++) {
							byte b = _buffer [i];
							if (b == '\r') {
								continue;
							} else if (b == '\n') {
								pkt = msg.ToString ();
								msg.Length = 0;
								if (pkt.StartsWith ("#")) {
									ParsePositionPacket (pkt);
								} else {
									Debug.LogWarning ("[TcpClient] BAD PACKET - " + pkt);
								}
							} else {
								msg.Append ((char)b);
							}
						}
					} else {
						// Timeout so close and retry
						Debug.Log("TcpClient - Timeout");
						_client.Close();
						_client = null;
						break;
					}
				}

			} catch (System.Exception ex) {
				if (shutdown == false) {
					Debug.LogError (ex.Message);
					_client = null;
					Thread.Sleep (1000);
				}
			}
		}
	}

	void ParsePositionPacket (string csvString)
	{
		string[] parts = csvString.Split (' ');
		
		string tagid = parts [1];
		
		Vector3 pos = new Vector3 ();
		pos.x = float.Parse (parts [2]);
		pos.y = float.Parse (parts [3]);
		pos.z = float.Parse (parts [4]);
		
		OnPacketPositionSent (tagid, pos);
	}

}
