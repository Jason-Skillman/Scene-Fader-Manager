using System.Collections;
using System.Collections.Generic;
using SceneFader;
using SceneFader.SceneManagement;
using UnityEngine;

public class SceneLoaderUtilityFunction : MonoBehaviour {

	public string[] scenes;

	#region Async
	
	public void LoadScene() {
		SceneLoaderUtility.LoadScene(scenes[0], () => print("Done"));
	}

	public void LoadScenesAdditive() {
		SceneLoaderUtility.LoadScenesAdditive(scenes, () => print("Done"));
	}

	public void LoadActiveScene() {
		SceneLoaderUtility.LoadActiveScene(scenes[0], scenes, () => print("Done"), true);
	}

	public void UnloadScene() {
		SceneLoaderUtility.UnloadScene(scenes[0], () => print("Done"));
	}

	public void UnloadScenes() {
		SceneLoaderUtility.UnloadScenes(scenes, () => print("Done"));
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

	//Todo: move
	/*public void StartFade() {
		//Coroutine co1 = StartCoroutine(SceneLoaderUtility.CoroutineLoadScenesAdditive(scenes));
		IEnumerator co1 = SceneLoaderUtility.CoroutineLoadScenesAdditive(scenes);

		SceneFaderManager.Instance.StartOperation(0, co1);
	}*/

}