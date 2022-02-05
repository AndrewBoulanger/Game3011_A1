using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum MiniGameState
{
    OutOfRange,
    InRange,
    GameActive
}

public class MinigameTrigger : MonoBehaviour
{

    [SerializeField]
    Canvas minigameCanvas;

    [SerializeField]
    TMPro.TextMeshProUGUI startTextPrompt;

    [SerializeField]
    GameObject minigamePanel;

    MiniGameState state = MiniGameState.OutOfRange; 

    PlayerMovement player;




    // Start is called before the first frame update
    void Start()
    {
        ChangeState(MiniGameState.OutOfRange);
    }

    // Update is called once per frame
    void Update()
    {
        if(state == MiniGameState.InRange)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {

                ChangeState(MiniGameState.GameActive);
            }
        }
        else if(state == MiniGameState.GameActive)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                ChangeState(MiniGameState.InRange);
            }
        }
    }

    void ChangeState(MiniGameState newState)
    {

        state = newState;
        if(newState == MiniGameState.OutOfRange)
        {
            minigamePanel.SetActive(false);
            startTextPrompt.enabled = false;
        }
        else if(newState == MiniGameState.InRange)
        {
            minigamePanel.SetActive(false);
            startTextPrompt.enabled = true;
            player.enabled = true;
            startTextPrompt.text = "Press F to play mini game";
        }
        else if(newState == MiniGameState.GameActive)
        {
            minigamePanel.SetActive(true);
            player.enabled = false;
            startTextPrompt.text = "Press Q to quit";
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerMovement>();
            ChangeState(MiniGameState.InRange);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            ChangeState(MiniGameState.OutOfRange);
        }
    }
}
