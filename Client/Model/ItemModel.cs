using Client.Controller;

namespace Client.Model
{
    public class ItemModel
    {
        /// <summary>
        /// Title of the article
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Cost of the article
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// Type of the article
        /// </summary>
        public ItemTypes Type { get; set; }

        /// <summary>
        /// Image of the article
        /// </summary>
        public Image? Image { get; set; }

        /// <summary>
        /// Value of the article
        /// </summary>
        public int Value { get; set; }
    }


}
