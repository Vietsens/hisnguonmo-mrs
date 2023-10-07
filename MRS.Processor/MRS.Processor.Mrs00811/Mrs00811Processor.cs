using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Core;
using MRS.MANAGER.Core.MrsReport;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisExpMestMedicine;
using Inventec.Common.DateTime;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMedicineTypeAcin;
namespace MRS.Processor.Mrs00811
{
    public class Mrs00811Processor : AbstractProcessor
    {
        public Mrs00811Filter filter;
        public List<Mrs00811RDO> listRdo = new List<Mrs00811RDO>();
        public List<Mrs00811RDO> listRdoKS = new List<Mrs00811RDO>();
        public List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        public List<V_HIS_MEDICINE_TYPE> listMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        public List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        public List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicineKS = new List<V_HIS_EXP_MEST_MEDICINE>();
        public List<V_HIS_ROOM> listRoom = new List<V_HIS_ROOM>();
        public List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        public List<HIS_PATIENT> listPatient = new List<HIS_PATIENT>();
        public List<HIS_MEDICINE> listMedicine = new List<HIS_MEDICINE>();

        public List<V_HIS_EXP_MEST_MEDICINE> ListExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        public List<V_HIS_EXP_MEST_MEDICINE> ListExpMestMedicineKS = new List<V_HIS_EXP_MEST_MEDICINE>();

        public List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();

        Dictionary<long, List<V_HIS_MEDICINE_TYPE_ACIN>> dicMedicineTypeAcin = new Dictionary<long, List<V_HIS_MEDICINE_TYPE_ACIN>>(); 
        public CommonParam commonParam = new CommonParam();
        public Mrs00811Processor(CommonParam param, string reportTypeName)
            : base(param, reportTypeName)
        {

        }
        public override Type FilterType()
        {
            return typeof(Mrs00811Filter);
        }

        protected override bool GetData()
        {
            filter = (Mrs00811Filter)this.reportFilter;
            bool result = false;
            try
            {
                listRoom = new HisRoomManager(commonParam).GetView(new HisRoomViewFilterQuery());
                HisMedicineTypeViewFilterQuery medicineTypeFilter = new HisMedicineTypeViewFilterQuery();
                listDepartment = new HisDepartmentManager(commonParam).Get(new HisDepartmentFilterQuery());
                listMedicineType = new HisMedicineTypeManager(commonParam).GetView(medicineTypeFilter);
                HisExpMestMedicineViewFilterQuery expMedicineFilter = new HisExpMestMedicineViewFilterQuery();
                expMedicineFilter.EXP_TIME_FROM = filter.TIME_FROM;
                expMedicineFilter.EXP_TIME_TO = filter.TIME_TO;
                expMedicineFilter.IS_EXPORT = true;
                if (filter.DEPARTMENT_IDs != null)
                {
                    expMedicineFilter.REQ_DEPARTMENT_IDs = filter.DEPARTMENT_IDs;
                }  
                listExpMestMedicine = new HisExpMestMedicineManager(commonParam).GetView(expMedicineFilter);
                listExpMestMedicine = listExpMestMedicine.Where(x => x.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK && x.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS && x.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL).ToList();
                //var IDs = listExpMestMedicine.Where(x=>x.TH_AMOUNT>0).Select(x => x.ID).ToList();
                var treatIds = listExpMestMedicine.Select(x => x.TDL_TREATMENT_ID??0).Distinct().ToList();
                int skip = 0;
                while (listExpMestMedicine.Count - skip > 0)
                {
                    var lstData = treatIds.Skip(skip).Take(MRS.MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += MRS.MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisTreatmentFilterQuery ssFilter = new HisTreatmentFilterQuery();
                    ssFilter.IDs = lstData;
                    var lstSS = new HisTreatmentManager(commonParam).Get(ssFilter);
                    if (IsNotNullOrEmpty(lstSS))
                    {
                        listTreatment.AddRange(lstSS);
                    }
                }


                var treatNOITRU = listTreatment.Where(x => x.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || x.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY).Select(x => x.ID).ToList();
                if (filter.CHECK_TYPE_TREATMENT == true)
                {
                    listTreatment = listTreatment.Where(x => x.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU&& x.TDL_TREATMENT_TYPE_ID!=IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY).ToList();
                }
                else
                {
                    listTreatment = listTreatment.Where(x => x.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || x.TDL_TREATMENT_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY).ToList();
                }
                var treatmentIds = listTreatment.Select(x => x.ID).ToList();
                listExpMestMedicineKS = listExpMestMedicine.Where(x=>x.MEDICINE_GROUP_ID==IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__KS && treatNOITRU.Contains(x.TDL_TREATMENT_ID??0)).ToList();
                ListExpMestMedicine = listExpMestMedicine.Where(x => treatmentIds.Contains(x.TDL_TREATMENT_ID ?? 0) && x.TDL_TREATMENT_ID != null).ToList();
                result = true;

                //get hoat chat
                GetMedicineTypeAcin();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex.Message);
                result = false;
            }
            return result;
        }

