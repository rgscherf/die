using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{

    float moveTimer = 0.5f;
    float moveTimerCurrent = 0f;

    public Die currentDie;

    bool MoveInputDetected(PlayerInputs i)
    {
        return moveTimerCurrent > moveTimer && i.SendingMoveInputThisFrame();
    }

    void Start()
    {
        if (currentDie != null)
        {
            currentDie.IsSelected = true;
        }
        else
        {
            Debug.Log("currentdie is null");
        }
    }

    void Update()
    {
        moveTimerCurrent += Time.deltaTime;
        var i = new PlayerInputs(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));
        if (MoveInputDetected(i) && currentDie)
        {
            moveTimerCurrent = 0f;
            currentDie.DoMovement(i.MovementDirection(), moveTimer);
        }
    }
}
