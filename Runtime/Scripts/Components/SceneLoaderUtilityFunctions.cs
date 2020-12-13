using System.Collections;
using System.Collections.Generic;
using SceneFader;
using SceneFader.SceneManagement;
using UnityEngine;

namespace SceneFader.Components {
	public class SceneLoaderUtilityFunctions : MonoBehaviour {
    
    	public string[] scenes;
    
    	#region Async
    	
    	public void LoadScene() {
    		SceneUtility.LoadScene(scenes[0], () => print("Done"));
    	}
    
    	public void LoadScenesAdditive() {
    		SceneUtility.LoadScenesAdditive(scenes, () => print("Done"));
    	}
    
    	public void LoadActiveScene() {
    		SceneUtility.LoadActiveScene(scenes[0], scenes, () => print("Done"), true);
    	}
    
    	public void UnloadScene() {
    		SceneUtility.UnloadScene(scenes[0], () => print("Done"));
    	}
    
    	public void UnloadScenes() {
    		SceneUtility.UnloadScenes(scenes, () => print("Done"));
    	}
    	
    	public void UnloadAllScenesExceptFor() {
    		SceneUtility.UnloadAllScenesExceptFor(scenes, () => print("Done"));
    	}
    
    	#endregion
    
    	#region IEnumerator
    
    	public void CoroutineLoadScene() {
    		StartCoroutine(SceneUtility.CoroutineLoadScene(scenes[0]));
    	}
    
    	public void CoroutineLoadScenesAdditive() {
    		StartCoroutine(SceneUtility.CoroutineLoadScenesAdditive(scenes));
    	}
    	
    	public void CoroutineUnloadScene() {
    		StartCoroutine(SceneUtility.CoroutineUnloadScene(scenes[0]));
    	}
    
    	public void CoroutineUnloadScenes() {
    		StartCoroutine(SceneUtility.CoroutineUnloadScenes(scenes));
    	}
    	
    	public void CoroutineUnloadAllScenesExceptFor() {
    		StartCoroutine(SceneUtility.CoroutineUnloadAllScenesExceptFor(scenes, () => print("Done")));
    	}
    
    	#endregion

	}
}
