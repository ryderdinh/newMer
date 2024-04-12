using System;
using System.Collections;
using Controllers;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private Database _gameDatabase;

    private void Awake()
    {
        DOTween.Init();
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
        Application.targetFrameRate = 60;
        _gameDatabase = Resources.Load<Database>("Database");
    }


    private static IEnumerator LoadAsyncScene(string sceneName, Action callback = null)
    {
        if (string.IsNullOrEmpty(sceneName))
            yield break;

        var asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait scene loaded
        while (!asyncLoad.isDone) yield return null;

        callback?.Invoke();
    }

    public void StartPairGame()
    {
        StartCoroutine(LoadAsyncScene(
            "Pair",
            () => { PairGameController.Instance.InitLevel(_gameDatabase.cardData); })
        );
    }

    public void BackToHome()
    {
        StartCoroutine(LoadAsyncScene("Loader"));
    }

    private async UniTask LoadScene(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        // enable loader canvas

        do
        {
            await UniTask.Delay(100);
            Debug.Log(scene.progress);
        } while (scene.progress < 0.9f);

        await UniTask.Delay(1000);

        scene.allowSceneActivation = true;

        // disable loader canvas
    }
}