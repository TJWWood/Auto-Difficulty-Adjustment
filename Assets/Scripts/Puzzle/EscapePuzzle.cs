using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class EscapePuzzle : MonoBehaviour
{
    public Material selectedObjectMat;
    public Material defaultMat;
    public Material goalPieceDefaultMat;
    //public Transform entranceDoor;
    public Camera mainCam;
    public Transform goalPiece;
    public Transform floor;

    private Transform _selection;
    public Light mainLight;
    public Light goalPieceLight;

    float timer;

    bool darkness = false;
    bool lockMovement = false;
    bool removePiece = false;

    public GameObject BrainFramework;
    private BrainFramework INSIGHT;

    void Ready()
    {
        INSIGHT.StartStream();
    }
    void Stream()
    {
        Debug.Log($"metrics: { INSIGHT.BRAIN.metrics } | command: { INSIGHT.BRAIN.command } | eyeAction: { INSIGHT.BRAIN.eyeAction } | upperFaceAction: { INSIGHT.BRAIN.upperFaceAction } | lowerFaceAction: { INSIGHT.BRAIN.lowerFaceAction }");
    }
    void Start()
    {
        INSIGHT = BrainFramework.GetComponent<BrainFramework>();
        INSIGHT.On("READY", Ready);
        INSIGHT.On("STREAM", Stream);
        //darkness = true;
        //Check difficulty modifiers decided from prev level
        if (LevelSystem.darkness)
        {
            darkness = true;
            //LevelSystem.darkness = false;
        }
        if(LevelSystem.lockMovement)
        {
            lockMovement = true;
            //LevelSystem.lockMovement = false;
        }
        if(LevelSystem.removePiece)
        {
            removePiece = true;
            //LevelSystem.removePiece = false;
        }

        timer = 10f;

        //if (SceneManager.GetActiveScene().name == "SlidePuzzleEasy2")
        //{      
            if (darkness)
            {       
                RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
                RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Custom;
                mainLight.enabled = false;
                goalPieceLight.enabled = true;
                PostProcessVolume ppv = FindObjectOfType<PostProcessVolume>();
                ppv.enabled = false;
                Shader s1 = Shader.Find("Unlit/Color");
                floor.GetComponent<Renderer>().material.shader = s1;
            }
        //}

        if (removePiece)
        {
            GameObject horObst = GameObject.Find("Horizontal");
            GameObject verObst = GameObject.Find("Vertical");
            GameObject[] horObstacles = new GameObject[horObst.transform.childCount];
            GameObject[] verObstacles = new GameObject[verObst.transform.childCount];
            //Debug.Log(horObst.transform.GetChild(0));
            //Debug.Log(verObst.name);

            for (int i = 0; i < horObst.transform.childCount; i++)
            {
                //Debug.Log(horObst.transform.GetChild(i));
                horObstacles[i] = horObst.transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < verObst.transform.childCount; i++)
            {
                verObstacles[i] = verObst.transform.GetChild(i).gameObject;
            }

            if (Random.Range(0f, 1f) >= 0.5)
            {
                //Debug.Log("DESTROYING HOR");
                Destroy(horObstacles[Random.Range(0, horObstacles.Length)]);
            }
            else
            {
                //Debug.Log("DESTROYING VER");
                Destroy(verObstacles[Random.Range(0, verObstacles.Length)]);
            }
        }
        
    }
    void Update()
    {
        //if (timer > 0f)
       // {
       //     timer -= Time.smoothDeltaTime;
            //entranceDoor.position += new Vector3(0f, Time.deltaTime * 40f, 0f);
      //  }
        // Bit shift the index of the layer (8) to get a bit mask

        if(Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        //layerMask = ~layerMask;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonUp(0))
        {
            // Does the ray intersect any objects excluding the game piece layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                if(_selection != hit.transform && _selection != null)
                {
                    if(_selection.name == "GoalPiece")
                    {
                        _selection.GetComponent<Renderer>().material = goalPieceDefaultMat;
                    }
                    else
                    {
                        _selection.GetComponent<Renderer>().material = defaultMat;
                    }
                }

                if(hit.transform == _selection)
                {
                    _selection.GetComponent<Renderer>().material = defaultMat;
                    _selection = goalPiece;
                }
                else
                {
                    _selection = hit.transform;
                }
                _selection.GetComponent<Renderer>().material = selectedObjectMat;


                //Debug.DrawRay(ray, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                //Debug.Log("Did Hit");
            }
            else
            {
                //Debug.DrawRay(ray, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                //Debug.Log("Did not Hit");
            }
        }

        if(_selection != null)
        {
            Vector3 movement = new Vector3(0.0f, 0.0f, 0.0f);
            //_selection.GetComponent<Rigidbody>().isKinematic = true;
            if (_selection.name == "GoalPiece")
            {
                if (darkness)
                {
                    goalPieceLight.enabled = true;
                }

                if(lockMovement)
                {
                    if (INSIGHT.BRAIN.command == "push")
                    {
                        _selection.Translate(Vector3.right * Time.deltaTime * 200f);
                    }
                    _selection.Translate(Vector3.left * Time.deltaTime * Input.GetAxis("HorizontalRight") * 200f);
                }
                else
                {
                    _selection.Translate(Vector3.left * Time.deltaTime * Input.GetAxis("Horizontal") * 200f);
                    if (INSIGHT.BRAIN.command == "push")
                    {
                        _selection.Translate(Vector3.right * Time.deltaTime * 200f);
                    }
                    //else if (INSIGHT.BRAIN.command == "neutral")
                    //{
                    //    _selection.Translate(Vector3.left * Time.deltaTime * 200f);
                    //}
                }
            }
            else if(_selection.parent.name == "Horizontal")
            {
                if(INSIGHT.BRAIN.command == "push")
                {
                    _selection.Translate(Vector3.right * Time.deltaTime * 200f);
                }
                //else if(INSIGHT.BRAIN.command == "neutral")
                //{
                //    _selection.Translate(Vector3.left * Time.deltaTime * 200f);
                //}
                _selection.Translate(Vector3.left * Time.deltaTime * Input.GetAxis("Horizontal") * 200f);
            }
            else if(_selection.parent.name == "Vertical")
            {
                if (INSIGHT.BRAIN.command == "push")
                {
                    _selection.Translate(Vector3.forward * Time.deltaTime * 200f);
                }
                //else if (INSIGHT.BRAIN.command == "neutral")
                //{
                //    _selection.Translate(Vector3.back * Time.deltaTime * 200f);
               // }
                _selection.Translate(Vector3.back * Time.deltaTime * Input.GetAxis("Vertical") * 200f);
            }
        }  
    } 
}

