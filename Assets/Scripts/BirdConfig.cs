using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bird", menuName = "ScriptableObjects/Birds", order = 1)]
public class BirdConfig : ScriptableObject
{
    public Sprite image;
    public string Name;
    public string Description;
}
