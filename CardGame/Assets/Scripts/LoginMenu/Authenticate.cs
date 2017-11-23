using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;

public class Authenticate : MonoBehaviour {

    public FirebaseAuth firebaseAuth;

    public InputField emailInput;
    public InputField passwordInput;
    public GameObject loginButton;
    public GameObject singUpButton;

	void Start () {
	    firebaseAuth = FirebaseAuth.DefaultInstance;
	}
}
