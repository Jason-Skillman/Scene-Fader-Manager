# Scene-Fader-Manager
A scene fader manager that loads in scenes and performs tasks. Comes with an extensive `SceneUtility` class which handles scene management.

## Prerequisites
This package uses the `State-Machine-Callback` packages. It is recommended to install this package before installing this one.

1. https://github.com/Jason-Skillman/State-Machine-Callback

---
**Note:**

Prerequisite packages can also be installed with the same steps below.

---

## How to install
This package can be installed through the Unity `Package Manager` with Unity version 2019.3 or greater.

Open up the package manager `Window/Package Manager` and click on `Add package from git URL...`.

![unity_package_manager_git_drop_down](Documentation~/images/unity_package_manager_git_drop_down.png)

Paste in this repository's url.

`https://github.com/Jason-Skillman/Scene-Fader-Manager.git`

![unity_package_manager_git_with_url](Documentation~/images/unity_package_manager_git_with_url.png)

Click `Add` and the package will be installed in your project.

---
**NOTE:** For Unity version 2019.2 or lower

If you are using Unity 2019.2 or lower than you will not be able to install the package with the above method. Here are a few other ways to install the package.
1. You can clone this git repository into your project's `Packages` folder.
1. Another alternative would be to download this package from GitHub as a zip file. Unzip and in the `Package Manager` click on `Add package from disk...` and select the package's root folder.

---

### Git submodule
Alternatively you can also install this package as a git submodule.

```console
$ git submodule add https://github.com/Jason-Skillman/Scene-Fader-Manager.git Packages/Scene-Fader-Manager
```

## How to setup
You can create the manager by going to `Create/Scene Fader/Scene Fader Manager` in the `Hierarchy` window. Only one manager should exist at any given time.

## SceneFaderManager
The manager is is charge of fading in and out the screen while performing tasks and operations.

![scene_fader_manager](Documentation~/images/scene_fader_manager.png)

Example 1: This example creates a `IEnumerator` tasks and gives it to the `SceneFaderManager`. While the screen has fade to black the scene will be loaded in.
```C#
//This task will load in Scene01
IEnumerator task1 = SceneUtility.CoroutineLoadScene("Scene01");

//Give the task to SceneFaderManager
SceneFaderManager.Instance.FadeAndPerformTasks(0, task1);
```

Example 2: This example creates two tasks and gives it to the `SceneFaderManager`. Together these tasks load in level 2 and unload level 1.
```C#
//This task will load in all the scenes in sceneArray
IEnumerator task1 = SceneUtility.CoroutineLoadScenesAdditive(sceneArray);
//This task will unload Scene01
IEnumerator task2 = SceneUtility.CoroutineUnloadScene("Scene01");

//Give the task to SceneFaderManager
SceneFaderManager.Instance.FadeAndPerformTasks(0, task1, task2);
```

These are just examples for loading in and out scenes but custom coroutines can be created and given to the `SceneFaderManager`. Ex. Generate a procedural level or spawn in all players.

|Event|Description|
|---|---|
|`OnFadeIn`|Fires when the manager has started fading in.|
|`OnTasksStarted`|Fires when the manager's tasks have just started.|
|`OnTasksFinished`|Fires when all of the tasks are finished and the manager starts fading out.|
|`OnFadeOut`|Fires when the manager has completely faded out and the screen is visible.|

## Components

### SceneUtilityFunctions
The `SceneUtilityFunctions` component is a wraper class that uses the static functions found in the `SceneUtility`. Can be used to instantly load or unload scenes with a UI button or other event.

### SceneFaderFunctions
The `SceneFaderFunctions` component is a wraper class that uses the functions found in the `SceneFaderManager`. Can be used to load or unload scenes using the fader's animation.

## SceneUtility
`SceneUtility` is a static class which houses multible methods to load in and out scenes. `SceneUtility` is a wrapper for Unity's `SceneManager` and adds extra functionality, guard clauses, and performace boosts.

|Property|Description|
|---|---|
|`LogType` `LogLevel`|Sets the utilities's log level.|

#### Static methods
These methods are all static and can be called from anywhere.

|Method|Description|
|---|---|
|`LoadScene()`|Loads a single scene.|
|`LoadScenesAdditive()`|Loads in an array of scenes additively.|
|`LoadActiveScene()`|Loads in the active base scene with an array of additive scenes.|
|`UnloadScene()`|Unloads a single scene.|
|`UnloadScenes()`|Unloads an array of scenes.|
|`UnloadAllScenesExceptFor()`|Unloads all scenes except for the provided array.|

#### Coroutines
These methods perform the same functions as the ones above but can be called from a `MonoBehaviour` and can be used with the `SceneFaderManager`'s task system.

|Method|Description|
|---|---|
|`CoroutineLoadScene()`|Loads a single scene.|
|`CoroutineLoadScenesAdditive()`|Loads in an array of scenes additively.|
|`CoroutineUnloadScene()`|Unloads a single scene.|
|`CoroutineUnloadScenes()`|Unloads an array of scenes.|
|`CoroutineUnloadAllScenesExceptFor()`|Unloads all scenes except for the provided array.|

|Event|Description|
|---|---|
|`Action<string[]>` `OnSceneLoaded`|Fires when a scene has been loaded.|
|`Action<string[]>` `OnSceneUnloaded`|Fires when a scene has been loaded.|
