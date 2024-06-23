using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateAppCanvas : MonoBehaviour
{
    public void RateApp()
    {
        Application.OpenURL ("market://details?id=" + Application.productName);
    }
}
