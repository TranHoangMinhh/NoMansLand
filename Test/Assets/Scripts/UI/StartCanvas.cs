using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCanvas : MonoBehaviour
{
    [SerializeField] private List<GameObject> disableComponents;
    [SerializeField] private List<GameObject> enableComponents;
    private void Awake()
    {
        foreach (GameObject obj in disableComponents)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in enableComponents)
        {
            obj.SetActive(true);
        }

    }

}
