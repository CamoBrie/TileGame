namespace TileGame.Code.Utils.Convenience
{
    internal static class ExtensionMethods
    {

        /// <summary>
        /// Maps a float value in a range to another range
        /// Can also be used for values outside the range
        /// </summary>
        /// <param name="value">the value to map</param>
        /// <param name="from1">start range minimum</param>
        /// <param name="to1">map range minimum</param>
        /// <param name="from2">start range maximum</param>
        /// <param name="to2">map range maximum</param>
        /// <returns></returns>
        internal static float Map(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}
