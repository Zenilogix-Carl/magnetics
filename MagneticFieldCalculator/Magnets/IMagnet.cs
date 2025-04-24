namespace Magnets
{
    public interface IMagnet
    {
        /// <summary>
        /// Remanence (Tesla)
        /// </summary>
        double Remanence { get; set; }

        /// <summary>
        /// Surface field (Tesla)
        /// </summary>
        double SurfaceField { get; set; }
    };
}
