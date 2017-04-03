using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class RegexTest : MonoBehaviour {
    public string name;
    public GameObject tankModel;


    [Regex(@"^(?:\d{1,3}\.){3}\d{1,3}$", "Invalid IP address!\nExample: '127.0.0.1'")]
    public string address = "192.168.0.1";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
