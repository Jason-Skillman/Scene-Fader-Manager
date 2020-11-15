using System;
using Singleton;
using StateMachine.Callback;
using UnityEngine;
using UnityEngine.Events;

namespace SceneLoaderManagement {
	public partial class SceneLoaderManager : SingletonBehavior<SceneLoaderManager>, IStateMachineCallback {
			
		/// <summary>
		/// The minimum time the loading screen will wait for in seconds
		/// </summary>
		public float minWaitTime = 3.0f;

		[Header("Animation Speeds")]
		public float fadeInSpeedMultiplier = 1.0f;

		public float fadeOutSpeedMultiplier = 1.0f;

		[Header("References")]
		public GameObject canvas;

		//public GameObject prefabLoadingScreen;

		private Animator animator;
		private CanvasGroup canvasGroup;

		private Action onFadeInFinish;
		//private GameObject loadingScreenRef;

		public bool IsOn { get; private set; }

		public float ProgressClamp { get; private set; }

		public int Progress {
			get { return (int) (ProgressClamp * 100); }
		}

		//private UnityAction g;

		protected override void Awake() {
			base.Awake();
			//DontDestroyOnLoad(gameObject);

			animator = GetComponent<Animator>();
			canvasGroup = canvas.GetComponent<CanvasGroup>();

			//loadingScreenRef = Instantiate(prefabLoadingScreen, transform);
		}

		private void Start() {
			IsOn = false;
			canvasGroup.blocksRaycasts = false;

			animator.SetFloat("fadeInSpeed", fadeInSpeedMultiplier);
			animator.SetFloat("fadeOutSpeed", fadeOutSpeedMultiplier);
		}

		private void Update() {
			//Debug
			if(Input.GetKeyDown(KeyCode.Alpha2)) {
				FadeOut();
			}
		}

		/// <summary>
		/// Fades out the canvas
		/// </summary>
		private void FadeOut() {
			IsOn = false;
			canvasGroup.blocksRaycasts = false;

			//loadingScreenRef.GetComponent<Animator>().SetTrigger("fadeOut");
			animator.SetBool("isShowing", false);
		}
		
		public void OnAnimationStart(AnimatorStateInfo stateInfo, int layerIndex) { }

		public void OnAnimationUpdate(AnimatorStateInfo stateInfo, int layerIndex) { }

		public void OnAnimationEnd(AnimatorStateInfo stateInfo, int layerIndex) {
			if(stateInfo.IsName("FadeIn")) {
				onFadeInFinish?.Invoke();
				onFadeInFinish = null;
			} else if(stateInfo.IsName("FadeOut")) {
				//print("Callback done");
			}
			
		}
		
	}
}