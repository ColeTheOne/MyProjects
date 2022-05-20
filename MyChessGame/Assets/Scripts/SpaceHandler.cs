using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceHandler : MonoBehaviour
{
    [SerializeField] private PieceAbstract playingPiece = null;
    [SerializeField] private SpriteRenderer moveableIcon = null;

    private Vector2 boardLocation;
    public bool isMoveable = false;
    private bool canEnPassant = false;
    private bool isEnPassant = false;

    public Vector2 GetBoardLocation() { return boardLocation; }

    public PieceAbstract GetPiece() { return playingPiece; }

    public bool GetCanEnPassant() { return canEnPassant; }

    public bool GetIsEnPassant() { return isEnPassant; }

    public void SetPiece(PieceAbstract piece) { playingPiece = piece; }

    public void SetCanEnPassant(bool canEnPassant) { this.canEnPassant = canEnPassant; }

    public void SetIsEnPassant(bool isEnPassant) { this.isEnPassant = isEnPassant; }

    public void ClearOccupied() 
    {
        playingPiece = null;
    }

    void Start()
    {
        boardLocation.x = transform.position.x;
        boardLocation.y = transform.position.z;
    }

    public void SetMoveableIconOn()
    {
        isMoveable = true;
        moveableIcon.enabled = true;
    }

    public void SetMoveableIconOff()
    {
        isMoveable = false;
        moveableIcon.enabled = false;
    }
}
