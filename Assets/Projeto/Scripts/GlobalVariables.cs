using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariables
{
    private static int score;

    public static int GetScore()
    {
        return score;
    }

    public static void SetScore(int value)
    {
        score = value;
    }
}
