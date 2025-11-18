using System.Collections.Generic;
using ChessEngine;
using ChessEngine.Game;

public interface ISkill
{
    void Execute();
    bool canExecute();
}
public interface IPieceSkill
{
    void LoadSelector(List<ChessPieceType> pieceTypes, bool isAll, ChessColor color);
    void Execute(List<VisualChessPiece> pieces);
}

public interface ITableSkill
{
    void LoadSelector();
    void Execute(List<VisualChessTableTile> tiles);
}
public enum SkillType
{
    Imme = 0,
    Piece,
    Table
}

public enum PieceSkillType
{
    Revenge = 0,
    Shield,
    FaseMove,
    ChangeKnight,
    RookInverse,
    ChaosKnight,
    ReturnPiece,
    IsPin,
    IsMoveSide,
    IsSnakePawn,
    GodsOne,
    DimensionBreak,
    Any,
    TopChange,
    FastReturn
}
