using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMng : MonoBehaviour
{
    [SerializeField] private string     menuScene;
    [SerializeField] private AudioClip  menuSceneMusic;
    [SerializeField] private string     gameScene;
    [SerializeField] private AudioClip  gameSceneMusic;

    Coroutine   startSceneCR;
    string      currentScene;
    AudioClip   currentSceneMusic;

    private void Start()
    {
        currentScene = "";
        currentSceneMusic = null;

        if (SceneManager.sceneCount == 1)
        {
            StartScene(menuScene, menuSceneMusic);
        }
    }
    public void StartScene(string sceneName)
    {
        if (startSceneCR != null) return;

        AudioClip music = null;
        if (sceneName == gameScene) music = gameSceneMusic;
        else if (sceneName == menuScene) music = menuSceneMusic;

        startSceneCR = StartCoroutine(StartSceneCR(sceneName, music));
    }

    public void StartScene(string sceneName, AudioClip music)
    {
        if (startSceneCR != null) return;

        startSceneCR = StartCoroutine(StartSceneCR(sceneName, music));
    }

    IEnumerator StartSceneCR(string sceneName, AudioClip music)
    { 
        if (currentSceneMusic)
        {
            SoundManager.Get().FadeOut(currentSceneMusic);
        }

        AsyncOperation operation;

        if (currentScene != "")
        {
            operation = SceneManager.UnloadSceneAsync(currentScene);

            yield return operation;
        }

        operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        operation.allowSceneActivation = true;

        yield return operation;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        startSceneCR = null;

        if (music)
        {
            SoundManager.Get().FadeIn(music, 1.0f, 1.0f, true);
        }

        currentScene = sceneName;
        currentSceneMusic = music;
    }
}
