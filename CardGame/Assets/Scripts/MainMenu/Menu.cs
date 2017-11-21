using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public GameObject settingsButton;
	public GameObject playButton;
	public GameObject collectionButton;
	public GameObject optionsBackground;
	public GameObject settingsBackgroundEffect;
    public Transform transform1;
    public Transform transform2;
    public float settingsSpeed;
    float startTime;
    bool open = false;
	bool close = false;
	bool opening = false;
    Vector3 defaultVector1;
    Vector3 defaultVector2;
	public Text successLogin;

    void Start()
    {
        defaultVector1 = transform1.position;
	    var user = PlayerPrefs.GetString("LoginUser", "Unknown");
	    successLogin.text = "Привет, наш альфа-тестер, " + user;
    }

    public void play() {
	
	}

    public void collection() {

        SceneManager.LoadScene("Collection");
    }

	public void options() {
		if (!opening) {
            opening = true;        
			playButton.SetActive (false);
			collectionButton.SetActive (false);
            startTime = Time.timeSinceLevelLoad;
            close = false;
			open = true;
        } else {
            opening = false;
			playButton.SetActive (true);
			collectionButton.SetActive (true);
			optionsBackground.SetActive(false);
			settingsBackgroundEffect.SetActive (false);
            startTime = Time.timeSinceLevelLoad;
            open = false;
			close = true;
			
		}
	}

	void Update() {
        if (settingsButton.transform.position == transform2.position)
        {
            if (opening)
            {
                settingsBackgroundEffect.SetActive(true);
                optionsBackground.SetActive(true);
            }
            open = false;
        }
        if (settingsButton.transform.position == defaultVector1)
        {
            close = false;
        }
        if (open)
        {
            settingsButton.transform.position = Vector3.Lerp(transform1.position, transform2.position, (Time.timeSinceLevelLoad - startTime) * settingsSpeed);        
        }
        if (close)
        {
            settingsButton.transform.position = Vector3.Lerp(transform1.position, defaultVector1, (Time.timeSinceLevelLoad - startTime) * settingsSpeed);
        }
        
    }
}
