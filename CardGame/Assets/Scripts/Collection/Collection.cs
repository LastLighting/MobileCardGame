using System.Collections;
using System.Text;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Collection : MonoBehaviour {

    public Card card;
    public float startTransformX;
    public float startTransformY;
    public float transformX;
    public float transformY;
    public string[] ids;
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
            for (int x = 0; x < 3; x++)
            {
                Card newCard = Instantiate(card, position, card.transform.rotation) as Card;
                newCard.Id = ids[y*3+x];
                newCard.Strength = Random.Range(1, 20);
                newCard.changeStrength(newCard.Strength);
                newCard.sprites[0] = Resources.Load("sprites/Cards/SmallSize/" + newCard.Id, typeof(Sprite)) as Sprite;
                newCard.sprites[1] = Resources.Load("sprites/Cards/FullSize/" + newCard.Id, typeof(Sprite)) as Sprite;
                newCard.changeSprite(0);
                newCard.transform.position = position;
                newCard.transform.localScale = new Vector3(1.3F, 1.3F, 0);
                   
                position.x = position.x + transformX;
            }
            position.x = transform.position.x - startTransformX;
            position.y = position.y - transformY;
        }
        StartCoroutine(GetText());

    }

    IEnumerator GetText()
    {
        User user = new User();
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
        if(request.isNetworkError) {
            Debug.Log(request.error);
        }
        else
        {
            User[] users = JsonHelper.FromJson<User>(request.downloadHandler.text);
            Debug.Log(users[0].password);
            Debug.Log(users[1].password);
        }
    }
}
