using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Game : BasicScreen
{
    public GameObject frontPanel;
    [Header("Text")]
    public TMP_Text scoreText;
    public TMP_Text pointsText;
    public TMP_Text timerText;
    public TMP_Text rewardText;
    [Header("Buttons")]
    public Button backButton;
    public Button[] eggsButtons;
    public Button[] boxButtons;
    [Header("Image")]
    public Image[] resultImages;
    public Image[] resultEggs;
    [Header("Sprites")]
    public Sprite[] eggColorsSprites;

    public Sprite defaultPanel;
    public Sprite correctPanel;
    public Sprite incorectPanel;
    public Sprite[] panelColors;
    [Header("Game")]
    public Colors[] eggColors;
    public Colors[] randonColors = { Colors.none, Colors.none, Colors.none };
    public Colors[] choosedColors = { Colors.none, Colors.none, Colors.none };
    public Colors currentColor = Colors.none;

    private int correctReplied;

    public enum Colors { green, red, blue, none}

    private void Start()
    {
        backButton.onClick.AddListener(Back);

        for(int i = 0; i < eggsButtons.Length; i++)
        {
            int index = i;
            eggsButtons[index].onClick.AddListener(() => SelectColor(index));
        }
        for (int i = 0; i < boxButtons.Length; i++)
        {
            int index = i;
            boxButtons[index].onClick.AddListener(() => PanelPressed(index));
        }
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(Back);

        for (int i = 0; i < eggsButtons.Length; i++)
        {
            int index = i;
            eggsButtons[index].onClick.RemoveListener(() => SelectColor(index));
        }
        for (int i = 0; i < boxButtons.Length; i++)
        {
            int index = i;
            boxButtons[index].onClick.RemoveListener(() => PanelPressed(index));
        }

    }


    public override void ResetScreen()
    {
        StopAllCoroutines();
    }

    public override void SetScreen()
    {
        SetText();
        StartGame();
    }

    private void SetText()
    {
        scoreText.text = PlayerPrefs.GetInt("Score").ToString();
        pointsText.text = PlayerPrefs.GetInt("Points").ToString();
    }

    private void StartGame()
    {
        currentColor = Colors.none;
        for (int i = 0; i < choosedColors.Length; i++)
        {
            choosedColors[i] = Colors.none;
        }
        for (int i = 0; i < randonColors.Length; i++)
        {
            randonColors[i] = Colors.none;
        }
        foreach (var egg in resultEggs)
        {
            egg.enabled = false;
        }
        foreach(var panel in boxButtons)
        {
            panel.gameObject.GetComponent<Image>().sprite = defaultPanel;
        }
        correctReplied = 0;
        StartCoroutine(Timer());
        SetShifre();
    }

    private void SetShifre()
    {
        frontPanel.SetActive(false);
        foreach (var egg in resultEggs)
        {
            egg.enabled = false;
        }
        foreach (var panel in boxButtons)
        {
            panel.gameObject.GetComponent<Image>().sprite = defaultPanel;
        }
        currentColor = Colors.none;
        for (int i = 0; i < choosedColors.Length; i++)
        {
            choosedColors[i] = Colors.none;
        }
        for (int i = 0; i < randonColors.Length; i++)
        {
            randonColors[i] = Colors.none;
        }
        randonColors = GetRandomColors(3);
    }
    Colors[] GetRandomColors(int count)
    {
        Colors[] allValidColors = { Colors.green, Colors.red, Colors.blue };
        Colors[] result = new Colors[count];

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, allValidColors.Length);
            result[i] = allValidColors[randomIndex];
        }

        return result;
    }
    private void RestartShiefre()
    {
        frontPanel.SetActive(false);
        foreach (var panel in boxButtons)
        {
            panel.gameObject.GetComponent<Image>().sprite = defaultPanel;
        }
        currentColor = Colors.none;
        for( int i = 0; i <  choosedColors.Length; i++)
        {
            choosedColors[i] = Colors.none;
        }

        foreach(var egg in resultEggs)
        {
            egg.enabled = false; 
        }
    }

    private void EndGame()
    {
        if(correctReplied == 0)
        {
            UIManager.Instance.ShowPopup(PopupTypes.LoseGame);
        }
        else
        {
            string key = "Achieve2";
            PlayerPrefs.SetInt(key, 1);
            int newScore = PlayerPrefs.GetInt("Score");
            int reward = 50 * correctReplied;
            newScore += reward;
            PlayerPrefs.SetInt("Score", newScore);
            UIManager.Instance.ShowPopup(PopupTypes.WinGame);
            rewardText.text = "+" + reward;
        }
    }

    private IEnumerator Timer()
    {
        int time = 90;
        while (time > 0)
        {
            int minutes = time / 60;
            int seconds = time % 60;

            timerText.text = $"{minutes:D2}:{seconds:D2}";
            yield return new WaitForSeconds(1);
            time--;
        }
        timerText.text = "00:00";
        EndGame();
    }
    private void SelectColor(int index)
    {
        currentColor = eggColors[index];
    }

    private void PanelPressed(int index)
    {
        if(currentColor == Colors.none) return;

        choosedColors[index] = currentColor;

        resultEggs[index].enabled = true;
        resultEggs[index].sprite = GetSpriteByColor(currentColor);

        int repies = 0;
        foreach(var reply in choosedColors)
        {
            if(reply != Colors.none)
            {
                repies++;
            }
        }
        if(repies == 3)
        {
            CheckShiefre();
          
        }
    }

    private Sprite GetSpriteByColor(Colors color)
    {
        for(int i = 0; i < eggColors.Length; i++)
        {
            if (eggColors[i] == color)
            {
                return eggColorsSprites[i]; 
            }
        }
        return null;
    }

    private void CheckShiefre()
    {
        int correct = 0;
        for(int i = 0; i <  choosedColors.Length; i++)
        {
            if (choosedColors[i] == randonColors[i])
            {
                correct++;
                boxButtons[i].gameObject.GetComponent<Image>().sprite = correctPanel;
            }
            else
            {
                boxButtons[i].gameObject.GetComponent<Image>().sprite = incorectPanel;
            }
        }
        frontPanel.SetActive(true);
        if (correct == 3)
        {
            for(int i = 0; i < randonColors.Length; i++)
            {
                boxButtons[i].gameObject.GetComponent<Image>().sprite = GetPanelSpriteByColor(randonColors[i]);
            }
            correctReplied++;
            Invoke("SetShifre", 3);
        }
        else
        {
            Invoke("RestartShiefre", 3);
        }
    }
    private Sprite GetPanelSpriteByColor(Colors color)
    {
        for (int i = 0; i < eggColors.Length; i++)
        {
            if (eggColors[i] == color)
            {
                return panelColors[i];
            }
        }
        return null;
    }
    private void Back()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Home);
    }
}
