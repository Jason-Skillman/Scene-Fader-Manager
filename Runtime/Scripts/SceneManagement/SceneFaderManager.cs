using System;
using System.Collections;
using Singleton;
using StateMachine.Callback;
using UnityEngine;
using UnityEngine.Events;

namespace SceneFader {
	public class SceneFaderManager : SingletonBehavior<SceneFaderManager>, IStateMachineCallback {

		/// <summary>
		/// The minimum time the loading screen will wait for in seconds
		/// </summary>
		[Header("Animation Speeds"), SerializeField]
		private float minWaitTime = 0.0f;
		[SerializeField]
		private float fadeInMultiplier = 0.15f;
		[SerializeField]
		private float fadeOutMultiplier = 0.15f;

		[Header("References"), SerializeField]
		private GameObject canvas = default;

		private Animator animator;
		private CanvasGroup canvasGroup;

		//private Action onFadeInFinish;
		private Func<IEnumerator[]> onFadeInFinish;
		
		private static readonly int FadeInMultiplier = Animator.StringToHash("fadeInMultiplier");
		private static readonly int FadeOutMultiplier = Animator.StringToHash("fadeOutMultiplier");
		private static readonly int IsShowing = Animator.StringToHash("isShowing");

		public bool IsOn { get; private set; }

		public float ProgressClamp { get; private set; }

		public int Progress => (int) (ProgressClamp * 100);

		protected override void Awake() {
			base.Awake();

			animator = GetComponent<Animator>();
			canvasGroup = canvas.GetComponent<CanvasGroup>();
		}

		private void Start() {
			IsOn = false;
			canvasGroup.blocksRaycasts = false;

			animator.SetFloat(FadeInMultiplier, fadeInMultiplier);
			animator.SetFloat(FadeOutMultiplier, fadeOutMultiplier);
		}

		private void Update() {
			//Debug
			if(Input.GetKeyDown(KeyCode.Alpha2)) {
				FadeOut();
			}

			if(Input.GetKeyDown(KeyCode.Alpha3)) {
				//StopCoroutine(coroutine);
			}
		}
		
		public void OnAnimationStart(AnimatorStateInfo stateInfo, int layerIndex) { }

		public void OnAnimationUpdate(AnimatorStateInfo stateInfo, int layerIndex) { }

		public void OnAnimationEnd(AnimatorStateInfo stateInfo, int layerIndex) {
			if(stateInfo.IsName("FadeIn")) {
				//Perform operations
				
				//onFadeInFinish?.Invoke();
				//onFadeInFinish = null;

				if(onFadeInFinish != null) {
					//Todo: cache
					StartCoroutine(CoroutinePerformTasks(onFadeInFinish?.Invoke(), () => {
						FadeOut();
					}));
				} else {
					FadeOut();
				}
				
			} else if(stateInfo.IsName("FadeOut")) {
				//Done fading out
			}
		}

		/// <summary>
		/// Main method!
		/// </summary>
		public void StartOperation(int seconds, params IEnumerator[] tasks) {
			if(IsOn) {
				Debug.LogWarning("Scene manager is already loading.");
				return;
			}

			IsOn = true;
			canvasGroup.blocksRaycasts = true;

			animator.SetBool("isShowing", true);

			//Add callback when fade in has finished
			onFadeInFinish = () => {
				return tasks;
			};
		}

		private IEnumerator CoroutinePerformTasks(IEnumerator[] tasks, Action onFinish = null) {
			foreach(IEnumerator task in tasks) {
				yield return task;
			}
			
			onFinish?.Invoke();
		}

		/// <summary>
		/// Fades out the canvas
		/// </summary>
		private void FadeOut() {
			IsOn = false;
			canvasGroup.blocksRaycasts = false;

			animator.SetBool(IsShowing, false);
		}
		
		
		
	}
}