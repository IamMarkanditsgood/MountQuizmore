using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Quiz", menuName = "ScriptableObjects/Quiz", order = 1)]
public class QuizConfig : ScriptableObject
{
    [Serializable]
    public class QuizQuestionData
    {
        public string question;
        public string[] answers;
        public int correctAnswer;
    }

    public QuizQuestionData[] questions;
}