        private void GetMedicineTypeAcin()
        {
            HisMedicineTypeAcinViewFilterQuery AcinGrefilter = new HisMedicineTypeAcinViewFilterQuery();
            var ListMedicineTypeAcin = new HisMedicineTypeAcinManager().GetView(AcinGrefilter);
            if (ListMedicineTypeAcin != null && ListMedicineTypeAcin.Count > 0)
            {
                dicMedicineTypeAcin = ListMedicineTypeAcin.GroupBy(o => o.MEDICINE_TYPE_ID).ToDictionary(o => o.Key, p => p.ToList());
            }

        }

        protected override bool ProcessData()
        
        {
            bool result = false;
            try
            {
                var group = ListExpMestMedicine.GroupBy(x => new { x.TDL_TREATMENT_ID, x.MEDICINE_TYPE_ID, x.EXP_MEST_ID, x.PRICE }).ToList();
                foreach (var item in group)
                {
                    Mrs00811RDO rdo = new Mrs00811RDO();
                    rdo.MEDICINE_TYPE_CODE = item.First().MEDICINE_TYPE_CODE;
                    rdo.MEDICINE_TYPE_NAME = item.First().MEDICINE_TYPE_NAME;
                    rdo.BYT_NUM_ORDER = item.First().MEDICINE_BYT_NUM_ORDER;
                    rdo.MEDICINE_USE_FROM = item.First().MEDICINE_USE_FORM_NAME;
                    rdo.PRICE = item.First().VIR_PRICE ?? 0;
                    rdo.AMOUNT = item.Sum(x=>x.AMOUNT);
                    rdo.TOTAL_PRICE = rdo.PRICE * rdo.AMOUNT;
                    rdo.TUTORIAL = item.First().TUTORIAL;
                    rdo.MEDICINE_REGISTER_NUMBER = item.First().MEDICINE_REGISTER_NUMBER;
                    rdo.APPROVAL_DATE = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.First().TDL_INTRUCTION_TIME ?? 0);
                    rdo.CONCENTRA = item.First().CONCENTRA;
                    rdo.DOCTOR_USERNAME = item.First().APPROVAL_USERNAME;
                    rdo.MEDICINE_USE_FROM_CODE = item.First().MEDICINE_USE_FORM_CODE;
                    rdo.ACTIVE_INGR_BHYT_CODE = item.First().ACTIVE_INGR_BHYT_CODE;
                    rdo.ACTIVE_INGR_BHYT_NAME = item.First().ACTIVE_INGR_BHYT_NAME;
                    rdo.IMP_HT_AMOUNT = item.Sum(x => x.TH_AMOUNT ?? 0);
                    rdo.TOTAL_AMOUNT = rdo.AMOUNT - rdo.IMP_HT_AMOUNT;
                    var room = listRoom.Where(x => x.ID == item.First().REQ_ROOM_ID).FirstOrDefault();
                    if (room != null)
                    {
                        rdo.ROOM_NAME = room.ROOM_NAME;
                    }
                    var depa = listDepartment.Where(x => x.ID == item.First().REQ_DEPARTMENT_ID).FirstOrDefault();
                    if (depa != null)
                    {
                        rdo.DEPARTMENT_CODE = depa.DEPARTMENT_CODE;
                        rdo.DEPARTMENT_NAME = depa.DEPARTMENT_NAME;
                    }
                    var trea = listTreatment.Where(x => x.ID == item.First().TDL_TREATMENT_ID).FirstOrDefault();
                    if (trea != null)
                    {
                        rdo.PATIENT_NAME = trea.TDL_PATIENT_NAME;
                        rdo.PATIENT_CODE = trea.TDL_PATIENT_CODE;
                        rdo.GENDER = trea.TDL_PATIENT_GENDER_NAME;
                        if (trea.TDL_PATIENT_DOB!=null)
                        {
                            rdo.DOB = trea.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                        }
                        rdo.ICD_CODE = trea.ICD_CODE;
                        rdo.ICD_NAME = trea.ICD_NAME;
                        rdo.ICD_SUB_CODE = trea.ICD_SUB_CODE;
                        rdo.ICD_SUB_TEXT = trea.ICD_TEXT;
                        rdo.IN_DATE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(trea.IN_DATE);
                        rdo.OUT_DATE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(trea.OUT_DATE ?? 0);
                        rdo.TREATMENT_DAY_COUNT = trea.TREATMENT_DAY_COUNT ?? 0;
                    }
                    //Thêm thông tin hoạt chất
                    if (dicMedicineTypeAcin.ContainsKey(item.First().MEDICINE_TYPE_ID))
                    {
                        var medicineTypeAcin = dicMedicineTypeAcin[item.First().MEDICINE_TYPE_ID];
                        if (medicineTypeAcin != null && medicineTypeAcin.Count > 0)
                        {
                            rdo.ACTIVE_INGREDIENT_IDs = string.Join(";", medicineTypeAcin.Select(o => o.ID).ToList());
                            rdo.ACTIVE_INGREDIENT_NAMEs = string.Join(";", medicineTypeAcin.Select(o => o.ACTIVE_INGREDIENT_NAME).ToList());
                            rdo.ACTIVE_INGREDIENT_CODEs = string.Join(";", medicineTypeAcin.Select(o => o.ACTIVE_INGREDIENT_CODE).ToList());
                        }
                    }
                    listRdo.Add(rdo);
                }
                var groups = listExpMestMedicineKS.GroupBy(x => new { x.REQ_DEPARTMENT_ID, x.REQ_ROOM_ID, x.MEDICINE_ID, x.MEDICINE_TYPE_ID }).ToList();
                foreach (var item in groups)
                {
                    
                    Mrs00811RDO rdo = new Mrs00811RDO();
                    var depa = listDepartment.Where(x => x.ID == item.First().REQ_DEPARTMENT_ID).FirstOrDefault();
                    if (depa!=null)
                    {
                        rdo.DEPARTMENT_CODE = depa.DEPARTMENT_CODE;
                        rdo.DEPARTMENT_NAME = depa.DEPARTMENT_NAME;
                    }
                    var room = listRoom.Where(x => x.ID == item.First().REQ_ROOM_ID).FirstOrDefault();
                    if (room != null)
                    {
                        rdo.ROOM_NAME = room.ROOM_NAME;
                    }
                    var mety = listMedicineType.Where(x => x.ID == item.First().MEDICINE_TYPE_ID).FirstOrDefault();
                    if (mety!=null)
                    {
                        rdo.MEDICINE_TYPE_CODE = mety.MEDICINE_TYPE_CODE;
                    rdo.MEDICINE_TYPE_NAME = mety.MEDICINE_TYPE_NAME;
                    rdo.ATC_CODE = mety.ATC_CODES;
                    rdo.ACTIVE_INGR_BHYT_NAME = mety.ACTIVE_INGR_BHYT_NAME;
                    rdo.NATIONAL_NAME = mety.NATIONAL_NAME;
                    }
                    rdo.MEDICINE_USE_FROM = item.First().MEDICINE_USE_FORM_NAME;
                    rdo.MEDICINE_USE_FROM_CODE = item.First().MEDICINE_USE_FORM_CODE;

                    //Thêm thông tin hoạt chất
                    if (dicMedicineTypeAcin.ContainsKey(item.First().MEDICINE_TYPE_ID))
                    {
                        var medicineTypeAcin = dicMedicineTypeAcin[item.First().MEDICINE_TYPE_ID];
                        if (medicineTypeAcin != null && medicineTypeAcin.Count > 0)
                        {
                            rdo.ACTIVE_INGREDIENT_IDs = string.Join(";", medicineTypeAcin.Select(o => o.ID).ToList());
                            rdo.ACTIVE_INGREDIENT_NAMEs = string.Join(";", medicineTypeAcin.Select(o => o.ACTIVE_INGREDIENT_NAME).ToList());
                            rdo.ACTIVE_INGREDIENT_CODEs = string.Join(";", medicineTypeAcin.Select(o => o.ACTIVE_INGREDIENT_CODE).ToList());
                        }
                    }
                    rdo.SERVICE_UNIT_NAME = item.First().SERVICE_UNIT_NAME;
                    rdo.EXP_TOTAL_AMOUNT = item.Sum(x => x.AMOUNT);

                    rdo.IMP_HT_AMOUNT = item.Sum(x=>x.TH_AMOUNT??0);
                    rdo.TOTAL_AMOUNT = rdo.EXP_TOTAL_AMOUNT + rdo.IMP_HT_AMOUNT;
                    rdo.PRICE = item.First().VIR_PRICE??0;
                    rdo.TOTAL_PRICE = (rdo.EXP_TOTAL_AMOUNT+ rdo.IMP_HT_AMOUNT) * rdo.PRICE;
                    listRdoKS.Add(rdo);
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex.Message);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("TIME_FROM", filter.TIME_FROM);
            dicSingleTag.Add("TIME_TO", filter.TIME_TO);
            objectTag.AddObjectData(store, "Report", listRdo);
            objectTag.AddObjectData(store, "Report2", listRdoKS.OrderBy(x=>x.DEPARTMENT_NAME).ToList());
            objectTag.AddObjectData(store,"Department", listRdoKS.GroupBy(x=>x.DEPARTMENT_NAME).Select(p=>p.First()).ToList());
            objectTag.AddRelationship(store,"Department", "Report2", "DEPARTMENT_NAME", "DEPARTMENT_NAME");
        }
    }
}
