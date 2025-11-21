using UnityEngine;
using UnityEngine.Events;
using ChessEngine.Game.Events;

namespace ChessEngine.Game
{
    /// <summary>
    /// A VisualChessTableTile component is to be attached to each individual tile that makes up a chess table, each tile holds information about intself like offset.
    /// </summary>
    /// Author: Intuitive Gaming Solutions
    public class VisualChessTableTile : MonoBehaviour
    {
        #region Editor Serialized Fields
        [Header("Settings - Materials")]
        [Tooltip("A reference to the material used for white team tiles.")]
        [SerializeField] Material m_WhiteTeamMaterial = null;
        [Tooltip("A reference to the material used for black team tiles.")]
        [SerializeField] Material m_BlackTeamMaterial = null;

        [Header("Settings - Positioning")]
        [Tooltip("The local space center position of the visual chess table tile.")]
        public Vector3 center;
        [Tooltip("The local space offset to apply when calculating the 'local position' of the tile.")]
        public Vector3 offset;
        [Tooltip("The alignment move to use when positioning the piece on the X axis.")]
        public AxisAlignment alignX = AxisAlignment.Upper;
        [Tooltip("The alignment move to use when positioning the piece on the Y axis.")]
        public AxisAlignment alignY = AxisAlignment.Center;
        [Tooltip("The alignment move to use when positioning the piece on the Z axis.")]
        public AxisAlignment alignZ = AxisAlignment.Lower;

        [Header("Settings - Dimensions")]
        [Tooltip("Is the visual chess table tile 2D?")]
        public bool is2D = false;
        [Tooltip("현재 타일이 이동할 수 있는 곳인가?")]
        public bool isTileblock = false;
        [Tooltip("The length of a tile.")]
        public float tileLength = 1f;
        [Tooltip("The width of a tile.")]
        public float tileWidth = 1f;
        [Tooltip("The height (or thickness) of a tile.")]
        public float tileHeight = 0.25f;
        #endregion
        #region Editor Serialized Event(s)
        [Header("Events")]
        [Tooltip("An event that is invoked when the position of the visual chess table tile is reset.\n\nArg0: VisualChessTableTile - The visual chess table tile involved in the event.")]
        public VisualChessTableTileUnityEvent PositionReset;
        [Tooltip("An event that is invoked whenever the material is reset for the visual chess table tile.\n\nArg0: VisualChessTableTile - The visual chess table tile involved in the event.")]
        public VisualChessTableTileUnityEvent MaterialReset;

        [Header("Events - Highlighting")]
        [Tooltip("An event that is invoked whenever the visual chess table tile is highlighted due to the tile being selected.")]
        public UnityEvent SelectHighlighted;
        [Tooltip("An event that is invoked whenever the visual chess table tile is highlighted due to a valid move to the tile.")]
        public UnityEvent MoveHighlighted;
        [Tooltip("An event that is invoked whenever the visual chess table tile is highlighted due to a valid attack to the tile.")]
        public UnityEvent AttackHighlighted;
        [Tooltip("An event that is invoked whenever the visual chess table tile is unhighlighted for any reason.")]
        public UnityEvent Unhighlighted;
        [Tooltip("타일이 블록되었을 때 실행되는 이벤트")]
        public UnityEvent TileBlocked;
        #endregion
        #region Public Properties
        /// <summary>Returns a reference to the ChessTableTile that this component visualizes.</summary>
        public ChessTableTile Tile { get; private set; }
        /// <summary>A reference to the VisualChessTable this visual tile is placed on.</summary>
        public VisualChessTable VisualChessTable { get; private set; }
        /// <summary>A reference to the active GameManager in the scene, set by VisualChessTable when it instantiates the tile.</summary>
        public ChessGameManager GameManager { get; internal set; }
        /// <summary>A reference to the Renderer associated with this tile.</summary>
        public Renderer Renderer { get; private set; }

        private TableSelector tableSelector;
        private PieceSelector pieceSelector;
        #endregion

