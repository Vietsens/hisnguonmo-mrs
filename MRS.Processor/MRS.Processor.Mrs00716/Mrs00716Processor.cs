using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.DateTime;
using MRS.MANAGER.Config;
using FlexCel.Report;

using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMedicineTypeAcin;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisSupplier;
using MOS.MANAGER.HisMedicineBean;

namespace MRS.Processor.Mrs00716
{
    class Mrs00716Processor : AbstractProcessor
    {
        private Mrs00716Filter filter;
        List<ManagerSql.TREATMENT> Listdata = new List<ManagerSql.TREATMENT>();
        List<Mrs00716RDO> ListRdo = new List<Mrs00716RDO>();
        CommonParam paramGet = new CommonParam();
        public Mrs00716Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00716Filter);
        }
        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00716Filter)reportFilter;
            try
            {
                Listdata = new MRS.Processor.Mrs00716.ManagerSql().GetTreatment(filter) ?? new List<ManagerSql.TREATMENT>();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;

            
        }
        private bool Less(long Dob, long IN_TIME, long Age)
        {
            return IN_TIME - Age * 10000000000 < Dob;
        }

        private bool More(long Dob, long IN_TIME, long Age)
        {
            return IN_TIME - Age * 10000000000 >= Dob;
        }


        protected override bool ProcessData()
        {
            var result = true;

            try
            {
                if (IsNotNullOrEmpty(Listdata))
                {
                    Mrs00716RDO rdo = new Mrs00716RDO();


                    for (int i = 0; i < Listdata.Count; i++)
                    {


                        if (Listdata[i].TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                        {
                            if (Listdata[i].TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {

                                rdo.COUNT_NU_BHYT++;

                                if (Listdata[i].TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                                {
                                    rdo.COUNT_NU_BOARDING_BHYT++;
                                }
                            }
                            if (Listdata[i].TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                rdo.COUNT_NU_ND++;
                                if (Listdata[i].TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                                {
                                    rdo.COUNT_NU_BOARDING_ND++;
                                }
                            }
                        }
                        if (Listdata[i].TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {

                            if (Listdata[i].TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                rdo.COUNT_NAM_BHYT++;
                                if (Listdata[i].TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                                {
                                    rdo.COUNT_NAM_BOARDING_BHYT++;
                                }
                            }
                            if (Listdata[i].TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                rdo.COUNT_NAM_ND++;
                                if (Listdata[i].TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                                {
                                    rdo.COUNT_NAM_BOARDING_ND++;
                                }
                            }

                        }

                        if (this.Less(Listdata[i].TDL_PATIENT_DOB, Listdata[i].IN_TIME, 6))
                        {
                            if (Listdata[i].TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                rdo.COUNT_CHILD_LESS6_BHYT++;
                                if (Listdata[i].TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                                {
                                    rdo.COUNT_CHILD_LESS6_BOARDING_BHYT++;
                                }
                            }
                            if (Listdata[i].TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                rdo.COUNT_CHILD_LESS6_ND++;
                                if (Listdata[i].TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                                {
                                    rdo.COUNT_CHILD_LESS6_BOARDING_ND++;
                                }
                            }
                        }
                        if (this.Less(Listdata[i].TDL_PATIENT_DOB, Listdata[i].IN_TIME, 15))
                        {
                            if (Listdata[i].TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                rdo.COUNT_CHILD_LESS15_BHYT++;
                                if (Listdata[i].TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                                {
                                    rdo.COUNT_CHILD_LESS15_BOARDING_BHYT++;
                                }
                            }
                            if (Listdata[i].TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                rdo.COUNT_CHILD_LESS15_ND++;
                                if (Listdata[i].TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                                {
                                    rdo.COUNT_CHILD_LESS15_BOARDING_ND++;
                                }
                            }
                        }
                        if (this.More(Listdata[i].TDL_PATIENT_DOB, Listdata[i].IN_TIME, 60))
                        {
                            if (Listdata[i].TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                rdo.COUNT_OLD_MOREIS_EQUAL60_BHYT++;
                                if (Listdata[i].TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                                {
                                    rdo.COUNT_OLD_MOREIS_EQUAL60_BOARDING_BHYT++;
                                }
                            }
                            if (Listdata[i].TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                rdo.COUNT_OLD_MOREIS_EQUAL60_ND++;
                                if (Listdata[i].TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                                {
                                    rdo.COUNT_OLD_MOREIS_EQUAL60_BOARDING_ND++;
                                }
                            }
                        }
                        if (Listdata[i].TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                        {
                            if (Listdata[i].TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                            {
                                rdo.COUNT_TRANSFER_EXAM++;
                            }
                            if (Listdata[i].TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                            {
                                rdo.COUNT_TRANSFER_BOARDING++;
                            }
                            if (Listdata[i].TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                            {
                                rdo.COUNT_TRANSFER_EXTERNAL++;
                            }
                        }
                    }

                    ListRdo.Add(rdo);
                }

            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM?? 0)); 

            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO ?? 0) );
            objectTag.AddObjectData(store, "Report", ListRdo);
        }
    }
}
