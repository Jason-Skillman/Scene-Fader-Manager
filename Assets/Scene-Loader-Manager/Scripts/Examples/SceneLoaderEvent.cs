using System.Collections;
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
		SceneLoaderUtility.LogLevel = SceneLoaderUtility.LogType.Less;
		SceneLoaderUtility.LoadScenesAdditiveAsync(scenes);
	}
	
}