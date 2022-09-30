using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    Button startButton = null;
    ObjectActiveFalse[] ActiveFalsedObj = null;

    private void Awake()
    {

        startButton = GameObject.Find("StartButton").GetComponent<Button>();

        StartCoroutine(LoadDefenseScene());
    }

    void Start()
    {
        //button condition init
        startButton.interactable = false;
        ActiveFalsedObj = GameObject.FindObjectsOfType<ObjectActiveFalse>();
    }

    public void OnStartButton()
    {
        startButton.interactable = false;
        StartCoroutine(CameraMove());
    }
    IEnumerator CameraMove()
    {
        while (true)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(-2.72f, 4.25f, -2.72f), Time.deltaTime);
            Debug.Log(Camera.main.transform.position.x);
            if (Camera.main.transform.position.x > -2.8f)
            {
                for (int i = 0; i < ActiveFalsedObj.Length; i++)
                {
                    ActiveFalsedObj[i].gameObject.SetActive(true);
                }
                SceneManager.UnloadScene("1_StartScene");
            }

            yield return null;
        }
    }
    IEnumerator LoadDefenseScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("2_DefenseScene", LoadSceneMode.Additive);

        while (true)
        {
            if (operation.isDone)
            {
                startButton.interactable = true;
            }

            yield return null;
        }

    }
}
