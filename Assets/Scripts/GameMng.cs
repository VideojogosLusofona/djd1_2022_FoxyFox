using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng : MonoBehaviour
{
    private static GameMng instance = null;

    private int score;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void ChangeScore(int deltaScore)
    {
        score += deltaScore;

        Debug.Log($"Score={score}");
    }

    public int GetScore()
    {
        return score;
    }
}
