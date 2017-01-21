using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour {

    TrumpManager tm;
    Vector3 startPos;
    float time;
    float respawn = 0;

    MeshRenderer mr;


    // Use this for initialization
    void Start () {
        tm = FindObjectOfType<TrumpManager>();
        mr = GetComponentInChildren<MeshRenderer>();
        startPos = transform.position;

    }

    void OnCollisionEnter(Collision collision)
    {
        mr.enabled = false;

    }

    void Update () {
        if(!mr.enabled)
        {
            respawn += Time.deltaTime;
            if(respawn > 7)
            {
                respawn = 0;
                mr.enabled = true;
            }
        }

        time += Time.deltaTime;
        transform.rotation *= Quaternion.Euler(0f, 50f * Time.deltaTime, 0f);
        transform.position = new Vector3(startPos.x, startPos.y * Mathf.Sin(time * 2) * 0.5f, startPos.z);
    }
}
