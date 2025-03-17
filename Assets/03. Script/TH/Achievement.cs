using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Achievement
{
    private string _name;
    private string _description;
    public bool isCleared;

    public Achievement(string name, string description, bool isCleared) 
    { 
        _name = name;
        _description = description;
        this.isCleared = isCleared;
    }
}
