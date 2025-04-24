using System.Numerics;

namespace Magnets
{
    public class CuboidMagnet : RectangularMagnet
    {
        public CuboidMagnet(float size) : base(new Vector3(size, size, size))
        {
        }
    }
}
