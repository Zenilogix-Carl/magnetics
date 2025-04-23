using System.Numerics;

namespace MagneticFieldCalculator
{
    class CubicMagnet : Magnet
    {
        public CubicMagnet(Vector position, double surfaceField, double size) : base(position, surfaceField)
        {
            Size = size;
        }

        /// <summary>
        /// Size in inches
        /// </summary>
        public double Size { get; }

        public override Vector DipoleField(Vector p)
        {
            var relativePosition = p - Position;
            relativePosition.Angle -= Angle;

            var dipoleField = DipoleField(relativePosition.X, relativePosition.Y);
            var result = new Vector { X = dipoleField.Bx, Y = dipoleField.By };

            //var fieldVector = ComputeFieldVector2(new Vector2((float)relativePosition.X, (float)relativePosition.Y));
            //var result = new Vector { X = fieldVector.X, Y = fieldVector.Y };

            result.Angle += Angle;
            return result;
        }

        // Compute dipole field vector at any (x, y) location from a cubic magnet centered at origin
        // Seems to correlate roughly with test measurements
        private (double Bx, double By) DipoleField(double x, double y)
        {
            // Compute magnetization aligned with cube faces (along x-axis)
            var mx = Magnetization; // Dipole moment vector aligned with x-axis

            // Compute relative position to magnet center (origin)
            var r = Math.Sqrt(x * x + y * y);

            // Ensure calculation doesn't break near the magnet
            if (r < Size / 2) r = Size / 2; // Adjust minimum distance

            // Compute dipole field using modified equation for finite-sized magnet
            var factor = (Mu0 / (4 * Math.PI)) / Math.Pow(r, 3);
            var dotProduct = mx * x;
            var bx = factor * (3 * dotProduct * x - mx);
            var by = factor * (3 * dotProduct * y);

            return (bx, by);
        }

        public Vector2 ComputeFieldVector(Vector2 relativePosition)
        {
            double bx = 0;
            double by = 0;
            var a = 0;// Size / 2.0;

            // Compute contributions from magnetized faces
            for (var sx = -1; sx <= 1; sx += 2)
            {
                for (var sy = -1; sy <= 1; sy += 2)
                {
                    var xp = relativePosition.X - sx * a; // Position relative to magnet face
                    var yp = relativePosition.Y - sy * a; // Position relative to magnet face
                    var rSquared = xp * xp + yp * yp;
                    var rCubed = Math.Pow(rSquared, 1.5);

                    if (rCubed != 0)
                    {
                        // Apply logarithmic correction for close distances
                        var correctionFactor = 1 + Math.Log(1 + (a / Math.Sqrt(rSquared)));
                        bx += correctionFactor * Mu0 * Magnetization * xp / (4 * Math.PI * rCubed);
                        by += correctionFactor * Mu0 * Magnetization * yp / (4 * Math.PI * rCubed);
                    }
                }
            }

            return new Vector2((float)bx, (float)by);
        }

        public Vector2 ComputeFieldVector2(Vector2 relativePosition)
        {
            double bx = 0, by = 0;
            var a = 0;// Size / 2.0;

            // Inline loop structure for polarity-specific contributions
            foreach (var sx in new[] { -1, 1 })
            foreach (var sy in new[] { -1, 1 })
            {
                var xp = relativePosition.X - sx * a; // Position relative to magnet face
                var yp = relativePosition.Y - sy * a; // Position relative to magnet face
                double rSquared = xp * xp + yp * yp; // to magnet face
                double rCubed = Math.Pow(rSquared, 1.5);

                if (rCubed != 0)
                {
                    // Determine polarity effect: north (+), south (-)
                    var poleSign = (sx == 1) ? 1 : -1; // North (+) right, South (-) left

                    var correctionFactor = 1 + Math.Log(1 + (a / Math.Sqrt(rSquared)));
                    bx += poleSign * correctionFactor * Mu0 * Magnetization * xp / (4 * Math.PI * rCubed);
                    by += poleSign * correctionFactor * Mu0 * Magnetization * yp / (4 * Math.PI * rCubed);
                }
            }

            return new Vector2((float)bx, (float)by);
        }
    }
}
