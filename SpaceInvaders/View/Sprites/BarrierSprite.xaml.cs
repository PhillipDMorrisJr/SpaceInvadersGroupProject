using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;

namespace SpaceInvaders.View.Sprites
{
    /// <summary>
    ///     Draws a barrier.
    /// </summary>
    public sealed partial class BarrierSprite
    {
        private readonly List<Rectangle> barrierPiecesList;

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BarrierSprite" /> class.
        ///     Precondition: none
        ///     Postconditon: Sprite created.
        /// </summary>
        public BarrierSprite()
        {
            this.InitializeComponent();
            barrierPiecesList = new List<Rectangle>();
            addAllBarrierPieces();
        }

        #endregion

        #region Methods

        private void addAllBarrierPieces()
        {
            addTopLeftToList();
            addTopMiddleToList();
            addTopRightToList();
            addBottomLeftToList();
            addBottomMiddleToList();
            addBottomRightToList();
        }

        private void addBottomRightToList()
        {
            barrierPiecesList.Add(bottomRight1);
            barrierPiecesList.Add(bottomRight2);
            barrierPiecesList.Add(bottomRight3);
            barrierPiecesList.Add(bottomRight4);
            barrierPiecesList.Add(bottomRight5);
            barrierPiecesList.Add(bottomRight6);
        }

        private void addBottomMiddleToList()
        {
            barrierPiecesList.Add(bottomMiddle1);
            barrierPiecesList.Add(bottomMiddle2);
            barrierPiecesList.Add(bottomMiddle3);
            barrierPiecesList.Add(bottomMiddle4);
            barrierPiecesList.Add(bottomMiddle5);
        }

        private void addBottomLeftToList()
        {
            barrierPiecesList.Add(bottomLeft1);
            barrierPiecesList.Add(bottomLeft2);
            barrierPiecesList.Add(bottomLeft3);
            barrierPiecesList.Add(bottomLeft4);
            barrierPiecesList.Add(bottomLeft5);
        }

        private void addTopRightToList()
        {
            barrierPiecesList.Add(topRight1);
            barrierPiecesList.Add(topRight2);
            barrierPiecesList.Add(topRight3);
            barrierPiecesList.Add(topRight4);
            barrierPiecesList.Add(topRight5);
            barrierPiecesList.Add(topRight6);
        }

        private void addTopMiddleToList()
        {
            barrierPiecesList.Add(topMiddle1);
            barrierPiecesList.Add(topMiddle2);
            barrierPiecesList.Add(topMiddle3);
            barrierPiecesList.Add(topMiddle4);
            barrierPiecesList.Add(topMiddle5);
            barrierPiecesList.Add(topMiddle6);
        }

        private void addTopLeftToList()
        {
            barrierPiecesList.Add(topLeft1);
            barrierPiecesList.Add(topLeft2);
            barrierPiecesList.Add(topLeft3);
            barrierPiecesList.Add(topLeft4);
            barrierPiecesList.Add(topLeft5);
            barrierPiecesList.Add(topLeft6);
        }

        /// <summary>
        ///     Removes the top left piece.
        /// </summary>
        public void RemoveTopLeftPiece()
        {
            disableRectanglesInPiece("topLeft");
        }

        /// <summary>
        ///     Removes the top middle piece.
        /// </summary>
        public void RemoveTopMiddlePiece()
        {
            disableRectanglesInPiece("topMiddle");
        }

        /// <summary>
        ///     Removes the top right piece.
        /// </summary>
        public void RemoveTopRightPiece()
        {
            disableRectanglesInPiece("topRight");
        }

        /// <summary>
        ///     Removes the bottom left piece.
        /// </summary>
        public void RemoveBottomLeftPiece()
        {
            disableRectanglesInPiece("bottomLeft");
        }

        /// <summary>
        ///     Removes the bottom middle piece.
        /// </summary>
        public void RemoveBottomMiddlePiece()
        {
            disableRectanglesInPiece("bottomMiddle");
        }

        /// <summary>
        ///     Removes the bottom right piece.
        /// </summary>
        public void RemoveBottomRightPiece()
        {
            disableRectanglesInPiece("bottomRight");
        }

        private void disableRectanglesInPiece(string rectanglePieceId)
        {
            foreach (var currentRectangle in barrierPiecesList)
            {
                if (currentRectangle.Name.Contains(rectanglePieceId))
                {
                    currentRectangle.Visibility = Visibility.Collapsed;
                }
            }
        }

        #endregion
    }
}