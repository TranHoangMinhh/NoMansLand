using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    // Start is called before the first frame update
    public enum Scene
    {
        Menu,
        Demo
    }

    // Update is called once per frame
    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}
