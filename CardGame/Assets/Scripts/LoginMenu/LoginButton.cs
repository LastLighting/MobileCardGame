using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginButton : MonoBehaviour {
    
    public FirebaseAuth firebaseAuth;

    public InputField emailInput;
    public InputField passwordInput;
 
    void Start () {
        firebaseAuth = FirebaseAuth.DefaultInstance;
    }

    private IEnumerator changeColor()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color32(123, 65, 193, 255);
        gameObject.GetComponentInChildren<Text>().color = new Color32(123, 65, 193, 255);
        yield return new WaitForSeconds(1);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color32(163, 163, 163, 255);
        gameObject.GetComponentInChildren<Text>().color = Color.white;
    }

    public void OnMouseDown()
    {
        StartCoroutine(changeColor());
    }

    public void OnMouseUp()
    {
        login(emailInput.text, passwordInput.text);
    }
    
    public void login(string email, string password)
    {
        firebaseAuth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                return;
            }
            else
            {
                FirebaseUser user = task.Result;
                PlayerPrefs.SetString("LoginUser", user != null ? user.Email : "Unknown");
                SceneManager.LoadScene("MainMenu");
            }
        });
    }
}
