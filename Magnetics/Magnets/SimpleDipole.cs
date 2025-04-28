using System;
using System.Numerics;

namespace Magnets
{
    /// <summary>
    /// A simple dipole based on equations for a rectangular magnet.
    /// </summary>
    /// <remarks>This is based on the equations for a rectangular magnet of zero size, removing all resulting zero terms.
    /// I don't know if this is correct.
    /// We do not override SurfaceField because zero size implies that there is no surface.
    /// </remarks>
    public class SimpleDipole : Magnet
    {
        /// <inheritdoc />
        public override Vector3 H(Vector3 position)
        {
            var r = RadiusValue(position);
            var h0 = Remanence / Constants.Mu0; // magnetic field strength
            var hx = h0 / (8.0 * Math.PI) *
                     Summation((i, j, k) => SignFunc(i + j + k) * Math.Log((r - position.Y) / (r + position.Y)));

            var hy = h0 / (8.0 * Math.PI) *
                     Summation((i, j, k) => SignFunc(i + j + k) * Math.Log((r - position.X) / (r + position.X)));

            var hz = h0 / (4.0 * Math.PI) *
                     Summation((i, j, k) => SignFunc(i + j + k + 1) *
                                            (Math.Atan(position.X * position.Z / (r * position.Y))
                                             + Math.Atan(position.Y * position.Z / (r * position.X))));

            return new Vector3((float)hx, (float)hy, (float)hz);
        }

        private static double Summation(Func<int, int, int, double> func)
        {
            return func(0, 0, 0) + func(0, 0, 1) + func(0, 1, 0) + func(0, 1, 1) + func(1, 0, 0) + func(1, 0, 1) + func(1, 1, 0) + func(1, 1, 1);
        }

        private static double Squared(double a)
        {
            return a * a;
        }

        /// <summary>
        /// R-sub(ijk) for given position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private static double RadiusValue(Vector3 position)
        {
            return Math.Sqrt(
                Squared(position.X)
                + Squared(position.Y)
                + Squared(position.Z));
        }

        /// <summary>
        /// Equivalent to -1^n
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static double SignFunc(int n)
        {
            return n % 2 == 0 ? 1 : -1;
        }
    }
}
