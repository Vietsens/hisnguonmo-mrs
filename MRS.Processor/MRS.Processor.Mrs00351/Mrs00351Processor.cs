using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisImpMestMedicine;

namespace MRS.Processor.Mrs00351
{
    class Mrs00351Processor : AbstractProcessor
    {
        Mrs00351Filter mrs00351Filter = new Mrs00351Filter();
        List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        List<Mrs00351RDO> lstMedicineReport = new List<Mrs00351RDO>();
        List<Mrs00351RDO> listDetail = new List<Mrs00351RDO>();
        Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicCurrentPatyAlter = new Dictionary<long, HIS_PATIENT_TYPE_ALTER>();
        List<HIS_EXP_MEST_MEDICINE> ListExpMestMedicine = new List<HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MEDICINE> ListImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<HIS_MEDICINE_TYPE> ListMedicineType = new List<HIS_MEDICINE_TYPE>();
        List<HIS_EXP_MEST> ListExpMest = new List<HIS_EXP_MEST>();

        List<string> DepartmentNames = new List<string>();

        public Mrs00351Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00351Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.mrs00351Filter = (Mrs00351Filter)this.reportFilter;

                CommonParam paramGet = new CommonParam();
                HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                if (mrs00351Filter.IS_TREATTING==true)
                {
                    treatmentFilter.IN_TIME_TO = mrs00351Filter.TIME_TO;
                    treatmentFilter.OUT_TIME_FROM = mrs00351Filter.TIME_TO;

                    var listTreatmentOut = new HisTreatmentManager(paramGet).Get(treatmentFilter);
                    listTreatment.AddRange(listTreatmentOut);
                }
                treatmentFilter = new HisTreatmentFilterQuery();
                treatmentFilter.IN_TIME_TO = mrs00351Filter.TIME_TO;
                treatmentFilter.IS_PAUSE = false;
                var listTreatmentNotOut = new HisTreatmentManager(paramGet).Get(treatmentFilter);
                listTreatment.AddRange(listTreatmentNotOut);
                HisMedicineTypeFilterQuery HisMedicineTypefilter = new HisMedicineTypeFilterQuery();
                //HisMedicineTypefilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                ListMedicineType = new HisMedicineTypeManager(paramGet).Get(HisMedicineTypefilter);
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listTreatment))
                {
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = listTreatment.Count;

                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var listSub = listTreatment.Skip(start).Take(limit).ToList();

                        HisPatientTypeAlterFilterQuery patyAlterFilter = new HisPatientTypeAlterFilterQuery();
                        patyAlterFilter.TREATMENT_IDs = listSub.Select(s => s.ID).ToList();
                        patyAlterFilter.ORDER_DIRECTION = "DESC";
                        patyAlterFilter.ORDER_FIELD = "LOG_TIME";
                        var listPatientTypeAlter = new HisPatientTypeAlterManager(paramGet).Get(patyAlterFilter);

                        if (IsNotNullOrEmpty(listPatientTypeAlter))
                        {
                            foreach (var item in listPatientTypeAlter)
                            {
                                if (dicCurrentPatyAlter.ContainsKey(item.TREATMENT_ID))
                                    continue;
                                dicCurrentPatyAlter[item.TREATMENT_ID] = item;
                            }
                        }
                        HisExpMestFilterQuery prescripFilter = new HisExpMestFilterQuery();
                        prescripFilter.TDL_TREATMENT_IDs = listSub.Select(s => s.ID).ToList();
                        prescripFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                        prescripFilter.MEDI_STOCK_ID = this.mrs00351Filter.MEDI_STOCK_ID;
                        if (mrs00351Filter.IS_TREATTING != true)
                        {
                            prescripFilter.FINISH_TIME_FROM = this.mrs00351Filter.TIME_FROM;
                            prescripFilter.FINISH_TIME_TO = this.mrs00351Filter.TIME_TO;
                        }
                        var lstPrescription = new HisExpMestManager().Get(prescripFilter);
                        if (lstPrescription != null)
                        {
                            ListExpMest.AddRange(lstPrescription);
                            HisExpMestMedicineFilterQuery expMestMedicineFilter = new HisExpMestMedicineFilterQuery();
                            expMestMedicineFilter.EXP_MEST_IDs = lstPrescription.Select(o => o.ID).Distinct().ToList();
                            expMestMedicineFilter.IS_EXPORT = true;
                            expMestMedicineFilter.IS_EXPEND = false;

                            var listExpMestMedicine = new HisExpMestMedicineManager().Get(expMestMedicineFilter);

                            if (IsNotNullOrEmpty(listExpMestMedicine))
                            {
                                ListExpMestMedicine.AddRange(listExpMestMedicine);
                            }
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
                var expMestMedicineIds = ListExpMestMedicine.Where(n=>n.TH_AMOUNT>0).Select(o => o.ID).Distinct().ToList();
                if (IsNotNullOrEmpty(expMestMedicineIds))
                {

                    var skip = 0;
                    while (expMestMedicineIds.Count - skip > 0)
                    {
                        var listIDs = expMestMedicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        List<V_HIS_IMP_MEST_MEDICINE> ImpMestMedicineLib = new ManagerSql().Get(listIDs);
                        ListImpMestMedicine.AddRange(ImpMestMedicineLib);
                    }
                }
                this.ProcessDataDetail();
                this.ProcessDetaiRDO();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        void ProcessDataDetail()
        {
            if (IsNotNullOrEmpty(listTreatment))
            {
                foreach (var treatment in listTreatment)
                {

                    if (!dicCurrentPatyAlter.ContainsKey(treatment.ID))
                    {
                        Inventec.Common.Logging.LogSystem.Info("Ho so dieu tri khong co thong tin doi tuong: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatment.TREATMENT_CODE), treatment.TREATMENT_CODE));
                        continue;
                    }

                    var patyAlter = dicCurrentPatyAlter[treatment.ID];
                    if (mrs00351Filter.PATIENT_TYPE_ID.HasValue && patyAlter.PATIENT_TYPE_ID != mrs00351Filter.PATIENT_TYPE_ID)
                    {
                        continue;
                    }
                    //Chỉ lấy bệnh nhân nội trú
                    if (patyAlter.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        continue;
                    }

                    var ListExpMestMedicineSub = ListExpMestMedicine.Where(o => o.TDL_TREATMENT_ID == treatment.ID).ToList();
                    if (ListExpMestMedicineSub.Count == 0)
                    {
                        continue;
                    }
                    foreach (var item in ListExpMestMedicineSub)
                    {
                        var medicineType = ListMedicineType.FirstOrDefault(o => o.ID == item.TDL_MEDICINE_TYPE_ID);
                        var expMest = ListExpMest.FirstOrDefault(o => o.ID == item.EXP_MEST_ID);
                        decimal ImpMobaAmount = ListImpMestMedicine.Where(o => o.TH_EXP_MEST_MEDICINE_ID == item.ID).Sum(p=>p.AMOUNT);
                        Mrs00351RDO rdo = new Mrs00351RDO(item, treatment);
                        rdo.AMOUNT = item.AMOUNT - ImpMobaAmount;
                        if (medicineType != null)
                        {
                            rdo.SERVICE_NAME = medicineType.MEDICINE_TYPE_NAME;
                            rdo.SERVICE_CODE = medicineType.MEDICINE_TYPE_CODE;
                            rdo.SERVICE_UNIT_NAME = (HisServiceUnitCFG.HisServiceUnits.FirstOrDefault(o => o.ID == medicineType.TDL_SERVICE_UNIT_ID) ?? new HIS_SERVICE_UNIT()).SERVICE_UNIT_NAME;
                            rdo.CONCENTRA = medicineType.CONCENTRA;
                        }
                        if (expMest != null)
                        {
                            rdo.REQ_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == expMest.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        }
                        listDetail.Add(rdo);
                    }
                }
            }
        }

        void ProcessDetaiRDO()
        {
            DepartmentNames = listDetail.OrderBy(o => o.REQ_DEPARTMENT_NAME).Select(p => p.REQ_DEPARTMENT_NAME).Distinct().ToList();
            foreach (var medicine in listDetail)
            {
                int i = DepartmentNames.IndexOf(medicine.REQ_DEPARTMENT_NAME)+1;
                if (i < 30)
                {
                    PropertyInfo piAmount = typeof(Mrs00351RDO).GetProperty("AMOUNT_" + i);
                    piAmount.SetValue(medicine, medicine.AMOUNT);
                }
            }

            if (IsNotNullOrEmpty(listDetail))
            {
                var rs = listDetail.GroupBy(p => new { p.TDL_MEDICINE_TYPE_ID, p.VIR_PRICE }).Select(grc => grc.ToList()).ToList();
                lstMedicineReport = new List<Mrs00351RDO>();
                foreach (var item in rs)
                {

                    Mrs00351RDO ado = new Mrs00351RDO(item[0],new HIS_TREATMENT());
                    PropertyInfo[] fieldArray = Inventec.Common.Repository.Properties.Get<Mrs00351RDO>().Where(o => o.Name.Contains("AMOUNT_")).ToArray();

                    foreach (var field in fieldArray)
                    {
                        field.SetValue(ado, item.ToList<Mrs00351RDO>().Sum(s => (Decimal)field.GetValue(s)));
                    }

                    lstMedicineReport.Add(ado);
                }
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                for (int i = 1; i < DepartmentNames.Count + 1; i++)
                {
                    if (i < 30)
                    {
                        dicSingleTag.Add(string.Format("REQUEST_DEPARTMENT_NAME_{0}", i), DepartmentNames[i - 1]);
                    }
                }

                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(mrs00351Filter.TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(mrs00351Filter.TIME_TO ?? 0));
                dicSingleTag.Add("MEDI_STOCK_NAME", (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == this.mrs00351Filter.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME);
                lstMedicineReport = lstMedicineReport.OrderBy(o => o.SERVICE_NAME).ToList();
                objectTag.AddObjectData(store, "Medicine", lstMedicineReport);
                objectTag.AddObjectData(store, "MedicineDetail", listDetail);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
