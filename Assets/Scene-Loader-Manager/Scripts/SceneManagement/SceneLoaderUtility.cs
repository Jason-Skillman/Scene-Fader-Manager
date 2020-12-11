using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement {
	public static class SceneLoaderUtility {

		private static readonly string Tag = "[SceneManager] ";

		//Todo: Add log filter

		public static void LoadSceneAsync(string scene, Action onTaskFinished = null) {
			//Block flow if the scene does not exist
			if(!Application.CanStreamedLevelBeLoaded(scene)) {
				Debug.LogWarning(Tag + "The scene \"" + scene + "\" cannot be found or does not exist.");
				return;
			}

			//Block flow if the scene has already been loaded
			Scene sceneObj = SceneManager.GetSceneByName(scene);
			if(sceneObj.isLoaded) {
				Debug.LogWarning(Tag + "The scene \"" + scene + "\" has already been loaded.");
				onTaskFinished?.Invoke();
				return;
			}

			AsyncOperation op = SceneManager.LoadSceneAsync(scene);
			op.completed += (e) => onTaskFinished?.Invoke();
		}

		public static void LoadScenesAdditiveAsync(string[] scenes, Action onTaskFinished = null, bool duplicateScenes = false) {
			AsyncOperation[] operations = new AsyncOperation[scenes.Length];

			//Step 1: Load all of operations
			for(var i = 0; i < scenes.Length; i++) {
				string scene = scenes[i];

				//Block flow if the scene does not exist
				if(!Application.CanStreamedLevelBeLoaded(scene)) {
					Debug.LogWarning(Tag + "The scene \"" + scene + "\" cannot be found or does not exist.");
					continue;
				}

				if(!duplicateScenes) {
					//Block flow if the scene has already been loaded
					Scene sceneObj = SceneManager.GetSceneByName(scene);
					if(sceneObj.isLoaded) {
						Debug.LogWarning(Tag + "The scene \"" + scene + "\" has already been loaded.");
						onTaskFinished?.Invoke();
						continue;
					}
				}

				//Load the scene and deactivate
				AsyncOperation op = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
				op.allowSceneActivation = false;
				operations[i] = op;
			}

			//Step 2: Activate all of the operations
			int completed = 0, required = operations.Length;
			foreach(AsyncOperation op in operations) {
				if(op == null) {
					required--;
					continue;
				}

				op.allowSceneActivation = true;

				op.completed += _ => {
					completed++;
					if(completed == required) {
						onTaskFinished?.Invoke();
					}
				};
			}
		}

		/// <summary>
		/// Loads in a base scene with optional additive scenes
		/// </summary>
		/// <param name="baseScene">The base scene to load</param>
		/// <param name="onTaskFinished">Callback when the task has finished</param>
		/// <param name="additiveScenes">The extra scenes to load</param>
		public static void LoadBaseScene(string baseScene, Action onTaskFinished = null, params string[] additiveScenes) {
			LoadSceneAsync(baseScene, () => { LoadScenesAdditiveAsync(additiveScenes, onTaskFinished); });
		}

		public static void UnloadSceneAsync(Action onTaskFinished = null, params string[] scenes) {
			foreach(string scene in scenes) {
				//Check if the scene does not exist
				if(!Application.CanStreamedLevelBeLoaded(scene)) {
					Debug.LogWarning(Tag + "The scene \"" + scene + "\" cannot be found or does not exist.");
					continue;
				}

				var task = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
				task.completed += (e) => {
					if(scene.Equals(scenes[scenes.Length - 1])) {
						onTaskFinished?.Invoke();
					}
				};
			}
		}

		/// <summary>
		/// Unloads all scenes except for a select few
		/// </summary>
		/// <param name="onTaskFinished">Callback when the task has finished</param>
		/// <param name="scenesExcept">The list of scenes to not unload</param>
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
				UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentScene);
			}

			//Callback
			onTaskFinished?.Invoke();
		}
	}
}

/*private static IEnumerator CoroutineLoadScenesAdditive(bool duplicateScenes = false, params string[] scenes) {
			//Todo:
			yield return null;

			List<AsyncOperation> listOperations = new List<AsyncOperation>();

			//Step 1: List all of operations
			foreach(string sceneName in scenes) {
				//Check if the scene is already loaded
				if(!duplicateScenes) {
					Scene scene = SceneManager.GetSceneByName(sceneName);
					if(scene.isLoaded) {
						//Skip loading scene if scene is already loaded
						continue;
					}
				}
				
				//Start loading the scene
				AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
				
				//Prevent the scene from activating
				asyncOperation.allowSceneActivation = false;
				
				listOperations.Add(asyncOperation);

				//Wait until the current scene is loaded but not activated
				while(asyncOperation.progress < 0.9f) {
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
		}

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