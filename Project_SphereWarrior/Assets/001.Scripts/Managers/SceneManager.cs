using System;
using UnityEngine;

public class SceneManager
{
    private Transform sceneTrans;
    public Transform SceneTrans
    {
        get
        {
            if (sceneTrans == null)
            {
                GameObject go = GameObject.Find("@SceneTrans");
                if (go == null)
                    go = new GameObject(name: "@SceneTrans");
                sceneTrans = go.transform;
                UnityEngine.Object.DontDestroyOnLoad(go);
            }
            return sceneTrans;
        }
    }
    private Define.Scene currentScene;
    private bool isLoading = false;
    private Action loadCallback;

    public void LoadScene(Define.Scene _scene,  Action _loadCallback = null)
    {

        if (isLoading) return;
        isLoading = true;
        loadCallback = _loadCallback;
        Managers.Pool.Clear();
        Managers.UI.CloseAllPopupUI();

        RemoveScene(currentScene, () =>
        {
            currentScene = _scene;
            AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync($"{_scene}");
            async.completed += (_) =>
            {
                AddScene(_scene, () => { loadCallback?.Invoke(); });
                isLoading = false;
            };
        });
    }

    public void RemoveScene(Define.Scene _scene, Action _callback = null)
    {
        SceneBase bs = null;
        switch (_scene)
        {

            default:
                _callback?.Invoke();
                return;
        }

        if (bs != null)
        {
            bs.Clear();
            UnityEngine.Object.Destroy(bs);
        }
        _callback?.Invoke();
    }

    // ?? ???
    public void AddScene(Define.Scene _scene, Action _addSceneCallback)
    {
        SceneBase bs = null;
        //Managers.Data.LoadSceneData(addScene);
        switch (_scene)
        {
            default:
                _addSceneCallback?.Invoke();
                return;
        }

        bs.Init();
        _addSceneCallback?.Invoke();
    }

    public T GetActiveScene<T>() where T : SceneBase
    {
        return SceneTrans.GetComponent<T>() as T;
    }
}