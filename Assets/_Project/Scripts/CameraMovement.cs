using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraMovement : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCamera;
    public float MoveSpeed;
    public float ScrollSpeed;

    private void Update()
    {
        Vector3 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.position += direction.normalized * MoveSpeed;
        VirtualCamera.m_Lens.OrthographicSize += Input.GetAxis("Mouse ScrollWheel") * -ScrollSpeed;
    }
}
