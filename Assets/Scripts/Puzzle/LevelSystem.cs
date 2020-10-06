using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public const int NUM_EASY_LEVELS = 3;
    public const int NUM_MED_LEVELS = 3;
    public const int NUM_HARD_LEVELS = 2;
    public static Dictionary<string, bool> easyLevels;
    public static Dictionary<string, bool> mediumLevels;
    public static Dictionary<string, bool> hardLevels;
    public static Dictionary<int, Dictionary<string, bool>> difficultyLevels;
    public static string[] difficulties = { "Easy", "Medium", "Hard" };
    public static int currentDifficulty = 0;
    public static bool collided;

    public static bool DDA;
    public static bool darkness;
    public static bool lockMovement;
    public static bool removePiece;

    // Start is called before the first frame update
    void Start()
    {
        collided = false;

        easyLevels = new Dictionary<string, bool>
        {
            {"null", false }
        };

        mediumLevels = new Dictionary<string, bool>
        {
            {"null", false }
        };

        hardLevels = new Dictionary<string, bool>
        {
            {"null", false }
        };


        for (int i = 0; i < NUM_EASY_LEVELS; i++)
        {
            easyLevels["SlidePuzzleEasy" + i] = false;
        }  
        for (int i = 0; i < NUM_MED_LEVELS; i++)
        {
            mediumLevels["SlidePuzzleMedium" + i] = false;
        }
        for (int i = 0; i < NUM_HARD_LEVELS; i++)
        {
            hardLevels["SlidePuzzleHard" + i] = false;
        }

        difficultyLevels = new Dictionary<int, Dictionary<string, bool>>
        {
            {0, easyLevels},
            {1, mediumLevels},
            {2, hardLevels}
        };

        //Debug.Log("LEVELS: " + easyLevels.Count);
    }

}
