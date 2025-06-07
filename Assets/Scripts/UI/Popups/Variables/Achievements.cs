using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Achievements : BasicPopup
{
    [Serializable]
    public class AchiveData
    {
        public Sprite image;
        public string name;
        public string description;
    }
    [SerializeField] private AchiveData[] achiveData;

    public Image art;
    public TMP_Text name;
    public TMP_Text description;

    private int currentAchive;
    
    public void Init(int index)
    {
        currentAchive = index;
    }

    public override void ResetPopup()
    {
    }

    public override void SetPopup()
    {
        art.sprite = achiveData[currentAchive].image;
        name.text = achiveData[currentAchive].name;
        description.text = achiveData[currentAchive].description;
    }

}
