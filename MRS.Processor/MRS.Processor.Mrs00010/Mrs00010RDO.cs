using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00010
{
    class Mrs00010RDO : MOS.EFMODEL.DataModels.HIS_SERVICE_REQ
    {
        public int COUNT_GROUP1 { get; set; }
        public int COUNT_GROUP2 { get; set; }
        public int COUNT_GROUP3 { get; set; }
        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public V_HIS_PATIENT_TYPE_ALTER currentPatientTypeAlter;

        public Mrs00010RDO(MOS.EFMODEL.DataModels.HIS_SERVICE_REQ data, Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter)
        {
            try
            {
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<MOS.EFMODEL.DataModels.HIS_SERVICE_REQ>();
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(data)));
                }
                var room = MANAGER.Config.HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == data.EXECUTE_ROOM_ID);
                if (room != null)
                {
                    this.EXECUTE_DEPARTMENT_NAME = room.DEPARTMENT_NAME;
                    this.EXECUTE_ROOM_NAME = room.ROOM_NAME;
                }
                this.currentPatientTypeAlter = ProcessPatientTypeAlter(data, dicPatientTypeAlter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }


        private V_HIS_PATIENT_TYPE_ALTER ProcessPatientTypeAlter(HIS_SERVICE_REQ data, Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter)
        {
            V_HIS_PATIENT_TYPE_ALTER currentPatientTypeAlter = null;
            try
            {
                if (dicPatientTypeAlter.ContainsKey(data.TREATMENT_ID)) currentPatientTypeAlter = dicPatientTypeAlter[data.TREATMENT_ID];
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }

            return currentPatientTypeAlter;
        }

        internal void CountPatientTypeGroup(long patientTypeId)
        {
            try
            {
                if (MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_IDs__REPORT_GROUP1.Contains(patientTypeId))
                {
                    COUNT_GROUP1++;
                }
                else
                {
                    COUNT_GROUP2++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
    }
}
