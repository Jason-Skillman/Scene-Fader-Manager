using System.Collections;
using System.Collections.Generic;
using SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SetProgressBar : MonoBehaviour {

	public Slider slider;
	
	void Update() {
		slider.value = SceneFaderManager.Instance.ProgressClamp;
	}
	
}