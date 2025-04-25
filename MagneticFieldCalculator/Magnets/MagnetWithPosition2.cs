using System.Numerics;

namespace Magnets
{
    /// <summary>
    /// Magnet with position in 2D space
    /// </summary>
    /// <typeparam name="TMagnet"></typeparam>
    public class MagnetWithPosition2<TMagnet> : IMagnet
        where TMagnet : Magnet
    {
        private TMagnet Magnet { get; }

        /// <summary>
        /// Position in 2D space (units in metres)
        /// </summary>
        public Vector2 Position { get; }

        /// <summary>
        /// Orientation in degrees. Zero places the magnet's pole on the co-ordinate system X axis
        /// </summary>
        public double Orientation { get; set; }

        public MagnetWithPosition2(TMagnet magnet, Vector2 vector2)
        {
            Magnet = magnet;
            Position = vector2;
        }

        /// <inheritdoc />
        public double Remanence
        {
            get => Magnet.Remanence;
            set => Magnet.Remanence = value;
        }

        /// <inheritdoc />
        public double SurfaceField
        {
            get => Magnet.SurfaceField;
            set => Magnet.SurfaceField = value;
        }

        /// <inheritdoc />
        public Vector3 H(Vector3 position) => Magnet.H(position);

        /// <inheritdoc />
        public Vector3 B(Vector3 position) => Magnet.B(position);
    }
}
