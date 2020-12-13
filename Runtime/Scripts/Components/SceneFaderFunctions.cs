using System.Collections;
using SceneFader.SceneManagement;
using UnityEngine;

namespace SceneFader.Components {
	public class SceneFaderFunctions : MonoBehaviour {

		public float minSecondsDelay;
		public string[] additiveScenes;
		
		public void LoadScene(string scene) {
			IEnumerator task1 = SceneUtility.CoroutineLoadScene(scene);
	
			SceneFaderManager.Instance.FadeAndPerformTasks(minSecondsDelay, task1);
		}
		
		public void LoadScenesAdditive() {
			IEnumerator task1 = SceneUtility.CoroutineLoadScenesAdditive(additiveScenes);
	
			SceneFaderManager.Instance.FadeAndPerformTasks(minSecondsDelay, task1);
		}
		
		public void UnloadScene(string scene) {
			IEnumerator task1 = SceneUtility.CoroutineUnloadScene(scene);
	
			SceneFaderManager.Instance.FadeAndPerformTasks(minSecondsDelay, task1);
		}
		
		public void UnloadScenes() {
			IEnumerator task1 = SceneUtility.CoroutineUnloadScenes(additiveScenes);
	
			SceneFaderManager.Instance.FadeAndPerformTasks(minSecondsDelay, task1);
		}
		
		public void UnloadAllScenesExceptFor() {
			IEnumerator task1 = SceneUtility.CoroutineUnloadAllScenesExceptFor(additiveScenes);
	
			SceneFaderManager.Instance.FadeAndPerformTasks(minSecondsDelay, task1);
		}
		
	}
}