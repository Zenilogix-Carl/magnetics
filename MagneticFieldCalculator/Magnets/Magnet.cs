using System;
using System.Numerics;

namespace Magnets
{
    public abstract class Magnet : IMagnet
    {
        /// <summary>
        /// Permeability of free space
        /// </summary>
        public const double Mu0 = 4 * Math.PI * 1e-7;

        /// <inheritdoc />
        public double Remanence { get; set; }

        /// <inheritdoc />
        public abstract double SurfaceField { get; set; }

        /// <summary>
        /// Field strength at specified position (Henry); pole aligned to Z axis (Henry = A/m)
        /// </summary>
        /// <param name="position">Position in metres</param>
        /// <returns>Magnetization vector (Henry)</returns>
        public abstract Vector3 H(Vector3 position);

        /// <summary>
        /// Field strength at specified position (Tesla); pole aligned to Z axis
        /// </summary>
        /// <param name="position">Position in metres</param>
        /// <returns>Field vector (Tesla)</returns>
        public Vector3 B(Vector3 position) => HToB(H(position));

        public static Vector3 HToB(Vector3 h) => Vector3.Multiply(h, (float)Mu0);
        public static Vector2 HToB(Vector2 h) => Vector2.Multiply(h, (float)Mu0);

        /// <summary>
        /// Computes and sets remanence from given field strength and distance
        /// </summary>
        /// <param name="fieldStrength">Tesla</param>
        /// <param name="distance">Distance from magnet center along Z (magnetization) axis</param>
        protected void SetRemanenceFromField(double fieldStrength, double distance)
        {
            Remanence = 1;
            Remanence = fieldStrength / SurfaceField;
        }
    }
}
