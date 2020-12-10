using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement {
	public partial class SceneLoaderManager {
		
		/*public void LoadScene(string sceneName) {
			//Block flow of control if the scene loader is already loading
			if(IsOn) {
				Debug.Log("Scene is already loading");
				return;
			}

			IsOn = true;
			canvasGroup.blocksRaycasts = true;

			animator.SetTrigger("fadeIn");

			fadeFinished = delegate {
				StartCoroutine(LoadSceneTask(sceneName, 1.0f));
			};
		}*/

		public void LoadSceneAdditive(params string[] scenes) {
			if(IsOn) {
				Debug.Log("Scene is already loading");
				return;
			}

			IsOn = true;
			canvasGroup.blocksRaycasts = true;

			animator.SetBool("isShowing", true);

			//Add callback when fade in has finished
			onFadeInFinish = () => {
				//Todo: cache ref
				StartCoroutine(TaskLoadSceneAdditive(scenes));
			};
		}

		/*private IEnumerator LoadSceneTask(string sceneName, float delay = 0.0f) {
			yield return new WaitForSeconds(delay);
			
			loadingScreenRef.GetComponent<Animator>().SetTrigger("fadeIn");

			float time = 0;

			//Begin to load the Scene you specify
			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

			//Don't let the Scene activate until you allow it to
			asyncOperation.allowSceneActivation = false;

			while(!asyncOperation.isDone) {
				//Keep track of how much time has gone by
				time += Time.deltaTime;

				//Update the async progress
				ProgressClamp = asyncOperation.progress;

				// Check if the load has finished
				if(asyncOperation.progress >= 0.9f) {
					//Block final load if minimum time has not been reached
					if(time < minWaitTime) {
						yield return null;
						continue;
					}

					//Finished

					asyncOperation.allowSceneActivation = true;

					FadeOut();
				}

				yield return null;
			}
		}*/

		private IEnumerator TaskLoadSceneAdditive(params string[] scenes) {
			yield return null;

			float time = 0;

			List<AsyncOperation> listOperations = new List<AsyncOperation>();

			//Load all of operations
			foreach(string sceneName in scenes) {
				//Begin to load the Scene you specify
				AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
				listOperations.Add(asyncOperation);

				//Don't let the Scene activate until you allow it to
				asyncOperation.allowSceneActivation = false;

				while(asyncOperation.progress < 0.9f) {
					time += Time.deltaTime;
					yield return null;
				}
			}

			//Activate all of the operations
			foreach(AsyncOperation operation in listOperations) {
				operation.allowSceneActivation = true;

				while(!operation.isDone) {
					time += Time.deltaTime;
					yield return null;
				}
			}

			//Wait for the min time
			while(true) {
				if(time >= minWaitTime)
					break;

				time += Time.deltaTime;
				yield return null;
			}

			FadeOut();
		}
		
	}
}