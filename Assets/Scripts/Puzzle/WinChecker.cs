using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinChecker : MonoBehaviour
{
    public static WinChecker control;
    public static int elevel1 = 0;
    public static int elevel2 = 0;
    public static int elevel3 = 0;
    public static int mlevel1 = 0;
    public static int difficulty = 0;

    //public static string[] difficulties = { "easy", "medium", "hard" };

    //public static Dictionary<string, bool> easyLevels;
    //public static Dictionary<string, bool> mediumLevels;
    //public static Dictionary<string, bool> hardLevels;
    //public static Dictionary<int, Dictionary<string, bool>> difficultyLevels;

    /**
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            easyLevels["SlidePuzzleEasy" + i] = false;
        }
        for (int i = 0; i < 1; i++)
        {
            easyLevels["SlidePuzzleMedium" + i] = false;
        }

        difficultyLevels[0] = easyLevels;
        difficultyLevels[1] = mediumLevels;
    }
    **/
}
