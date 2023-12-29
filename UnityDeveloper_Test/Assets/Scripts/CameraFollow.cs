using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private const float YMin = -50.0f;
    private const float YMax = 50.0f;

    public Transform lookAt;
    public float distance = 10.0f;
    public float currentX;
    public float currentY;
    public float currentZ;
    public float sensivity = 4.0f;

    void LateUpdate()
    {
        currentX += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        currentY += Input.GetAxis("Mouse Y") * sensivity * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, YMin, YMax);

        Quaternion rotation = Quaternion.Euler(-currentY, currentX, currentZ);

        Vector3 direction = rotation * new Vector3(0, 0, -distance);
        Vector3 desiredPosition = lookAt.position + direction;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * sensivity);

        transform.rotation = rotation;
    }
}