using UnityEngine;
using UnityEngine.UI;
using TMPro; 


public class ScoreManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static int score;

    TMP_Text text;
    void Start()
    {
        text = GetComponent<TMP_Text>();
        if (text == null)
        {
            Debug.LogError("ScoreManager: No TMP_Text component found on the GameObject.");
        }

        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (score < 0)
        {
            score = 0;
        }

        text.text = "" + score;
    }
    public static void AddPoints(int points)
    {
        score += points;
    }
    public static void SubtractPoints(int points)
    {
        score -= points;
    }
    public static void ResetScore()
    {
        score = 0;
    }
}
