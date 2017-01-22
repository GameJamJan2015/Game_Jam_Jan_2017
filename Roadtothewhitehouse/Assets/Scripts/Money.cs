using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour {

    TrumpManager tm;
    BoxCollider bc;
    Vector3 startPos;
    float time;
    float respawn = 0;

    MeshRenderer mr;
    ParticleSystem ps;


    // Use this for initialization
    void Start () {
        tm = FindObjectOfType<TrumpManager>();
        mr = GetComponentInChildren<MeshRenderer>();
        bc = GetComponent<BoxCollider>();
        ps = GetComponent<ParticleSystem>();
        startPos = transform.position;

    }

    void OnTriggerEnter(Collider collision)
    {
        mr.enabled = false;
        bc.enabled = false;
        tm.AddMoney(5000);
        ps.Play();
    }

    void Update () {
        if(!mr.enabled)
        {
            respawn += Time.deltaTime;
            if(respawn > 7)
            {
                respawn = 0;
                mr.enabled = true;
                bc.enabled = true;
            }
        }

        time += Time.deltaTime;
        transform.rotation *= Quaternion.Euler(0f, 50f * Time.deltaTime, 0f);
        //transform.position = new Vector3(startPos.x, startPos.y * Mathf.Sin(time * 2) * 0.5f, startPos.z);
    }
}
