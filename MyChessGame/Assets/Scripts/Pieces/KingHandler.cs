using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingHandler : PieceAbstract
{
    private bool hasMoved = false;

    List<Vector2> possibleSpaces = new List<Vector2>();

    public bool GetHasMoved() { return hasMoved; }

    public void SetHasMoved() { hasMoved = true; }

    public override void CheckForMovableSpaces()
    {
        SpaceHandler[] board = gameManager.GetBoard();

        PopulateKingMoves();

        foreach (Vector2 possibleSpace in possibleSpaces)
        {
            foreach (SpaceHandler space in board)
            {
                if (possibleSpace == space.GetBoardLocation())
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

        if (hasMoved)
            return;

        if (CanCastleLeft())
        {
            foreach(SpaceHandler space in board)
            {
                if(space.GetBoardLocation().x == piecePosition.x - 2 && space.GetBoardLocation().y == piecePosition.y)
                    gameManager.AddMoveableSpace(space);
            }
        }

        if (CanCastleRight())
        {
            foreach (SpaceHandler space in board)
            {
                if (space.GetBoardLocation().x == piecePosition.x + 2 && space.GetBoardLocation().y == piecePosition.y)
                    gameManager.AddMoveableSpace(space);
            }
        }
    }

    private void PopulateKingMoves()
    {
        Vector2 space_1 = new Vector2(piecePosition.x + 1, piecePosition.y);
        possibleSpaces.Add(space_1);
        Vector2 space_2 = new Vector2(piecePosition.x + 1, piecePosition.y + 1);
        possibleSpaces.Add(space_2);
        Vector2 space_3 = new Vector2(piecePosition.x, piecePosition.y + 1);
        possibleSpaces.Add(space_3);
        Vector2 space_4 = new Vector2(piecePosition.x - 1, piecePosition.y + 1);
        possibleSpaces.Add(space_4);
        Vector2 space_5 = new Vector2(piecePosition.x - 1, piecePosition.y);
        possibleSpaces.Add(space_5);
        Vector2 space_6 = new Vector2(piecePosition.x - 1, piecePosition.y - 1);
        possibleSpaces.Add(space_6);
        Vector2 space_7 = new Vector2(piecePosition.x, piecePosition.y - 1);
        possibleSpaces.Add(space_7);
        Vector2 space_8 = new Vector2(piecePosition.x + 1, piecePosition.y - 1);
        possibleSpaces.Add(space_8);
    }

    private bool CanCastleLeft()
    {
        SpaceHandler[] board = gameManager.GetBoard();

        for(int i = 1; i <= 3; i++)
        {
            foreach(SpaceHandler space in board)
            {
                if(space.GetBoardLocation().x == piecePosition.x - i && space.GetBoardLocation().y == piecePosition.y)
                {
                    if (space.GetPiece())
                        return false;
                }
            }
        }

        foreach(SpaceHandler space in board)
        {
            if (space.GetBoardLocation().x == piecePosition.x - 4 && space.GetBoardLocation().y == piecePosition.y)
            {
                PieceAbstract piece = space.GetPiece();

                if (piece.gameObject.TryGetComponent<RookHandler>(out RookHandler rookHandler))
                    if (!rookHandler.GetHasMoved())
                        return true;
            }
        }
        return false;
    }

    private bool CanCastleRight()
    {
        SpaceHandler[] board = gameManager.GetBoard();

        for (int i = 1; i <= 2; i++)
        {
            foreach (SpaceHandler space in board)
            {
                if (space.GetBoardLocation().x == piecePosition.x + i && space.GetBoardLocation().y == piecePosition.y)
                {
                    if (space.GetPiece())
                        return false;
                }
            }
        }

        foreach (SpaceHandler space in board)
        {
            if (space.GetBoardLocation().x == piecePosition.x + 3 && space.GetBoardLocation().y == piecePosition.y)
            {
                PieceAbstract piece = space.GetPiece();

                if (piece.gameObject.TryGetComponent<RookHandler>(out RookHandler rookHandler))
                    if (!rookHandler.GetHasMoved())
                        return true;
            }
        }
        return false;
    }

    public void TryCastle(SpaceHandler space)
    {
        if (space.GetBoardLocation().x - GetPiecePosition().x == 2)
        {
            CastleRight();
            gameManager.MovePiece(this, space, GetSpaceHandler());
            gameManager.UpdateHasMoved(this);
        }
        else if (GetPiecePosition().x - space.GetBoardLocation().x == 2)
        {
            CastleLeft();
            gameManager.MovePiece(this, space, GetSpaceHandler());
            gameManager.UpdateHasMoved(this);
        }
        else
        {
            gameManager.MovePiece(this, space, GetSpaceHandler());
            gameManager.UpdateHasMoved(this);
        }
    }

    private void CastleRight()
    {
        SpaceHandler[] board = gameManager.GetBoard();

        PieceAbstract rook = null;

        foreach (SpaceHandler space in board)
        {
            if (space.GetBoardLocation().y == GetPiecePosition().y && space.GetBoardLocation().x == GetPiecePosition().x + 3)
            {
                rook = space.GetPiece();
                break;
            }
        }

        SpaceHandler newSpace = null;

        foreach (SpaceHandler space in board)
        {
            if (space.GetBoardLocation().y == GetPiecePosition().y && space.GetBoardLocation().x == GetPiecePosition().x + 1)
            {
                newSpace = space;
                break;
            }
        }

        gameManager.MovePiece(rook, newSpace, rook.GetSpaceHandler(), false);
        gameManager.UpdateHasMoved(rook);
    }

    private void CastleLeft()
    {
        SpaceHandler[] board = gameManager.GetBoard();

        PieceAbstract rook = null;

        foreach (SpaceHandler space in board)
        {
            if (space.GetBoardLocation().y == GetPiecePosition().y && space.GetBoardLocation().x == GetPiecePosition().x - 4)
            {
                rook = space.GetPiece();
                break;
            }
        }

        SpaceHandler newSpace = null;

        foreach (SpaceHandler space in board)
        {
            if (space.GetBoardLocation().y == GetPiecePosition().y && space.GetBoardLocation().x == GetPiecePosition().x - 1)
            {
                newSpace = space;
                break;
            }
        }

        gameManager.MovePiece(rook, newSpace, rook.GetSpaceHandler(), false);
        gameManager.UpdateHasMoved(rook);
    }
}
