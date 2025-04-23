namespace MagneticFieldCalculator
{
    public abstract class Magnet
    {
        public const double Mu0 = 4 * Math.PI * 1e-7;  // Permeability of free space (T*in/A)

        /// <summary>
        /// Surface field (Tesla)
        /// </summary>
        public double SurfaceField { get; }

        public Vector Position { get; }

        /// <summary>
        /// Orientation relative to polar origin
        /// </summary>
        public double Angle { get; set; }

        public double Magnetization => SurfaceField / Mu0;

        protected Magnet(Vector position, double surfaceField)
        {
            Position = position;
            SurfaceField = surfaceField;
        }

        public abstract Vector DipoleField(Vector c);
    }
}
