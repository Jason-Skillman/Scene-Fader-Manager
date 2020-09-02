using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour {

	public static SceneLoaderManager Instance { get; private set; }

	public GameObject prefabLoadingScreen;

	[Header("References")]
	public GameObject canvas;
	
	private Animator animator;
	private CanvasGroup canvasGroup;

	private Action fadeFinished;
	private GameObject loadingScreenRef;
	
	public bool IsOn { get; private set; }

	void Awake() {
		if(Instance == null) Instance = this;
		else Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
		
		animator = canvas.GetComponent<Animator>();
		canvasGroup = canvas.GetComponent<CanvasGroup>();
	}

	private void Start() {
		IsOn = false;
		canvasGroup.blocksRaycasts = false;
		
		if(loadingScreenRef == null) {
			loadingScreenRef = Instantiate(prefabLoadingScreen, transform);
		}
	}

	void Update() {
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
			StartCoroutine(Load(sceneName));

			loadingScreenRef.GetComponent<Animator>().SetTrigger("fadeIn");
		};
	}

	private IEnumerator Load(string sceneName) {
		yield return null;

		//Begin to load the Scene you specify
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
		
		//Don't let the Scene activate until you allow it to
		asyncOperation.allowSceneActivation = false;
		Debug.Log("Pro :" + asyncOperation.progress);
		
		//When the load is still in progress, output the Text and progress bar
		while(!asyncOperation.isDone) {
			//Output the current progress
			//m_Text.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";
			Debug.Log("Pro :" + (asyncOperation.progress * 100) + "%");

			// Check if the load has finished
			if(asyncOperation.progress >= 0.9f) {
				//Change the Text to show the Scene is ready
				//m_Text.text = "Press the space bar to continue";
				Debug.Log("Press the space bar to continue");
				
				//Wait to you press the space key to activate the Scene
				if(Input.GetKeyDown(KeyCode.Space)) {
					//Activate the Scene
					asyncOperation.allowSceneActivation = true;

					FadeOut();
				}
				
			}

			yield return null;
		}
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