using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseObjectInfo : MonoBehaviour
{

    [SerializeField] private LobbyManager.PlayerCharacter characterType;
    [SerializeField] private int characterIndex;

    public LobbyManager.PlayerCharacter GetCharacterType() { return characterType; }
    public int GetCharacterIndex() { return characterIndex; }

}
