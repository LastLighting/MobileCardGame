using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PhotonConnection : Photon.MonoBehaviour
{
	private GameObject mineDeck;
	
	void Start () 
	{ 
		if (PhotonNetwork.connectionState == ConnectionState.Disconnected) 
		{ 
			try 
			{ 
				PhotonNetwork.ConnectUsingSettings("CardGame 0.0.1"); 
				PhotonNetwork.autoJoinLobby = true; 
			} 
			catch (Exception e) 
			{ 
				Debug.Log("Не удалось подключиться к серверу"); 
			} 
		} 
		else 
		{ 
			Debug.Log("Успешное подключение к серверу"); 
		} 
	} 

	void OnJoinedLobby() 
	{ 
		PhotonNetwork.JoinOrCreateRoom("testRoom", new RoomOptions(), TypedLobby.Default);
		PhotonNetwork.playerName = PlayerPrefs.GetString("LoginUser", "Unknown");


		/*var connectedRoom = PhotonNetwork.GetRoomList().ToList().FindAll(f => f.MaxPlayers > f.PlayerCount).FirstOrDefault();
		RoomOptions roomOptions = new RoomOptions() { IsVisible = true, MaxPlayers = 2 };
		if (connectedRoom != null)
		{
			PhotonNetwork.JoinOrCreateRoom(connectedRoom.Name, roomOptions, TypedLobby.Default);
			/*PhotonNetwork.CreateRoom("GameRoom", roomOptions, TypedLobby.Default);#1#
			GameObject.Find("Debug").GetComponent<Text>().text = "Подключение к комнате" + connectedRoom.Name;
		}
		else
		{
			PhotonNetwork.JoinOrCreateRoom("CardGame" + Guid.NewGuid().ToString("N"), roomOptions, TypedLobby.Default);
			/*PhotonNetwork.JoinRoom(connectedRoom.Name);#1#
			GameObject.Find("Debug").GetComponent<Text>().text = "Комната создана";
		}*/
	} 
 
	public void OnJoinedRoom() 
	{ 
		GameObject mineHand = PhotonNetwork.Instantiate("CardHand", new Vector3(0, -4.1f), transform.rotation, 0);
		mineHand.gameObject.transform.parent = GameObject.Find("Canvas").transform;
		mineHand.name = "CardHand" + PlayerPrefs.GetString("LoginUser", "Unknown");
		if (PhotonNetwork.player.UserId == PhotonNetwork.playerList[0].UserId)
		{
			mineHand.GetComponent<HandPhoton>().isAvailable = false;
		}
		else
		{
			mineHand.GetComponent<HandPhoton>().isAvailable = true;
		}
		
		mineDeck = PhotonNetwork.Instantiate("Deck", new Vector3(7, -3.2f), transform.rotation, 0);
		mineDeck.transform.parent = GameObject.Find("NumberCanvas").transform;
		
		User user = new User();
		user.email = PlayerPrefs.GetString("LoginUser", "Unknown");
		
		DeckBean deckBean = new DeckBean();
		deckBean.user = user;
		deckBean.name = PlayerPrefs.GetString("DeckName", "Unknown");
		
		GameObject mineScore = PhotonNetwork.Instantiate("ScoreBox", new Vector3(-7.5f, -1.1f), transform.rotation, 0);
		mineScore.name = "Score" + PlayerPrefs.GetString("LoginUser", "Unknown");
		mineScore.transform.parent = GameObject.Find("NumberCanvas").transform;
		
		StartCoroutine(getUserDeck(deckBean));
	} 
	
	IEnumerator getUserDeck(DeckBean deckBean)
	{
		string jsonToServer = JsonUtility.ToJson(deckBean);
		UnityWebRequest request = new UnityWebRequest("https://cardgamejavaserver.herokuapp.com/deck/userDeck", "POST");
		byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonToServer);
		request.uploadHandler = new UploadHandlerRaw(bodyRaw);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		yield return request.Send();
		if (!request.isNetworkError)
		{            
			DeckBean deck = JsonUtility.FromJson<DeckBean>(request.downloadHandler.text);
			Vector3 pos = transform.position;
			pos.y = pos.y - 4f;
			pos.x = pos.x - 3.5f;
			int count = 0;
			Card leader = PhotonNetwork.Instantiate("Card", transform.position, transform.rotation, 0).GetComponent<Card>();
			leader.GetComponent<CardPhoton>().isVisible = true;
			leader.isLeader = true;
			leader.Id = deck.leader.id;
			leader.name = deck.leader.name;
			leader.Strength = deck.leader.strength;
			leader.changeStrength(leader.Strength);
			leader.sprites[0] = Resources.Load("sprites/Cards/SmallSize/" + leader.Id, typeof(Sprite)) as Sprite;
			leader.changeSprite(0);
			leader.transform.localScale = new Vector3(1F, 1F, 0);
			leader.transform.position = new Vector3(-7.3F, -3.5F, -1);
			
			deck.cards = UtilityHelper.RandomizeList(deck.cards);
			List<CardBean> cardsForHand = deck.cards.GetRange(0, 10);
			List<CardBean> cardsForDeck = deck.cards.GetRange(10, deck.cards.Count - 10);
			mineDeck.GetComponent<DeckPhoton>().cardsInDeck = cardsForDeck;
			foreach (CardBean card in cardsForHand)
			{
				Card cardForGame = PhotonNetwork.Instantiate("Card", transform.position, transform.rotation, 0).GetComponent<Card>();
				cardForGame.name = card.id;
				cardForGame.gameObject.AddComponent<CardOnBoard>();
				cardForGame.Id = card.id;
				cardForGame.Strength = card.strength;
				cardForGame.changeStrength(cardForGame.Strength);
				cardForGame.sprites[0] = Resources.Load("sprites/Cards/SmallSize/" + cardForGame.Id, typeof(Sprite)) as Sprite;
				cardForGame.sprites[1] = Resources.Load("sprites/Cards/FullSize/" + cardForGame.Id, typeof(Sprite)) as Sprite;
				cardForGame.changeSprite(0);
				cardForGame.transform.localScale = new Vector3(0.6F, 0.6F, 0);
				cardForGame.transform.parent = GameObject.Find("CardHand" + PlayerPrefs.GetString("LoginUser", "Unknown")).transform;
				count += 1;
				cardForGame.transform.Find("Strenge").GetComponent<SpriteRenderer>().sortingOrder = count + 1;
				cardForGame.transform.position = pos;
				pos.z -= 0.1f;
				pos.x += 0.8f;
				cardForGame.GetComponentInChildren<SpriteRenderer>().sortingOrder = count;
			}
		}
	}
}