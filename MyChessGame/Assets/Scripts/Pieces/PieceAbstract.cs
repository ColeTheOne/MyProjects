using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceColor
{
    White,
    Black
}

public abstract class PieceAbstract : MonoBehaviour
{
    [SerializeField] protected GameManager gameManager = null;
    [SerializeField] protected SpaceHandler space = null;
    [SerializeField] private PieceColor pieceColor;

    protected Vector2 piecePosition;

    public PieceColor GetPieceColor() { return pieceColor; }

    public Vector2 GetPiecePosition() { return piecePosition; }

    public SpaceHandler GetSpaceHandler() { return space; }

    public void SetGameManager(GameManager gaMa) { gameManager = gaMa; }

    public void SetSpaceHandler(SpaceHandler space) 
    { 
        this.space = space; 
        piecePosition = space.GetBoardLocation();
    }

    public abstract void CheckForMovableSpaces();

    void Start()
    {
        piecePosition.x = transform.position.x;
        piecePosition.y = transform.position.z;
    }

    protected bool? CheckCanCapture(SpaceHandler space, PieceAbstract piece)
    {
        if (space.GetPiece() == false)
        {
            return null; //Space is not occupied
        }
        else if(piece.GetPieceColor() == space.GetPiece().GetPieceColor())
        {
            return false; //Space is occupied by piece of the same Color
        }
        else
        {
            return true; //Space is occupied by piece of a different Color
        }
    }
}
