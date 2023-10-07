using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatientTypeAlter;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00022
{
    class Mrs00022RDO : MOS.EFMODEL.DataModels.HIS_SERVICE_REQ
    {
        public int COUNT_GROUP1 { get; set; }
        public int COUNT_GROUP2 { get; set; }
        public int COUNT_GROUP3 { get; set; }
        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public string EXECUTE_DEPARTMENT_CODE { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public string EXECUTE_ROOM_CODE { get; set; }
        public V_HIS_PATIENT_TYPE_ALTER currentPatientTypeAlter;

        public Mrs00022RDO(MOS.EFMODEL.DataModels.HIS_SERVICE_REQ data)
        {
            try
            {
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<MOS.EFMODEL.DataModels.HIS_SERVICE_REQ>();
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(data)));
                }
                SetExtendField(this);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        public Mrs00022RDO()
        {
            // TODO: Complete member initialization
        }

        internal void SetExtendField(MOS.EFMODEL.DataModels.HIS_SERVICE_REQ data)
        {
            try
            {
                ExecutePlaceFieldCharge(data);
                this.currentPatientTypeAlter = ProcessPatientTypeAlter(data);

                CountPatientTypeGroup(this.currentPatientTypeAlter.PATIENT_TYPE_ID);
                //SetExecuteDepartmentName(data);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ExecutePlaceFieldCharge(HIS_SERVICE_REQ data)
        {
            try
            {
                var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == data.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM();
                this.EXECUTE_ROOM_CODE = room.ROOM_CODE;
                this.EXECUTE_ROOM_NAME = room.ROOM_NAME;
                this.EXECUTE_DEPARTMENT_CODE = room.DEPARTMENT_CODE;
                this.EXECUTE_DEPARTMENT_NAME = room.DEPARTMENT_NAME;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            
        }

        //private void SetExecuteDepartmentName(MOS.EFMODEL.DataModels.HIS_SERVICE_REQ data)
        //{
        //    try
        //    {
        //        var dep = MRS.MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.SingleOrDefault(o => o.ID == data.EXECUTE_DEPARTMENT_ID);
        //        EXECUTE_DEPARTMENT_NAME = (dep != null ? dep.DEPARTMENT_NAME : "");
        //    }
        //    catch (Exception ex)
        //    {
        //        LogSystem.Error(ex);
        //    }
        //}

        private V_HIS_PATIENT_TYPE_ALTER ProcessPatientTypeAlter(HIS_SERVICE_REQ data)
        {
            V_HIS_PATIENT_TYPE_ALTER currentPatientTypeAlter = null;
            try
            {
                GetCurrentPatientTypeAlter(data.TREATMENT_ID, data.INTRUCTION_TIME, ref currentPatientTypeAlter);
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
                else if (MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_IDs__REPORT_GROUP2.Contains(patientTypeId))
                {
                    COUNT_GROUP2++;
                }
                else if (MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_IDs__REPORT_GROUP3.Contains(patientTypeId))
                {
                    COUNT_GROUP3++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        public static void GetCurrentPatientTypeAlter(long treatmentId, long instructionTime, ref MOS.EFMODEL.DataModels.V_HIS_PATIENT_TYPE_ALTER hisPatientTypeAlter)
        {
            try
            {
                hisPatientTypeAlter = new HisPatientTypeAlterManager().GetViewApplied(treatmentId, instructionTime);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
    }
}
