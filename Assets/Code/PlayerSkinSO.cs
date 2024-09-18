using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkinSO", menuName = "Scriptable Objects/PlayerSkinSO")]
public class PlayerSkinSO : ScriptableObject
{
    [SerializeField] private int _skinPrice;
    [SerializeField] private string _skinName;
    [SerializeField] private Material _skinMaterial;

    public int GetSkinPrice()
    {
        return _skinPrice;
    }
    public string GetSkinName()
    {
        return _skinName;
    }
    public Material GetSkinMaterial()
    {
        return _skinMaterial;
    }
}
