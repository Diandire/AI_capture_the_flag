using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates the score when the enemy flag is dropped inside the friendly base
/// The score is updated every second
/// </summary>
public class SetScore : MonoBehaviour
{
    public GameObject EnemyFlag,FriendlyFlag;
    public int Score;
    public Text scoreTextObject;

    private bool _enemyFlagInBase;
    private const float ScoreTickDuration = 1.0f;

    /// <summary>
    /// Collision with base trigger
    /// </summary>
    /// <param name="other">the collidee</param>
    void OnTriggerEnter(Collider other)
    {
        // Only react to the enemy flag
        if(other.gameObject.name.Equals(EnemyFlag.name))
        {
            _enemyFlagInBase = true;
            Debug.Log("Scored a point");
            switch(EnemyFlag.name)
            {
                case Names.RedFlag :
                    BlackBoard.RedFlagTaken=true;break;

                case Names.BlueFlag :
                    BlackBoard.BlueFlagTaken=true;break;
            }
            StartCoroutine(UpdateScore());
        }
        else if(other.gameObject.name.Equals(FriendlyFlag.name))
        {
            switch(other.gameObject.name)
            {
                case Names.RedFlag :
                    BlackBoard.RedFlagInBase=true;break;

                case Names.BlueFlag :
                    BlackBoard.BlueFlagInBase=true;break;
            }
        }
    }

    /// <summary>
    /// The object has left the base
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        // only react to the enemy flag
        if (other.gameObject.name.Equals(EnemyFlag.name))
        {
            _enemyFlagInBase = false;
            switch(other.gameObject.name)
            {
                case Names.RedFlag :
                    BlackBoard.RedFlagTaken=false;break;

                case Names.BlueFlag :
                    BlackBoard.BlueFlagTaken=false;break;
            }
        }
        else if(other.gameObject.name.Equals(FriendlyFlag.name))
        {
            switch(other.gameObject.name)
            {
                case Names.RedFlag :
                    BlackBoard.RedFlagInBase=false;break;

                case Names.BlueFlag :
                    BlackBoard.BlueFlagInBase=false;break;
            }
        }
    }

    /// <summary>
    /// This actually updates the score every second while the flag is in the base
    /// There is no upper limit to the score
    /// </summary>
    /// <returns>Enmuerator for Coroutine</returns>
    IEnumerator UpdateScore()
    {
        // The score updates as long as the flag is in the base
        while(_enemyFlagInBase)
        {
            yield return new WaitForSeconds(ScoreTickDuration);
            Score++;
            scoreTextObject.text=Score.ToString();
        }
    }
    
}
