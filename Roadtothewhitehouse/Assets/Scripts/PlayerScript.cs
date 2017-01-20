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
        UpdatePathFinding();
        UpdateJump();
        UpdateMovemet();
    }

    private void UpdateMovemet()
    {
        //if (!IsGrounded)
        {
            Quaternion deltaRotation = Quaternion.Euler(new Vector3(360, 0, 0) * Time.deltaTime);

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                RigidBody.AddTorque(new Vector3(10000, 0, 0), ForceMode.VelocityChange);
               // RigidBody.MoveRotation(deltaRotation* transform.localRotation );
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                RigidBody.MoveRotation(RigidBody.rotation * deltaRotation);
            }
        }
    }

    private void UpdateJump()
    {
        if (IsGrounded && Input.GetKey(KeyCode.Space))
            RigidBody.AddForce(transform.up * 500);
    }

    private void UpdatePathFinding()
    {
        // Keep around head
        Vector3 dir = Vector3.down;
        float length = 100;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, length))
        {
            var fakeForward = hit.collider.transform.forward;
            fakeForward.y = 0;

            var fakePos = transform.position;
            fakePos.y = 0;

            var dest = DistanceToLine(hit.collider.transform.position, fakeForward, fakePos);
            dest.y = transform.position.y;

            RigidBody.MovePosition(Vector3.MoveTowards(RigidBody.position, dest, .5f));



            var lookAt = hit.collider.transform.forward;
            lookAt.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAt), Time.fixedDeltaTime * 10);

            IsGrounded = hit.distance < 1;
        }

        // Force
        RigidBody.AddForce(transform.forward * 80);
    }

    public Vector3 DistanceToLine(Vector3 origin, Vector3 dir, Vector3 point)
    {
        return UnityEditor.HandleUtility.ProjectPointLine(point, origin - dir * 100, origin + dir * 100);
    }

}