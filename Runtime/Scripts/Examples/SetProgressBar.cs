﻿using System.Collections;
using System.Collections.Generic;
using SceneFader;
using UnityEngine;
using UnityEngine.UI;

public class SetProgressBar : MonoBehaviour {

	public Slider slider;
	
	void Update() {
		slider.value = SceneFaderManager.Instance.ProgressClamp;
	}
	
}