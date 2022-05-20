using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopHandler : PieceAbstract
{
    public override void CheckForMovableSpaces()
    {
        SpaceHandler[] board = gameManager.GetBoard();

        for (float i = 1; i <= 8; i++)
        {
            Vector2 checkPosition;
            checkPosition.y = piecePosition.y + i;
            checkPosition.x = piecePosition.x + i;

            foreach (SpaceHandler space in board)
            {
                Vector2 spaceLocation = space.GetBoardLocation();
                if (checkPosition == spaceLocation)
                {
                    bool? canCapture = CheckCanCapture(space, this);
                    switch (canCapture)
                    {
                        case null:
                            gameManager.AddMoveableSpace(space);
                            break;
                        case false:
                            i = 9;
                            break;
                        case true:
                            gameManager.AddMoveableSpace(space);
                            i = 9;
                            break;
                    }
                }
            }
            if (checkPosition.y >= 8 || checkPosition.x >= 8)
                break;
        }

        for (float i = 1; i <= 8; i++)
        {
            Vector2 checkPosition;
            checkPosition.y = piecePosition.y + i;
            checkPosition.x = piecePosition.x - i;

            foreach (SpaceHandler space in board)
            {
                Vector2 spaceLocation = space.GetBoardLocation();
                if (checkPosition == spaceLocation)
                {
                    bool? canCapture = CheckCanCapture(space, this);
                    switch (canCapture)
                    {
                        case null:
                            gameManager.AddMoveableSpace(space);
                            break;
                        case false:
                            i = 9;
                            break;
                        case true:
                            gameManager.AddMoveableSpace(space);
                            i = 9;
                            break;
                    }
                }
            }
            if (checkPosition.y >= 8 || checkPosition.x <= 1)
                break;
        }

        for (float i = 1; i <= 8; i++)
        {
            Vector2 checkPosition;
            checkPosition.y = piecePosition.y - i;
            checkPosition.x = piecePosition.x + i;

            foreach (SpaceHandler space in board)
            {
                Vector2 spaceLocation = space.GetBoardLocation();
                if (checkPosition == spaceLocation)
                {
                    bool? canCapture = CheckCanCapture(space, this);
                    switch (canCapture)
                    {
                        case null:
                            gameManager.AddMoveableSpace(space);
                            break;
                        case false:
                            i = 9;
                            break;
                        case true:
                            gameManager.AddMoveableSpace(space);
                            i = 9;
                            break;
                    }
                }
            }
            if (checkPosition.y <= 1 || checkPosition.x >= 8)
                break;
        }

        for (float i = 1; i <= 8; i++)
        {
            Vector2 checkPosition;
            checkPosition.y = piecePosition.y - i;
            checkPosition.x = piecePosition.x - i;

            foreach (SpaceHandler space in board)
            {
                Vector2 spaceLocation = space.GetBoardLocation();
                if (checkPosition == spaceLocation)
                {
                    bool? canCapture = CheckCanCapture(space, this);
                    switch (canCapture)
                    {
                        case null:
                            gameManager.AddMoveableSpace(space);
                            break;
                        case false:
                            i = 9;
                            break;
                        case true:
                            gameManager.AddMoveableSpace(space);
                            i = 9;
                            break;
                    }
                }
            }
            if (checkPosition.y <= 1 || checkPosition.x <= 1)
                break;
        }
    }
}
