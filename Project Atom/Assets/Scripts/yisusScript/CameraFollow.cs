using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float height;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float angle;
    [SerializeField]
    private float smoothTime;
    [SerializeField]
    private bool lookTarget;
    [SerializeField]
    private Joystick joyShoot;
    [SerializeField]
    private float joyDistance;
    private Vector3 moveCameraSpeed;

    private float minHeight;
    private float maxHeight;
    private float minDistance;
    private float maxDistance;

    // Update is called once per frame
    private void Start()
    {
        MoveCamera();
        minHeight = height;
        maxHeight = height + 10f;
        minDistance = distance;
        maxDistance = 1f;
    }

    void FixedUpdate()
    {

        MoveCamera();

    }

    void MoveCamera()
    {
        if (!target) return;

        Vector3 worldPosition = (Vector3.forward * -distance) + (Vector3.up * height);

        Vector3 rotateVector = (Quaternion.AngleAxis(angle, Vector3.up) * worldPosition);

        Vector3 joyPos = new Vector3(joyShoot.Horizontal, 0, joyShoot.Vertical) * joyDistance;

        Vector3 flatTargetPos = target.position;
        flatTargetPos.y = 0f;

        Vector3 finalPos = flatTargetPos + joyPos + rotateVector;


        transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref moveCameraSpeed, smoothTime);

        if(lookTarget)transform.LookAt(flatTargetPos);

    }

    public void CameraZoomOut()
    {
      /*
      height = Mathf.Lerp(minHeight, maxHeight, Time.deltaTime * 2f);
      distance = Mathf.Lerp(minDistance, maxDistance, Time.deltaTime * 2f);
      */
      height = maxHeight;
      distance = maxDistance;
    }

    public void CameraZoomIn()
    {
      height = minHeight;
      distance = minDistance;
    }
}
