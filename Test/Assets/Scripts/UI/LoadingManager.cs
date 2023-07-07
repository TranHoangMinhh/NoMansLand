using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using TMPro;

public class LoadingManager : MonoBehaviour
{

    public static LoadingManager Instance;

    [SerializeField] private GameObject loadingCanvas;
    // [SerializeField] private Image progressBar;

    // Hold the progress value for every frame update
    private float _target;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void LoadScene(int sceneId)
    {
        _target = 0;

        var scene = SceneManager.LoadSceneAsync(sceneId);
        scene.allowSceneActivation = false;

        loadingCanvas.SetActive(true);

        do
        {
            await Task.Delay(100);
            _target = scene.progress;
        } while (scene.progress < 0.9f);

        // Not neccessary, only to make loading look more visible
        await Task.Delay(100);

        scene.allowSceneActivation = true;
        loadingCanvas.SetActive(false);
    }
}
