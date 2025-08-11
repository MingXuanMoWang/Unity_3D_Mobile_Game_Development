using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    public static CameraPoint Instance = null;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "CameraPoint.tif");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
