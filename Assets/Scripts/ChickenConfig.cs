using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Chicken", menuName = "ScriptableObjects/Chickens", order = 1)]
public class ChickenConfig : ScriptableObject
{
    public ChickenType chickenType;
    public int reward; 
}
