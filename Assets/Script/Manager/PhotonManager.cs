using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [Header("[ConnectViewPage]")]
    [SerializeField] GameObject connectViewPage = null;
    [SerializeField] TextMeshProUGUI connectInfo = null;

    [Header("RoomListPage")]
    [SerializeField] GameObject searchRoomPage = null;

    Camera myCamera;
    Vector3 originCamearPos;

    bool onMenu = false;

    private void Awake()
    {
        //Bring Camera Component
        myCamera = Camera.main;
        originCamearPos = myCamera.transform.position;

        //screen setting
        Screen.SetResolution(400, 300, false);

        Debug.Log("invasion allow : " + GameManager.INSTANCE.INVASIONALLOW);

        
    }
    private void Update()
    {
        connectInfo.text = PhotonNetwork.NetworkClientState.ToString();
    }

    #region Button Event
    public void OnInvasionPermitButton()
    {
        //invasion allow toggle
        GameManager.INSTANCE.INVASIONALLOW = (GameManager.INSTANCE.INVASIONALLOW == false);

        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("invasion allow : " + GameManager.INSTANCE.INVASIONALLOW);

    }

    public void OnGoOffenseButton()
    {
        StartCoroutine(Uptrans(searchRoomPage));
        StartCoroutine(ChangeCameraView());
        myCamera.transform.position = originCamearPos;
        myCamera.orthographicSize = 2.5f;
    }
    #endregion

    #region Page Up&Down Coroutine
    IEnumerator Uptrans(GameObject page)
    {
        while (true)
        {
            page.transform.position =
                Vector3.Lerp(page.transform.position, page.transform.position + new Vector3(0, 100f, 0), Time.deltaTime * 20f);
            yield return null;

            if (page.transform.position.y >= 540f)
            {
                yield break;
            }
        }
    }
    IEnumerator Downtrans(GameObject page)
    {
        while (true)
        {
            page.transform.position =
                Vector3.Lerp(page.transform.position, page.transform.position + new Vector3(0, -100f, 0), Time.deltaTime * 30f);
            yield return null;

            if (page.transform.position.y <= -540f)
            {
                yield break;
            }
        }
    }
    #endregion

    #region Change CameraView Coroutine
    IEnumerator ChangeCameraView()
    {
        StartCoroutine(CameraViewSize());
        while (true)
        {
            if (!onMenu)
            {
                myCamera.transform.position = originCamearPos;
                myCamera.orthographicSize = 2.5f;
                yield break;
            }
            myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, new Vector3(-4.9f, 4.8f, -6.5f), Time.deltaTime * 2f);

            yield return null;
            if (myCamera.transform.position.x <= -4.89f)
            {
                yield break;
            }
        }

    }
    IEnumerator CameraViewSize()
    {
        while (true)
        {
            myCamera.orthographicSize -= 0.04f;
            yield return new WaitForSeconds(Time.deltaTime);
            if (myCamera.orthographicSize <= 0.3f)
            {
                yield break;
            }
        }
    }
    #endregion
}
