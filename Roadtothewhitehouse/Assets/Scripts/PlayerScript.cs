using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody RigidBody { get; private set; }

    public bool IsGrounded { get; private set; }

    // Use this for initialization
    void Start()
    {
        this.RigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        UpdateMovementPath();
    }

    private void UpdateMovementPath()
    {
        // Force
        RigidBody.AddForce(transform.forward * 100);

        // Keep around head
        Vector3 dir = Vector3.down;
        float length = 100;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, length))
        {
            var dest = DistanceToLine(hit.collider.transform.position, hit.collider.transform.forward, transform.position - transform.up * 1);
            dest.y = transform.position.y;

            RigidBody.MovePosition(Vector3.MoveTowards(RigidBody.position, dest, .05f));

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(hit.collider.transform.forward), Time.fixedDeltaTime * 4);

            IsGrounded = hit.distance < 1;
        }
    }

    public Vector3 DistanceToLine(Vector3 origin, Vector3 dir, Vector3 point)
    {
        return UnityEditor.HandleUtility.ProjectPointLine(point, origin - dir * 100, origin + dir * 100);
    }

    private Vector3 GetDistPointToLine(Vector3 origin, Vector3 direction, Vector3 point)
    {
        Vector3 point2origin = origin - point;
        Vector3 point2closestPointOnLine = point2origin - Vector3.Dot(point2origin, direction) * direction;
        return point2closestPointOnLine;
    }
}