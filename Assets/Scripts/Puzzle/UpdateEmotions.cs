
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdateEmotions : MonoBehaviour
{
    public Text excitementText;
    public Text focusText;
    public Text levelText;
    double excitement;
    double focus;
    int difficulty;
    double oldExc;
    double oldFoc;

    int emotionStorePrevSize;
    int focusStorePrevSize;

    public GameObject BrainFramework;
    private BrainFramework INSIGHT;

    //ArrayList possibleLevels = new ArrayList();
    bool DDA;

    int i;
    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        if(LevelSystem.DDA == true)
        {
            DDA = true;

            emotionStorePrevSize = EmotionStore.excitements.Count;
            focusStorePrevSize = EmotionStore.focus.Count;

            INSIGHT = BrainFramework.GetComponent<BrainFramework>();
            INSIGHT.On("READY", Ready);
            INSIGHT.On("STREAM", Stream);
        }
        else
        {
            DDA = false;
        }

        difficulty = LevelSystem.currentDifficulty;

        excitement = 0;
        excitementText.text = "Excitement: " + excitement.ToString();

        focus = 0;
        focusText.text = "focus: " + focus.ToString();

        levelText.text = "Level: " + SceneManager.GetActiveScene().name;

        if(!DDA)
        {
            Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            canvas.gameObject.SetActive(false);
        }
    }

    void Ready()
    {
        Debug.Log("INSIGHT READY");
        INSIGHT.StartStream();
        INSIGHT.SaveProfile();
    }

    void Stream()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateEmotionValues();
        ChangeLevel();
    }

    void UpdateEmotionValues()
    {
        /**
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (excitement != 100)
            {
                excitement += .5;
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (excitement != 0)
            {
                excitement -= .5;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (focus != 100)
            {
                focus += .3;
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (focus != 0)
            {
                focus -= .3;
            }
        }
        **/

        //CALCULATE AVERAGE EXCITEMENT
        if (EmotionStore.excitements.Count > emotionStorePrevSize)
        {
            excitement = 0;
            for (int i = 0; i < EmotionStore.excitements.Count; i++)
            {
                excitement += System.Convert.ToDouble(EmotionStore.excitements[i]);
                oldExc = excitement;
                Debug.LogError("EXCITEMENT " + EmotionStore.excitements[i]);
            }

            Debug.LogError("TOTAL EXC " + excitement);
            Debug.LogError("EXC STORECOUNT: " + EmotionStore.excitements.Count);
            emotionStorePrevSize = EmotionStore.excitements.Count;
            if(excitement > oldExc)
            {
                excitement = excitement / EmotionStore.excitements.Count;
            }
            
            Debug.Log("AVG: " + (excitement /= EmotionStore.excitements.Count));
            EmotionStore.excitements.Clear();
            
        }

        //CALCULATE AVERAGE FOCUS
        if (EmotionStore.focus.Count > focusStorePrevSize)
        {
            focus = 0;
            for (int i = 0; i < EmotionStore.focus.Count; i++)
            {
                focus += System.Convert.ToDouble(EmotionStore.focus[i]);
                oldFoc = focus;
                Debug.LogError("FOC" + EmotionStore.focus[i]);
            }

            Debug.LogError("TOTAL FOCUS" + focus);
            Debug.LogError("FOCUS STORECOUNT: " + EmotionStore.focus.Count);
            focusStorePrevSize = EmotionStore.focus.Count;
            if(focus > oldFoc)
            {
                focus = focus / EmotionStore.focus.Count;
            }
            
            Debug.Log("AVG: " + (focus /= EmotionStore.focus.Count));
            EmotionStore.focus.Clear();
        }   
        excitementText.text = "Excitement: " + excitement.ToString();
        focusText.text = "focus: " + focus.ToString();
    }

    void ChangeLevel()
    {
        bool harder = false;
        //  bool changeType = false;
        bool easier = false;

        difficulty = LevelSystem.currentDifficulty;

        if (LevelSystem.collided)
        {
            //Debug.Log("CURRENT DIFFICULTY: " + LevelSystem.currentDifficulty);
            LevelSystem.collided = false;
            if (DDA)
            {
                if (excitement >= .55 && focus <= .43 && focus > .17)
                {
                    //FUN BUT ON VERGE OF TOO EASY - DARKNESS MODIFIER OR LOCKED MOVEMENT MODIFIER - 50/50 CHANCE
                    if(Random.Range(0f,1.0f) > .5f)
                    {
                        LevelSystem.lockMovement = true;
                        LevelSystem.darkness = false;
                        LevelSystem.removePiece = false;
                    }
                    else
                    {
                        LevelSystem.darkness = true;
                        LevelSystem.lockMovement = false;
                        LevelSystem.removePiece = false;
                    }
                }
                else if(excitement >= .55 && focus <= .33 && focus > 0.0)
                {
                    //FUN BUT TOO EASY - UP DIFFICULTY TIER
                    //Debug.Log("MOVING UP DIFFICULY TIER");
                    harder = true;
                    if (difficulty == 2)
                    {
                        //Debug.Log("Can't get harder");
                    }
                    else
                    {
                        difficulty++;
                        LevelSystem.currentDifficulty = difficulty;
                        harder = false;
                    }
                }
                else if(excitement >= .55 && focus >= .33 && focus < .55)
                {
                    //FUN BUT ON VERGE OF TOO HARD - REMOVE PIECE MODIFIER
                    LevelSystem.removePiece = true;
                    LevelSystem.lockMovement = false;
                    LevelSystem.darkness = false;
                }
                else if(excitement >= .55 && focus >= .55)
                {
                    //FUN BUT TOO HARD - DARKNESS MODIFIER OR LOCKED MOVEMENT MODIFIER - 50/50 CHANCE - && - DOWN DIFFICULTY TIER
                    if (Random.Range(0f, 1.0f) > .5f)
                    {
                        LevelSystem.lockMovement = true;
                        LevelSystem.darkness = false;
                        LevelSystem.removePiece = false;
                    }
                    else
                    {
                        LevelSystem.darkness = true;
                        LevelSystem.lockMovement = false;
                        LevelSystem.removePiece = false;
                    }
                    easier = true;
                    if (difficulty == 0)
                    {
                        //Debug.Log("Cant get easier");
                    }
                    else
                    {
                        Debug.Log("MOVING DOWN DIFFICULTY TIER");
                        difficulty--;
                        LevelSystem.currentDifficulty = difficulty;
                        easier = false;
                    }
                }
                else if(excitement <= .55 && excitement > .33 && focus >= .37 && focus < .53)
                {
                    //SOME FUN BUT ON VERGE OF DIFFICULT - DARKNESS MODIFIER OR LOCKED MOVEMENT MODIFIER - 50/50 CHANCE
                    if (Random.Range(0f, 1.0f) > .5f)
                    {
                        LevelSystem.lockMovement = true;
                        LevelSystem.removePiece = false;
                        LevelSystem.darkness = false;
                    }
                    else
                    {
                        LevelSystem.darkness = true;
                        LevelSystem.lockMovement = false;
                        LevelSystem.removePiece = false;
                    }
                }
                else if(excitement <= .55 && excitement > .33 && focus >= .55)
                {
                    //SOME FUN BUT TOO HARD - REMOVE PIECE MODIFIER - //USE EXTRA 'EASIER' MODIFIER HERE I THINK??
                    LevelSystem.removePiece = true;
                    LevelSystem.darkness = false;
                    LevelSystem.lockMovement = false;
                }


                else if(excitement <= .27 && excitement > 0.0 && focus >= .55)
                {
                    //NOT FUN AND TOO HARD - DOWN DIFFICULTY TIER
                    easier = true;
                    if (difficulty == 0)
                    {
                        //Debug.Log("Cant get easier");
                    }
                    else
                    {
                        //Debug.Log("MOVING DOWN DIFFICULTY TIER");
                        difficulty--;
                        LevelSystem.currentDifficulty = difficulty;
                        easier = false;
                    }
                }
                else if(excitement <= .27 && excitement > 0.0 && focus <= .27 && focus > 0.0)
                {
                    //NOT FUN AND TOO EASY - DARKNESS MODIFIER OR LOCKED MOVEMENT MODIFIER - 50/50 CHANCE
                    if (Random.Range(0f, 1.0f) > .5f)
                    {
                        LevelSystem.lockMovement = true;
                        LevelSystem.removePiece = false;
                        LevelSystem.darkness = false;
                    }
                    else
                    {
                        LevelSystem.darkness = true;
                        LevelSystem.lockMovement = false;
                        LevelSystem.removePiece = false;
                    }
                }
                else if(excitement <= .37 && excitement > .17 && focus <= .37 && focus > .23)
                {
                    //SOME FUN AND SOME HARD - UP DIFFICULTY TIER
                    //Debug.Log("MOVING UP DIFFICULY TIER");
                    harder = true;
                    if(difficulty == 2)
                    {
                        //Debug.Log("Can't get harder");
                    }
                    else
                    {
                        difficulty++;
                        LevelSystem.currentDifficulty = difficulty;
                        harder = false;
                    }
                    //LevelSystem.currentDifficulty = difficulty;
                    //Debug.Log("NEW DIFFICULTY LEVEL: " + difficulty);
                    //WinChecker.difficulty = difficulty;
                    //harder = false;
                }


                StreamWriter writer = new StreamWriter("emotionFile.txt", true);
                writer.WriteLine("Current Level: " + SceneManager.GetActiveScene().name + ", " + "Difficulty: " + difficulty + ", " + "Excitement: " + excitement + ", " + "Focus: " + focus
                    + ", " + "New Modifiers: " + "Harder: " + harder + ", " + "Easier: " + easier + ", " + "Darkness: " + LevelSystem.darkness + ", " + "Lock Movement: " + LevelSystem.lockMovement
                    + ", " + "Remove Piece: " + LevelSystem.removePiece);
                writer.Close();
            }

            /*
            if(changeType)
            {
                foreach (int level in System.Enum.GetValues(typeof(LevelTypes)))
                {
                    if (level != currentLevel)
                    {
                        possibleLevels.Add(level);
                    }
                }
                currentLevel = (int)possibleLevels.ToArray()[Random.Range(0, possibleLevels.Count)];
            }
            levelText.text = "Level: " + (LevelTypes)currentLevel;
            possibleLevels.Clear();
            */

            //MANUALLY CHECK IF FINAL LEVEL OF EASY LEVELS
            if (SceneManager.GetActiveScene().name == "SlidePuzzleEasy" + LevelSystem.NUM_EASY_LEVELS)
            {
                LevelSystem.difficultyLevels[difficulty][SceneManager.GetActiveScene().name.ToString()] = true;
                if(difficulty != 1)
                {
                    difficulty++;
                    LevelSystem.currentDifficulty = difficulty;
                }
                
                i = 0;
                Debug.Log("last easy diff" + difficulty);
                //Debug.Log("1" + LevelSystem.difficultyLevels[difficulty]["SlidePuzzle" + LevelSystem.difficulties[0] + "0"]);


            }

            //MANUALLY CHECK IF FINAL LEVEL OF MEDIUM LEVELS
            if (SceneManager.GetActiveScene().name == "SlidePuzzleMedium" + LevelSystem.NUM_MED_LEVELS)
            {
                LevelSystem.difficultyLevels[difficulty][SceneManager.GetActiveScene().name.ToString()] = true;
                if(difficulty != 2)
                {
                    difficulty++;
                    LevelSystem.currentDifficulty = difficulty;
                }
                
                i = 0;
                Debug.Log("last med");
            }

            //MANUALLY CHECK IF FINAL LEVEL OF HARD LEVELS
            if (SceneManager.GetActiveScene().name == "SlidePuzzleHard" + LevelSystem.NUM_HARD_LEVELS)
            {
                LevelSystem.difficultyLevels[difficulty][SceneManager.GetActiveScene().name.ToString()] = true;
                if (difficulty != 1)
                {
                    if(LevelSystem.difficultyLevels[0].ContainsValue(false))
                    {
                        difficulty = 0;
                    }
                    else if(LevelSystem.difficultyLevels[1].ContainsValue(false))
                    {
                        difficulty = 1;
                    }
                    //difficulty--;
                    LevelSystem.currentDifficulty = difficulty;
                }
                //difficulty++;
                //SceneManager.LoadScene("MainScene");
            }

            //Debug.Log("1" + LevelSystem.difficultyLevels[difficulty]["SlidePuzzle" + LevelSystem.difficulties[1] + "" + 0]);
            //Debug.Log("2" + LevelSystem.difficultyLevels[difficulty]["SlidePuzzle" + LevelSystem.difficulties[1] + "" + 1]);
            //Debug.Log("3" + LevelSystem.difficultyLevels[difficulty]["SlidePuzzle" + LevelSystem.difficulties[1] + "" + 2]);
            //Debug.Log("MED" + "1" + " " + LevelSystem.difficultyLevels[1]["SlidePuzzle" + LevelSystem.difficulties[1] + "1"]);
            if (!LevelSystem.difficultyLevels[0].ContainsValue(false) && !LevelSystem.difficultyLevels[1].ContainsValue(false) && !LevelSystem.difficultyLevels[2].ContainsValue(false))
            {
                SceneManager.LoadScene("MainScene");
            }
            else
            {
                for (i = 0; i <= LevelSystem.difficultyLevels[difficulty].Count; i++)
                {

                    Debug.Log("diff" + difficulty);

                    if (LevelSystem.difficultyLevels[difficulty]["SlidePuzzle" + LevelSystem.difficulties[difficulty] + "" + i] == false)
                    {
                        //Debug.Log("i1" + i);
                        //Debug.Log("HERE" + i + " " + LevelSystem.difficultyLevels[difficulty]["SlidePuzzle" + LevelSystem.difficulties[difficulty] + i]);
                        LevelSystem.difficultyLevels[difficulty]["SlidePuzzle" + LevelSystem.difficulties[difficulty] + "" + i] = true;
                        Debug.Log("SlidePuzzle" + LevelSystem.difficulties[difficulty] + "" + (i + 1));
                        SceneManager.LoadSceneAsync("SlidePuzzle" + LevelSystem.difficulties[difficulty] + "" + (i + 1));
                        i = 5;
                    }
                    else
                    {
                        continue;
                    }


                    //Debug.Log(i);
                }
            }
        }
        
    }

}
