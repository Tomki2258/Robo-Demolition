using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layers : MonoBehaviour
{
    public LayerMask _trashlayer;
    public LayerMask _playerLayer;
    private void Start()
    {
        Physics.IgnoreLayerCollision(_trashlayer.value, _playerLayer.value, true);
    }

    public int GetTrashLayer()
    {
        return _trashlayer;
    }
}
