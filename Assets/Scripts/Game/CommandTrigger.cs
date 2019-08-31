using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTrigger : MonoBehaviour {

    [SerializeField] private bool isFinal;
    [SerializeField] private bool isGameOver;

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();

        if (character != null)
        {
            if (isGameOver)
            {
                AudioManager.Instance().Play(AudioClips.OnFail);
                GameController.Instance().EndLevel();
                return;
            }

            character.EndCommand();

            if (isFinal)
            {
                AudioManager.Instance().Play(AudioClips.OnComplite);
                GameController.Instance().EndLevel(true);
            }

            if (!isFinal && !isGameOver)
                AudioManager.Instance().Play(AudioClips.OnEnter);
        }
    }
}
