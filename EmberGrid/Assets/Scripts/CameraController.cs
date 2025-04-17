using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            move += Vector3.up;
        if (Input.GetKey(KeyCode.S))
            move += Vector3.down;
        if (Input.GetKey(KeyCode.A))
            move += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            move += Vector3.right;

        transform.position += move * moveSpeed * Time.deltaTime;
    }
}
