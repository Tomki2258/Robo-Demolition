using System.Collections.Generic;
using UnityEngine;

public class PlayerDemolition : MonoBehaviour
{
    public Renderer _playerRenderer;
    public float _playerHeight;
    private readonly List<GameObject> _destroyedObjects = new();

    private void FixedUpdate()
    {
        CollabseObjects();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroyable"))
        {
            Debug.Log("Destroyable object");
            if (CheckDestroy(other.gameObject))
                if (!_destroyedObjects.Contains(other.gameObject))
                {
                    other.tag = "Destroyed";
                    other.gameObject.isStatic = false;
                    _destroyedObjects.Add(other.gameObject);
                }
        }
    }

    public void UpdatePlayerSize()
    {
        _playerHeight = _playerRenderer.bounds.size.y * 2 + 4;
    }

    private void CollabseObjects()
    {
        foreach (var gobj in _destroyedObjects)
        {
            var _destroyable = gobj.GetComponent<Destroyable>();
            destoryableType _type = _destroyable._type;
            if(_type == destoryableType.NONE)
            {
                if (Vector3.Distance(_destroyable.transform.position, _destroyable.GetCollabseVector()) > 0.2f)
                    _destroyable.transform.position = Vector3.Lerp(
                        new Vector3(
                            _destroyable.transform.position.x + Mathf.Sin(Time.time * 50) * 0.03f,
                            _destroyable.transform.position.y,
                            _destroyable.transform.position.z + +Mathf.Sin(Time.time * 50) * 0.03f),
                        _destroyable.GetCollabseVector(), _destroyable._fallingSpeed * Time.deltaTime);
            }
            else if(_type == destoryableType.Falling)
            {
                if(!_destroyable.CheckHorizontal())
                {
                    _destroyable._fallingSpeed += Time.deltaTime * 2;
                    gobj.transform.RotateAround(gobj.transform.position,
                        _destroyable.GetCollabseVector()
                        , _destroyable._fallingSpeed);
                }
                else
                {
                    _destroyable.PlayFallingSound();
                    _destroyedObjects.Remove(gobj);
                }
            }
        }
    }

    private bool CheckDestroy(GameObject _object)
    {
        var _objectRenderer = _object.GetComponent<Renderer>();
        var sizeY = _objectRenderer.bounds.size.y;
        if (sizeY < _playerHeight) return true;
        return false;
    }
}