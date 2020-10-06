using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button button;
    public Toggle toggle;
    
    // Start is called before the first frame update
    void Start()
    {

        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

    }

    void TaskOnClick()
    {
        SceneManager.LoadSceneAsync("SlidePuzzleEasy1", LoadSceneMode.Single);
        Toggle tgl = toggle.GetComponent<Toggle>();
        if (tgl.isOn == true)
        {
            LevelSystem.DDA = true;
        }
        else
        {
            LevelSystem.DDA = false;
        }
        //Debug.Log(LevelSystem.DDA);
    }


}
