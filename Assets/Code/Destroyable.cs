using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public float _fallingSpeed;
    private Vector3 _collapseVector;
    private float _height;
    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _height = _renderer.bounds.size.y;

        _collapseVector = new Vector3(
            transform.position.x,
            transform.position.y - _height,
            transform.position.z);
    }

    public Vector3 GetCollabseVector()
    {
        return _collapseVector;
    }
}