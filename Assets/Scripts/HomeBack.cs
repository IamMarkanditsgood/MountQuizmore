using Mkey;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeBack : MonoBehaviour
{
    public void GoHome()
    {
       
        SceneManager.LoadScene("MainMenu");
    }

    public void GoHomeWin()
    {
        GameLevelHolder.CurrentLevel++;
        HintHolder.Add(1);
        ShuffleHolder.Add(1);

        int newScore = PlayerPrefs.GetInt("Score") + 240;
        PlayerPrefs.SetInt("Score", newScore);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");
    }
}
