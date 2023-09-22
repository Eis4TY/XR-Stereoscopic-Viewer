using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMove : MonoBehaviour
{
    public Transform head;
    [Space]
    [Header("Position")]
    public bool position_x = true;
    public bool position_y = true;
    public bool position_z = true;
    [Space]
    [Header("Rotation")]
    public bool rotation = true;
    public bool forceVertical = true;
    [Space]
    [Header("FollowMode")]
    public bool lazyMove = false;
    public float moveSpeed = 0.1f;
    
    [Space]
    public bool lazyRotate = false;
    public float rotationSpeed = 10.0f;
    
    private Vector3 targetPos;
    private Vector3 currentMoveVelocity;
    void Update()
    {
        targetPos.x = position_x ? head.position.x : transform.position.x;
        targetPos.y = position_y ? head.position.y : transform.position.y;
        targetPos.z = position_z ? head.position.z : transform.position.z;

        if (lazyMove)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref currentMoveVelocity, moveSpeed);
        }
        else
        {
            transform.position = targetPos;
        }

        if (rotation & position_x & position_x)
        {
            if (lazyRotate)
            {
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, head.rotation, rotationSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, head.rotation, rotationSpeed * Time.deltaTime);

            }
            else
            {
                transform.eulerAngles = new Vector3(head.eulerAngles.x, head.eulerAngles.y, head.eulerAngles.z);
            }
            if (forceVertical)
            {
                transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);

            }
        }
    }
}
