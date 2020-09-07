using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SceneLoaderManagement {
	public partial class SceneLoaderManager : MonoBehaviour {

		public static SceneLoaderManager Instance { get; private set; }

		/// <summary>
		/// The minimum time the loading screen will wait for in seconds
		/// </summary>
		public float minWaitTime = 3.0f;

		[Header("Animation Speeds")]
		public float fadeInSpeedMultiplier = 1.0f;

		public float fadeOutSpeedMultiplier = 1.0f;

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
			get { return (int) (ProgressClamp * 100); }
		}

		private UnityAction g;

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

			animator.SetFloat("fadeInSpeed", fadeInSpeedMultiplier);
			animator.SetFloat("fadeOutSpeed", fadeOutSpeedMultiplier);
		}

		private void Update() {
			//Debug
			if(Input.GetKeyDown(KeyCode.H)) {
				FadeOut();
			}
		}

		/// <summary>
		/// Fades out the canvas
		/// </summary>
		private void FadeOut() {
			IsOn = false;
			canvasGroup.blocksRaycasts = false;

			loadingScreenRef.GetComponent<Animator>().SetTrigger("fadeOut");
			animator.SetTrigger("fadeOut");
		}

		/// <summary>
		/// Called externally by the animator when an animation has finished
		/// </summary>
		public void AnimationFinished() {
			if(fadeFinished != null) {
				fadeFinished();

				//Clear the event
				//fadeFinished = delegate { };
				fadeFinished = null;
			}

		}

		/// <summary>
		/// Called externally by the loading canvas's animator when an animation has finished
		/// </summary>
		public void LoadingCanvasAnimationFinished() {
			if(fadeFinished != null) {
				fadeFinished();

				//Clear the event
				//fadeFinished = delegate { };
				fadeFinished = null;
			}

		}

	}
}