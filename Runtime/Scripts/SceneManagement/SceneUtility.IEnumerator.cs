using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneFader.SceneManagement {
	public static partial class SceneUtility {

		#region LoadScene

		/// <summary>
		/// Loads a single scene.
		/// </summary>
		/// <param name="scene">The scene name.</param>
		/// <param name="onFinished">Optional callback.</param>
		/// <returns></returns>
		public static IEnumerator CoroutineLoadScene(string scene, Action onFinished = null) {
			//Block flow if the scene does not exist
			if(!Application.CanStreamedLevelBeLoaded(scene)) {
				if(LogLevel >= LogType.Less)
					Debug.LogWarning(Tag + "The scene \"" + scene + "\" cannot be found or does not exist.");
				yield break;
			}

			AsyncOperation op = SceneManager.LoadSceneAsync(scene);
			
			//Wait until the current scene is loaded
			while(op.progress < 0.9f) {
				yield return null;
			}
			//Debug
			//yield return new WaitForSeconds(1f);

			onFinished?.Invoke();
			OnSceneLoaded?.Invoke(new []{scene});
		}
		
		/// <summary>
		/// Loads in an array of scenes additively.
		/// </summary>
		/// <param name="scenes">The array of scene names.</param>
		/// <param name="onFinished">Optional callback.</param>
		/// <param name="duplicateScenes">Should duplicate scenes be allowed. False by default.</param>
		/// <returns></returns>
		public static IEnumerator CoroutineLoadScenesAdditive(string[] scenes, Action onFinished = null, bool duplicateScenes = false) {
			AsyncOperation[] operations = new AsyncOperation[scenes.Length];

			//Step 1: List all of operations
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

				//Start loading the scene
				AsyncOperation op = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
				op.allowSceneActivation = false;
				operations[i] = op;

				//Wait until the current scene is loaded but not activated
				while(op.progress < 0.9f) {
					yield return null;
				}
				//Debug
				//yield return new WaitForSeconds(1f);
			}

			//Step 2: Activate all of the operations at once
			foreach(AsyncOperation op in operations) {
				if(op == null) continue;
				
				op.allowSceneActivation = true;

				while(!op.isDone) {
					yield return null;
				}
				//Debug
				//yield return new WaitForSeconds(1f);
			}
			
			onFinished?.Invoke();
			OnSceneLoaded?.Invoke(scenes);
		}

		#endregion

		#region UnloadScene

		/// <summary>
		/// Unloads a single scene.
		/// </summary>
		/// <param name="scene">The scene to unload.</param>
		/// <param name="onFinished">Optional callback.</param>
		/// <returns></returns>
		public static IEnumerator CoroutineUnloadScene(string scene, Action onFinished = null) {
			//Block flow if the scene does not exist
			if(!Application.CanStreamedLevelBeLoaded(scene)) {
				if(LogLevel >= LogType.Less)
					Debug.LogWarning(Tag + "The scene \"" + scene + "\" cannot be found or does not exist.");
				yield break;
			}
			
			//Block flow if the scene is not loaded
			Scene sceneObj = SceneManager.GetSceneByName(scene);
			if(!sceneObj.isLoaded) {
				if(LogLevel >= LogType.All)
					Debug.LogWarning(Tag + "The scene \"" + scene + "\" is not loaded.");
				yield break;
			}
			
			//Block flow if their is only one scene left
			if(SceneManager.sceneCount == 1) {
				if(LogLevel >= LogType.Less)
					Debug.LogWarning(Tag + "The scene \"" + scene + "\" is the last scene. Cant unload the last scene.");
				yield break;
			}
			
			AsyncOperation op = SceneManager.UnloadSceneAsync(scene);
			
			//Wait until the current scene is loaded
			while(!op.isDone) {
				yield return null;
			}
			//Debug
			//yield return new WaitForSeconds(1f);

			onFinished?.Invoke();
			OnSceneUnloaded?.Invoke(new []{scene});
		}
		
		/// <summary>
		/// Unloads an array of scenes.
		/// </summary>
		/// <param name="scenes">The scenes to unload.</param>
		/// <param name="onFinished">Optional callback.</param>
		/// <returns></returns>
		public static IEnumerator CoroutineUnloadScenes(string[] scenes, Action onFinished = null) {
			foreach(string scene in scenes) {
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
				
				//Block flow if their is only one scene left
				if(SceneManager.sceneCount == 1) {
					if(LogLevel >= LogType.Less)
						Debug.LogWarning(Tag + "The scene \"" + scene + "\" is the last scene. Cant unload the last scene.");
					continue;
				}

				AsyncOperation op = SceneManager.UnloadSceneAsync(scene);

				while(!op.isDone) {
					yield return null;
				}
				//Debug
				//yield return new WaitForSeconds(1f);
			}
			
			onFinished?.Invoke();
			OnSceneUnloaded?.Invoke(scenes);
		}

		/// <summary>
		/// Unloads all scenes except for the provided array.
		/// </summary>
		/// <param name="scenesExcept">The list of scenes to not unload</param>
		/// <param name="onFinished">Optional callback.</param>
		/// <returns></returns>
		public static IEnumerator CoroutineUnloadAllScenesExceptFor(string[] scenesExcept, Action onFinished = null) {
			string[] scenesToUnload = new string[SceneManager.sceneCount];
			List<string> unloadedScenes = new List<string>();

			//Loop through all of the existing scenes
			for(int i = 0; i < SceneManager.sceneCount; i++) {
				Scene scene = SceneManager.GetSceneAt(i);
				unloadedScenes.Add(scene.name);
				
				//Skip unloading if the scene is excluded
				bool flagSkip = false;
				foreach(string sceneExcept in scenesExcept) {
					if(scene.name.Equals(sceneExcept)) {
						flagSkip = true;
						break;
					}
				}
				if(flagSkip) continue;

				scenesToUnload[i] = scene.name;
			}

			foreach(string scene in scenesToUnload) {
				if(scene == null) continue;
				
				AsyncOperation op = SceneManager.UnloadSceneAsync(scene);
				
				while(!op.isDone) {
					yield return null;
				}
				//Debug
				//yield return new WaitForSeconds(1f);
			}
			
			onFinished?.Invoke();
			OnSceneUnloaded?.Invoke(unloadedScenes.ToArray());
		}
		
		#endregion

	}
}