using System.Collections;
using System.Collections.Generic;
using SceneFader;
using SceneFader.SceneManagement;
using UnityEngine;

public class SceneLoaderUtilityFunction : MonoBehaviour {

	public string[] scenes;

	public void LoadScene() {
		StartCoroutine(SceneLoaderUtility.CoroutineLoadScene(scenes[0]));
	}

	public void LoadScenesAdditive() {
		StartCoroutine(SceneLoaderUtility.CoroutineLoadScenesAdditive(scenes));
	}

	public void UnloadScene() {
		StartCoroutine(SceneLoaderUtility.CoroutineUnloadScene(scenes[0]));
	}

	public void UnloadScenes() {
		StartCoroutine(SceneLoaderUtility.CoroutineUnloadScenes(scenes));
	}

	/*public void StartFade() {
		//Coroutine co1 = StartCoroutine(SceneLoaderUtility.CoroutineLoadScenesAdditive(scenes));
		IEnumerator co1 = SceneLoaderUtility.CoroutineLoadScenesAdditive(scenes);

		SceneFaderManager.Instance.StartOperation(0, co1);
	}*/
	
}