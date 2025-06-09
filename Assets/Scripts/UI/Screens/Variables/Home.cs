using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Home : BasicScreen
{
    [SerializeField] private string sceneName;
    public AvatarManager avatarManager;
    public AnimalConfig[] animals;
    [Header("Buttons")]
    public TMP_Text playerName;
    public TMP_Text score;
    public TMP_Text points;
    public TMP_Text animalName;
    public TMP_Text animalDescription;
    public TMP_Text scorePrice;
    public TMP_Text pointsPrice;
    [Header("Buttons")]
    public Button birdSchoolButton;
    public Button gameButton;
    public Button nextButton;
    public Button prevButton;
    public Button askButton;
    public Button closeButton;
    public Button unlockButton;
    public Button playerButton;
    [Header("iMAGE")]
    public Image bird;
    [Header("Objects")]
    public GameObject scoreObject;
    public GameObject pointsObject;

    private int currentAnimal;

    private void Start()
    {
        birdSchoolButton.onClick.AddListener(BirdSchoolButton);
        gameButton.onClick.AddListener(GameButton);
        nextButton.onClick.AddListener(NextButton);
        prevButton.onClick.AddListener(PrevButton);
        askButton.onClick.AddListener(AskButton);
        closeButton.onClick.AddListener(CloseButton);
        unlockButton.onClick.AddListener(UnlockButton);
        playerButton.onClick.AddListener(PlayerButton);
    }

    private void OnDestroy()
    {
        birdSchoolButton.onClick.RemoveListener(BirdSchoolButton);
        gameButton.onClick.RemoveListener(GameButton);
        nextButton.onClick.RemoveListener(NextButton);
        prevButton.onClick.RemoveListener(PrevButton);
        askButton.onClick.RemoveListener(AskButton);
        closeButton.onClick.RemoveListener(CloseButton);
        unlockButton.onClick.RemoveListener(UnlockButton);
        playerButton.onClick.RemoveListener(PlayerButton);
    }

    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {
        ConfigureSceen();
    }

    private void ConfigureSceen()
    {
        SetText();
        SetButtons();
        bird.sprite = animals[currentAnimal].art;
        avatarManager.SetSavedPicture();
    }

    private void SetText()
    {
        playerName.text = PlayerPrefs.GetString("Name", "UserName");
        score.text = PlayerPrefs.GetInt("Score").ToString();
        points.text = PlayerPrefs.GetInt("Points").ToString();

        animalName.text = animals[currentAnimal].animalName;
        animalDescription.text = animals[currentAnimal].animalDescription;
        scorePrice.text = animals[currentAnimal].scorePrice.ToString();
        pointsPrice.text = animals[currentAnimal].pointsPrice.ToString();
    }

    private void SetButtons()
    {
        string key = animals[currentAnimal].animalName;

        if (PlayerPrefs.GetInt(key) == 0)
        {
            scoreObject.SetActive(true);
            pointsObject.SetActive(true);
            askButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(false);
            unlockButton.gameObject.SetActive(true);
            int currentScore = PlayerPrefs.GetInt("Score");
            int currentPoints = PlayerPrefs.GetInt("Points");
            if (currentScore >= animals[currentAnimal].scorePrice && currentPoints >= animals[currentAnimal].pointsPrice)
            {
                unlockButton.interactable = true;
            }
            else
            {
                unlockButton.interactable = false;
            }
        }
        else
        {
            scoreObject.SetActive(false);
            pointsObject.SetActive(false);
            unlockButton.gameObject.SetActive(false);
            askButton.gameObject.SetActive(true);
            closeButton.gameObject.SetActive(false);

        }

        if (currentAnimal == 0)
        {
            prevButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(true);
        }
        else if (currentAnimal == animals.Length - 1)
        {
            prevButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);
        }
        else
        {
            prevButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
        }
    }

    private void BirdSchoolButton()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Quiz);
    }
    private void GameButton()
    {
        SceneManager.LoadScene(sceneName);
    }
    private void NextButton()
    {
        prevButton.gameObject.SetActive(false);
        if (currentAnimal < animals.Length-1)
        {
            currentAnimal++;
            if(currentAnimal == animals.Length - 1)
            {
                nextButton.gameObject.SetActive(false);
            }
            ConfigureSceen();
        }
    }
    private void PrevButton()
    {
        nextButton.gameObject.SetActive(false);
        if (currentAnimal > 0)
        {
            currentAnimal--;
            if (currentAnimal == 0)
            {
                prevButton.gameObject.SetActive(false);
            }
            ConfigureSceen();
        }
    }
    private void AskButton()
    {
        askButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(true);
        int randomAdvice = Random.Range(0, animals[currentAnimal].animalAdvice.Length);
        animalDescription.text = animals[currentAnimal].animalAdvice[randomAdvice];
        string key = "Achieve0";
        PlayerPrefs.SetInt(key, 1);
        TryToGetPoint();
    } 
    private void CloseButton()
    {
        askButton.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(false);
        animalDescription.text = animals[currentAnimal].animalDescription;
    }
    private void UnlockButton()
    {
        int currentScore = PlayerPrefs.GetInt("Score");
        int currentPoints = PlayerPrefs.GetInt("Points");

        if(currentScore >= animals[currentAnimal].scorePrice && currentPoints >= animals[currentAnimal].pointsPrice)
        {
            currentScore -= animals[currentAnimal].scorePrice;
            currentPoints -= animals[currentAnimal].pointsPrice;

            PlayerPrefs.SetInt("Score", currentScore);
            PlayerPrefs.SetInt("Points", currentPoints);

            unlockButton.gameObject.SetActive(false);
            askButton.gameObject.SetActive(true);

            string key = animals[currentAnimal].animalName;
            PlayerPrefs.SetInt(key, 1);
            ConfigureSceen();
        }
    }

    private void PlayerButton()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Profile);
    }

    private void TryToGetPoint()
    {
        int chance = Random.Range(1, 51);
        if (chance == 1)
        {
            int newpoints = PlayerPrefs.GetInt("Points") + 1;
            PlayerPrefs.SetInt("Points", newpoints);
            points.text = PlayerPrefs.GetInt("Points").ToString();
            UIManager.Instance.ShowPopup(PopupTypes.GetPoint);
        }
    }
}

