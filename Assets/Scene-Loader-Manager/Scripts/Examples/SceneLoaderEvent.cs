﻿using System.Collections;
using System.Collections.Generic;
using SceneManagement;
using UnityEngine;

public class SceneLoaderEvent : MonoBehaviour {

	public string[] scenes;

	public void LoadAdd() {
		//SceneLoaderManager.Instance.LoadSceneAdditive(scenes: scenes);
		//SceneLoaderManager.Instance.LoadSceneAdditive(duplicateScenes: false, scenes: scenes);
		
		//SceneLoaderUtility.LoadSceneAsync("test");
		//StartCoroutine(SceneLoaderUtility.LoadScenesAdditiveAsync(scenes));
		
		/*SceneLoaderUtility.LoadScenesAdditiveAsync(scenes, () => {
			print("done");
			//SceneLoaderUtility.SetActiveScene("Scene02")
		});*/
		
		/*StartCoroutine(SceneLoaderUtility.CoroutineLoadScenesAdditive(scenes, () => {
			print("done");
			//SceneLoaderUtility.SetActiveScene("Scene02")
		}));*/

		StartCoroutine(SceneLoaderUtility.CoroutineLoadSceneAsync("Scene02", () => print("done")));


	}
	
}