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
        private Quaternion _unrotation;
        private double _orientation;

        private TMagnet Magnet { get; }

        /// <summary>
        /// Position in 2D space
        /// </summary>
        public Vector2 Position { get; }

        /// <summary>
        /// Orientation in degrees. Zero places the magnet's pole on the co-ordinate system X axis
        /// </summary>
        public double Orientation
        {
            get => _orientation;
            set
            {
                _orientation = value;
                _unrotation = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), (float)(-value * Scale.DegreesToRadians));
            }
        }

        public MagnetWithPosition2(TMagnet magnet, Vector2 vector2)
        {
            Magnet = magnet;
            Position = vector2;
        }

        public Vector2 H(Vector2 position)
        {
            // Transform point to magnet's 3D space

            // co-ordinate relative to magnet center
            var transformed2 = position - Position;

            // Rotate according to magnet's orientation
            transformed2 = Vector2.Transform(transformed2, _unrotation);

            var h = Magnet.H(new Vector3(0, transformed2.Y, transformed2.X));

            return new Vector2(h.Z, h.Y);
        }

        public Vector2 B(Vector2 position)
        {
            return Magnets.Magnet.HToB(H(position));
        }

        public double Remanence
        {
            get => Magnet.Remanence;
            set => Magnet.Remanence = value;
        }

        public double SurfaceField
        {
            get => Magnet.SurfaceField;
            set => Magnet.SurfaceField = value;
        }
    }
}
