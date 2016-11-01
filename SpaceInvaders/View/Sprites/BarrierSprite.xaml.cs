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
        #region Data members

        private readonly List<Rectangle> barrierPiecesList;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BarrierSprite" /> class.
        ///     Precondition: none
        ///     Postconditon: Sprite created.
        /// </summary>
        public BarrierSprite()
        {
            this.InitializeComponent();
            this.barrierPiecesList = new List<Rectangle>();
            this.addAllBarrierPieces();
        }

        #endregion

        #region Methods

        private void addAllBarrierPieces()
        {
            this.addTopLeftToList();
            this.addTopMiddleToList();
            this.addTopRightToList();
            this.addBottomLeftToList();
            this.addBottomMiddleToList();
            this.addBottomRightToList();
        }

        private void addBottomRightToList()
        {
            this.barrierPiecesList.Add(this.bottomRight1);
            this.barrierPiecesList.Add(this.bottomRight2);
            this.barrierPiecesList.Add(this.bottomRight3);
            this.barrierPiecesList.Add(this.bottomRight4);
            this.barrierPiecesList.Add(this.bottomRight5);
            this.barrierPiecesList.Add(this.bottomRight6);
        }

        private void addBottomMiddleToList()
        {
            this.barrierPiecesList.Add(this.bottomMiddle1);
            this.barrierPiecesList.Add(this.bottomMiddle2);
            this.barrierPiecesList.Add(this.bottomMiddle3);
            this.barrierPiecesList.Add(this.bottomMiddle4);
            this.barrierPiecesList.Add(this.bottomMiddle5);
        }

        private void addBottomLeftToList()
        {
            this.barrierPiecesList.Add(this.bottomLeft1);
            this.barrierPiecesList.Add(this.bottomLeft2);
            this.barrierPiecesList.Add(this.bottomLeft3);
            this.barrierPiecesList.Add(this.bottomLeft4);
            this.barrierPiecesList.Add(this.bottomLeft5);
        }

        private void addTopRightToList()
        {
            this.barrierPiecesList.Add(this.topRight1);
            this.barrierPiecesList.Add(this.topRight2);
            this.barrierPiecesList.Add(this.topRight3);
            this.barrierPiecesList.Add(this.topRight4);
            this.barrierPiecesList.Add(this.topRight5);
            this.barrierPiecesList.Add(this.topRight6);
        }

        private void addTopMiddleToList()
        {
            this.barrierPiecesList.Add(this.topMiddle1);
            this.barrierPiecesList.Add(this.topMiddle2);
            this.barrierPiecesList.Add(this.topMiddle3);
            this.barrierPiecesList.Add(this.topMiddle4);
            this.barrierPiecesList.Add(this.topMiddle5);
            this.barrierPiecesList.Add(this.topMiddle6);
        }

        private void addTopLeftToList()
        {
            this.barrierPiecesList.Add(this.topLeft1);
            this.barrierPiecesList.Add(this.topLeft2);
            this.barrierPiecesList.Add(this.topLeft3);
            this.barrierPiecesList.Add(this.topLeft4);
            this.barrierPiecesList.Add(this.topLeft5);
            this.barrierPiecesList.Add(this.topLeft6);
        }

        /// <summary>
        ///     Removes the top left piece.
        /// </summary>
        public void RemoveTopLeftPiece()
        {
            this.disableRectanglesInPiece("topLeft");
        }

        /// <summary>
        ///     Removes the top middle piece.
        /// </summary>
        public void RemoveTopMiddlePiece()
        {
            this.disableRectanglesInPiece("topMiddle");
        }

        /// <summary>
        ///     Removes the top right piece.
        /// </summary>
        public void RemoveTopRightPiece()
        {
            this.disableRectanglesInPiece("topRight");
        }

        /// <summary>
        ///     Removes the bottom left piece.
        /// </summary>
        public void RemoveBottomLeftPiece()
        {
            this.disableRectanglesInPiece("bottomLeft");
        }

        /// <summary>
        ///     Removes the bottom middle piece.
        /// </summary>
        public void RemoveBottomMiddlePiece()
        {
            this.disableRectanglesInPiece("bottomMiddle");
        }

        /// <summary>
        ///     Removes the bottom right piece.
        /// </summary>
        public void RemoveBottomRightPiece()
        {
            this.disableRectanglesInPiece("bottomRight");
        }

        private void disableRectanglesInPiece(string rectanglePieceId)
        {
            foreach (var currentRectangle in this.barrierPiecesList)
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