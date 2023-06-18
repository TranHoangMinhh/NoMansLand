using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame(int sceneId)
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        LoadingManager.Instance.LoadScene(sceneId);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!!");
        Application.Quit();
    }
}
