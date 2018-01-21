using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginButton : MonoBehaviour {
    
    public FirebaseAuth firebaseAuth;
    public Text loginText;
    public GameObject modal;
    public GameObject blur;
    public InputField emailInput;
    public InputField passwordInput;
    public GameObject mainTheme;

    void Start () {
        firebaseAuth = FirebaseAuth.DefaultInstance;
    }

    private IEnumerator changeColor()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color32(0, 0, 0, 255);
        gameObject.GetComponentInChildren<Text>().color = new Color32(0, 0, 0, 255);
        yield return new WaitForSeconds(0.3f);
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
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            loginText.text = "Поля почта или пароль пусты";
            blur.SetActive(true);
            modal.SetActive(true);
            return;
        }

        firebaseAuth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                string err = task.Exception.InnerExceptions[0].Message;
                
                if (err.Contains("There is no user record"))
                {
                    loginText.text = "Неправильная почта или пароль";
                }
                else
                if (err.Contains("The email address is badly formatted."))
                {
                    loginText.text = "Неправильный адрес электронной почты";
                }
                else
                if (err.Contains("The password is invalid"))
                {
                    loginText.text = "Неправильный пароль";
                }
                else
                    loginText.text = "" + task.Exception.InnerExceptions[0].Message;
                blur.SetActive(true);
                modal.SetActive(true);
                return;
            } else {
                FirebaseUser user = task.Result;
                PlayerPrefs.SetString("LoginUser", user != null ? user.Email : "Unknown");
                SceneManager.LoadScene("MainMenu");
                GameObject.DontDestroyOnLoad(mainTheme);
            }
        });
    }
}
