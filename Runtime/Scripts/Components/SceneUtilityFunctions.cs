using System.Collections;
using System.Collections.Generic;
using SceneFader.SceneManagement;
using UnityEngine;

namespace SceneFader.Components {
	public class SceneUtilityFunctions : MonoBehaviour {
    
        public string[] additiveScenes;
    
    	#region Async
    	
    	public void LoadScene(string scene) {
    		SceneUtility.LoadScene(scene);
    	}
    
    	public void LoadScenesAdditive() {
    		SceneUtility.LoadScenesAdditive(additiveScenes);
    	}
    
    	public void LoadActiveScene(string scene) {
    		SceneUtility.LoadActiveScene(scene, additiveScenes);
    	}
    
    	public void UnloadScene(string scene) {
    		SceneUtility.UnloadScene(scene);
    	}
    
    	public void UnloadScenes() {
    		SceneUtility.UnloadScenes(additiveScenes);
    	}
    	
    	public void UnloadAllScenesExcept() {
    		SceneUtility.UnloadAllScenesExcept(additiveScenes);
    	}
    
    	#endregion
    
    	#region IEnumerator
    
    	public void CoroutineLoadScene(string scene) {
    		StartCoroutine(SceneUtility.CoroutineLoadScene(scene));
    	}
    
    	public void CoroutineLoadScenesAdditive() {
    		StartCoroutine(SceneUtility.CoroutineLoadScenesAdditive(additiveScenes));
    	}
    	
    	public void CoroutineUnloadScene(string scene) {
    		StartCoroutine(SceneUtility.CoroutineUnloadScene(scene));
    	}
    
    	public void CoroutineUnloadScenes() {
    		StartCoroutine(SceneUtility.CoroutineUnloadScenes(additiveScenes));
    	}
    	
    	public void CoroutineUnloadAllScenesExcept() {
    		StartCoroutine(SceneUtility.CoroutineUnloadAllScenesExcept(additiveScenes));
    	}
    
    	#endregion

	}
}
