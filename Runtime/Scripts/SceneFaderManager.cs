using System;
using System.Collections;
using Singleton;
using StateMachine;
using UnityEngine;
using UnityEngine.Events;

namespace SceneFader {
	public class SceneFaderManager : SingletonBehavior<SceneFaderManager>, IStateMachineCallback {
		
		private const string Tag = "[SceneFaderManager] ";

		/// <summary>
		/// The minimum time the loading screen will wait for in seconds
		/// </summary>
		[Header("Animation Speeds"), SerializeField]
		private float fadeInMultiplier = 1.75f;
		[SerializeField]
		private float fadeOutMultiplier = 1.75f;

		[Header("References"), SerializeField]
		private GameObject canvas = default;

		[SerializeField, Space]
		[Tooltip("Fires when the manager has started fading in.")]
		private UnityEvent onFadeIn = default;
		[SerializeField]
		[Tooltip("Fires when the manager's tasks have just started.")]
		private UnityEvent onTasksStarted = default;
		[SerializeField]
		[Tooltip("Fires when all of the tasks are finished and the manager starts fading out.")]
		private UnityEvent onTasksFinished = default;
		[SerializeField]
		[Tooltip("Fires when the manager has completely faded out and the screen is visible.")]
		private UnityEvent onFadeOut = default;

		private Animator animator;
		private CanvasGroup canvasGroup;

		private Func<IEnumerator[]> onFadeInFinish;
		private Coroutine coroutineTasks;
		private float minSeconds;
		
		private static readonly int FadeInMultiplier = Animator.StringToHash("fadeInMultiplier");
		private static readonly int FadeOutMultiplier = Animator.StringToHash("fadeOutMultiplier");
		private static readonly int IsShowing = Animator.StringToHash("isShowing");

		/// <summary>
		/// Fires when the manager has started fading in.
		/// </summary>
		public event Action OnFadeIn;
		/// <summary>
		/// Fires when the manager's tasks have just started.
		/// </summary>
		public event Action OnTasksStarted;
		/// <summary>
		/// Fires when all of the tasks are finished and the manager starts fading out.
		/// </summary>
		public event Action OnTasksFinished;
		/// <summary>
		/// Fires when the manager has completely faded out and the screen is visible.
		/// </summary>
		public event Action OnFadeOut;

		public bool IsWorking { get; private set; }

		//public float ProgressClamp { get; private set; }

		//public int Progress => (int) (ProgressClamp * 100);

		protected override void Awake() {
			base.Awake();

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
				onFadeIn?.Invoke();
				OnFadeIn?.Invoke();
				
				//Perform operations
				if(onFadeInFinish != null) {
					coroutineTasks = StartCoroutine(CoroutinePerformTasks(minSeconds, onFadeInFinish?.Invoke(), () => {
						FadeOut();
						onTasksFinished?.Invoke();
					}));
				} else {
					FadeOut();
					onTasksFinished?.Invoke();
					OnTasksFinished?.Invoke();
				}

				onFadeInFinish = null;

			} else if(stateInfo.IsName("FadeOut")) {
				onFadeOut?.Invoke();
				OnFadeOut?.Invoke();
			}
		}

		/// <summary>
		/// Fades out the screen and performs the list of tasks. Fades back in when tasks are completed.
		/// <para>This is where you want to load in and out scenes.</para>
		/// <para>Main method for using the SceneFaderManager.</para>
		/// </summary>
		/// <param name="seconds">The minimum amount of seconds to wait for.</param>
		/// <param name="tasks">The array of tasks to perform.</param>
		public void FadeAndPerformTasks(float seconds = 0, params IEnumerator[] tasks) {
			//Block flow if tasks is empty
			if(tasks.Length <= 0) return;
			
			//Block flow if manager is already working
			if(IsWorking) {
				Debug.LogWarning(Tag + "Cant perform tasks, already working.");
				return;
			}

			minSeconds = seconds;

			//Add callback when fade in has finished
			onFadeInFinish = () => tasks;
			
			FadeIn();
		}

		private IEnumerator CoroutinePerformTasks(float seconds, IEnumerator[] tasks, Action onFinish = null) {
			DateTime timeStart = DateTime.Now;

			onTasksStarted?.Invoke();
			OnTasksStarted?.Invoke();

			foreach(IEnumerator task in tasks) {
				yield return task;
				
				//Wait until next frame until starting the next task
				yield return null;
			}
			
			DateTime timeEnd = DateTime.Now;
			TimeSpan elapsedTime = timeEnd - timeStart;

			//Check if the minimum amount of seconds has gone by
			if(elapsedTime.TotalSeconds < seconds) {
				float timeLeft = seconds - (float)elapsedTime.TotalSeconds;
				yield return new WaitForSeconds(timeLeft);
			}

			onFinish?.Invoke();
		}
		
		/// <summary>
		/// Fades in the canvas.
		/// </summary>
		private void FadeIn() {
			IsWorking = true;
			canvasGroup.blocksRaycasts = true;
			
			animator.SetBool(IsShowing, true);
		}

		/// <summary>
		/// Fades out the canvas.
		/// </summary>
		private void FadeOut() {
			IsWorking = false;
			canvasGroup.blocksRaycasts = false;

			animator.SetBool(IsShowing, false);
		}
		
	}
}
