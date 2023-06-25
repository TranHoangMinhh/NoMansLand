using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MinimapController : NetworkBehaviour
{
    public Transform targetToFollow;
    public bool rotateWithTheTarget = true;
}
