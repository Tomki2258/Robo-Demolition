using System.Collections;
using UnityEngine;

public class Trash : MonoBehaviour
{
    private Vector3 _targetVector;
    private readonly int _trashDelay = 5;
    public bool _trashMove;
    private MeshCollider _meshCollider;
    private Layers _layers;
    private void Start()
    {
        _layers = FindFirstObjectByType<Layers>();
        _meshCollider = GetComponent<MeshCollider>();
        int LayerName = LayerMask.NameToLayer("Trash");
        gameObject.layer = LayerName;
        StartCoroutine(MoveTrash());
    }

    private void FixedUpdate()
    {
        if (_trashMove && transform.position != _targetVector)
            transform.position = Vector3.Lerp(transform.position, _targetVector, 0.025f);
    }

    private IEnumerator MoveTrash()
    {
        yield return new WaitForSeconds(_trashDelay);
        _targetVector = new Vector3(
            transform.position.x,
            transform.position.y - 3,
            transform.position.z);
        _trashMove = true;
        _meshCollider.enabled = false;
    }
}