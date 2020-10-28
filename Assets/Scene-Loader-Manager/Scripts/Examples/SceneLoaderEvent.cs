﻿using System.Collections;
using System.Collections.Generic;
using SceneLoaderManagement;
using UnityEngine;

public class SceneLoaderEvent : MonoBehaviour {

	public string[] scenes;

	public void LoadAdd() {
		SceneLoaderManager.Instance.LoadSceneAdditive(scenes);
	}
	
}