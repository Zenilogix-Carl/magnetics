using System.Numerics;

namespace Magnets
{
    public abstract class Magnet
    {
        /// <summary>
        /// Permeability of free space
        /// </summary>
        public const double Mu0 = 4 * Math.PI * 1e-7;

        /// <summary>
        /// Remanence (Tesla)
        /// </summary>
        public double Remanence { get; set; }

        /// <summary>
        /// Surface field (Tesla)
        /// </summary>
        public abstract double SurfaceField { get; set; }

        /// <summary>
        /// Field strength at specified position (Henry); pole aligned to Z axis (Henry = A/m)
        /// </summary>
        /// <param name="position">Position in metres</param>
        /// <returns></returns>
        public abstract Vector3 H(Vector3 position);

        /// <summary>
        /// Field strength at specified position (Tesla); pole aligned to Z axis
        /// </summary>
        /// <param name="position">Position in metres</param>
        /// <returns>Field vector (Tesla)</returns>
        public Vector3 B(Vector3 position)
        {
            return Vector3.Multiply(H(position), (float)Mu0);
        }

        /// <summary>
        /// Computes and sets remanence from given field strength and distance
        /// </summary>
        /// <param name="fieldStrength">Tesla</param>
        /// <param name="distance">Distance from magnet center along Z (magnetization) axis</param>
        protected void SetRemanenceFromField(double fieldStrength, double distance)
        {
            Remanence = 1;
            Remanence = fieldStrength / B(new Vector3(0, 0, (float)distance)).Z;
        }
    }
}
