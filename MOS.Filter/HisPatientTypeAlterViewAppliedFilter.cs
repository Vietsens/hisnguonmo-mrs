using System;

namespace MOS.Filter
{
    public class HisPatientTypeAlterViewAppliedFilter
    {
        public long TreatmentId { get; set; }
        public long InstructionTime { get; set; }
        
        public HisPatientTypeAlterViewAppliedFilter()
        {
        }

        public HisPatientTypeAlterViewAppliedFilter(long instructionTime, long treatmentId)
        {
            InstructionTime = instructionTime;
            TreatmentId = treatmentId;
        }
    }
}
