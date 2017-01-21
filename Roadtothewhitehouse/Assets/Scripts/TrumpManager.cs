using System;
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
            if (Input.GetKeyUp(KeyCode.Space))
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
            if (Input.GetKeyUp(KeyCode.Space))
            {
                this.State = GameState.MENU;
                this.GameOverUI.GetComponent<moveImage>().deactivate();
                this.GameOverUI.enabled = false;
                this.TitleUI.enabled = true;
            }
        }

    }

    private void UpdateMoney()
    {
        Money += (int)(Time.deltaTime * 100);

        UI.GetComponentInChildren<Text>().text = "$" + Money;
    }

    public void OnDeath()
    {
        if (State == GameState.GAME)
        {
            this.State = GameState.GAMEOVER;
            this.UI.enabled = false;

            VO.DeathReactions();

            this.GameOverUI.enabled = true;
            this.GameOverUI.GetComponent<moveImage>().activate();
            this.GameOverUI.GetComponentInChildren<Text>().text = "$" + Money;
        }
    }
}
