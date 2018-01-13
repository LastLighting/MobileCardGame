using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecksController : MonoBehaviour
{
    public Deck deck;
    public float startTransformX;
    public float startTransformY;
    public float transformX;
    public string[] ids;
    void Start()
    {
        Vector3 position = transform.position;
        position.y = position.y + startTransformY;
        position.x = position.x - startTransformX;
        position.z = 0;
        for (int x = 0; x < 3; x++)
        {
            Deck newDeck = Instantiate(deck, position, deck.transform.rotation) as Deck;
            Card card = new Card();
            
            newDeck.Leader = card;
            newDeck.Leader.Id = ids[x];
            newDeck.Leader.Strength = Random.Range(1, 20);
            newDeck.changeStrengthLeader(newDeck.Leader.Strength);
            newDeck.Leader.sprites[0] = Resources.Load("sprites/Cards/SmallSize/" + newDeck.Leader.Id, typeof(Sprite)) as Sprite;
            newDeck.Leader.sprites[1] = Resources.Load("sprites/Cards/FullSize/" + newDeck.Leader.Id, typeof(Sprite)) as Sprite;
            newDeck.changeSprite(0);
            newDeck.transform.position = position;
            newDeck.transform.localScale = new Vector3(1.3F, 1.3F, 0);
            newDeck.transform.parent = GameObject.Find("Content").transform;

            position.x = position.x + transformX;
        }
    }      
}
