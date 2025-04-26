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

        /// <summary>
        /// Gets or sets the magnet's surface field (Tesla)
        /// </summary>
        /// <remarks>
        /// Getter should calculate the surface field using <see cref="Remanence"/> property.
        /// Implementation of this setter computes remanence using getter calculation.
        /// It will initially set remanence to 1, calculates surface field for that remanence, and finally scales remanence to attain the specified surface field value.
        /// Throws <see cref="System.NotImplementedException"/> if getter not overridden.</remarks>
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
        public Vector3 B(Vector3 position) => H(position) * (float)Constants.Mu0;
    }
}
