using System;
using System.Numerics;

namespace Magnets
{
    /// <summary>
    /// Magnet with position in 2D space
    /// </summary>
    public class MagnetWithPosition2 : IMagnet
    {
        private IMagnet Magnet { get; }

        /// <summary>
        /// Position in 2D space (units in metres)
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Orientation in degrees. Zero aligns the magnet's pole with the co-ordinate system X axis
        /// </summary>
        public double Orientation { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MagnetWithPosition2()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="magnet">Underlying magnet</param>
        /// <param name="position">Initial position in 2D space</param>
        /// <param name="orientation">Orientation angle (degrees)</param>
        public MagnetWithPosition2(IMagnet magnet, Vector2 position, double orientation)
        {
            Magnet = magnet;
            Position = position;
            Orientation = orientation;
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

        /// <summary>
        /// Not supported
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Vector3 H(Vector3 position) => throw new InvalidOperationException("Not valid for co-ordinates in 3-space; use Vector2 instead");

        /// <summary>
        /// Not supported
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Vector3 B(Vector3 position) => throw new InvalidOperationException("Not valid for co-ordinates in 3-space; use Vector2 instead");

        /// <summary>
        /// Calculates field strength at specified position; pole aligned to X axis
        /// </summary>
        /// <param name="position">Position (units in metres) relative to magnet's center (magnet's co-ordinate system)</param>
        /// <returns>Magnetization vector (Henry) (A/m) in magnet's co-ordinate system</returns>
        /// <remarks>Computes field strength vector in Henry (A/m) at specified position relative to magnet's co-ordinate system.</remarks>
        public Vector2 H(Vector2 position)
        {
            // Magnet is positioned at a specified point with a specified rotation, so:
            // - First back out position co-ordinate so that position is relative to magnet's co-ordinate origin
            // - Apply inverse of magnet's orientation (rotation) so that position is correctly located in magnet's co-ordinate space
            // - Compute H vector
            // - Apply rotation so that H vector is aligned with external co-ordinate system
            return Magnet.H((position - Position).Rotate(-Orientation)).Rotate(Orientation);
        }

        /// <summary>
        /// Calculate field strength (flux density) at specified position; pole aligned to X axis
        /// </summary>
        /// <param name="position">Position (units in metres) relative to magnet's center (magnet's co-ordinate system)</param>
        /// <returns>Field vector (Tesla) in magnet's co-ordinate system</returns>
        /// <remarks>Computes flux density vector in Tesla at specified position relative to magnet's co-ordinate system.</remarks>
        public Vector2 B(Vector2 position) => H(position).HToB();
    }
}
