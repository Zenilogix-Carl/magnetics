using System.Numerics;

namespace Magnets
{
    public static class MagnetHelper
    {
        /// <summary>
        /// Calculates field strength at specified position in two-dimensional space; pole aligned to x-axis
        /// </summary>
        /// <param name="magnet">This magnet</param>
        /// <param name="position">Test position in magnet's local co-ordinate system based on magnet center (values in metres)</param>
        /// <returns>Field strength (Henry) as a vector in two-dimensional space</returns>
        public static Vector2 H(this IMagnet magnet, Vector2 position) => magnet.H(position.ToVector3()).ToVector2();

        /// <summary>
        /// Calculates field strength (flux density) at specified position in two-dimensional space; pole aligned to x-axis
        /// </summary>
        /// <param name="magnet"></param>
        /// <param name="position">Test position in magnet's local co-ordinate system based on magnet center (values in metres)</param>
        /// <returns>Flux density (Tesla) as a vector in two-dimensional space.</returns>
        public static Vector2 B(this IMagnet magnet, Vector2 position) => magnet.B(position.ToVector3()).ToVector2();
    }
}
