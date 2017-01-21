using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveImage : MonoBehaviour {

    public bool activated;

    private float time = 0;
    private float amount = 0.2f;
    private float speed = 3f;
    public RawImage rawimage;
  
    Vector3 rawimagescale;
    Vector3 oldrawimagepos;
    Vector3 oldrawimagescale;
    Quaternion oldrawimagerotation;

	// Use this for initialization
	void Start () {

        this.oldrawimagepos = transform.localPosition;
        this.oldrawimagescale = transform.localScale;
        this.oldrawimagerotation = transform.localRotation;


        activated = true;
        rawimagescale = Vector3.one * 2;//rawimage.transform.localScale;
    }
	
	// Update is called once per frame
	void Update () {

        //Debug options
        //if (Input.GetKeyUp(KeyCode.W) && activated)
        //{
        //    activated = false;
        //}
        //if (Input.GetKeyUp(KeyCode.Q) && !activated)
        //{
        //    activated = true;
        //}

        if (activated)
        {
            time += Time.deltaTime;

            rawimage.transform.localScale = new Vector3(rawimagescale.x + Mathf.Sin(time * speed) * amount, rawimagescale.y + Mathf.Sin(time * speed) * amount, rawimagescale.z);

            rawimage.transform.position = Vector3.Lerp(rawimage.transform.position, new Vector3(Screen.width / 2f, Screen.height / 2f, 0), Time.deltaTime * 0.5f);
            rawimage.transform.rotation = Quaternion.Slerp(rawimage.transform.rotation, Quaternion.identity, Time.deltaTime);
        }

        if (!activated)
        {
            rawimage.transform.localScale = oldrawimagescale;
            rawimage.transform.localPosition = oldrawimagepos;
            rawimage.transform.localRotation = oldrawimagerotation;
        }       
    }

    /// <summary>
    /// Call this function to activate the game over screen
    /// </summary>
    public void activate()
    {
        Start();
    }

    /// <summary>
    /// call this function to reset the game over screen
    /// </summary>
    public void deactivate()
    {
        activated = false;
    }
}
