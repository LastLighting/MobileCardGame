using System.Collections;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using UnityEngine.Networking;

public class SingUpButton : MonoBehaviour {
	
	private FirebaseAuth firebaseAuth;
	
	public InputField emailInput;
	public InputField passwordInput;
    public Text singUpText;
    public GameObject modal;
    public GameObject blur;
	
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
		singUp(emailInput.text, passwordInput.text);
	}
	
	public void singUp(string email, string password)
	{
		if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
		{
            singUpText.text = "Поля почта или пароль пусты";
            blur.SetActive(true);
            modal.SetActive(true);
            return;
		}

		firebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
		{
			if (task.IsCanceled)
			{
                singUpText.text = "Регистрация отменена";
                blur.SetActive(true);
                modal.SetActive(true);
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
				return;
			}     
            if (task.IsFaulted)
			{
                if (task.Exception.InnerExceptions.Count > 0)
                {
                    string err = task.Exception.InnerExceptions[0].Message;
                    if(err.Contains("The email address is already"))
                    {
                        singUpText.text = "Данный аккаунт уже занят";
                    }
                    else 
                    if(err.Contains("Password should be at least 6 characters")){
                        singUpText.text = "Пароль должен содержать не менее 6 символов";
                    } else 
                    if (err.Contains("The email address is badly formatted.")) {
                        singUpText.text = "Неправильный адрес электронной почты";
                    } else
                        singUpText.text = "" + task.Exception.InnerExceptions[0].Message;

                }                                               
                blur.SetActive(true);
                modal.SetActive(true);
                Debug.LogError("CreateUserWithEmailAndPasswordAsync error: " + task.Exception);        
                return ;
            }

			FirebaseUser newUser = task.Result;
            singUpText.text = "Зарегистрирован аккаунт " + newUser.Email;
			User user = new User();
			user.email = newUser.Email;
			user.uuid = newUser.UserId;
			StartCoroutine(serverRegistration(user));
            blur.SetActive(true);
            modal.SetActive(true);			
		});
	}
	
	IEnumerator serverRegistration(User user)
	{
		string jsonToServer = JsonUtility.ToJson(user);
		UnityWebRequest request = new UnityWebRequest("https://cardgamejavaserver.herokuapp.com/user/registration", "POST");
		byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonToServer);
		request.uploadHandler = new UploadHandlerRaw(bodyRaw);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		yield return request.Send(); 
		if(request.isNetworkError) {
			singUpText.text = "Регистрация отменена. Ошибка сервера";
		}
	}
}
