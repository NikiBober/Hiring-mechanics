using UnityEngine;

public class FreezeRotation : MonoBehaviour
{
    private Quaternion _rotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = _rotation;
    }
}
