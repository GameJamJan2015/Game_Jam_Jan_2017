using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    public TrumpManager Manager;

    [SerializeField]
    private PlayerScript Player;

    [SerializeField]
    private Transform TitleTransform;

    [SerializeField]
    private Transform centerTrump;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.State == TrumpManager.GameState.GAME)
        {
            float zoom = Mathf.Max(10, Player.RigidBody.velocity.magnitude * 0.5f);

            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(centerTrump.position - transform.position) , Time.deltaTime * 4);

            transform.position = Vector3.Lerp(transform.position, 
                (zoom * (Player.transform.position - centerTrump.position).normalized) + Player.transform.position, Time.deltaTime * 13);
        }
        else //if (Manager.State == TrumpManager.GameState.MENU)
        {
            transform.position = Vector3.Lerp(transform.position, TitleTransform.position, Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, TitleTransform.rotation, Time.deltaTime);
        }
    }
}
