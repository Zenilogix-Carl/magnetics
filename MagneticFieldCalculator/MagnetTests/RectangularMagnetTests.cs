using System.Numerics;
using Magnets;
using NUnit.Framework;

namespace MagnetTests
{
    [TestFixture]
    public class RectangularMagnetTests
    {
        [Test]
        public void BasicTest()
        {
            var magnet =
                new RectangularMagnet(Vector3.Multiply(new Vector3(.25f, .25f, .25f) /*inches*/, (float)Constants.MetresPerInch))
                {
                    SurfaceField = 4601 * Constants.TeslaPerGauss
                };

            var b = magnet.B(Vector3.Multiply(new Vector3(0f, 0f, 0.3f + magnet.Size.Z/2.0f), (float)Constants.MetresPerInch));
        }

        [Test]
        public void BasicTestNegative()
        {
            var magnet = new RectangularMagnet(Vector3.Multiply(new Vector3(.25f, .25f, .25f) /*inches*/, (float)Constants.MetresPerInch))
            {
                SurfaceField = 4601 * Constants.TeslaPerGauss
            };

            var b = magnet.B(Vector3.Multiply(new Vector3(0f, 0f, -0.3f - magnet.Size.Z / 2.0f), (float)Constants.MetresPerInch));
        }

        [Test]
        public void SetRemanenceTest()
        {
            var magnet = new RectangularMagnet(Vector3.Multiply(new Vector3(.25f, .25f, .25f) /*inches*/, (float)Constants.MetresPerInch))
            {
                SurfaceField = 4601 * Constants.TeslaPerGauss
            };
        }

        [Test]
        public void BasicTest2()
        {
            var magnet = new RectangularMagnet(new Vector3(.00635f, .00635f, .00635f) /*meters*/)
            {
                Remanence = 1.0555
            };

            var h = magnet.H(new Vector3(0f, 0f, magnet.Size.Z));
        }

        [Test]
        public void RotationTest()
        {
            var testPoint = new Vector2((float)(0.5 * Constants.MetresPerInch), 0);

            var m1 = new MagnetWithPosition2(new CubicMagnet((float)(0.25 * Constants.MetresPerInch))
            {
                SurfaceField = 4601 * Constants.TeslaPerGauss
            }, new Vector2(0, 0))
            {
                Orientation = 180
            };

            var b = m1.B(testPoint);
        }

        [Test]
        public void CircleTest()
        {
            var magnet = CreateMagnet(new Vector2(0, 0), 0);
            var nominalPosition = new Vector2((float)(0.3 * Constants.MetresPerInch),0);
            for (var i = 0; i < 360; i+=10)
            {
                var testPoint = nominalPosition.Rotate(i);
                var b = magnet.B(testPoint);
                var bR = b.Rotate(-i);
                Console.WriteLine($"Angle: {i} Test Point: ({testPoint.X*Constants.InchesPerMeter:F3}, {testPoint.Y * Constants.InchesPerMeter:F3}) B:({b.X*1000:F1}, {b.Y*1000:F1})mT  BrX:{bR.X * 1000:F1}mT");
            }
        }

        [Test]
        public void SensorTest()
        {
            var m1 = CreateMagnet(new Vector2((float)(1.05 * Constants.MetresPerInch), 0).Rotate(30), 30);
            var m2 = CreateMagnet(new Vector2((float)(1.05 * Constants.MetresPerInch), 0).Rotate(-30), 180-30);

            var sensorNominalPosition = new Vector2((float)(0.595 * Constants.MetresPerInch), 0);

            Console.WriteLine($"Angle, B(mT) V");
            for (var i = -30; i <= 30; i+= 5)
            {
                var sensorPosition = sensorNominalPosition.Rotate(i);
                var bT = (m1.B(sensorPosition) + m2.B(sensorPosition)).Rotate(-i);
                var bmT = bT * 1000;
                var v = (bT.X * 100) + 2.5; // sensor model; 100mV/mT quiescent at Vcc/2, Vcc = 5V
                Console.WriteLine($"{i}, {bmT.X:F1} {v:F2}");
            }
        }

        /// <summary>
        /// Creates a magnet representative of a real-world 0.25" cubic Neodymium magnet having a 4601 Gauss surface field
        /// placed in a 2D space
        /// </summary>
        /// <param name="position">Where in our 2D co-ordinate system the magnet is placed</param>
        /// <param name="orientation">Magnet's orientation in our 2D space</param>
        /// <returns>The created magnet</returns>
        private static MagnetWithPosition2 CreateMagnet(Vector2 position, double orientation)
        {
            return new MagnetWithPosition2(new CubicMagnet((float)(0.25 * Constants.MetresPerInch)), position)
            {
                Orientation = orientation,
                SurfaceField = 4601 * Constants.TeslaPerGauss
            };
        }
    }
}
