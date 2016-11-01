using System.Runtime.Serialization;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Manages the Barrier class.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.GameObject" />
    public class Barrier : GameObject
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Barrier" /> class.
        /// </summary>
        public Barrier()
        {
            Sprite = new BarrierSprite();
        }
    }
}