        // Unity callback(s).
        #region Unity Callbacks
        void Awake()
        {
            // Find Renderer reference.
            Renderer = GetComponent<Renderer>();
        }
        #endregion

        // Public method(s).
        #region Initialization & Engine Interface
        /// <summary>Initializes the visual tile, positions it at the given index on the relevant VisualChessTable, and stores the relevant ChessTableTile reference.</summary>
        /// <param name="pVisualChessTable">The visual chess table to place the tile on.</param>
        /// <param name="pTile">The chess table to spawn the tile on.</param>
        public void Initialize(VisualChessTable pVisualChessTable, ChessTableTile pTile, int idx)
        {
            Tile = pTile;
            VisualChessTable = pVisualChessTable;

            isTileblock = false;

            // Reset the position of the tile.
            ResetPosition();
        }

        /// <summary>Resets the position of the visual chess table tile.</summary>
        public void ResetPosition()
        {
            transform.localPosition = GetLocalPosition(VisualChessTable);

            // Invoke the PositionReset Unity event.
            PositionReset?.Invoke(this);
        }

        public void CloseTileBlock()
        {
            Tile.SetTileBlock(isTileblock);
            Unhighlighted?.Invoke();
        }

        //타일 상태 업데이트
        public void UpdateTileBlock()
        {
            Tile.SetTileBlock(isTileblock);

            // Invoke the PositionReset Unity event.
            TileBlocked?.Invoke();
        }

        /// <summary>
        /// Returns the VisualChessPiece on this tile, otherwise null.
        /// Only valid when called after this tile has been placed on the board using 'Initialize'.
        /// </summary>
        /// <returns>VisualChessPiece on this tile, otherwise null.</returns>
        public VisualChessPiece GetVisualPiece()
        {
            // Try to find a chess piece on this tile.
            for (int i = 0; i < VisualChessTable.VisualPieceCount; ++i)
            {
                VisualChessPiece piece = VisualChessTable.GetVisualPieceByIndex(i);
                if (piece != null && piece.Piece.TileIndex == Tile.TileIndex)
                    return piece;
            }

            return null;
        }
        #endregion
        #region Positioning
        /// <summary>Computes and returns the local position of the chess table tile using the provided tile dimensions and offset(s).</summary>
        /// <param name="pChessTable"></param>
        /// <returns>the local position of the chess table tile using the provided tile dimensions and offset(s).</returns>
        public Vector3 GetLocalPosition(VisualChessTable pChessTable)
        {
            Vector3 localPosition;

            float xOffset = 0f;
            float yOffset = 0f;
            float zOffset = 0f;

            // Calculate offsets based on alignment modes.
            switch (alignX)
            {
                case AxisAlignment.Center:
                    xOffset = -(2 * tileWidth) + (tileWidth * Tile.TileIndex.x) + tileWidth / 2;
                    break;
                case AxisAlignment.Upper:
                case AxisAlignment.Lower:
                    xOffset = -(4 * tileWidth) + (tileWidth * Tile.TileIndex.x);
                    break;
            }

            if (is2D)
            {
                switch (alignY)
                {
                    case AxisAlignment.Center:
                        yOffset = -(2 * tileLength) + (tileLength * Tile.TileIndex.y) + tileLength / 2;
                        break;
                    case AxisAlignment.Upper:
                    case AxisAlignment.Lower:
                        yOffset = -(4 * tileLength) + (tileLength * Tile.TileIndex.y);
                        break;
                }
            }
            else
            {
                switch (alignY)
                {
                    case AxisAlignment.Center:
                        yOffset = tileHeight / 2;
                        break;
                    case AxisAlignment.Upper:
                        yOffset = tileHeight;
                        break;
                    case AxisAlignment.Lower:
                        yOffset = 0f;
                        break;
                }
            }

            if (!is2D)
            {
                switch (alignZ)
                {
                    case AxisAlignment.Center:
                        zOffset = -(2 * tileLength) + (tileLength * Tile.TileIndex.y) + tileLength / 2;
                        break;
                    case AxisAlignment.Upper:
                    case AxisAlignment.Lower:
                        zOffset = -(4 * tileLength) + (tileLength * Tile.TileIndex.y);
                        break;
                }
            }

            localPosition = new Vector3(xOffset, yOffset, zOffset) + offset;

            return localPosition;
        }

