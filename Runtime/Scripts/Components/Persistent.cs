using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneFader.Components {
	public class Persistent : MonoBehaviour {

		private void Awake() {
			DontDestroyOnLoad(gameObject);
		}

	}
}
