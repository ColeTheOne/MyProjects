using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnHandler : MonoBehaviour
{
    [SerializeField] private GameManager gameManager = null;
    [SerializeField] private Text whitesTurnText = null;
    [SerializeField] private Text blacksTurnText = null;

    private PieceColor currentTurn = PieceColor.White;

    public PieceColor GetCurrentTurn() { return currentTurn; }

    public void SetCurrentTurn()
    {
        if(currentTurn == PieceColor.White)
            currentTurn = PieceColor.Black;
        else if(currentTurn == PieceColor.Black)
            currentTurn = PieceColor.White;

        UpdateTurnUI(currentTurn);

        UpdateCanEnPassant(currentTurn);
    }

    private void UpdateTurnUI(PieceColor currentTurn)
    {
        switch (currentTurn)
        {
            case PieceColor.White:
                whitesTurnText.enabled = true;
                blacksTurnText.enabled = false;
                break;
            case PieceColor.Black:
                blacksTurnText.enabled = true;
                whitesTurnText.enabled = false;
                break;
        }
    }

    private void UpdateCanEnPassant(PieceColor currentTurn)
    {
        SpaceHandler[] board = gameManager.GetBoard();

        if (currentTurn == PieceColor.White)
        {
            foreach(SpaceHandler space in board)
            {
                if(space.GetBoardLocation().y  == 4)
                    space.SetCanEnPassant(false);
            }
        }

        if (currentTurn == PieceColor.Black)
        {
            foreach(SpaceHandler space in board)
            {
                if (space.GetBoardLocation().y == 5)
                    space.SetCanEnPassant(false);
            }
        }
    }
}
