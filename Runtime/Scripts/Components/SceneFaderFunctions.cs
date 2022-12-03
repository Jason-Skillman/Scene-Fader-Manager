namespace JasonSkillman.SceneFader.Components {
	using System.Collections;
	using SceneFader.SceneManagement;
	using UnityEngine;
	
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
		
		public void UnloadAllScenesExcept() {
			IEnumerator task1 = SceneUtility.CoroutineUnloadAllScenesExcept(additiveScenes);
	
			SceneFaderManager.Instance.FadeAndPerformTasks(minSecondsDelay, task1);
		}
		
	}
}