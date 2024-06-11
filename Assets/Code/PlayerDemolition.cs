using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDemolition : MonoBehaviour
{
    public Renderer _playerRenderer;
    public float _playerHeight = 0;
    private List<GameObject> _destroyedObjects = new List<GameObject>();
    public void UpdatePlayerSize()
    {
        _playerHeight = _playerRenderer.bounds.size.y * 3;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroyable"))
        {
            Debug.Log("Destroyable object");
            if (CheckDestroy(other.gameObject))
            {
                if(!_destroyedObjects.Contains(other.gameObject))
                {
                    other.tag = "Destroyed";
                    other.gameObject.isStatic = false;
                    _destroyedObjects.Add(other.gameObject);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        CollabseObjects();
    }

    private void CollabseObjects()
    {
        foreach (GameObject gobj in _destroyedObjects)
        {   
            Destroyable _destroyable = gobj.GetComponent<Destroyable>();
            if (Vector3.Distance(_destroyable.transform.position,_destroyable.GetCollabseVector()) > 0.2f)
            {
                _destroyable.transform.position = Vector3.Lerp(
                    new Vector3(
                        _destroyable.transform.position.x + Mathf.Sin(Time.time * 50 ) * 0.03f,
                        _destroyable.transform.position.y,
                        _destroyable.transform.position.z +  + Mathf.Sin(Time.time * 50 ) * 0.03f), _destroyable.GetCollabseVector(), _destroyable._fallingSpeed * Time.deltaTime);
            }
        }
    }
    private bool CheckDestroy(GameObject _object)
    {
        Renderer _objectRenderer = _object.GetComponent<Renderer>();
        float sizeY = _objectRenderer.bounds.size.y;
        if (sizeY < _playerHeight) return true;
        return false;
    }
}
