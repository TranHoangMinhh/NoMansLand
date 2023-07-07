using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputfieldBehavior : MonoBehaviour
{

    [SerializeField] private GameObject inputFieldOutline;
    [SerializeField] private TMP_InputField inputField;


    private void Update()
    {
        // Set active for input field outline
        if (inputField.isFocused)
        {
            inputFieldOutline.SetActive(true);
        }
        
        if (!inputField.isFocused)
        {
            inputFieldOutline.SetActive(false);
        }
    }

}
