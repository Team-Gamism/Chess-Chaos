using System.Collections.Generic;
using ChessEngine;
using ChessEngine.Game;

public interface ICardSkill
{

    /// <summary>
    /// 스킬 사용 버튼을 눌렸을 때 실행되는 함수
    /// </summary>
    public virtual void Execute() { }

    /// <summary>
    /// 선택 완료 버튼이 눌렸을 때 실행되는 함수
    /// </summary>
    /// <param name="pieceData"></param>
    public virtual void Execute(PieceData pieceData) { }

}

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
    DimensionBreak
}
