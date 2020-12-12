using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneFader.SceneManagement {
	public static partial class SceneLoaderUtility {

		private static readonly string Tag = "[SceneManager] ";

		public static LogType LogLevel { get; set; } = LogType.All;

		public enum LogType {
			Less = 0,
			All = 1,
			None = 2,
		}

		#region LoadScene

		/// <summary>
		/// Loads a single scene.
		/// </summary>
		/// <param name="scene">The scene to unload.</param>
		/// <param name="onFinished">Optional callback.</param>
		public static void LoadScene(string scene, Action onFinished = null) {
			//Block flow if the scene does not exist
			if(!Application.CanStreamedLevelBeLoaded(scene)) {
				if(LogLevel >= LogType.Less)
					Debug.LogWarning(Tag + "The scene \"" + scene + "\" cannot be found or does not exist.");
				return;
			}

			AsyncOperation op = SceneManager.LoadSceneAsync(scene);
			op.completed += _ => onFinished?.Invoke();
		}

		/// <summary>
		/// Loads in an array of scenes additively.
		/// </summary>
		/// <param name="scenes">The array of scene names.</param>
		/// <param name="onFinished">Optional callback.</param>
		/// <param name="duplicateScenes">Should duplicate scenes be allowed. False by default.</param>
		public static void LoadScenesAdditive(string[] scenes, Action onFinished = null, bool duplicateScenes = false) {
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
			foreach(AsyncOperation op in operations) {
				if(op == null) continue;

				op.allowSceneActivation = true;
			}

			//Step 3: Apply callback to the last scene
			for(int i = operations.Length - 1; i >= 0; i--) {
				AsyncOperation op = operations[i];

				if(op == null) continue;

				op.completed += _ => onFinished?.Invoke();
				break;
			}
		}

		/// <summary>
		/// Loads in the active base scene with an array of additive scenes.
		/// </summary>
		/// <param name="activeScene">The base scene to load as the active scene.</param>
		/// <param name="scenes">The additional scenes to load additively after the base scene.</param>
		/// <param name="onFinished">Optional callback.</param>
		/// <param name="duplicateScenes">Should duplicate scenes be allowed. False by default.</param>
		public static void LoadActiveScene(string activeScene, string[] scenes, Action onFinished = null, bool duplicateScenes = false) {
			LoadScene(activeScene, () => LoadScenesAdditive(scenes, onFinished, duplicateScenes));
		}

		#endregion

		#region UnloadScene

		/// <summary>
		/// Unloads a single scene.
		/// </summary>
		/// <param name="scene">The scene to unload.</param>
		/// <param name="onFinished">Optional callback.</param>
		public static void UnloadScene(string scene, Action onFinished = null) {
			//Block flow if the scene does not exist
			if(!Application.CanStreamedLevelBeLoaded(scene)) {
				if(LogLevel >= LogType.Less)
					Debug.LogWarning(Tag + "The scene \"" + scene + "\" cannot be found or does not exist.");
				return;
			}
			//Block flow if the scene is not loaded
			Scene sceneObj = SceneManager.GetSceneByName(scene);
			if(!sceneObj.isLoaded) {
				if(LogLevel >= LogType.All)
					Debug.LogWarning(Tag + "The scene \"" + scene + "\" is not loaded.");
				return;
			}
			
			AsyncOperation op = SceneManager.UnloadSceneAsync(scene);
			op.completed += _ => onFinished?.Invoke();
		}
		
		/// <summary>
		/// Unloads an array of scenes.
		/// </summary>
		/// <param name="scenes">The scenes to unload.</param>
		/// <param name="onFinished">Optional callback.</param>
		public static void UnloadScenes(string[] scenes, Action onFinished = null) {
			AsyncOperation[] operations = new AsyncOperation[scenes.Length];

			for(var i = 0; i < scenes.Length; i++) {
				string scene = scenes[i];
				
				//Block flow if the scene does not exist
				if(!Application.CanStreamedLevelBeLoaded(scene)) {
					if(LogLevel >= LogType.Less)
						Debug.LogWarning(Tag + "The scene \"" + scene + "\" cannot be found or does not exist.");
					continue;
				}

				//Block flow if the scene is not loaded
				Scene sceneObj = SceneManager.GetSceneByName(scene);
				if(!sceneObj.isLoaded) {
					if(LogLevel >= LogType.All)
						Debug.LogWarning(Tag + "The scene \"" + scene + "\" is not loaded.");
					continue;
				}

				AsyncOperation op = SceneManager.UnloadSceneAsync(scene);
				operations[i] = op;
			}

			for(int i = operations.Length-1; i >= 0; i--) {
				AsyncOperation op = operations[i];
				
				if(op == null) continue;

				op.completed += _ => onFinished?.Invoke();
				break;
			}
		}

		#endregion

		/// <summary>
		/// Unloads all scenes except for a select few.
		/// </summary>
		/// <param name="onFinished"></param>
		/// <param name="scenesExcept">The list of scenes to not unload</param>
		/*public static void UnloadAllScenesAsyncExcept(Action onTaskFinished = null, params string[] scenesExcept) {
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
		}*/

		public static void SetActiveScene(string scene) {
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
		}

	}
}