        /// <summary>
        /// Returns the exact world space position of the center of the visual chess table tile.
        /// Based on the local space 'center' field.
        /// </summary>
        /// <returns>The exact world space position of the center of the visual chess table tile.</returns>
        public Vector3 GetCenterPosition()
        {
            return transform.TransformPoint(center);
        }
        #endregion
        #region Rendering
        /// <summary>Resets the material on a given tile to it's appropriate color.</summary>
        public void ResetMaterial()
        {
            // Reset the material on this tile.
            if (Renderer != null)
            {
                Renderer.material = Tile.Color == ChessColor.White ? m_WhiteTeamMaterial : m_BlackTeamMaterial;
            }

            // Invoke the 'MaterialReset' Unity event.
            MaterialReset?.Invoke(this);
        }
        #endregion
        #region Selection
        /// <summary>Selects this tile on whatever GameManager is associated with this VisualChessTableTile.</summary>
        public void Select()
        {
            // if there is a valid GameManager referenced select this tile.
            if (GameManager != null && !GameManager.isCardSelect &&
                !GameManager.isPieceSelect && !GameManager.isPromotionSelect &&
                !GameManager.isCardInfo && !GameManager.isGameEnd)
            {
                GameManager.SelectTile(this);
            }
        }
        #endregion

        public void BtnSelect()
        {
            if (GameManager != null)
            {
                if (GameManager.isCardSelect)
                {
                    tableSelector = FindFirstObjectByType<TableSelector>();
                    if (GetVisualPiece() != null || isTileblock) return;
                    if (tableSelector.selectedTiles.Count < tableSelector.cardData.MaxZoneCnt)
                    {
                        if (!tableSelector.selectedTiles.Contains(this))
                            tableSelector.AddEntity(this);
                        else
                            tableSelector.DestroyEntity(this);
                    }
                    else
                    {
                        if (!tableSelector.selectedTiles.Contains(this))
                        {
                            tableSelector.DestroyFirstEntity();
                            tableSelector.AddEntity(this);
                        }
                        else
                        {
                            tableSelector.DestroyEntity(this);
                        }
                    }
                }
                else if (GameManager.isPieceSelect)
                {
                    pieceSelector = FindFirstObjectByType<PieceSelector>();

                    ChessPiece piece = GetVisualPiece()?.Piece;

                    if (piece == null || piece.Color != pieceSelector.SelectColor) return;

                    // 기존 VisualChessPiece의 변수에서 가져오던 bool 값을
                    // 라이브러리에 저장되어 있는 변수의 bool 값을 참조

                    // 보호막
                    if (pieceSelector.type == PieceSkillType.Shield)
                    {
                        if (piece.IsShield) return;
                        AddPieceSelectorAttribute();
                    }

                    // 초속 복귀
                    else if (pieceSelector.type == PieceSkillType.FastReturn)
                    {
                        if (piece.IsEmerReturn || 
                            piece.IsRevenge) return;
                        AddPieceSelectorAttribute();
                    }

                    // 반격
                    else if (pieceSelector.type == PieceSkillType.Revenge)
                    {
                        if (piece.IsRevenge ||
                            piece.IsEmerReturn) return;
                        AddPieceSelectorAttribute();
                    }
                    // faseMove -> fastMove
                    // 2칸이동
                    else if (pieceSelector.type == PieceSkillType.FaseMove)
                    {
                        if (piece.IsTwoMove ||
                            piece.IsMoveSide) return;
                        AddPieceSelectorAttribute();
                    }

                    // 대각선 이동
                    else if (pieceSelector.type == PieceSkillType.IsMoveSide)
                    {
                        if (piece.IsMoveSide ||
                            piece.IsTwoMove) return;
                        AddPieceSelectorAttribute();
                    }

                    // 암습의폰
                    else if (pieceSelector.type == PieceSkillType.IsSnakePawn)
                    {
                        if (piece.IsSnakePawn ||
                            piece.IsMoveSide ||
                            piece.IsTwoMove) return;
                        AddPieceSelectorAttribute();
                    }

                    // 고정
                    else if (pieceSelector.type == PieceSkillType.IsPin)
                    {
                        if (piece.IsPin) return;
                        AddPieceSelectorAttribute();
                    }

                    // 퀸 변환
                    else if (pieceSelector.type == PieceSkillType.GodsOne)
                    {
                        if (piece.IsShield ||
                            piece.IsRevenge ||
                            piece.IsTwoMove ||
                            piece.IsMoveSide ||
                            piece.IsSnakePawn ||
                            piece.IsPin) return;
                        AddPieceSelectorAttribute();
                    }

                    // 2개 기물 랜덤 이동
                    else if (pieceSelector.type == PieceSkillType.DimensionBreak)
                    {
                        if (piece.IsPin) return;
                        AddPieceSelectorAttribute();
                    }

                    // 모두 적용
                    else if (pieceSelector.type == PieceSkillType.Any)
                    {
                        if (piece.IsPin) return;
                        AddPieceSelectorAttribute();
                    }

                    // 혼란 나이트
                    else if (pieceSelector.type == PieceSkillType.ChaosKnight)
                    {
                        if (piece.IsPin) return;
                        AddPieceSelectorAttribute();
                    }

                    // 룩 이동
                    else if (pieceSelector.type == PieceSkillType.TopChange)
                    {
                        if (piece.IsPin)
                            return;

                        if (pieceSelector.IsMoveable(GetVisualPiece().Piece.Tile))
                        {
                            AddPieceSelectorAttribute();
                        }
                    }

                }
            }
        }

