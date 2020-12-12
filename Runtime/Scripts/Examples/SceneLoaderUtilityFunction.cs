using System.Collections;
using System.Collections.Generic;
using SceneFader;
using SceneFader.SceneManagement;
using UnityEngine;

public class SceneLoaderUtilityFunction : MonoBehaviour {

	public string[] scenes;

	#region Async
	
	public void LoadScene() {
		SceneLoaderUtility.LoadSceneAsync(scenes[0], () => print("Done"));
	}

	public void LoadScenesAdditive() {
		SceneLoaderUtility.LoadScenesAdditiveAsync(scenes, () => print("Done"));
		//StartCoroutine(SceneLoaderUtility.LoadScenesAdditiveAsync(scenes, () => print("Done")));
	}

	public void UnloadScene() {
		
	}

	public void UnloadScenes() {
		
	}

	#endregion

	#region IEnumerator

	public void CoroutineLoadScene() {
		StartCoroutine(SceneLoaderUtility.CoroutineLoadScene(scenes[0]));
	}

	public void CoroutineLoadScenesAdditive() {
		StartCoroutine(SceneLoaderUtility.CoroutineLoadScenesAdditive(scenes));
	}

	public void CoroutineUnloadScene() {
		StartCoroutine(SceneLoaderUtility.CoroutineUnloadScene(scenes[0]));
	}

	public void CoroutineUnloadScenes() {
		StartCoroutine(SceneLoaderUtility.CoroutineUnloadScenes(scenes));
	}

	#endregion

	/*public void StartFade() {
		//Coroutine co1 = StartCoroutine(SceneLoaderUtility.CoroutineLoadScenesAdditive(scenes));
		IEnumerator co1 = SceneLoaderUtility.CoroutineLoadScenesAdditive(scenes);

		SceneFaderManager.Instance.StartOperation(0, co1);
	}*/

}