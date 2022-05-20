using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //[SerializeField] private float speed = 5f;
    [SerializeField] private SpaceHandler[] boardSpaces = new SpaceHandler[64];
    [SerializeField] public List<PieceAbstract> whitePieces = new List<PieceAbstract>();
    [SerializeField] public List<PieceAbstract> blackPieces = new List<PieceAbstract>();
    [SerializeField] public GameObject whiteQueenPreFab = null;
    [SerializeField] public GameObject blackQueenPreFab = null;
    [SerializeField] private TurnHandler turnHandler = null;
    [SerializeField] private LayerMask layerMask = new LayerMask();

    public List<SpaceHandler> moveableSpaces = new List<SpaceHandler>();
    private PieceAbstract selectedPiece;
    
    public SpaceHandler[] GetBoard() { return boardSpaces; }

    public List<PieceAbstract> GetWhitePieces() { return whitePieces; }

    public List <PieceAbstract> GetBlackPieces() { return blackPieces; }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                ClearMoveableSpace();
                return;
            }

            if (hit.collider.TryGetComponent<PieceAbstract>(out PieceAbstract piece))
            {
                if (turnHandler.GetCurrentTurn() != piece.GetPieceColor() && CheckCanCapture(piece))
                {
                    ClearMoveableSpace();
                    MovePiece(selectedPiece, piece.GetSpaceHandler(), selectedPiece.GetSpaceHandler());
                    return;
                }
                else if (turnHandler.GetCurrentTurn() != piece.GetPieceColor())
                {
                    ClearMoveableSpace();
                    return;
                }

                ClearMoveableSpace();
                selectedPiece = piece;
                piece.GetSpaceHandler().SetMoveableIconOn();
                piece.CheckForMovableSpaces();
                return;
            }

            if (hit.collider.TryGetComponent<SpaceHandler>(out SpaceHandler space))
            {
                if(space == selectedPiece.GetSpaceHandler())
                {
                    ClearMoveableSpace();
                    return;
                }
                    
                if (space.isMoveable && selectedPiece.gameObject.TryGetComponent<KingHandler>(out KingHandler king))
                {
                    king.TryCastle(space);
                }
                else if(space.isMoveable && space.GetIsEnPassant())
                {
                    switch (selectedPiece.GetPieceColor()) 
                    {
                        case PieceColor.White:
                            MovePiece(selectedPiece, space, selectedPiece.GetSpaceHandler());
                            CaptureEnPassantWhite(space);
                            break;
                        case PieceColor.Black:
                            MovePiece(selectedPiece, space, selectedPiece.GetSpaceHandler());
                            CaptureEnPassantBlack(space);
                            break;
                    }
                }
                else if (space.isMoveable)
                {
                    MovePiece(selectedPiece, space, selectedPiece.GetSpaceHandler());
                    UpdateHasMoved(selectedPiece);
                }

                if (selectedPiece.gameObject.TryGetComponent<PawnHandler>(out PawnHandler pawn))
                {
                    if ((pawn.GetPieceColor() == PieceColor.White && pawn.GetPiecePosition().y == 8) || (pawn.GetPieceColor() == PieceColor.Black && pawn.GetPiecePosition().y == 1))
                    {
                        pawn.Promote();
                    }
                }

                ClearMoveableSpace();
            }
        }
    }

    public void AddMoveableSpace(SpaceHandler space)
    {
        moveableSpaces.Add(space);

        space.SetMoveableIconOn();
    }

    private void ClearMoveableSpace() 
    { 
        foreach(SpaceHandler space in boardSpaces) 
        { 
            space.SetIsEnPassant(false);
            space.SetMoveableIconOff(); 
        }

        moveableSpaces.Clear(); 
    }

    public void MovePiece(PieceAbstract piece, SpaceHandler newSpace, SpaceHandler oldSpace, bool specialMove = true)
    {
        piece.gameObject.transform.position = new Vector3(newSpace.transform.position.x, piece.transform.position.y, newSpace.transform.position.z);

        if(newSpace.GetPiece() != null)
            CapturePiece(newSpace.GetPiece());

        piece.SetSpaceHandler(newSpace);

        oldSpace.ClearOccupied();

        newSpace.SetPiece(piece);

        if (specialMove)
            turnHandler.SetCurrentTurn();
    }

    public void CapturePiece(PieceAbstract capturedPiece)
    {
        switch (capturedPiece.GetPieceColor())
        {
            case PieceColor.White:
                whitePieces.Remove(capturedPiece);
                break;
            case PieceColor.Black:
                blackPieces.Remove(capturedPiece);
                break;
        }
        Destroy(capturedPiece.gameObject);
    }

    private bool CheckCanCapture(PieceAbstract piece)
    {
        foreach(SpaceHandler space in moveableSpaces)
        {
            PieceAbstract _piece = space.GetPiece();

            if (_piece == piece)
                return true;
        }
        return false;
    }

    public void UpdateHasMoved(PieceAbstract piece)
    {
        if (piece.gameObject.TryGetComponent<PawnHandler>(out PawnHandler pawnHandler))
            pawnHandler.SetHasMoved();

        if(piece.gameObject.TryGetComponent<RookHandler>(out RookHandler rookHandler))
            rookHandler.SetHasMoved();

        if (piece.gameObject.TryGetComponent<KingHandler>(out KingHandler kingHandler))
            kingHandler.SetHasMoved();
    }

    private void CaptureEnPassantWhite(SpaceHandler newSpace)
    {
        foreach(SpaceHandler space in boardSpaces)
        {
            if(space.GetBoardLocation().y == newSpace.GetBoardLocation().y - 1 && space.GetBoardLocation().x == newSpace.GetBoardLocation().x)
            {
                PieceAbstract pawn = space.GetPiece();
                CapturePiece(pawn);
                space.ClearOccupied();
            }
        }
    }

    private void CaptureEnPassantBlack(SpaceHandler newSpace)
    {
        foreach (SpaceHandler space in boardSpaces)
        {
            if (space.GetBoardLocation().y == newSpace.GetBoardLocation().y + 1 && space.GetBoardLocation().x == newSpace.GetBoardLocation().x)
            {
                PieceAbstract pawn = space.GetPiece();
                CapturePiece(pawn);
                space.ClearOccupied();
            }
        }
    }
}
