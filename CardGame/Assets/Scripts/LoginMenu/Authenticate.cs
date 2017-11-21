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


/*
 
    

    public void login(string email, string password)
    {
        firebaseAuth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync error: " + task.Exception);
                if (task.Exception.InnerExceptions.Count > 0)
                    UpdateErrorMessage(task.Exception.InnerExceptions[0].Message);
                return;
            }

            FirebaseUser user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);
            Debug.Log("11");
            //PlayerPrefs.SetString("LoginUser", user != null ? user.Email : "Unknown");
            SceneManager.LoadScene(1);
        });
    }
    
}
*/
