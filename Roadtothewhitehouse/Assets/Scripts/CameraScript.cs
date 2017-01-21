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

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.State == TrumpManager.GameState.GAME)
        {
            float zoom = Mathf.Max( 9, Player.RigidBody.velocity.magnitude);

            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(Player.transform.position - transform.position) , Time.deltaTime * 4);

            transform.position = Vector3.Lerp(transform.position, 
                Player.transform.position + Player.transform.right * zoom , Time.deltaTime * 13);
        }
        else if (Manager.State == TrumpManager.GameState.MENU)
        {
            transform.position = Vector3.Lerp(transform.position, TitleTransform.position, Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, TitleTransform.rotation, Time.deltaTime);
        }
    }
}
