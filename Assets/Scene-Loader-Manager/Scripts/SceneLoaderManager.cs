using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour {

	public static SceneLoaderManager Instance { get; private set; }

	/// <summary>
	/// The minimum time the loading screen will wait for in seconds
	/// </summary>
	public float minWaitTime = 3.0f;

	[Header("References")]
	public GameObject canvas;
	public GameObject prefabLoadingScreen;

	private Animator animator;
	private CanvasGroup canvasGroup;

	private Action fadeFinished;
	private GameObject loadingScreenRef;
	
	public bool IsOn { get; private set; }
	
	public float ProgressClamp { get; private set; }

	public int Progress {
		get {
			return (int)(ProgressClamp * 100);
		}
	}

	private void Awake() {
		if(Instance == null) Instance = this;
		else Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
		
		animator = canvas.GetComponent<Animator>();
		canvasGroup = canvas.GetComponent<CanvasGroup>();
		
		loadingScreenRef = Instantiate(prefabLoadingScreen, transform);
	}

	private void Start() {
		IsOn = false;
		canvasGroup.blocksRaycasts = false;
	}

	private void Update() {
		//Debug
		if(Input.GetKeyDown(KeyCode.H)) {
			FadeOut();
		}
	}

	public void LoadScene(string sceneName) {
		//Block flow of control if the scene loader is already loading
		if(IsOn) {
			Debug.Log("Scene is already loading");
			return;
		}
		
		IsOn = true;
		canvasGroup.blocksRaycasts = true;
		
		animator.SetTrigger("fadeIn");
		
		fadeFinished = delegate {
			StartCoroutine(LoadSingle(sceneName));

			loadingScreenRef.GetComponent<Animator>().SetTrigger("fadeIn");
		};
	}
	
	public void LoadSceneAdditive(params string[] scenes) {
		//Block flow of control if the scene loader is already loading
		if(IsOn) {
			Debug.Log("Scene is already loading");
			return;
		}
		
		IsOn = true;
		canvasGroup.blocksRaycasts = true;
		
		animator.SetTrigger("fadeIn");
		
		fadeFinished = delegate {
			StartCoroutine(LoadAdd(scenes));

			loadingScreenRef.GetComponent<Animator>().SetTrigger("fadeIn");
		};
	}

	private IEnumerator LoadSingle(string sceneName) {
		yield return null;

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
	}

	private IEnumerator LoadAdd(params string[] scenes) {
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
	
	private void FadeOut() {
		IsOn = false;
		canvasGroup.blocksRaycasts = false;
			
		loadingScreenRef.GetComponent<Animator>().SetTrigger("fadeOut");
		animator.SetTrigger("fadeOut");
	}
	
	/// <summary>
	/// Called externally by the animation
	/// </summary>
	public void AnimationFinished() {
		if(fadeFinished != null) {
			fadeFinished();
			
			//Clear the event
			//fadeFinished = delegate { };
			fadeFinished = null;
		}
		
	}

}