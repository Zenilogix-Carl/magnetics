using System.Numerics;

namespace Magnets
{
    /// <summary>
    /// A rectangular magnet
    /// </summary>
    /// <remarks>
    /// From: https://www.e-magnetica.pl/doku.php/calculator/field_of_cuboid_magnet_or_rectangular_solenoid
    /// Based on: https://doi.org/10.1063/5.0010982
    /// </remarks>
    public class RectangularMagnet : Magnet
    {
        private readonly Vector3 _size;

        /// <summary>
        /// Half-lengths
        /// </summary>
        private readonly Vector3 _a;

        /// <summary>
        /// Size in metres, magnetized along Z axis
        /// </summary>
        public Vector3 Size
        {
            get => _size;
            private init
            {
                _size = value;
                _a = new Vector3(value.X / 2.0f, value.Y / 2.0f, value.Z / 2.0f);
            }
        }

        public RectangularMagnet(Vector3 size)
        {
            Size = size;
        }

        /// <inheritdoc />
        public override double SurfaceField
        {
            get => B(new Vector3(0,0,Size.Z/2.0f)).Z;
            set => SetRemanenceFromField(value, Size.Z / 2.0);
        }

        /// <inheritdoc />
        public override Vector3 H(Vector3 position)
        {
            var h0 = Remanence / Mu0; // magnetic field strength
            var hx = (h0 / (8.0 * Math.PI)) *
                     Summation((i, j, k) =>
                     {
                         var r = RadiusValue(position, i, j, k);
                         return SignFunc(i + j + k) * Math.Log(
                             (r - (position.Y + _a.Y * SignFunc(j + 1)))
                             / (r + (position.Y + _a.Y * SignFunc(j + 1))));
                     });

            var hy = (h0 / (8.0 * Math.PI)) *
                     Summation((i, j, k) =>
                     {
                         var r = RadiusValue(position, i, j, k);
                         return SignFunc(i + j + k) * Math.Log(
                             (r - (position.X + _a.X * SignFunc(i + 1)))
                             / (r + (position.X + _a.X * SignFunc(i + 1))));
                     });

            var hz = (h0 / (4.0 * Math.PI)) *
                     Summation((i, j, k) =>
                     {
                         var r = RadiusValue(position, i, j, k);
                         return SignFunc(i + j + k + 1) *
                                (Math.Atan(((position.X + _a.X * SignFunc(i + 1)) * (position.Z + _a.Z * SignFunc(k + 1))) / (r * (position.Y + _a.Y * SignFunc(j + 1))))
                                 + Math.Atan(((position.Y + _a.Y * SignFunc(j + 1)) * (position.Z + _a.Z * SignFunc(k + 1))) / (r * (position.X + _a.X * SignFunc(i + 1)))));
                     });

            return new Vector3((float)(position.X < 0 ? -hx : hx), (float)(position.Y < 0 ? -hy : hy), (float)(position.Z < 0 ? -hz : hz));
        }

        private static double Summation(Func<int, int, int, double> func)
        {
            double sum = 0;

            for (var i = 0; i <= 1; i++)
            {
                for (var j = 0; j <= 1; j++)
                {
                    for (var k = 0; k <= 1; k++)
                    {
                        sum += func(i, j, k);
                    }
                }
            }

            return sum;
        }

        private static double Squared(double a)
        {
            return a * a;
        }

        /// <summary>
        /// R-sub(ijk) for given position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private double RadiusValue(Vector3 position, int i, int j, int k)
        {
            return Math.Sqrt(
                Squared(position.X + _a.X * SignFunc(i + 1))
                + Squared(position.Y + _a.Y * SignFunc(j + 1))
                + Squared(position.Z + _a.Z * SignFunc(k + 1)));
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
