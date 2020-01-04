using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public Transform target;

    Vector3 velocity = Vector3.zero;        // zero's out velocity
    Vector3 targetPos;

    public float smoothTime = .15f;         // time to follow target

    void FixedUpdate()
    {
        if (target != null)
        {
            targetPos = target.position;            // target position
            targetPos.z = transform.position.z;     // align camera and targets z position

            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime * Time.deltaTime);
        }
    }
}
