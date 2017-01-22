using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

    private float time;

	//// Use this for initialization
	//void Start () {
		
	//}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if (time >= 6.8f)
        {
            SceneManager.LoadScene(1);
        }
	}
}
