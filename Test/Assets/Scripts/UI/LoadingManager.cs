using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System.Threading.Tasks;

public class LoadingManager : MonoBehaviour
{

    public static LoadingManager Instance;

    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private bool needCanvas = true;

    // Hold the progress value for every frame update
    //private float _target;

    public enum Scenes
    {
        StartScene,
        MainMenuScene,
        LobbyScene,
        ChooseScene,
        GameScene
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void LoadScene(Scenes scene)
    {
        //_target = 0;

        var loadedScene = SceneManager.LoadSceneAsync(scene.ToString());
        loadedScene.allowSceneActivation = false;

        if (needCanvas)
            loadingCanvas.SetActive(true);

        do
        {
            await Task.Delay(100);
        } while (loadedScene.progress < 0.9f);

        // Not neccessary, only to make loading look more visible
        //await Task.Delay(100);

        loadedScene.allowSceneActivation = true;
        if (needCanvas)
            loadingCanvas.SetActive(false);
    }

    public void LoadSceneNetwork(Scenes scene)
    {
        LobbyManager.Instance.DisableOnEnterGame();
        NetworkManager.Singleton.SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Single);
    }

}
