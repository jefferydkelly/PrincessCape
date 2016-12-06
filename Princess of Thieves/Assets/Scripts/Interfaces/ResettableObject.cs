using UnityEngine;
using System.Collections;

public class ResettableObject: JDMappableObject {
    protected Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }
    public virtual void Reset()
    {
        transform.position = startPosition;
    }
}
