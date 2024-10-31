using System;
using UnityEngine;

public class PoweredEnemyEffect : MonoBehaviour
{
    [SerializeField] private int _rotationSpeed;
    private void FixedUpdate()
    {
        ManagePowerUpEffect();
    }
    private void ManagePowerUpEffect()
    {
        transform.Rotate(0,_rotationSpeed,0);
    }
}
