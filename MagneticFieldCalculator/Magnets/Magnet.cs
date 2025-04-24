using System.Numerics;

namespace Magnets
{
    public abstract class Magnet
    {
        public const double Mu0 = 4 * Math.PI * 1e-7;  // Permeability of free space

        /// <summary>
        /// Remanence
        /// </summary>
        public double Remanence { get; set; }

        /// <summary>
        /// Field strength at specified position; pole aligned to Z axis
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public abstract Vector3 H(Vector3 position);

        /// <summary>
        /// Sets remanence from surface field strength at pole
        /// </summary>
        /// <param name="fieldStrength">Field strength in Tesla</param>
        public abstract void SetRemanenceFromSurfaceField(double fieldStrength);

        public Vector3 B(Vector3 position)
        {
            return Vector3.Multiply(H(position), (float)Mu0);
        }

        /// <summary>
        /// Computes and sets remanence from given field strength and distance
        /// </summary>
        /// <param name="fieldStrength">Tesla</param>
        /// <param name="distance">Distance from magnet center along Z (magnetization) axis</param>
        public void SetRemanenceFromField(double fieldStrength, double distance)
        {
            Remanence = 1;
            var position = new Vector3(0, 0, (float)distance);
            var b = B(position);
            Remanence = fieldStrength / b.Z;
        }
    }
}
