using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : BasicScreen
{
    public Button backButton;
    public Button quizButton;
    public TMP_Text countdownText;
    public GameObject timer;
    public GameObject quizStart;
    public GameObject quiz;
    public TMP_Text score;
    public TMP_Text points;

    private DateTime nextAvailableTime;
    private bool rewardAvailable;

    private const string NextTimeKey = "NextRewardTime";

    [Header("QuizGame")]
    public QuizConfig[] quizConfigs;
    private QuizConfig currentQuiz;

    public Button answerButton;

    public TMP_Text question;

    public Button[] repliesButton;
    public TMP_Text[] repliesText;
    public Sprite defaultButton;
    public Sprite currentButton;
    public Sprite correctButton;
    public Sprite incorectButton;

    public Image[] indicators;
    public Sprite currentindicator;
    public Sprite correctIndicator;
    public Sprite nonCorrectIndicator;
    public Sprite defaultIndicator;

    private int currentQuestion;
    private int currentChoose = -1;
    [SerializeField]private List<int> replies = new List<int>();

    public TMP_Text rewardText;

    private bool canPress;

    void Start()
    {
        LoadNextAvailableTime();
        UpdateUI();

        answerButton.onClick.AddListener(Answer);

        for(int i = 0; i < repliesButton.Length; i++)
        {
            int index = i;
            repliesButton[index].onClick.AddListener(() => Choose(index));
        }

        quizButton.onClick.AddListener(QuizStartButton);
        backButton.onClick.AddListener(Back);
    }

    private void OnDestroy()
    {
        answerButton.onClick.RemoveListener(Answer);

        for (int i = 0; i < repliesButton.Length; i++)
        {
            int index = i;
            repliesButton[index].onClick.RemoveListener(() => Choose(index));
        }

        quizButton.onClick.RemoveListener(QuizStartButton);
        backButton.onClick.RemoveListener(Back);
    }

    void Update()
    {
        if (!rewardAvailable)
        {
            TimeSpan timeLeft = nextAvailableTime - DateTime.Now;

            if (timeLeft <= TimeSpan.Zero)
            {
                rewardAvailable = true;
                UpdateUI();
            }
            else
            {
                countdownText.text = $"{timeLeft.Hours:D2}:{timeLeft.Minutes:D2}";
            }
        }
    }
    
    public override void ResetScreen()
    {
        quiz.SetActive(false);
    }

    public override void SetScreen()
    {
        canPress = true;
        UpdateUI();
        SetText();
    }
    private void SetText()
    {
        score.text = PlayerPrefs.GetInt("Score").ToString();
        points.text = PlayerPrefs.GetInt("Points").ToString();
    }

    private void StartQuiz()
    {
        answerButton.interactable = false;
        int randomQuiz = UnityEngine.Random.Range(0, quizConfigs.Length);
        currentQuiz = quizConfigs[randomQuiz];
        
        replies.Clear();
        currentQuestion = 0;
        SetQuestion();

        
    }

    private void SetQuestion()
    {
        if (currentQuestion == currentQuiz.questions.Length)
        {
            FinishGame();
            return;
        }

        currentChoose = -1;
        SetIndocators();
        foreach(var button in repliesButton)
        {
            button.gameObject.GetComponent<Image>().sprite = defaultButton;
        }
        for(int i = 0; i <  repliesText.Length; i++)
        {
            repliesText[i].text = currentQuiz.questions[currentQuestion].answers[i];
            repliesText[i].color = Color.white;
        }
        question.text = currentQuiz.questions[currentQuestion].question;
        canPress = true;
    }

    private void FinishGame()
    {
        int correct = 0;
        foreach(var reply in replies)
        {
            if(reply == 1)
            {
                correct++;
            }
        }
        if(correct == 3)
        {
            string key = "Achieve1";
            PlayerPrefs.SetInt(key, 1);
        }
        if(correct == 0)
        {
            UIManager.Instance.ShowPopup(PopupTypes.LoseQuiz);
        }
        else
        {
            int newScore = PlayerPrefs.GetInt("Score");
            int reward = 150 * correct;
            newScore += reward;
            PlayerPrefs.SetInt("Score", newScore);
            UIManager.Instance.ShowPopup(PopupTypes.WinQuiz);
            rewardText.text = "+" + reward.ToString();
        }
    }

    private void SetIndocators()
    {
        for (int i = 0; i < indicators.Length; i++)
        {
            if (currentQuestion == i)
            {
                indicators[i].sprite = currentindicator;
            }
            else if (i > currentQuestion)
            {
                indicators[i].sprite = defaultIndicator;
            }
        }
        for (int i = 0; i < replies.Count; i++)
        {
            if (replies[i] == 1)
            {
                indicators[i].sprite = correctIndicator;
            }
            else if (replies[i] == -1)
            {
                indicators[i].sprite = nonCorrectIndicator;
            }
        }
    }

   
    private void Choose(int index)
    {
        if (!canPress) return;

        answerButton.interactable = true;
        currentChoose = index;
        foreach (var button in repliesButton)
        {
            button.gameObject.GetComponent<Image>().sprite = defaultButton;
        }
        repliesButton[currentChoose].gameObject.GetComponent<Image>().sprite = currentButton;
        for (int i = 0; i < repliesText.Length; i++)
        {
            repliesText[i].color = Color.white;
        }
        repliesText[currentChoose].color = Color.black;
    }
    private void Answer()
    {
        if (!canPress) return;
        canPress = false;
        answerButton.interactable = false;
        bool isCorrect = CheckReply();
        if (isCorrect)
        {
            repliesButton[currentChoose].gameObject.GetComponent<Image>().sprite = correctButton;
            indicators[currentQuestion].sprite = currentindicator;
            replies.Add(1);
        }
        else
        {
            repliesButton[currentChoose].gameObject.GetComponent<Image>().sprite = incorectButton;
            indicators[currentQuestion].sprite = nonCorrectIndicator;
            replies.Add(-1);
        }
        repliesText[currentChoose].color = Color.white;
        currentQuestion++;
        Invoke("SetQuestion", 2);
    }

    private bool CheckReply()
    {
        if (currentChoose == currentQuiz.questions[currentQuestion].correctAnswer)
        {
            return true;
        }
        return false;
    }

    private void QuizStartButton()
    {
        if (!rewardAvailable)
            return;
        quizStart.SetActive(false);  
        quiz.SetActive(true);
        StartQuiz();

        nextAvailableTime = DateTime.Now.AddDays(1);
        PlayerPrefs.SetString(NextTimeKey, nextAvailableTime.ToBinary().ToString());
        PlayerPrefs.Save();

        rewardAvailable = false;
        
    }

    void LoadNextAvailableTime()
    {
        if (PlayerPrefs.HasKey(NextTimeKey))
        {
            long binaryTime = Convert.ToInt64(PlayerPrefs.GetString(NextTimeKey));
            nextAvailableTime = DateTime.FromBinary(binaryTime);
            rewardAvailable = DateTime.Now >= nextAvailableTime;
        }
        else
        {
            rewardAvailable = true;
        }
    }

    void UpdateUI()
    {
        if (quiz.activeInHierarchy) return;
        if (rewardAvailable)
        {
            quizStart.SetActive(true);
            timer.SetActive(false);
        }
        else
        {
            quizStart.SetActive(false);
            timer.SetActive(true);
        }
        quizButton.interactable = rewardAvailable;
        countdownText.gameObject.SetActive(!rewardAvailable);
    }

    private void Back()
    {
        if (!canPress) return;
        UIManager.Instance.ShowScreen(ScreenTypes.Home);
    }
}
