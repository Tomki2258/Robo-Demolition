using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class tamusUtils
{
    public static float GetDistance(GameObject _one, GameObject _two)
    {
        return Vector3.Distance(_one.transform.position, _two.transform.position);
    }
}
