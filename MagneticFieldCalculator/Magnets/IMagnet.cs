using System.Numerics;

namespace Magnets
{
    // Interface for a Magnet
    public interface IMagnet
    {
        /// <summary>
        /// Gets or sets the magnet's remanence (Tesla)
        /// </summary>
        /// <remarks>Field calculations are dependent on remanence, as is surface field. If <see cref="SurfaceField"/> is set, remanence will be calculated from it.</remarks>
        double Remanence { get; set; }

        /// <summary>
        /// Gets or sets the magnet's surface field (Tesla)
        /// </summary>
        /// <remarks>Throws <see cref="System.NotImplementedException"/> if getter not implemented.
        /// Default setter computes remanence, but depends on getter implementation</remarks>
        double SurfaceField { get; set; }

        /// <summary>
        /// Calculates field strength at specified position; pole aligned to Z axis
        /// </summary>
        /// <param name="position">Position (units in metres) relative to magnet's center (magnet's co-ordinate system)</param>
        /// <returns>Magnetization vector (Henry) (A/m) in magnet's co-ordinate system</returns>
        /// <remarks>Computes field strength vector in Henry (A/m) at specified position relative to magnet's co-ordinate system.
        /// By convention, Z axis is assigned to direction of magnet's North pole, X and Y are perpendicular.</remarks>
        Vector3 H(Vector3 position);

        /// <summary>
        /// Calculate field strength (flux density) at specified position; pole aligned to Z axis
        /// </summary>
        /// <param name="position">Position (units in metres) relative to magnet's center (magnet's co-ordinate system)</param>
        /// <returns>Field vector (Tesla) in magnet's co-ordinate system</returns>
        /// <remarks>Computes flux density vector in Henry (A/m) at specified position relative to magnet's co-ordinate system.
        /// By convention, Z axis is assigned to direction of magnet's North pole, X and Y are perpendicular.</remarks>
        Vector3 B(Vector3 position);
    };
}
