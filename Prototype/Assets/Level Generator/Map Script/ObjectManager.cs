﻿using UnityEngine;
using System.Collections;

public class ObjectManager : MonoBehaviour {

	public GameObject wall;
	public GameObject crow;
	public GameObject grozzle;
	public GameObject crawler;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject createWall(){
		return wall;
	}

	public GameObject createCrow(){
		return crow;
	}

	public GameObject createGrozzle(){
		return grozzle;
	}

	public GameObject createCrawler(){
		return crawler;
	}
	
}