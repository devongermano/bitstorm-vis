﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class BitStormAPI : MonoBehaviour
{

    private Survey surveyScript = null;

    public string UrlPrefix = "http://192.168.99.100:9902/";

    public string LastResult = string.Empty;
    public string LastError = string.Empty;


    public void Start()
    {
        surveyScript = GetComponent<Survey>();
    }

    public void DiscoverAnchors()
    {
        StartCoroutine(DoDiscoverAnchors());
    }

    public IEnumerator DoDiscoverAnchors()
    {
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        string postData = "{}";
        Dictionary<string, string> postHeader = new Dictionary<string, string>();

        postHeader.Add("Content-Type", "application/json");


        Debug.Log("Sending discover request to Bitstorm API");
        WWW w = new WWW(UrlPrefix + "anchor/discover", encoding.GetBytes(postData), postHeader);
        yield return w;

        if (!string.IsNullOrEmpty(w.error))
        {
            Debug.LogError(w.error);
            Debug.LogError(w.text);
            yield return false;
        }

        Debug.Log("Delay and check for anchors ...");
        yield return new WaitForSeconds(1.0f);
        w = new WWW(UrlPrefix + "anchor/*");
        yield return w;

        if (!string.IsNullOrEmpty(w.error))
        {
            Debug.LogError(w.error);
            Debug.LogError(w.text);
            yield return false;
        }

        if (surveyScript == null)
        {
            Debug.Log("No Survey Script found.  Done.");
            yield return false;
        }

        //    {"E937": {"position": [0, 0, 0], "anchorid": "E937"}, "18FC": {"position": [0, 0, 0], "anchorid": "18FC"}, "C825": {"position": [0, 0, 0], "anchorid": "C825"}, "1BCC": {"position": [0, 0, 0], "anchorid": "1BCC"}}
        var j = JSON.Parse(w.text);
        for (int i = 0; i < j.Count; i++)
        {
            surveyScript.SubmitAnchor(j[i]["anchorid"]);
        }

        yield return true;
    }

    public IEnumerator DoGetAnchor(string anchorid)
    {
        LastError = string.Empty;

        WWW w = new WWW(UrlPrefix + "anchor/" + anchorid);
        yield return w;

        if (!string.IsNullOrEmpty(w.error))
        {
            Debug.LogError(w.error);
            Debug.LogError(w.text);
            LastError = w.error;
            LastResult = w.text;
            yield return false;
        }

        LastResult = w.text;
    }

    public IEnumerator DoSurvey(string anchorid, string otherid, int reps, int dly)
    {
        LastError = string.Empty;

        WWW w = new WWW(string.Format(UrlPrefix + "anchor/range/{0}/{1}/{2}/{3}", anchorid, otherid, reps, dly));
        yield return w;

        if (!string.IsNullOrEmpty(w.error))
        {
            Debug.LogError(w.error);
            Debug.LogError(w.text);
            LastError = w.error;
            LastResult = w.text;
            yield return false;
        }

        LastResult = w.text;
    }

    public IEnumerator DoUpdateAnchor(string anchorId, Vector3 position)
    {
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        Dictionary<string, string> postHeader = new Dictionary<string, string>();

        postHeader.Add("Content-Type", "application/json");

        //string postData = string.Format("{'anchorid':'{0}', 'position':[{1},{2},{3}]}", anchorId, position.x.ToString(), position.y.ToString(), position.z.ToString());
        string postData = "{\"anchorid\":\"" + anchorId + "\",\"position\":[" + position.x.ToString() + "," + position.y.ToString() + "," + position.z + "]}";


        Debug.Log("DoUpdateAnchor: " + postData);
        WWW w = new WWW(UrlPrefix + "anchor/", encoding.GetBytes(postData), postHeader);
        yield return w;

        if (!string.IsNullOrEmpty(w.error))
        {
            Debug.LogError(w.error);
            Debug.LogError(w.text);
            yield return false;
        }
    }
}

