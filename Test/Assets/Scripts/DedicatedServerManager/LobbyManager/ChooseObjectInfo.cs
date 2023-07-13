using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseObjectInfo : MonoBehaviour
{

    [SerializeField] private LobbyManager.PlayerCharacter characterType;
    [SerializeField] private LobbyManager.SideWeapon sideWeapon;
    [SerializeField] private int index;

    public LobbyManager.PlayerCharacter GetCharacterType() { return characterType; }
    public LobbyManager.SideWeapon GetSideWeapon() { return sideWeapon; }
    public int GetIndex() { return index; }

}
