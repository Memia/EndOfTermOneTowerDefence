using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToObject : MonoBehaviour
{
    public Transform parent;

    // Use this for initialization
    void Start()
    {
        transform.SetParent(parent);
    }
}
