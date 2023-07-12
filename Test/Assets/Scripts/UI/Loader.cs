using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public static class Loader
{
    // Start is called before the first frame update
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        CharacterSelectScene,
        ChooseScene,
        LobbyScene,

    }

    private static Scene targetScene;

    public static void LoadNetwork(Scene targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }

    // Update is called once per frame
    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;
        SceneManager.LoadScene(targetScene.ToString());
    }
}
