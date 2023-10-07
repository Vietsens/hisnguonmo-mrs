
namespace SDA.Filter
{
    public class SdaGroupFilter : FilterBase
    {
        public string ID_PATH { get; set; }
        public string CODE_PATH { get; set; }

        /// <summary>
        /// Co phai don vi goc hay khong.
        /// TRUE - La don vi goc (PARENT_ID = null).
        /// FALSE - Khong phai don vi goc (PARENT_ID # null)
        /// NULL - Ca 2
        /// </summary>
        public bool? IS_ROOT { get; set; }
        public long? PARENT_ID { get; set; }

        public SdaGroupFilter()
            : base()
        {
        }
    }
}
