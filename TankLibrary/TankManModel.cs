namespace TankLibrary
{
    public class TankManModel
    {
        /// <summary>
        /// Name of the user to identify the user
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Number of the coins
        /// </summary>
        public int Coins { get; set; }

        /// <summary>
        /// The tank
        /// </summary>
        public TankModel? Tank { get; set; }
    }
}
