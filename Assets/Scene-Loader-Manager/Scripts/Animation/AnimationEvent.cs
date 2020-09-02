using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour {

	public UnityEvent onEvent01;

	public void OnEvent01() {
		onEvent01?.Invoke();
	}
	
}