using System.Collections;
using UnityEngine;

public class Trash : MonoBehaviour
{
    private Vector3 _targetVector;
    private readonly int _trashDelay = 3;
    private bool _trashMove;

    private void Start()
    {
        _targetVector = new Vector3(
            transform.position.x,
            transform.position.y - 3,
            transform.position.z);

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
        _trashMove = true;
    }
}