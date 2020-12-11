using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement {
	public static partial class SceneLoaderUtility {

		private static readonly string Tag = "[SceneManager] ";

		public static LogType LogLevel { get; set; } = LogType.All;

		public enum LogType {
			Less = 0,
			All = 1,
			None = 2,
		}

		/// <summary>
		/// Loads a single scene.
		/// </summary>
		/// <param name="scene">The scene name.</param>
		/// <param name="onFinished">Optional callback action.</param>
		public static void LoadSceneAsync(string scene, Action onFinished = null) {
			//Block flow if the scene does not exist
			if(!Application.CanStreamedLevelBeLoaded(scene)) {
				if(LogLevel >= LogType.Less)
					Debug.LogWarning(Tag + "The scene \"" + scene + "\" cannot be found or does not exist.");
				return;
			}

			AsyncOperation op = SceneManager.LoadSceneAsync(scene);
			op.completed += (e) => onFinished?.Invoke();
		}

		/// <summary>
		/// Loads in an array of scenes additively.
		/// </summary>
		/// <param name="scenes">The array of scene names.</param>
		/// <param name="onFinished">Optional callback action.</param>
		/// <param name="duplicateScenes">Should duplicate scenes be allowed. False by default.</param>
		[Obsolete]
		public static void LoadScenesAdditiveAsync(string[] scenes, Action onFinished = null, bool duplicateScenes = false) {
			AsyncOperation[] operations = new AsyncOperation[scenes.Length];

			//Step 1: Load all of operations
			for(var i = 0; i < scenes.Length; i++) {
				string scene = scenes[i];

				//Block flow if the scene does not exist
				if(!Application.CanStreamedLevelBeLoaded(scene)) {
					if(LogLevel >= LogType.Less)
						Debug.LogWarning(Tag + "The scene \"" + scene + "\" cannot be found or does not exist.");
					continue;
				}

				if(!duplicateScenes) {
					//Block flow if the scene has already been loaded
					Scene sceneObj = SceneManager.GetSceneByName(scene);
					if(sceneObj.isLoaded) {
						if(LogLevel >= LogType.All)
							Debug.LogWarning(Tag + "The scene \"" + scene + "\" has already been loaded.");
						continue;
					}
				}

				//Load the scene and deactivate
				AsyncOperation op = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
				op.allowSceneActivation = false;
				operations[i] = op;
			}

			//Step 2: Activate all of the operations
			//int completed = 0, required = operations.Length;
			foreach(AsyncOperation op in operations) {
				if(op == null) {
					//required--;
					continue;
				}

				op.allowSceneActivation = true;
				
				
				//Todo: I hate this
				/*while(true) {
					Debug.Log(op.progress);
					yield return null;
				}*/
				
				/*yield return new WaitForSeconds(1);

				op.completed += _ => {
					/*completed++;
					if(completed == required) {
						//Debug.Log("test");
						onFinished?.Invoke();
					}#1#
					
					if(scene.Equals(scenes[scenes.Length - 1])) {
						onFinished?.Invoke();
					}
				};*/
			}
		}

		[Obsolete]
		public static void UnloadSceneAsync(string[] scenes, Action onFinished = null) {
			foreach(string scene in scenes) {
				//Block flow if the scene does not exist
				if(!Application.CanStreamedLevelBeLoaded(scene)) {
					if(LogLevel >= LogType.Less)
						Debug.LogWarning(Tag + "The scene \"" + scene + "\" cannot be found or does not exist.");
					continue;
				}

				AsyncOperation op = SceneManager.UnloadSceneAsync(scene);
				op.completed += e => {
					if(scene.Equals(scenes[scenes.Length - 1])) {
						onFinished?.Invoke();
					}
				};
			}
		}

		/// <summary>
		/// Unloads all scenes except for a select few
		/// </summary>
		/// <param name="onTaskFinished">Callback when the task has finished</param>
		/// <param name="scenesExcept">The list of scenes to not unload</param>
		[Obsolete]
		public static void UnloadAllScenesAsyncExcept(Action onTaskFinished = null, params string[] scenesExcept) {
			int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;

			//Loop through all of the existing scenes
			for(int i = 0; i < sceneCount; i++) {
				//Fetch the current scene
				Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);

				//Loop through the list of excepted scenes
				bool flagSkip = false;
				foreach(string sceneExcept in scenesExcept) {
					//Check if the current scene is excluded from the unload
					if(currentScene.name.Equals(sceneExcept)) {
						flagSkip = true;
						break;
					}
				}

				if(flagSkip) continue;

				//Unload the scene
				SceneManager.UnloadSceneAsync(currentScene);
			}

			//Callback
			onTaskFinished?.Invoke();
		}
		
		public static void SetActiveScene(string scene) {
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
		}

		/*public static void LoadBaseScene(string baseScene, Action onTaskFinished = null, params string[] additiveScenes) {
			LoadSceneAsync(baseScene, () => { LoadScenesAdditiveAsync(additiveScenes, onTaskFinished); });
		}*/
		
	}
}

/*

		public void UnloadScenes(params string[] scenes) {
			Uni
		}
		
		private static IEnumerator CoroutineUnloadScenes(params string[] scenes) {
			//Todo:
			yield return null;

			List<AsyncOperation> listOperations = new List<AsyncOperation>();

			//Step 1: List all of operations
			foreach(string sceneName in scenes) {
				//Skip operation if scene is not loaded
				Scene scene = SceneManager.GetSceneByName(sceneName);
				if(!scene.isLoaded) continue;
				
				AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
				
				//Prevent the scene from activating
				//asyncOperation.allowSceneActivation = false;
				
				listOperations.Add(asyncOperation);

				//Wait until the current scene is loaded but not activated
				while(true) {
					//time += Time.deltaTime;
					Debug.Log("" + asyncOperation.progress);
					yield return null;
				}
				//yield return new WaitForSeconds(1f);
			}

			//Step 2: Activate all of the operations
			foreach(AsyncOperation operation in listOperations) {
				operation.allowSceneActivation = true;

				while(!operation.isDone) {
					yield return null;
				}
			}
		}*/