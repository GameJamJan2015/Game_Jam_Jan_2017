using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceManager : MonoBehaviour {

    public AudioSource source;
    public AudioClip[] reactions;

    public AudioClip[] randomreactions;
    public AudioClip[] gamestartreactions;
    public AudioClip[] deathreactions;
    public AudioClip[] pickupPoints;
    public AudioClip[] highscorereactions;

    // Use this for initialization
    void Start () {
        

		
	}

    // Plays a random reaction when jumping over walls and such
    public void RandomReaction()
    {
        int randomIndex = Random.Range(0, randomreactions.Length);
        source.clip = randomreactions[randomIndex];
        source.Play();
    }

    // Plays a random hype for game start
    public void GameStartReaction()
    {
        int randomIndex = Random.Range(0, gamestartreactions.Length);
        source.clip = gamestartreactions[randomIndex];
        source.Play();
    }

    // Plays a random death for game over
    public void DeathReactions()
    {
        int randomIndex = Random.Range(0, deathreactions.Length);
        source.clip = deathreactions[randomIndex];
        source.Play();
    }

    // Plays a random dollar pickup
    public void PointsReactions()
    {
        int randomIndex = Random.Range(0, pickupPoints.Length);
        source.clip = pickupPoints[randomIndex];
        source.Play();
    }

    // Plays a random dollar pickup
    public void HighScoreReaction()
    {
        int randomIndex = Random.Range(0, highscorereactions.Length);
        source.clip = highscorereactions[randomIndex];
        source.Play();
    }

    // Plays a random sound effect ayy
    public void TestRandomEffect(AudioClip[] audiolist)
    {
        int randomIndex = Random.Range(0, audiolist.Length);
        source.clip = audiolist[randomIndex];
        source.Play();
    }

}
