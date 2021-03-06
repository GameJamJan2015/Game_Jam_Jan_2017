﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrumpManager : MonoBehaviour
{

    public enum GameState
    {
        GAME, MENU, GAMEOVER
    }

    public GameState State { get; set; }

    [SerializeField]
    private VoiceManager VO;

    [SerializeField]
    private Canvas TitleUI;

    [SerializeField]
    private Canvas GameOverUI;

    [SerializeField]
    private Canvas UI;

    [SerializeField]
    private PlayerScript Player;

    [SerializeField]
    private ParticleSystem ps;

    private int Money;

    // Use this for initialization
    void Start()
    {
        this.State = GameState.MENU;
        this.GameOverUI.enabled = false;
        this.UI.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (State == GameState.MENU)
        {
            if (Input.GetButtonUp("Jump"))
            {
                this.State = GameState.GAME;
                this.TitleUI.enabled = false;
                this.UI.enabled = true;
                this.Money = 0;
                Player.Spawn();
                VO.GameStartReaction();
            }
        }
        else if (State == GameState.GAME)
        {
            UpdateMoney();
        }
        else if (State == GameState.GAMEOVER)
        {
            if (Input.GetButtonUp("Jump"))
            {
                this.State = GameState.MENU;
                this.GameOverUI.GetComponent<moveImage>().deactivate();
                this.GameOverUI.enabled = false;
                this.TitleUI.enabled = true;
            }
        }

    }

    public void AddMoney(int money)
    {
        this.Money += money;

        VO.PointsReactions();

        StopAllCoroutines();
        StartCoroutine(MoneyAnim(money));

        ps.transform.position = Player.transform.position;
        ps.Play();
    }

    private void UpdateMoney()
    {
        if (Player.RigidBody.velocity.magnitude > 1)
            Money += (int)(Time.deltaTime * 100);

        UI.GetComponentInChildren<Text>().text = "$" + Money;
    }

    IEnumerator MoneyAnim(int money)
    {
        var tx = UI.transform.GetChild(1).GetComponent<Text>();

        tx.enabled = true;
        tx.text += "+ $" + money + "\n";

        yield return new WaitForSeconds(1.5f);

        tx.text = "";
        tx.enabled = false;
    }

    public void OnDeath()
    {
        if (State == GameState.GAME)
        {
            this.State = GameState.GAMEOVER;
            this.UI.enabled = false;

            this.GameOverUI.enabled = true;
            this.GameOverUI.GetComponent<moveImage>().activate();
            this.GameOverUI.GetComponentInChildren<Text>().text = "$" + Money;

            var high = PlayerPrefs.GetInt("score", 0);
            if (high < Money)
            {
                PlayerPrefs.SetInt("score", Money);
                PlayerPrefs.Save();
                GameOverUI.transform.GetChild(2).GetComponent<Text>().text = "New Highscore: $" + Money;

                VO.HighScoreReaction();
            }
            else
            {
                GameOverUI.transform.GetChild(2).GetComponent<Text>().text = "Highscore: $" + high;

                VO.DeathReactions();
            }

        }
    }
}
