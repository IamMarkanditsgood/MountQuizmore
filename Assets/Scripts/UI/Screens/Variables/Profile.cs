using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Profile : BasicScreen
{
    public AvatarManager avatarManager;
    [Header("Text")]
    public TMP_InputField playerName;
    public TMP_Text score;
    public TMP_Text points;
    [Header("Buttons")]
    public Button editPhotoButton;
    public Button editNameButton;
    public Button closeButton;
    public Button[] achievementButtons;
    [Header("Sprites")]
    public Sprite[] openedAchievements;


    private int currentAnimal;

    private void Start()
    {
        editPhotoButton.onClick.AddListener(EditPhotoButton);
        editNameButton.onClick.AddListener(EditNameButton);
        closeButton.onClick.AddListener(CloseButton);

        for(int i = 0;i < achievementButtons.Length; i++)
        {
            int index = i;
            achievementButtons[index].onClick.AddListener(() => AchievePopup(index));
        }
    }

    private void OnDestroy()
    {
        editPhotoButton.onClick.RemoveListener(EditPhotoButton);
        editNameButton.onClick.RemoveListener(EditNameButton);
        closeButton.onClick.RemoveListener(CloseButton);

        for (int i = 0; i < achievementButtons.Length; i++)
        {
            int index = i;
            achievementButtons[index].onClick.RemoveListener(() => AchievePopup(index));
        }
    }

    public override void ResetScreen()
    {
        if(playerName.interactable == true)
        {
            PlayerPrefs.SetString("Name", playerName.text);
        }
        playerName.interactable = false;
    }

    public override void SetScreen()
    {
        ConfigureSceen();
    }

    private void ConfigureSceen()
    {
        playerName.interactable = false;
        SetText();
        SetAcheivements();
        avatarManager.SetSavedPicture();
    }

    private void SetText()
    {
        playerName.text = PlayerPrefs.GetString("Name", "UserName");
        score.text = PlayerPrefs.GetInt("Score").ToString();
        points.text = PlayerPrefs.GetInt("Points").ToString();
    }
   
    private void SetAcheivements()
    {
        for(int i = 0; i < achievementButtons.Length; i++)
        {
            string key = $"Achieve{i}";
            if(PlayerPrefs.GetInt(key) == 1)
            {
                achievementButtons[i].gameObject.GetComponent<Image>().sprite = openedAchievements[i];
            }
        }
    }

    private void EditNameButton()
    {
        playerName.interactable = !playerName.interactable;
        PlayerPrefs.SetString("Name", playerName.text);
    }
    private void EditPhotoButton()
    {
        avatarManager.PickFromGallery();
    }
    
    private void CloseButton()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Home);
    }

    private void AchievePopup(int index)
    {
        Achievements achievements = (Achievements)UIManager.Instance.GetPopup(PopupTypes.Achieve);
        achievements.Init(index);
        UIManager.Instance.ShowPopup(PopupTypes.Achieve);
    }
}