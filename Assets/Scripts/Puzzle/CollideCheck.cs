using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollideCheck : MonoBehaviour
{
    //public static bool win = false;
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other);
        if(other.name == "GoalPiece")
        {
            //Destroy(other);
            //win = true;
            LevelSystem.collided = true;
            //Debug.Log("WIN");
        }
        //Debug.Log(other.name);
    }
}
