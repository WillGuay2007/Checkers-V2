using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{

    [SerializeField] private Game game;

    //Infos: (X,Y) = (1,1) est en bas a gauche
    private Tile SelectedTile;
    private Tile[] UnsortedTiles;
    private List<Piece> UnsortedPieces;

    private Tile[,] Tiles = new Tile[6, 6]; // [x,y] de 1 à 5 inclus (L'index 0 est ignoré pour raisons personnelles)

    void Awake() // Le awake est pour par creer de conflit avec RealPlayer-> enabletileselection quand unsorted tiles peut potentiellement etre null.
    {
        UnsortedTiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
        UnsortedPieces = new List<Piece>(FindObjectsByType<Piece>(FindObjectsSortMode.None));
        InitializeTiles();
        InitializePieces();
    }

    //Ca update le selectedtile (la plupart du temps lorsque le joueur clique) et highlight tous les legal moves.
    public void UpdateSelectedTile(Tile NewSelectedTile)
    {
        if (NewSelectedTile == null)
        {
            SelectedTile = null;
            ResetAllTiles();
            return;
        }

        if (NewSelectedTile.HasPiece() && NewSelectedTile.OccupyingPiece.IsOpponent)
        {
            return;
        }

        SelectedTile = NewSelectedTile;
        ResetAllTiles();

        if (SelectedTile.HasPiece())
        {
            List<Move> LegalMoves = GetLegalMovesForPiece(SelectedTile.OccupyingPiece);
            foreach (Move move in LegalMoves)
            {
                move.Destination.HighlightAsLegalMove();
            }
        }
    }

    //Ca fait le move (deplacement et capture)
    public void MovePiece(Move move)
    {
        //Deplacer la piece avant de gerer la capture
        move.Piece.SetTile(move.Destination);


        //Gérer la promotion en roi dans ce bloc de code.
        //-------------------------------
        if (!move.Piece.IsKing)
        {
            int y = move.Destination.yPosition;

            if (!move.Piece.IsOpponent && y == 5) // Joueur atteint le haut
            {
                move.Piece.PromoteToKing();
                print("Un pion du joueur est promu Roi !");
            }
            else if (move.Piece.IsOpponent && y == 1) // AI atteint le bas
            {
                move.Piece.PromoteToKing();
                print("Un pion de l'AI est promu Roi !");
            }
        }
        //-------------------------------

        //Gérer la capture dans ce bloc de code.
        //-------------------------------
        if (move.IsCapture && move.CapturedPiece != null)
        {

            //Faire disparaitre la piece capturée
            //-------
            Tile capturedTile = move.CapturedPiece.GetTile();
            capturedTile.ClearPiece();

            UnsortedPieces.Remove(move.CapturedPiece);
            Destroy(move.CapturedPiece.gameObject);
            //-------

            //Cette ligne marche car la piece a deja ete deplacee avant, donc ca va prendre les legal moves de la nouvelle position.
            List<Move> nextMoves = GetLegalMovesForPiece(move.Piece);
            bool hasAnotherCapture = false;

            //Ca check si il y a une autre capture possible.
            foreach (Move m in nextMoves)
            {
                if (m.IsCapture)
                {
                    hasAnotherCapture = true;
                    break;
                }
            }

            //Si il n'a pas d'autre captures, il va aller aux 3 dernieres lignes (pour changer de tour). Sinon, il va appeler MovePiece recursivement.
            if (hasAnotherCapture)
            {
                //Capture multiple pour AI
                if (move.Piece.IsOpponent)
                {
                    List<Move> captureMoves = new List<Move>();
                    foreach (Move m in nextMoves)
                    {
                        if (m.IsCapture)
                            captureMoves.Add(m);
                    }
                    if (captureMoves.Count > 0)
                    {
                        Debug.Log("AI enchaine une capture");
                        Move next = captureMoves[Random.Range(0, captureMoves.Count)];

                        //Coroutine parce que je veut quil attende pour pas que ca soit instant
                        StartCoroutine(ChainAICapture(next));
                    }
                    return;
                }
                //Capture multiple pour le joueur
                else
                {
                    Debug.Log("Chaîne de capture obligatoire !");
                    ResetAllTiles();

                    foreach (Tile t in UnsortedTiles)
                        t.PlayerCanSelect = false;

                    foreach (Move m in nextMoves)
                    {
                        if (m.IsCapture)
                        {
                            m.Destination.HighlightAsLegalMove();
                            m.Destination.PlayerCanSelect = true;
                        }
                    }

                    return;
                }
            }
        }
        //-------------------------------

        ResetAllTiles();
        SelectedTile = null;
        game.SwitchTurn();
    }

    private IEnumerator ChainAICapture(Move nextMove)
    {
        yield return new WaitForSeconds(0.5f);
        MovePiece(nextMove);
    }

    //Quand c'est le tour du joueur.
    public void EnableTileSelection()
    {
        List<Move> captures = GetAllCaptures(false);

        if (IsInTerminalPosition(false)) {
            print("The player has lost.");
            game.EndGame();
            return; 
        }

        //Ca va highlight les captures obligatoires si il y en a sans avoir besoin de sélectioner les tuiles.
        if (captures.Count > 0)
        {
            Debug.Log("Captures obligatoires disponibles");
            ResetAllTiles();

            foreach (Move m in captures)
            {
                m.Destination.HighlightAsLegalMove();
                m.Destination.PlayerCanSelect = true;
            }
        }
        else
        {
            //activer la selection de toutes les tuiles.
            foreach (Tile tile in UnsortedTiles)
            {
                tile.PlayerCanSelect = true;
            }
        }
    }

    //Quand c'est le tour de l'AI
    public void DisableTileSelection()
    {
        foreach (Tile tile in UnsortedTiles)
        {
            tile.PlayerCanSelect = false;
            UpdateSelectedTile(null);
        }
    }

    //Regarde si la destination est un move legal de la tuile selectionée. si oui, retourner le move.
    public Move? GetMoveForDestination(Tile destination)
    {
        //Ce bloc est la pour faire en sorte que si tu clique sur une case legale (qui est une capture), ca va marcher meme si elle n'est pas "l'enfant " de la selectedtile
        List<Move> captures = GetAllCaptures(false);
        if (captures.Count > 0)
        {
            foreach (Move move in captures)
            {
                if (move.Destination == destination)
                    return move;
            }
            return null;
        }

        if (SelectedTile == null || !SelectedTile.HasPiece())
            return null;

        //Va seulement chercher dans les legal moves de la piece de la selected tile et non ceux globaux.
        List<Move> legalMoves = GetLegalMovesForPiece(SelectedTile.OccupyingPiece);
        foreach (Move move in legalMoves)
        {
            if (move.Destination == destination)
                return move;
        }

        return null;
    }

    //Ca te donne la piece de la tile selectionée. principalement s'en servir dans les autres classes.
    public Piece GetSelectedPiece()
    {
        return SelectedTile != null ? SelectedTile.OccupyingPiece : null;
    }

    //Note personnelle: si il y'a une capture possible, ca va retourner seulement les captures. Ca se complemente bien avec HasCaptures.
    public List<Move> GetAllLegalMoves(bool forOpponent)
    {
        if (HasCaptures(forOpponent))
            return GetAllCaptures(forOpponent);

        List<Move> moves = new List<Move>();

        foreach (Piece piece in UnsortedPieces)
        {
            if (piece.IsOpponent != forOpponent) continue;
            moves.AddRange(GetLegalMovesForPiece(piece));
        }

        return moves;
    }

    public bool IsInTerminalPosition(bool forOpponent)
    {
        return GetAllLegalMoves(forOpponent).Count <= 0;
    }

    public List<Move> GetAllCaptures(bool forOpponent)
    {
        List<Move> captures = new List<Move>();

        foreach (Piece piece in UnsortedPieces)
        {
            if (piece == null) continue;
            if (piece.IsOpponent != forOpponent) continue;

            List<Move> moves = GetLegalMovesForPiece(piece);
            foreach (Move m in moves)
            {
                if (m.IsCapture)
                    captures.Add(m);
            }
        }

        return captures;
    }

    public bool HasCaptures(bool forOpponent)
    {
        if (GetAllCaptures(forOpponent).Count > 0)
            return true;
        return false;
    }

    //Remettre la couleur originale de toutes les tuiles.
    private void ResetAllTiles()
    {
        foreach (Tile tile in UnsortedTiles)
        {
            tile.ResetColor();
        }
    }

    //Ca te donne une liste de tout les moves possibles d'une piece incluant les captures.
    private List<Move> GetLegalMovesForPiece(Piece piece)
    {
        List<Move> legalMoves = new List<Move>();
        int X = piece.GetTile().xPosition;
        int Y = piece.GetTile().yPosition;

        //Cette ligne créé soit un tableau avec les 2 directions si c'est un roi ou un avec une seule direction si c'est un pion.
        int[] directions = piece.IsKing ? new int[] { +1, -1 } : new int[] { piece.IsOpponent ? -1 : +1 };

        //Si il ya 2 directions, ca va loop chacunes d'entres elles afin de mettre tous les legal moves.
        foreach (int direction in directions)
        {
            // Gauche
            if (X - 1 >= 1 && Y + direction >= 1 && Y + direction <= 5)
            {
                Tile targetTile = Tiles[X - 1, Y + direction];
                if (!targetTile.HasPiece())
                {
                    legalMoves.Add(new Move(piece, targetTile));
                }
            }

            // Gauche capture
            if (X - 2 >= 1 && Y + direction * 2 >= 1 && Y + direction * 2 <= 5)
            {
                Tile middleTile = Tiles[X - 1, Y + direction];
                Tile landingTile = Tiles[X - 2, Y + direction * 2];
                if (middleTile.HasPiece() && !landingTile.HasPiece() && middleTile.OccupyingPiece.IsOpponent != piece.IsOpponent)
                {
                    legalMoves.Add(new Move(piece, landingTile, true, middleTile.OccupyingPiece));
                }
            }

            // Droite
            if (X + 1 <= 5 && Y + direction >= 1 && Y + direction <= 5)
            {
                Tile targetTile = Tiles[X + 1, Y + direction];
                if (!targetTile.HasPiece())
                {
                    legalMoves.Add(new Move(piece, targetTile));
                }
            }

            // Droite capture
            if (X + 2 <= 5 && Y + direction * 2 >= 1 && Y + direction * 2 <= 5)
            {
                Tile middleTile = Tiles[X + 1, Y + direction];
                Tile landingTile = Tiles[X + 2, Y + direction * 2];
                if (middleTile.HasPiece() && !landingTile.HasPiece() && middleTile.OccupyingPiece.IsOpponent != piece.IsOpponent)
                {
                    legalMoves.Add(new Move(piece, landingTile, true, middleTile.OccupyingPiece));
                }
            }
        }

        return legalMoves;
    }

    //Je fait le tableau de tiles et je donne la reference de board a chaque tiles.
    private void InitializeTiles()
    {
        foreach (Tile tile in UnsortedTiles)
        {
            // convertir la position du monde en grille -> (1,2,3,4,5) avec le +3 comme offset
            tile.xPosition = (int)tile.transform.position.x + 3;
            tile.yPosition = (int)tile.transform.position.y + 3;
            tile.SetBoard(this);

            Tiles[tile.xPosition, tile.yPosition] = tile;
        }
    }

    //connecter les pièce a leurs tile originale.
    private void InitializePieces()
    {
        foreach (Piece piece in UnsortedPieces)
        {
            int x = (int)piece.transform.position.x + 3;
            int y = (int)piece.transform.position.y + 3;

            Tile tile = Tiles[x, y];
            piece.SetTile(tile);
        }
    }

}
