using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EditorManager : MonoBehaviour
{

	#region UI_References
	
	[SerializeField] private Button buttonAdvanced;
	[SerializeField] private Button buttonGenerate;
	[SerializeField] private Button buttonPlay;

	[SerializeField] private Slider sliderInitialProb;
	[SerializeField] private Slider sliderBirthLimit;
	[SerializeField] private Slider sliderDeathLimit;

	[SerializeField] private TMP_InputField inputSeed;
	[SerializeField] private TMP_InputField inputWidth;
	[SerializeField] private TMP_InputField inputHeight;

	[SerializeField] private GameObject advancedPanel;
	[SerializeField] private GameObject mapHolder;
	
	#endregion

	#region Generation_Parameters

	private int seed;
	private int width = 60;
	private int height = 60;
	private int birthLimit = 4;
	private int deathLimit = 3;

	private float initialProbability = .6f;

	#endregion
	
	// Use this for initialization
	void Start () {
		
		// Default values
		inputWidth.text = "60";
		inputHeight.text = "60";
		
		advancedPanel.SetActive(false);
	}

	
	public void OnClickAdvanced()
	{
		buttonAdvanced.gameObject.SetActive(false);
		advancedPanel.SetActive(true);
	}

	public void OnClickGenerate()
	{
		if (mapHolder != null)
			Destroy(mapHolder);
		
		Debug.LogWarning("Generating Map");
		Debug.Log("Seed: " + (seed == 0 ? Environment.TickCount : seed));
		Debug.Log("Dimensions: " + width + " x " + height);
		Debug.Log("Initial probability: " + initialProbability);
		Debug.Log("Birth / Death limits: " + birthLimit + " / " + deathLimit);
		
		Random.InitState(seed == 0 ? Environment.TickCount : seed);
		mapHolder = new GameObject("Map");
		mapHolder.transform.position = Vector3.zero;
		
		Terrain terrain = mapHolder.AddComponent<Terrain>();
		terrain.Generate(width, height, initialProbability, birthLimit, deathLimit);
	}

	public void OnClickPlay()
	{
		
	}


	public void OnSeedChange()
	{
		if (!String.IsNullOrEmpty(inputSeed.text))
			seed = int.Parse(inputSeed.text);
		else
			seed = 0;
	}

	public void OnWidthChange()
	{
		if (!String.IsNullOrEmpty(inputWidth.text))
			width = int.Parse(inputWidth.text);
	}

	public void OnHeightChange()
	{
		if (!String.IsNullOrEmpty(inputHeight.text))
			height = int.Parse(inputHeight.text);
	}

	public void OnInitialProbabilityChange()
	{
		initialProbability = sliderInitialProb.value;
	}
	
	public void OnBirthLimitChange()
	{
		if (sliderBirthLimit.value < sliderDeathLimit.value)
			sliderDeathLimit.value = sliderBirthLimit.value;

		birthLimit = (int) sliderBirthLimit.value;
	}

	public void OnDeathLimitChange()
	{
		if (sliderBirthLimit.value < sliderDeathLimit.value)
			sliderBirthLimit.value = sliderDeathLimit.value;

		deathLimit = (int) sliderDeathLimit.value;
	}
}
