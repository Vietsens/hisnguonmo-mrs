
namespace MOS.Filter
{
    public class HisMestPatientTypeViewFilter : FilterBase
    {
        public long? MEDI_STOCK_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }

        public HisMestPatientTypeViewFilter()
            : base()
        {
        }
    }
}
