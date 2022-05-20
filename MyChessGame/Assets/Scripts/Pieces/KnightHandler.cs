using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightHandler : PieceAbstract
{
    List<Vector2> possibleSpaces = new List<Vector2>();

    public override void CheckForMovableSpaces()
    {
        SpaceHandler[] board = gameManager.GetBoard();

        PopulateKnightMoves();

        foreach(Vector2 possibleSpace in possibleSpaces)
        {
            foreach(SpaceHandler space in board)
            {
                if(possibleSpace == space.GetBoardLocation())
                {
                    bool? canCapture = CheckCanCapture(space, this);
                    switch (canCapture)
                    {
                        case null:
                            gameManager.AddMoveableSpace(space);
                            break;
                        case false:
                            break;
                        case true:
                            gameManager.AddMoveableSpace(space);
                            break;
                    }
                }
            }
        }

        possibleSpaces.Clear();
    }

    private void PopulateKnightMoves()
    {
        Vector2 space_1 = new Vector2(piecePosition.x + 1, piecePosition.y + 2);
        possibleSpaces.Add(space_1);
        Vector2 space_2 = new Vector2(piecePosition.x + 2, piecePosition.y + 1);
        possibleSpaces.Add(space_2);
        Vector2 space_3 = new Vector2(piecePosition.x + 1, piecePosition.y - 2);
        possibleSpaces.Add(space_3);
        Vector2 space_4 = new Vector2(piecePosition.x + 2, piecePosition.y - 1);
        possibleSpaces.Add(space_4);
        Vector2 space_5 = new Vector2(piecePosition.x - 1, piecePosition.y + 2);
        possibleSpaces.Add(space_5);
        Vector2 space_6 = new Vector2(piecePosition.x - 2, piecePosition.y + 1);
        possibleSpaces.Add(space_6);
        Vector2 space_7 = new Vector2(piecePosition.x - 1, piecePosition.y - 2);
        possibleSpaces.Add(space_7);
        Vector2 space_8 = new Vector2(piecePosition.x - 2, piecePosition.y - 1);
        possibleSpaces.Add(space_8);
    }
}
