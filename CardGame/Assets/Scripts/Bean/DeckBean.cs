using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeckBean
{
    public string name;
    public CardBean leader;
    public List<CardBean> cards;
    public User user;
}
