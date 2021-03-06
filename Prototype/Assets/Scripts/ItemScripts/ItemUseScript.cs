﻿using UnityEngine;
using System.Collections;

/*Defines all actions the player undertakes
 *in using various items*/
public class ItemUseScript : MonoBehaviour {

	//for item scrolling:
	private int selected = 0;
	private string[] items = {"*Air Freshener",
							  "*Botany Book",
							  "*Epipen",
							  "*Extinguisher",
							  "*Meat",
							  "*Flare Gun",
							  "*Trip Mine",
							  "*Mutated Fruit",
							  "*Geiger Counter",
							  "*Muffling Rags",
							  "Kevlar Vest",
							  "Night-Vision Goggles"};

	//0 FOR AIR FRESHENER
	private bool canUseAirFreshener = true;

	//1 FOR BOTANY BOOK
	private bool canUseBook = true;
		//as well as variables for MUTATED FRUITS

	//2 FOR EPIPEN
	private bool canUseEpipen = true; //whether player can use Epipen or not

	//3 FOR EXTINGUISHER
	private bool canUseExtinguisher = true;
	public GameObject fluid;

	//4 FOR MEAT
	public GameObject meat;
	private bool hasMeat = true;
	private float meatSpeed = 10f;

	//5 FOR FLARE GUN
	private int numFlares = 2;
	public GameObject flare;
	public float flareSpeed = 20f;

	//6 FOR TRIP MINE
	public GameObject mine;
	private int numMines = 3;

	//7 FOR MUTATED FRUITS
	private int numFruits = 3;
	private float newDamageRatio = 2f;
	private float lifeIncrease = 1f;
	private int plusFruits = 2;

	//8 FOR GEIGER COUNTER
	private bool canUseCounter = true;

	//9 FOR MUFFLING RAGS
	private bool canUseRags = true;

	//10 FOR KEVLAR VEST

	//11 FOR NIGHT-VISION GOGGLES
	//Light Variables
	GameObject nightVision;
	Light lightComp;
	public Color lightColor;
	public float lightInt;
	public float lightRange;
	
	//Player Variables
	GameObject playerObject;
	public Transform target;

	// Use this for initialization
	void Start () {
		playerObject = GameObject.FindGameObjectWithTag("Player");
		target = playerObject.transform;
		
		nightVision = new GameObject("Night Vision Mode");
		lightComp = nightVision.AddComponent<Light>();
		lightComp.color = Color.clear;
	}
	
	// Update is called once per frame
	void Update () {
		ItemScroll ();

		/*Allows player to use selected Item with I*/
		if (Input.GetKeyDown (KeyCode.I))
		{
			switch(selected)
			{
				case 0:
					UseAirFreshener();
					break;
				case 1:
					BotanyBook();
					break;
				case 2:
					Epipen();
					break;
				case 3:
					Extinguisher();
					break;
				case 4:
					ThrowMeat();
					break;
				case 5:
					UseFlare();
					break;
				case 6:
					DropMine();
					break;
				case 7:
					UseMutatedFruit();
					break;
				case 8:
					GeigerCounter();
					break;
				case 9:
					MufflingRags();
					break;
				case 10:
					KevlarVest();
					break;
				case 11:
					NightVision();
					break;
			}
		}

		//Update position of Night Vision Light
		nightVision.transform.position = target.position;
	}

	/*Item Scroll: By using the right or left arrow keys, Player
	 *can scroll through available items*/
	//MORE FOR TESTING
	string currentItem = "";
	void ItemScroll()
	{
		//Scrolls through items
		if (Input.GetKeyDown (KeyCode.RightArrow)) //scrolling right
		{
			selected++;
			if (selected >= 12)
			{
					selected = 0;
			}
		} 
		else if (Input.GetKeyDown (KeyCode.LeftArrow)) //scrolling left
		{
			selected --;
			if (selected <= -1)
			{
				selected = 11;
			}
		}
		currentItem = "" + items [selected];
	}
	
	void OnGUI(){
		GUI.Box(new Rect(0, 0, Screen.width/8, Screen.height/16), currentItem);
	}

