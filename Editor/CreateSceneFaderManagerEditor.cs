﻿namespace JasonSkillman.SceneFader.Editor {
	using SceneFader;
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.EventSystems;
	
	public static class CreateSceneFaderManagerEditor {
	
		[MenuItem("GameObject/Scene Fader/Scene Fader Manager", false, 10)]
		static void CreateSceneFaderManager(MenuCommand menuCommand) {
			//Check if the manager has already been created
			SceneFaderManager manager = Object.FindObjectOfType<SceneFaderManager>();

			if(manager != null) {
				Debug.LogWarning("SceneFaderManager has already been created.");
				Selection.activeObject = manager;
				return;
			}

			//Use the asset database to fetch the console prefab
			GameObject managerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
				"Packages/com.jasonskillman.scenefadermanager/Runtime/Prefabs/SceneFaderManager.prefab");

			//Instantiate the prefab in the hierarchy
			PrefabUtility.InstantiatePrefab(managerPrefab);
        
			Selection.activeObject = managerPrefab;
			
			
			//Instantiate an EventSystem if one does not exist
			GameObject eventSystem = GameObject.Find("EventSystem");
			if(eventSystem != null) return;
        
			eventSystem = new GameObject("EventSystem");
			eventSystem.AddComponent<EventSystem>();
			eventSystem.AddComponent<StandaloneInputModule>();
		}

	}
}
