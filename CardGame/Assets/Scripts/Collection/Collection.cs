using System.Collections;
using System.Collections.Generic;
using System.Text;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Collection : MonoBehaviour {

    public Card card;
    public float startTransformX;
    public float startTransformY;
    public float transformX;
    public float transformY;
    
    private FirebaseAuth firebaseAuth;

    public InputField emailInput;
    public InputField passwordInput;
    public GameObject loginButton;
    public GameObject singUpButton;
    
    void Start () {
        Vector3 position = transform.position;
        position.y = position.y + startTransformY;
        position.x = position.x - startTransformX;
        position.z = 0;
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                Card newCard = Instantiate(card, position, card.transform.rotation) as Card;
                newCard.changeSprite(Random.Range(0,3));
                position.x = position.x + transformX;
            }
            position.x = transform.position.x - startTransformX;
            position.y = position.y - transformY;
        }
        /*StartCoroutine(GetText());*/

    }

    /*IEnumerator GetText()
    {
        RegistrationUserForm user = new RegistrationUserForm();
        user.email = "test@mail.com";
        user.password = "123q";
        user.username = "test";
        string jsonToServer = JsonUtility.ToJson(user);
        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/user/registration", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonToServer);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.Send(); 
    }*/
}
