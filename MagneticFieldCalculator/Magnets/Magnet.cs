using System;
using System.Numerics;

namespace Magnets
{
    /// <summary>
    /// Magnet class
    /// </summary>
    public abstract class Magnet : IMagnet
    {
        /// <inheritdoc />
        public double Remanence { get; set; }

        /// <inheritdoc />
        public virtual double SurfaceField
        {
            get => throw new NotImplementedException();
            set
            {
                Remanence = 1;
                Remanence = value / SurfaceField;
            }
        }

        /// <inheritdoc />
        public abstract Vector3 H(Vector3 position);

        /// <inheritdoc />
        public Vector3 B(Vector3 position) => Vector3.Multiply(H(position), (float)Constants.Mu0);
    }
}
