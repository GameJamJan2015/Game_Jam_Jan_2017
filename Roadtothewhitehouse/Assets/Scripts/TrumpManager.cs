using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private PlayerScript Player;

    // Use this for initialization
    void Start()
    {
        this.State = GameState.MENU;
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
                Player.Spawn();
                VO.GameStartReaction();
            }
        }
        else if (State == GameState.GAME)
        {

        }
        else if (State == GameState.GAMEOVER)
        {

        }

    }

    public void OnDeath()
    {
        this.State = GameState.MENU;
        this.TitleUI.enabled = true;
    }
}
