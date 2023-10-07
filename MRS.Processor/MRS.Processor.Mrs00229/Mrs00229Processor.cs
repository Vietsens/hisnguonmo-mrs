using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisImpMest;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisInvoice;
using MOS.MANAGER.HisInvoiceDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.Processor.Mrs00229;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisDepartment;

namespace MRS.Processor.Mrs00229
{
    public class Mrs00229Processor : AbstractProcessor
    {
        private List<Mrs00229RDO> listMrs00229Rdos = new List<Mrs00229RDO>();
        private Mrs00229Filter FilterMrs00229;

        private List<Mrs00229RDO> listRdo;
        private const long quannhan = 1;
        private const long thannhan = 2;
        private const long thuong = 3;
        private const long vienphi = 4;
        long patientTypeIdBhyt = 0;

        //HIS_DEPARTMENT department = new HIS_DEPARTMENT(); 

        public Mrs00229Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00229Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            var paramGet = new CommonParam();
            try
            {
                FilterMrs00229 = ((Mrs00229Filter)this.reportFilter);
                patientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;

                listRdo = new ManagerSql().Get(FilterMrs00229);
                if (FilterMrs00229.INPUT_DATA_IDs != null)
                {
                    listRdo = listRdo.Where(o => this.IsVienphi(o, FilterMrs00229.INPUT_DATA_IDs) || this.IsQuannhan(o, FilterMrs00229.INPUT_DATA_IDs) || this.IsQuanthan(o, FilterMrs00229.INPUT_DATA_IDs) || this.IsThuong(o, FilterMrs00229.INPUT_DATA_IDs)).ToList();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private bool IsVienphi(Mrs00229RDO o, List<long> list)
        {
            bool result = false;
            try
            {
                if (list == null)
                {
                    return false;
                }
                if (list.Contains(vienphi))
                {
                    if (o.TDL_PATIENT_TYPE_ID != this.patientTypeIdBhyt)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        private bool IsQuannhan(Mrs00229RDO o, List<long> list)
        {
            bool result = false;
            try
            {
                if (list == null)
                {
                    return false;
                }
                if (list.Contains(quannhan))
                {
                    if (o.TDL_PATIENT_TYPE_ID == this.patientTypeIdBhyt && CheckHeinCardNumberTypeQuan(o.TDL_HEIN_CARD_NUMBER))
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        private bool IsQuanthan(Mrs00229RDO o, List<long> list)
        {
            bool result = false;
            try
            {
                if (list == null)
                {
                    return false;
                }
                if (list.Contains(thannhan))
                {
                    if (o.TDL_PATIENT_TYPE_ID == this.patientTypeIdBhyt && CheckHeinCardNumberTypeThan(o.TDL_HEIN_CARD_NUMBER))
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        private bool IsThuong(Mrs00229RDO o, List<long> list)
        {
            bool result = false;
            try
            {
                if (list == null)
                {
                    return false;
                }
                if (list.Contains(thuong))
                {
                    if (o.TDL_PATIENT_TYPE_ID == this.patientTypeIdBhyt && CheckHeinCardNumberTypeThuong(o.TDL_HEIN_CARD_NUMBER))
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        private bool CheckHeinCardNumberTypeQuan(string HeinCardNumber)
        {
            bool result = false;
            try
            {
                if (!String.IsNullOrEmpty(HeinCardNumber))
                {
                    if (IsNotNullOrEmpty(MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__01))
                    {
                        foreach (var type in MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__01)
                        {
                            if (HeinCardNumber.StartsWith(type))
                            {
                                result = true;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private bool CheckHeinCardNumberTypeThan(string HeinCardNumber)
        {
            bool result = false;
            try
            {
                if (!String.IsNullOrEmpty(HeinCardNumber))
                {
                    if (IsNotNullOrEmpty(MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__02))
                    {
                        foreach (var type in MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__02)
                        {
                            if (HeinCardNumber.StartsWith(type))
                            {
                                result = true;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private bool CheckHeinCardNumberTypeThuong(string HeinCardNumber)
        {
            bool result = false;
            try
            {
                if (!String.IsNullOrEmpty(HeinCardNumber))
                {
                    result = !CheckHeinCardNumberTypeQuan(HeinCardNumber) && !CheckHeinCardNumberTypeThan(HeinCardNumber);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {

                listMrs00229Rdos = listRdo.GroupBy(o => new { o.TDL_TREATMENT_ID, o.MEDICINE_TYPE_CODE, o.PRICE }).Select(p => new Mrs00229RDO
                {
                    TDL_TREATMENT_ID = p.First().TDL_TREATMENT_ID,
                    TDL_TREATMENT_CODE = p.First().TDL_TREATMENT_CODE,
                    TDL_PATIENT_ID = p.First().TDL_PATIENT_ID,
                    TDL_PATIENT_NAME = p.First().TDL_PATIENT_NAME,
                    MEDICINE_TYPE_NAME = p.First().MEDICINE_TYPE_NAME,
                    MEDICINE_TYPE_CODE = p.First().MEDICINE_TYPE_CODE,
                    PRICE = p.First().PRICE,
                    AMOUNT = p.Sum(q => q.AMOUNT),
                    TOTAL_PRICE = p.First().PRICE * p.Sum(q => q.AMOUNT)
                }).ToList();
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("EXP_TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00229Filter)this.reportFilter).EXP_TIME_FROM));
            dicSingleTag.Add("EXP_TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00229Filter)this.reportFilter).EXP_TIME_TO));
           dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == FilterMrs00229.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
            objectTag.AddObjectData(store, "ReportDetail", listRdo);
            objectTag.AddObjectData(store, "Report", listMrs00229Rdos);
            var rdo1 = listMrs00229Rdos.GroupBy(o => o.TDL_TREATMENT_ID).Select(p => p.First()).ToList();
            objectTag.AddObjectData(store, "Name", rdo1);
            objectTag.AddRelationship(store, "Name", "Report", "TDL_TREATMENT_ID", "TDL_TREATMENT_ID");

        }
    }
}
