
namespace MOS.Filter
{
    public class HisMestPatientTypeFilter : FilterBase
    {
        public long? MEDI_STOCK_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }

        public HisMestPatientTypeFilter()
            : base()
        {
        }
    }
}
