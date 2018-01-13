using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardBean : IEqualityComparer<CardBean>, IEquatable<CardBean>
{
    public string id;
    public string name;
    public int strength;

    public bool Equals(CardBean x, CardBean y)
    {
        throw new NotImplementedException();
    }

    public int GetHashCode(CardBean obj)
    {
        throw new NotImplementedException();
    }

    public bool Equals(CardBean other)
    {
        return this.id == other.id;
    }
}
