using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct PlayerData : IEquatable<PlayerData>, INetworkSerializable {


    public ulong clientId;
    public FixedString64Bytes playerName;
    public FixedString64Bytes playerId;


    //In game
    public int skinId;
    public int sideWeaponId;
    public int killNumber;
    public int diedNumber;
    


    public bool Equals(PlayerData other) {
        return 
            clientId == other.clientId &&
            playerId == other.playerId &&
            playerName == other.playerName &&
            skinId == other.skinId &&
            sideWeaponId == other.sideWeaponId &&
            killNumber == other.killNumber &&
            diedNumber == other.diedNumber;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref playerId);

        serializer.SerializeValue(ref skinId);
        serializer.SerializeValue(ref sideWeaponId);
        serializer.SerializeValue(ref killNumber);
        serializer.SerializeValue(ref diedNumber);

    }

}