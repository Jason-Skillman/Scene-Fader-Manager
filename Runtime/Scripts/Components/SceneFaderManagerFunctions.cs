using System.Collections;
using SceneFader.SceneManagement;
using UnityEngine;

namespace SceneFader.Components {
	public class SceneFaderManagerFunctions : MonoBehaviour {
		
		public string[] scenes;
		
		public void LoadScenesAdditive() {
			IEnumerator task1 = SceneUtility.CoroutineLoadScenesAdditive(scenes);
	
			SceneFaderManager.Instance.FadeAndPerformTasks(0, task1);
		}

		public void LoadActiveSceneWithExtras() {
			IEnumerator task1 = SceneUtility.CoroutineLoadScene(scenes[0]);
			IEnumerator task2 = SceneUtility.CoroutineLoadScenesAdditive(scenes);
			IEnumerator task3 = SceneUtility.CoroutineLoadScenesAdditive(scenes, duplicateScenes: true);
			
			SceneFaderManager.Instance.FadeAndPerformTasks(0, task1, task2, task3);
			SceneFaderManager.Instance.FadeAndPerformTasks(0);
		}
		
	}
}