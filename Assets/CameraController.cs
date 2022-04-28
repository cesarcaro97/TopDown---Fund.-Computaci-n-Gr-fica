using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float followSpeed = 1;

    private Transform target = null;


    private void LateUpdate()
    {
        if (target)
            transform.position = Vector3.Lerp(transform.position, target.position + Vector3.forward * transform.position.z, followSpeed * Time.deltaTime);
    }


    internal void SetUp(int width, int height, bool closeLook = false)
    {
        Camera.main.orthographicSize = closeLook ? 2 : 6;
        
        var centerPos = new Vector3(width / 2, height / 2, transform.position.z);
        transform.position = centerPos;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
