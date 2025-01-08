using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI_Control;
using System.Security.Cryptography;
using UnityEngine.Video;
using UnityEngine.UI;
using Unity.VisualScripting;


public class UIManager : MonoBehaviour
{
    public CharacterManerger characterManerger;
    public UIFadeIOControl uIFade;
    public DeathUI deathUI;
    public Canvas canvas;
    public GameObject videoPlayerUI;
    public Text RespawnCountDown;
    public Text DeathTitle;
    public Text EndTitle;
    public float WaitingTime {get; set;}
    VideoPlayer videoPlayer;
    
    void Start()
    {
        videoPlayer = videoPlayerUI.GetComponentInChildren<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(videoPlayer.gameObject.activeSelf && videoPlayer.isPaused)
        {
            canvas.sortingOrder = 100;
            uIFade.FadeOutToIn(5);
            videoPlayerUI.gameObject.SetActive(false);
        }
        RespawnCountDown.text = "重生等待秒數：" + deathUI.CountDownText(5).ToString();
        DeathTitle.text = "菜 你死了：" + characterManerger.Character.DeathTimes + "次";
        EndTitle.text = "" + characterManerger.Character.DeathTimes;
    }


    public void SetUIForSeconds(GameObject gameObject)
    {
        StartCoroutine(_SetUIForSeconds(WaitingTime, gameObject));
    }
    public void CloseUIForSeconds(GameObject gameObject)
    {
        StartCoroutine(_CloseUIForSeconds(WaitingTime, gameObject));
    }

    IEnumerator _SetUIForSeconds(float sec, GameObject gameObject)
    {
        yield return new WaitForSeconds(sec);
        gameObject.SetActive(true);
    }

    IEnumerator _CloseUIForSeconds(float sec, GameObject gameObject)
    {
        yield return new WaitForSeconds(sec);
        gameObject.SetActive(false);
    }
}
