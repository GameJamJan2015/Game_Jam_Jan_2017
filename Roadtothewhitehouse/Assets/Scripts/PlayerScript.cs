using System;
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
        this.RigidBody.isKinematic = true;
        this.IsDead = true;
    }

    public void Spawn()
    {
        this.RigidBody.isKinematic = false;
        this.IsDead = false;
        this.transform.position = SpawnTransform.position;
        this.transform.rotation = SpawnTransform.rotation;
        this.RigidBody.velocity = Vector3.zero;
        this.RigidBody.angularVelocity = Vector3.zero;
        this.MinSpeed = 0.05f;

        lastPathPosition = SpawnTransform.position - transform.forward * 1;
        currentDir = transform.forward;

        //RigidBody.AddForce(currentDir * 1, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDead)
        {
            UpdateJump();
            UpdateSpeed();
        }
    }

    void FixedUpdate()
    {
        if (!IsDead)
        {
            UpdatePathFinding();
            UpdateMovemet();
        }
    }

    private void UpdateSpeed()
    {
        MinSpeed += Time.deltaTime * 0.001f;

        var localVel = transform.InverseTransformDirection(RigidBody.velocity);
        if (localVel.magnitude < 1)
        {
            var addVel = transform.forward;
            addVel.y = 0;
            RigidBody.AddForce(addVel * MinSpeed, ForceMode.Acceleration);
        }
    }

    public void Kill()
    {
        this.RigidBody.isKinematic = true;
        Manager.OnDeath();
    }

    private void UpdateMovemet()
    {
        if (transform.position.y < -60)
        {
            Kill();
        }

        if (!IsGrounded)
        {
            if (Input.GetButton("RotateLeft"))
            {
                //Quaternion deltaRotation = Quaternion.Euler(new Vector3(-180, 0, 0));
                // RigidBody.AddRelativeTorque(new Vector3(-10000, 0, 0), ForceMode.VelocityChange);
                //RigidBody.MoveRotation(Quaternion.Slerp(RigidBody.rotation, RigidBody.rotation * deltaRotation, Time.deltaTime * 270));
                transform.Rotate(-10, 0, 0, Space.Self);
                //RigidBody.AddRelativeTorque(new Vector3(-5000, 0, 0), ForceMode.VelocityChange);
            }
            else if (Input.GetButton("RotateRight"))
            {
              //  transform.Rotate(10, 0, 0, Space.Self);

                transform.rotation *= Quaternion.Euler(new Vector3(10, 0, 0)); 
                // Quaternion deltaRotation = Quaternion.Euler(new Vector3(180, 0, 0));
                // RigidBody.MoveRotation(Quaternion.Slerp(RigidBody.rotation, RigidBody.rotation * deltaRotation, Time.deltaTime * 270));
                //
              //  RigidBody.AddRelativeTorque(new Vector3(5000, 0, 0), ForceMode.VelocityChange);
            }
            else if (Input.GetButton("Dash"))
            {
                RigidBody.AddForce(transform.up * -800 * Time.deltaTime, ForceMode.Impulse);
            }


            //var lookAt = currentDir;
            //lookAt.y = 0;
            //var newY = Quaternion.LookRotation(lookAt).eulerAngles.y;
            //transform.localRotation = (Quaternion.Slerp(transform.rotation,
            //      Quaternion.Euler(transform.localRotation.eulerAngles.x, newY, transform.localRotation.eulerAngles.z), Time.deltaTime * 100));
        }
        else
        {
            //transform.rotation = Quaternion.Slerp(transform.rotation,
            //            Quaternion.LookRotation(currentDir), Time.deltaTime * 20);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(currentDir), 1f);

        //print(IsGrounded);
    }

    private void UpdateJump()
    {
        if (IsGrounded && Input.GetButtonDown("Jump"))
        {
            RigidBody.AddForce(Vector3.up * 800);
        }
    }

    private Vector3 currentPathPosition;
    private Vector3 lastPathPosition;
    private Vector3 currentDir;
    private void UpdatePathFinding()
    {
        // Keep around head
        Vector3 dir = Vector3.down;
        float length = 1000;

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

                    var averagePos = decorator.GetCenterFromVertexIndex(triangles[hit.triangleIndex * 3 + 0]);

                    var fakeForward = (averagePos - lastPathPosition).normalized;
                   

                    if ( Vector3.Dot(currentDir, (fakeForward)) < 0)
                    {
                        fakeForward = -fakeForward;
                    }

                    Debug.DrawRay(averagePos, fakeForward);

                    if (currentPathPosition != averagePos && fakeForward != Vector3.zero)
                    {
                        lastPathPosition = currentPathPosition;
                        currentPathPosition = averagePos;
                        currentDir = fakeForward;
                    }

                    fakeForward = currentDir;
                    fakeForward.y = 0;





                    var fakePos = transform.position;
                    fakePos.y = averagePos.y;


                    var dest = DistanceToLine(averagePos, fakeForward.normalized, fakePos);
                    dest.y = transform.position.y;

                    RigidBody.MovePosition(new Vector3(dest.x, transform.position.y, dest.z));

                    Debug.Log(dest + " | " + transform.position);
                    Debug.DrawLine(fakePos, transform.position, Color.yellow);
                  //  Debug.DrawRay(transform.position, (transform.position - dest).normalized * 4f, Color.red, 0f);
                    // transform.position = Vector3.MoveTowards(RigidBody.position, dest, 5f);
                }
            }
            IsGrounded = hit.distance < 1.2f;
        }
        else
        {
            IsGrounded = false;
        }


        // Force
        var addVel = transform.forward;
        addVel.y = 0;
        RigidBody.AddForce(addVel * MinSpeed, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].thisCollider.GetType() == typeof(BoxCollider))
        {
            if (collision.collider.tag != "GameObject")
            {
                Kill();
            }
        }
    }

    public Vector3 DistanceToLine(Vector3 origin, Vector3 dir, Vector3 point)
    {
        return UnityEditor.HandleUtility.ProjectPointLine(point, origin - dir * 100, origin + dir * 100);
    }

}