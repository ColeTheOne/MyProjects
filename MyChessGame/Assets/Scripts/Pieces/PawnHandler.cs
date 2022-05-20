using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnHandler : PieceAbstract
{
    private bool CanEnPassant = false;
    private bool hasMoved = false;
    private float moveModifier = 1;


    private bool GetCanEnPassant() { return CanEnPassant; }
    public void SetHasMoved() { hasMoved = true; }
    public void SetCanEnPassant() { CanEnPassant = false; }

    public override void CheckForMovableSpaces()
    {
        SpaceHandler[] board = gameManager.GetBoard();

        if (GetPieceColor() == PieceColor.Black)
            moveModifier = -1;

        foreach (SpaceHandler space in board)
        {
            if(space.GetBoardLocation().y == piecePosition.y + moveModifier && space.GetBoardLocation().x == piecePosition.x)
            {
                bool? canCapture = CheckCanCapture(space, this);
                if(canCapture == null)
                    gameManager.AddMoveableSpace(space);
                break;
            }
        }

        foreach (SpaceHandler space in board)
        {
            if (space.GetBoardLocation().y == piecePosition.y + moveModifier && space.GetBoardLocation().x == piecePosition.x + 1)
            {
                bool isEnPassant = CheckEnPassant(1);
                bool? canCapture = CheckCanCapture(space, this);
                if (canCapture == true || isEnPassant)
                    gameManager.AddMoveableSpace(space);
                if (isEnPassant)
                    space.SetIsEnPassant(true);
                break;
            }
        }

        foreach (SpaceHandler space in board)
        {
            if (space.GetBoardLocation().y == piecePosition.y + moveModifier && space.GetBoardLocation().x == piecePosition.x - 1)
            {
                bool isEnPassant = CheckEnPassant(-1);
                bool? canCapture = CheckCanCapture(space, this);
                if (canCapture == true || isEnPassant)
                    gameManager.AddMoveableSpace(space);
                if (isEnPassant)
                    space.SetIsEnPassant(true);
                break;
            }
        }

        if (gameManager.moveableSpaces.Count == 0)
            return;

        if (hasMoved || gameManager.moveableSpaces[0].GetBoardLocation().x != piecePosition.x)
            return;

        foreach (SpaceHandler space in board)
        {
            if (space.GetBoardLocation().y == piecePosition.y + (2 * moveModifier) && space.GetBoardLocation().x == piecePosition.x)
            {
                bool? canCapture = CheckCanCapture(space, this);
                if (canCapture == null)
                    gameManager.AddMoveableSpace(space);
                space.SetCanEnPassant(true);
                break;
            }
        }
    }

    public void Promote()
    {
        GameObject newQueen = null;

        if (GetPieceColor() == PieceColor.White)
        {
            newQueen = Instantiate(gameManager.whiteQueenPreFab, transform.position, Quaternion.identity);
            newQueen.GetComponent<PieceAbstract>().SetGameManager(gameManager);
            gameManager.whitePieces.Add(newQueen.GetComponent<PieceAbstract>());
        }
        else if(GetPieceColor() == PieceColor.Black)
        {
            newQueen = Instantiate(gameManager.blackQueenPreFab, transform.position, Quaternion.identity);
            newQueen.GetComponent<PieceAbstract>().SetGameManager(gameManager);
            gameManager.blackPieces.Add(newQueen.GetComponent<PieceAbstract>());
        }

        gameManager.MovePiece(newQueen.GetComponent<PieceAbstract>(), GetSpaceHandler(), GetSpaceHandler(), false);

        gameManager.CapturePiece(this);
    }

    private bool CheckEnPassant(float direction)
    {
        //Debug.Log("Checking");

        SpaceHandler[] board = gameManager.GetBoard();

        if((GetPieceColor() == PieceColor.White && piecePosition.y == 5) || (GetPieceColor()==PieceColor.Black && piecePosition.y == 4))
        {
            foreach (SpaceHandler space in board)
            {
                if (space.GetBoardLocation().y == piecePosition.y && space.GetBoardLocation().x == piecePosition.x + direction)
                {
                    if(space.GetCanEnPassant())
                        return true;
                }
            }
        }
        return false;
    }
}
