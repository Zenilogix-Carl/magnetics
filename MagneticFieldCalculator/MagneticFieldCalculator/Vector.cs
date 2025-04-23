namespace MagneticFieldCalculator
{
    public class Vector
    {
        private const double DegToRad = Math.PI / 180.0; // Conversion factor (degrees to radians)
        private const double RadToDeg = 180.0 / Math.PI;

        public double X { get; set; }
        public double Y { get; set; }

        public double Magnitude
        {
            get => Math.Sqrt(X * X + Y * Y);

            set
            {
                var thetaRad = Angle * DegToRad;
                X = value * Math.Cos(thetaRad);
                Y = value * Math.Sin(thetaRad);
            }
        }

        /// <summary>
        /// Synonym for Magnitude
        /// </summary>
        public double Distance
        {
            get => Magnitude;
            set => Magnitude = value;
        }

        public double Angle
        {
            get => Math.Atan2(Y, X) * RadToDeg;
            set
            {
                var thetaRad = value * DegToRad;
                var magnitude = Magnitude;
                X = magnitude * Math.Cos(thetaRad);
                Y = magnitude * Math.Sin(thetaRad);
            }
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector
            {
                X = a.X + b.X,
                Y = a.Y + b.Y,
            };
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector
            {
                X = a.X - b.X,
                Y = a.Y - b.Y,
            };
        }
    }
}
