using System;
using System.Collections;
using Singleton;
using StateMachine.Callback;
using UnityEngine;
using UnityEngine.Events;

namespace SceneFader {
	public class SceneFaderManager : SingletonBehavior<SceneFaderManager>, IStateMachineCallback {
		
		private const string Tag = "[SceneFaderManager] ";

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

		private Func<IEnumerator[]> onFadeInFinish;
		private Coroutine coroutineTasks;
		
		private static readonly int FadeInMultiplier = Animator.StringToHash("fadeInMultiplier");
		private static readonly int FadeOutMultiplier = Animator.StringToHash("fadeOutMultiplier");
		private static readonly int IsShowing = Animator.StringToHash("isShowing");

		public bool IsOn { get; private set; }

		public float ProgressClamp { get; private set; }

		public int Progress => (int) (ProgressClamp * 100);

		protected override void Awake() {
			base.Awake();
			DontDestroyOnLoad(gameObject);

			animator = GetComponent<Animator>();
			canvasGroup = canvas.GetComponent<CanvasGroup>();
		}

		private void Start() {
			canvasGroup.blocksRaycasts = false;

			animator.SetFloat(FadeInMultiplier, fadeInMultiplier);
			animator.SetFloat(FadeOutMultiplier, fadeOutMultiplier);
		}

		public void OnAnimationStart(AnimatorStateInfo stateInfo, int layerIndex) { }

		public void OnAnimationUpdate(AnimatorStateInfo stateInfo, int layerIndex) { }

		public void OnAnimationEnd(AnimatorStateInfo stateInfo, int layerIndex) {
			if(stateInfo.IsName("FadeIn")) {
				//Perform operations
				if(onFadeInFinish != null) {
					coroutineTasks = StartCoroutine(CoroutinePerformTasks(onFadeInFinish?.Invoke(), () => {
						FadeOut();
					}));
				} else {
					FadeOut();
				}

				onFadeInFinish = null;

			} else if(stateInfo.IsName("FadeOut")) {
				//Todo: add events here
			}
		}

		/// <summary>
		/// Fades out the screen and performs the list of tasks. Fades back in when tasks are completed.
		/// <para>This is where you want to load in and out scenes.</para>
		/// <para>Main method for using the SceneFaderManager.</para>
		/// </summary>
		/// <param name="seconds"></param>
		/// <param name="tasks"></param>
		public void FadeAndPerformTasks(int seconds, params IEnumerator[] tasks) {
			//Block flow if tasks is empty
			if(tasks.Length <= 0) return;
			
			//Block flow if manager is already working
			if(IsOn) {
				Debug.LogWarning(Tag + "Cant perform tasks, already working.");
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
				
				//Wait until next frame until starting the next task
				yield return null;
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
