using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00099
{
    class Mrs00099RDO
    {
        public string SERVICE_REQ_CODE { get; set; }

        public long INTRUCTION_TIME { get; set; }
        public long PATIENT_ID { get; set; }

        public string INTRUCTION_DATE_STR { get; set; }
        public string GENDER_NAME { get; set; }
        public string DOB_YEAR { get; set; }
        public string PATIENT_NAME { get; set; }
        public string VIR_ADDRESS { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string ICD_DIIM { get; set; }
        public string ICD_TEXT { get; set; }
        public string REQUEST_ROOM { get; set; }
        public string EXEXUTE_ROOM { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string DIIM_RESULT { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public long? IS_SIZE_FILM_20_25 { get; set; }
        public string PATIENT_CODE { get; set; }

        public long? BEGIN_TIME { get; set; }
        public string DESCRIPTION { get; set; }
        public long? END_TIME { get; set; }
        public string INSTRUCTION_NOTE { get; set; }
        public long? NUMBER_OF_FILM { get; set; }
        public string FILM_SIZE_NAME { get; set; }

        public long REQUEST_ROOM_ID { get; set; }
        public long? TDL_PATIENT_DOB { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal PRICE { get; set; }

        public Mrs00099RDO() { }

        public Mrs00099RDO(Mrs00099RDO r)
        {
            try
            {
                this.REQUEST_ROOM = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == r.REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;

                this.EXEXUTE_ROOM = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == r.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;

                this.INTRUCTION_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.INTRUCTION_TIME);
                if (r.TDL_PATIENT_DOB > 0)
                {
                    this.DOB_YEAR = (r.TDL_PATIENT_DOB ?? 0).ToString().Substring(0, 4);
                }

               
                var filmSize = MANAGER.Config.HisFilmSizeCFG.FILM_SIZEs.FirstOrDefault(o => o.ID == this.FILM_SIZE_ID);
                if (filmSize != null)
                {
                    this.FILM_SIZE_NAME = filmSize.FILM_SIZE_NAME;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        public long? FILM_SIZE_ID { get; set; }

        public long? KSK_ORDER { get; set; }

        public string TREATMENT_CODE { get; set; }

        public string WORK_PLACE_NAME { get; set; }

        public long TDL_SERVICE_TYPE_ID { get; set; }

        public string SERVICE_TYPE_CODE { get; set; }

        public string SERVICE_TYPE_NAME { get; set; }

        public long EXECUTE_ROOM_ID { get; set; }
    }
}
