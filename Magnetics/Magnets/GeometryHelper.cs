using System.Numerics;

namespace Magnets
{
    public static class GeometryHelper
    {
        /// <summary>
        /// Project magnet co-ordinate space onto 2D plane
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        /// <remarks>Magnet Z (pole axis) mapped to plane X, magnet Y mapped to plane Y, magnet X discarded</remarks>
        public static Vector2 ToVector2(this Vector3 vector) => new Vector2(vector.Z, vector.Y);

        /// <summary>
        /// Raise 2D plane into 3D space
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        /// <remarks>Plane X mapped to magnet Z (pole axis), magnet Y mapped to plane Y, magnet X always zero</remarks>
        public static Vector3 ToVector3(this Vector2 vector) => new Vector3(0, vector.Y, vector.X);

        /// <summary>
        /// Rotate <see cref="Vector2"/> by angle in degrees
        /// </summary>
        /// <param name="vector">Vector to rotate</param>
        /// <param name="angle">Angle in degrees</param>
        /// <returns></returns>
        public static Vector2 Rotate(this Vector2 vector, double angle)
        {
            return Vector2.Transform(vector, Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), (float)(angle * Constants.RadiansPerDegree)));
        }
    }
}
