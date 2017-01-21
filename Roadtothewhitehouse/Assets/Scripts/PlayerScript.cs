﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody RigidBody { get; private set; }

    public bool IsGrounded { get; private set; }

    public bool IsDead { get; set; }

    [SerializeField]
    public TrumpManager Manager;

    [SerializeField]
    private Transform SpawnTransform;

    private float MinSpeed;

    // Use this for initialization
    void Start()
    {
        this.RigidBody = GetComponent<Rigidbody>();
        this.IsDead = true;
    }

    public void Spawn()
    {
        this.IsDead = false;
        this.transform.position = SpawnTransform.position;
        this.transform.rotation = SpawnTransform.rotation;
        this.RigidBody.velocity = Vector3.zero;
        this.RigidBody.angularVelocity = Vector3.zero;
        this.MinSpeed = 1.3f;

        lastPathPosition = SpawnTransform.position;
        lastDir = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeed();
    }

    void FixedUpdate()
    {
        if (!IsDead)
        {
            UpdatePathFinding();
            UpdateJump();
            UpdateMovemet();
        }
    }

    private void UpdateSpeed()
    {
        MinSpeed += Time.deltaTime * 0.001f;
    }

    private void OnDeath()
    {
        Manager.OnDeath();
    }

    private void UpdateMovemet()
    {
        if (transform.position.y < -10)
        {
            OnDeath();
        }

        if (!IsGrounded)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Quaternion deltaRotation = Quaternion.Euler(new Vector3(-1, 0, 0));
                // RigidBody.AddRelativeTorque(new Vector3(-10000, 0, 0), ForceMode.VelocityChange);
                RigidBody.MoveRotation(Quaternion.Slerp(RigidBody.rotation, RigidBody.rotation * deltaRotation, Time.deltaTime * 270));
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Quaternion deltaRotation = Quaternion.Euler(new Vector3(1, 0, 0));
                RigidBody.MoveRotation(Quaternion.Slerp(RigidBody.rotation, RigidBody.rotation * deltaRotation, Time.deltaTime * 270));

                //RigidBody.AddRelativeTorque(new Vector3(10000, 0, 0), ForceMode.VelocityChange);
            }
        }
    }

    struct TriangleShit
    {
       public Vector3 dir;
        public Vector3 pos1;
        public Vector3 pos2;
    }

    private void UpdateJump()
    {
        if (IsGrounded && Input.GetKey(KeyCode.Space))
            RigidBody.AddForce(transform.up * 500);
    }

    private Vector3 currentPathPosition;
    private Vector3 lastPathPosition;
    private Vector3 lastDir;
    private void UpdatePathFinding()
    {
        // Keep around head
        Vector3 dir = Vector3.down;
        float length = 100;

        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, dir, out hit, length))
        //{
        //    var fakeForward = hit.collider.transform.forward;
        //    fakeForward.y = 0;

        //    var fakePos = transform.position;
        //    fakePos.y = 0;

        //    var dest = DistanceToLine(hit.collider.transform.position, fakeForward, fakePos);
        //    dest.y = transform.position.y;

        //    RigidBody.MovePosition(Vector3.MoveTowards(RigidBody.position, dest, .5f));


        //    var lookAt = hit.collider.transform.forward;
        //    lookAt.y = 0;
        //    RigidBody.MoveRotation( Quaternion.Slerp(transform.rotation, 
        //        Quaternion.Euler( transform.rotation.eulerAngles.x, Quaternion.LookRotation(lookAt).eulerAngles.y, transform.rotation.eulerAngles.z),
        //          Time.fixedDeltaTime * 100));

        //    IsGrounded = hit.distance < 1;
        //}

        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, length))
        {
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider != null && meshCollider.sharedMesh != null)
            {
                var decorator = hit.collider.GetComponent<SplineDecorator>();
                if (decorator != null)
                {
                    int[] triangles = meshCollider.sharedMesh.triangles;
                    Vector3[] vertices = meshCollider.sharedMesh.vertices;

                    Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
                    Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
                    Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];


                    //TriangleShit[] lol = new TriangleShit[] { new TriangleShit() { dir = p0 - p1, pos1 = p0, pos2 = p1 }
                    //, new TriangleShit() { dir = p2 - p0, pos1 = p2, pos2 = p0 },
                    //    new TriangleShit() { dir = p2 - p1, pos1 = p2, pos2 = p1 } };

                    //var test = lol.OrderBy(x => x.dir.magnitude).ToArray()[1];


                    var averagePos =  (p0 + p1 + p2) / 3f;

                    var fakeForward = (averagePos - lastPathPosition).normalized;
                    fakeForward.y = 0;

                    //if (fakeForward == Vector3.zero)
                    //{
                    //    fakeForward = lastDir;
                    //    fakeForward.y = 0;
                    //}

                    var fakePos = transform.position;
                    fakePos.y = 0;

                    var dest = DistanceToLine(averagePos, fakeForward, fakePos);
                    dest.y = transform.position.y;


                    if (currentPathPosition != averagePos)
                    {
                        lastPathPosition = currentPathPosition;
                        currentPathPosition = averagePos;
                    }

                    Debug.DrawRay(averagePos, -fakeForward);

                    print(averagePos);

                    RigidBody.MovePosition(Vector3.MoveTowards(RigidBody.position, dest, .5f));
                }
            }


            //var lookAt = hit.collider.transform.forward;
            //lookAt.y = 0;
            //RigidBody.MoveRotation(Quaternion.Slerp(transform.rotation,
            //    Quaternion.Euler(transform.rotation.eulerAngles.x, Quaternion.LookRotation(lookAt).eulerAngles.y, transform.rotation.eulerAngles.z),
            //      Time.fixedDeltaTime * 100));

            IsGrounded = hit.distance < 1;
        }

        // Force
        RigidBody.AddForce(transform.forward * MinSpeed, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].thisCollider.GetType() == typeof(BoxCollider))
        {
            OnDeath();
        }
    }

    public Vector3 DistanceToLine(Vector3 origin, Vector3 dir, Vector3 point)
    {
        return UnityEditor.HandleUtility.ProjectPointLine(point, origin - dir * 100, origin + dir * 100);
    }

}