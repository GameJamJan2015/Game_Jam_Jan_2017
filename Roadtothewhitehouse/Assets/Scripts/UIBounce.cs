using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBounce : MonoBehaviour {

    private Vector3 rawimagescale;
    private float time = 0;
    public float amount = 0.2f;
    public float speed = 3f;
    private Text rawimage;

    // Use this for initialization
    void Start () {
        rawimage = GetComponent<Text>();
        rawimagescale = rawimage.transform.localScale;
	}

    void OnEnable()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update () {
        time += Time.deltaTime;

        rawimage.transform.localScale = new Vector3(rawimagescale.x + Mathf.Sin(time * speed) * amount, rawimagescale.y + Mathf.Sin(time * speed) * amount, rawimagescale.z);
    }
}