	/*AIR FRESHENER: the player becomes invisible to blind enemies*/
	void UseAirFreshener()
	{
		canUseAirFreshener = false;
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("enemy");
		for (int i = 0; i < enemies.Length; i++)
		{
			if(enemies[i].GetComponent<SimpleEnemyScript>().GetEnemyType() == "blind")
			{
				enemies[i].GetComponent<SimpleEnemyScript>().SetCanMove (false); //enemy doesn't move
			}
		}
	}

	/*BOTANY BOOK: the player will have more positive and less
	 *negative effects from mutated fruit*/
	//POTENTIALLY MORE TO THIS THAN IS HERE
	void BotanyBook()
	{
		if (canUseBook)
		{
			canUseBook = false;
			newDamageRatio = 1f;
			lifeIncrease = 2f;
			plusFruits = 3;
		}
	}

	/*EPIPEN: the player can move faster and takes less damage*/
	void Epipen()
	{
		if (canUseEpipen)
		{
			canUseEpipen = false;
			//Adjust speed of player and amount of damage done
			TopDownCharacterController.SetSpeedBonus(35f);
			TopDownCharacterController.SetInDamageRatio(0.5f);
		}
	}

	/*EXTINGUISHER: player can spray extinguisher fluid*/
	void Extinguisher()
	{
		if(canUseExtinguisher)
		{
			canUseExtinguisher = false;
			GameObject fluidActual = Instantiate(fluid, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
			fluidActual.transform.Rotate (-90f, 0f, 180f);
			fluidActual.transform.parent = gameObject.transform;
		}
	}

	/*FLARE GUN: player shoots flare to which enemies gather*/
	void UseFlare()
	{
		if (numFlares > 0)
		{
			GameObject shotFlare = Instantiate (flare, transform.position, transform.rotation) as GameObject;
			shotFlare.rigidbody.velocity = (gameObject.rigidbody.velocity + transform.forward*flareSpeed);
			numFlares --;
		}
	}

	/*MEAT: player throws meat to which carnivorous enemies gather*/
	void ThrowMeat()
	{
		if (hasMeat)
		{
			GameObject thrownMeat = Instantiate (meat, transform.position, transform.rotation) as GameObject;
			thrownMeat.rigidbody.velocity = (gameObject.rigidbody.velocity + transform.forward*meatSpeed);
			hasMeat = false;
		}
	}

	/*TRIP MINE: player drops mine*/
	void DropMine()
	{
		if (numMines > 0)
		{
			Instantiate(mine, transform.position, transform.rotation);
			numMines --;
		}
	}

	/*MUTATED FRUIT: player uses a mutated fruit to various effects*/
	void UseMutatedFruit()
	{
		if (numFruits != 0)
		{
			numFruits --;

			int effect = Random.Range (0, 3);
			switch (effect) {
			case 0: //Player will get hurt twice as much
					TopDownCharacterController.SetInDamageRatio (newDamageRatio);
					Debug.Log ("HURT TWICE AS MUCH");
					break;
			case 1: //Player regains health
					TopDownCharacterController.IncreaseLife (lifeIncrease);
					Debug.Log ("LIFE INCREASE +1");
					break;
			case 2: //Player gets more fruit
					numFruits += plusFruits;
					Debug.Log ("TWO MORE FRUITS");
					break;
			}
		}
	}

	/*GEIGER COUNTER: warns player of nearby mutated enemy*/
	void GeigerCounter()
	{
		//This still has errors - will come up with a NullReferenceException for line 38 of GeigerCounterScript
		/*gameObject.GetComponent<GeigerCounterScript> ().enabled = true;
		canUseCounter = false;*/
	}

	/*MUFFLING RAGS: walking makes less noise*/
	void MufflingRags()
	{
		TopDownCharacterController.SetNoise (Random.Range(0.0f,2.0f));
		canUseRags = false;
	}

	/*KEVLAR VEST: protects player from projectiles*/
	void KevlarVest()
	{

	}

	/*NIGHT-VISION GOGGLES: player can see better in dark*/
	void NightVision()
	{
		if (lightComp.color == Color.clear) //Turn light on
		{
			lightComp.color = lightColor;
			//To make light follow player
			nightVision.transform.position = target.position;
			lightComp.intensity = lightInt;
			lightComp.range = lightRange;
		}
		
		else //Light Off
		{lightComp.color = Color.clear;}

		//Update position of Night Vision Light
		nightVision.transform.position = target.position;

	}
}