        private void AddPieceSelectorAttribute()
        {
            if (pieceSelector.selectedPieces.Count < pieceSelector.cardData.MaxPieceCount)
            {
                if (!pieceSelector.pieceTypes.Contains(GetVisualPiece().Piece.GetChessPieceType())) return;

                if (!pieceSelector.selectedPieces.Contains(GetVisualPiece()))
                    pieceSelector.AddEntity(this);
                else
                    pieceSelector.DestroyEntity(this);
            }
            else
            {
                if (!pieceSelector.pieceTypes.Contains(GetVisualPiece().Piece.GetChessPieceType())) return;

                if (!pieceSelector.selectedPieces.Contains(GetVisualPiece()))
                {
                    pieceSelector.DestroyFirstEntity();
                    pieceSelector.AddEntity(this);
                }
                else
                {
                    pieceSelector.DestroyEntity(this);
                }
            }
        }

        #region Interal Highlight Callback(s)
        /// <summary>Invoked to notify this component it's been highlighted due to a selection.</summary>
        internal void Internal_OnSelectHighlighted()
        {
            // Invoke the 'SelectHighlighted' Unity event.
            SelectHighlighted?.Invoke();
        }

        /// <summary>Invoked to notify this component it's been highlighted due to a valid move.</summary>
        internal void Internal_OnMoveHighlighted()
        {
            // Invoke the 'MoveHighlighted' Unity event.
            MoveHighlighted?.Invoke();
        }

        /// <summary>Invoked to notify this component it's been highlighted due to a valid attack.</summary>
        internal void Internal_OnAttackHighlighted()
        {
            // Invoke the 'AttackHighlighted' Unity event.
            AttackHighlighted?.Invoke();
        }

        /// <summary>Invoked to notify this component it's no longer highlighted.</summary>
        internal void Internal_OnUnhighlighted()
        {
            // Invoke the 'Unhighlighted' Unity event.
            Unhighlighted?.Invoke();
        }
        #endregion
    }
}
