using FluentAssertions;
using MagneticFieldCalculator;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class VectorTests
    {
        [TestCase(10, 10, 0, 14.14, 0, 14.14)]
        [TestCase(10, 0, 45, 7.07, 7.07, 10)]
        [TestCase(10, 0, 90, 0, 10, 10)]
        [TestCase(10, 0, 135, -7.07, 7.07, 10)]
        [TestCase(10, 0, 180, -10, 0, 10)]
        [TestCase(10, 0, -135, -7.07, -7.07, 10)]
        [TestCase(10, 0, -90, 0, -10, 10)]
        [TestCase(10, 0, -45, 7.07, -7.07, 10)]
        [TestCase(10, -10, 0, 14.14, 0, 14.14)]
        [TestCase(10, 0, -45, 7.07, -7.07, 10)]
        [TestCase(-10, 10, 0, 14.14, 0, 14.14)]
        [TestCase(-10, 0, 45, 7.07, 7.07, 10)]
        [TestCase(-10, -10, 0, 14.14, 0, 14.14)]
        [TestCase(-10, 0, -45, 7.07, -7.07, 10)]
        public void AngleTest(double initialX, double initialY, double angle, double expectedX, double expectedY, double expectedDistance)
        {
            var vector = new Vector { X = initialX, Y = initialY };
            const double precision = .01;
            vector.Angle = angle;
            vector.X.Should().BeApproximately(expectedX, precision);
            vector.Y.Should().BeApproximately(expectedY, precision);
            vector.Distance.Should().BeApproximately(expectedDistance, precision);
            vector.Angle.Should().BeApproximately(angle, precision);
        }


        [TestCase(10, 0, 0, 10, 0, 10)]
        [TestCase(10, 45, 0, 10, 0, 10)]
        [TestCase(10, 90, 0, 10, 0, 10)]
        [TestCase(10, 135, 0, 10, 0, 10)]
        [TestCase(10, 180, 0, 10, 0, 10)]
        [TestCase(10, -45, 0, 10, 0, 10)]
        [TestCase(10, -90, 0, 10, 0, 10)]
        [TestCase(10, -135, 0, 10, 0, 10)]
        [TestCase(10, 0, 45, 7.07, 7.07, 10)]
        [TestCase(10, 0, 90, 0, 10, 10)]
        [TestCase(10, 0, -45, 7.07, -7.07, 10)]
        [TestCase(10, 0, -90, 0, -10, 10)]
        public void AngleTest2(double initialDistance, double initialAngle, double angle, double expectedX, double expectedY, double expectedDistance)
        {
            var vector = new Vector { Distance = initialDistance, Angle = initialAngle };
            const double precision = .01;
            vector.Angle = angle;
            vector.X.Should().BeApproximately(expectedX, precision);
            vector.Y.Should().BeApproximately(expectedY, precision);
            vector.Distance.Should().BeApproximately(expectedDistance, precision);
            vector.Angle.Should().BeApproximately(angle, precision);
        }
    }
}
