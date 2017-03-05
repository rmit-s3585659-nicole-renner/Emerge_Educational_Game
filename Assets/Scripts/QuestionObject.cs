using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestionObject {

    public string QuestionText;
    public List<string> correctAnswer;
    public List<string> incorrectAnswer;

    public QuestionObject(string question, List<string> correctans, List<string> incorrectans)
    {
        incorrectAnswer = new List<string>();
        correctAnswer = new List<string>();
        QuestionText = question;
        if (correctans.Count > 0)
        {
            foreach (string ans in correctans)
            {
                correctAnswer.Add(ans);
            }
        }
        if (incorrectans.Count > 0)
        {
            foreach (string ans in incorrectans)
            {
                incorrectAnswer.Add(ans);
            }
        }
    }
}
