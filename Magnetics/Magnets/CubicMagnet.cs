using System.Numerics;

namespace Magnets
{
    public class CubicMagnet : RectangularMagnet
    {
        public CubicMagnet(float size) : base(new Vector3(size, size, size))
        {
        }
    }
}
