using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraMovement : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCamera;
    public float MoveSpeed = 0.2f;
    public float ScrollSpeed = 1f;

    private void Awake()
    {
        if (VirtualCamera == null)
        {
            VirtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
            if (VirtualCamera == null)
            {
                Debug.LogError("No VirtualCamera found in the scene", this);
            }
        }
    }

    private void Update()
    {
        Vector3 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.position += direction.normalized * MoveSpeed;
        VirtualCamera.m_Lens.OrthographicSize += Input.GetAxis("Mouse ScrollWheel") * -ScrollSpeed;
    }
}
