using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Animal", menuName = "ScriptableObjects/Animal", order = 1)]
public class AnimalConfig : ScriptableObject
{
    public Sprite art;
    public string animalName;
    public string animalDescription;
    public string[] animalAdvice;
    public int scorePrice;
    public int pointsPrice;
}
