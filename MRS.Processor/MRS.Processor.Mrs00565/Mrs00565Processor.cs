using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMest;
using AutoMapper;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisPatientTypeAlter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.Repository;
using System.Reflection;
using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;

namespace MRS.Processor.Mrs00565
{
    public class Mrs00565Processor : AbstractProcessor
    {
        Mrs00565Filter castFilter = null;
        private List<Mrs00565RDO> listMrs00565RDOs = new List<Mrs00565RDO>();
        CommonParam paramGet = new CommonParam();
        List<Mrs00565RDO> ListRdo = new List<Mrs00565RDO>();
        List<Mrs00565RDO> Parent = new List<Mrs00565RDO>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicineView = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterialView = new List<V_HIS_EXP_MEST_MATERIAL>();

        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineTL = new List<V_HIS_IMP_MEST_MEDICINE>();//thuốc trả lại
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterialTL = new List<V_HIS_IMP_MEST_MATERIAL>();//vật tư trả lại

        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineTH = new List<V_HIS_IMP_MEST_MEDICINE>();//thuốc thu hồi
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterialTH = new List<V_HIS_IMP_MEST_MATERIAL>();//vật tư thu hồi

        List<HIS_DEPARTMENT> department = new List<HIS_DEPARTMENT>();
        List<V_HIS_MEDI_STOCK> mediStock = new List<V_HIS_MEDI_STOCK>();
        List<HIS_EXP_MEST> listPrescription = new List<HIS_EXP_MEST>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_PATIENT_TYPE_ALTER> lastPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();
        private string a = "";
        Dictionary<long, PropertyInfo> dicExpAmountType = new Dictionary<long, PropertyInfo>();
        Dictionary<long, PropertyInfo> dicImpAmountType = new Dictionary<long, PropertyInfo>();
        public Mrs00565Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            a = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00565Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                castFilter = (Mrs00565Filter)this.reportFilter;
                //get dữ liệu:
                if (IsNotNullOrEmpty(castFilter.DEPARTMENT_IDs))
                {
                    department = HisDepartmentCFG.DEPARTMENTs.Where(o => castFilter.DEPARTMENT_IDs.Contains(o.ID)).ToList();
                }
                if (IsNotNullOrEmpty(castFilter.MEDI_STOCK_IDs))
                {
                    mediStock = HisMediStockCFG.HisMediStocks.Where(o => castFilter.MEDI_STOCK_IDs.Contains(o.ID)).ToList();
                }

               
                var presFilter = new HisExpMestFilterQuery
                {
                    FINISH_TIME_FROM = castFilter.TIME_FROM,
                    FINISH_TIME_TO = castFilter.TIME_TO,
                    MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs,
                    REQ_DEPARTMENT_IDs = castFilter.DEPARTMENT_IDs,
                    EXP_MEST_TYPE_IDs = castFilter.EXP_MEST_TYPE_IDs,
                   
                };

                listPrescription = new HisExpMestManager(paramGet).Get(presFilter);
                listPrescription = listPrescription.Where(o => o.TDL_TREATMENT_ID.HasValue).ToList();
                if (IsNotNullOrEmpty(listPrescription))
                {
                    var listTreatmentId = listPrescription.GroupBy(o => o.TDL_TREATMENT_ID).Select(p => p.First().TDL_TREATMENT_ID.Value).ToList();
                    listPatientTypeAlter = new HisPatientTypeAlterManager(paramGet).GetViewByTreatmentIds(listTreatmentId).OrderByDescending(o => o.LOG_TIME).ToList();
                    lastPatientTypeAlter = listPatientTypeAlter.GroupBy(o => o.TREATMENT_ID).Select(p => p.First()).ToList();


                    if (IsNotNullOrEmpty(castFilter.TREATMENT_TYPE_IDs))
                    {
                        listPrescription = listPrescription.Where(o => lastPatientTypeAlter.Exists(p => p.TREATMENT_ID == o.TDL_TREATMENT_ID && castFilter.TREATMENT_TYPE_IDs.Contains(p.TREATMENT_TYPE_ID))).ToList();
                    }
                    if (IsNotNullOrEmpty(castFilter.PATIENT_TYPE_IDs))
                    {
                        listPrescription = listPrescription = listPrescription.Where(o => lastPatientTypeAlter.Exists(p => p.TREATMENT_ID == o.TDL_TREATMENT_ID && castFilter.PATIENT_TYPE_IDs.Contains(p.PATIENT_TYPE_ID))).ToList();

                    }
                    if (castFilter.PATIENT_TYPE_ID != null)
                    {
                        listPrescription = listPrescription.Where(o => lastPatientTypeAlter.Exists(p => p.TREATMENT_ID == o.TDL_TREATMENT_ID && castFilter.PATIENT_TYPE_ID == p.PATIENT_TYPE_ID)).ToList();
                    }
                }
                var listExpMestIds = listPrescription.Select(s => s.ID).ToList();
                if (IsNotNullOrEmpty(listExpMestIds))
                {
                    var skip = 0;
                    while (listExpMestIds.Count - skip > 0)
                    {
                        var listIds = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        var metyFilterExpMestMaterial = new HisExpMestMaterialViewFilterQuery
                        {
                            EXP_MEST_IDs = listIds,
                            IS_EXPEND = castFilter.IS_EXPEND,
                            IS_EXPORT = true
                        };
                        var expMestMaterialViews = new HisExpMestMaterialManager(paramGet).GetView(metyFilterExpMestMaterial);
                        listExpMestMaterialView.AddRange(expMestMaterialViews);

                        var metyFilterExpMestMedicine = new HisExpMestMedicineViewFilterQuery
                        {
                            EXP_MEST_IDs = listIds,
                            IS_EXPEND = castFilter.IS_EXPEND,
                            IS_EXPORT = true
                        };
                        var expMestMedicineViews = new HisExpMestMedicineManager(paramGet).GetView(metyFilterExpMestMedicine);
                        listExpMestMedicineView.AddRange(expMestMedicineViews);
                    }
                }
                makeRdo();

                result = true;
                //Dữ liệu trả lại
                //GetImpMestMediMateTL();

                //Dữ liệu thu hồi
                GetImpMestMediMateTH();


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        private void GetImpMestMediMateTH()
        {
            List<long> listTHExpMestId = new List<long>();
            listTHExpMestId.AddRange(listExpMestMedicineView.Where(o => o.TH_AMOUNT > 0).Select(p => p.EXP_MEST_ID ?? 0).ToList());
            listTHExpMestId.AddRange(listExpMestMaterialView.Where(o => o.TH_AMOUNT > 0).Select(p => p.EXP_MEST_ID ?? 0).ToList());
            listTHExpMestId = listTHExpMestId.Distinct().ToList();
            if (listTHExpMestId != null && listTHExpMestId.Count > 0)
            {
                int start = 0;
                int count = listTHExpMestId.Count;
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                    List<long> listTHExpMestIdSub = listTHExpMestId.Skip(start).Take(limit).ToList();
                    HisImpMestFilterQuery impMestFilter = new HisImpMestFilterQuery();
                    impMestFilter.MOBA_EXP_MEST_IDs = listTHExpMestIdSub;
                    impMestFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    var mobaImpMestSub = new HisImpMestManager().Get(impMestFilter) ?? new List<HIS_IMP_MEST>();
                    HisImpMestMedicineViewFilterQuery impMestMediFilter = new HisImpMestMedicineViewFilterQuery();
                    impMestMediFilter.IMP_MEST_IDs = mobaImpMestSub.Select(o => o.ID).ToList();
                    var impMestMedicineTHSub = new HisImpMestMedicineManager().GetView(impMestMediFilter);
                    if (impMestMedicineTHSub != null)
                    {
                        this.listImpMestMedicineTH.AddRange(impMestMedicineTHSub);
                    }
                    HisImpMestMaterialViewFilterQuery impMestMateFilter = new HisImpMestMaterialViewFilterQuery();
                    impMestMateFilter.IMP_MEST_IDs = mobaImpMestSub.Select(o => o.ID).ToList();
                    var impMestMaterialTHSub = new HisImpMestMaterialManager().GetView(impMestMateFilter);
                    if (impMestMaterialTHSub != null)
                    {
                        this.listImpMestMaterialTH.AddRange(impMestMaterialTHSub);
                    }
                    start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                }
            }
        }

        private void GetImpMestMediMateTL()
        {
            HisImpMestMedicineViewFilterQuery impMestMedicineFilterTL = new HisImpMestMedicineViewFilterQuery();
            impMestMedicineFilterTL.IMP_TIME_FROM = castFilter.TIME_FROM;
            impMestMedicineFilterTL.IMP_TIME_TO = castFilter.TIME_TO;
            impMestMedicineFilterTL.IMP_MEST_STT_IDs = new List<long>(){
                IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BTL,
                IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DMTL,
                IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL,
                IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL,
                IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL,
                IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL,
                IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH,
                IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT
                };
            this.listImpMestMedicineTL = new HisImpMestMedicineManager().GetView(impMestMedicineFilterTL);

            HisImpMestMaterialViewFilterQuery impMestMaterialFilterTL = new HisImpMestMaterialViewFilterQuery();
            impMestMaterialFilterTL.IMP_TIME_FROM = castFilter.TIME_FROM;
            impMestMaterialFilterTL.IMP_TIME_TO = castFilter.TIME_TO;
            impMestMaterialFilterTL.IMP_MEST_STT_IDs = new List<long>(){
                IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BTL,
                IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DMTL,
                IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL,
                IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL,
                IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL,
                IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL,
                IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH,
                IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT
                };
            this.listImpMestMaterialTL = new HisImpMestMaterialManager().GetView(impMestMaterialFilterTL);
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                ListRdo.Clear();

                var GroupbyMedicineIds = listExpMestMedicineView.GroupBy(o => new { o.MEDICINE_TYPE_ID, o.PRICE }).ToList();
                Decimal sum = 0;

                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<Mrs00565RDO>();

                foreach (var group in GroupbyMedicineIds)
                {
                    List<V_HIS_EXP_MEST_MEDICINE> Medicinesub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                    var impMestMediTHSub = listImpMestMedicineTH.Where(o => group.Select(p => p.ID).ToList().Contains(o.TH_EXP_MEST_MEDICINE_ID ?? 0)).ToList();
                    var groupBH = group.Where(o => o.PATIENT_TYPE_ID == 01 && o.IS_EXPEND != 1).ToList();
                    var groupVP = group.Where(o => o.PATIENT_TYPE_ID != 01 && o.IS_EXPEND != 1).ToList();
                    var groupHP = group.Where(o => o.IS_EXPEND == 1).ToList();
                    
                    
                    

               

                    Mrs00565RDO rdo = new Mrs00565RDO();
                    
                    rdo.MEDICINE_TYPE_NAME = Medicinesub.First().MEDICINE_TYPE_NAME;
                    rdo.MEDICINE_TYPE_CODE = Medicinesub.First().MEDICINE_TYPE_CODE;
                    rdo.CONCENTRA = Medicinesub.First().CONCENTRA;
                    rdo.UNIT = Medicinesub.First().SERVICE_UNIT_NAME;
                    rdo.PRICE = Medicinesub.First().PRICE??0;
                    rdo.AMOUNT = group.Sum(o => o.AMOUNT);
                    rdo.TT_PRICE = (Medicinesub.First().PRICE??0) * group.Sum(o => o.AMOUNT);
                    rdo.TYPE = "THUỐC";
                    rdo.AMOUTN_TH = group.Sum(x => x.TH_AMOUNT??0);
                    rdo.AMOUNT_TH = impMestMediTHSub.Sum(x => x.AMOUNT);
                    
                    rdo.AMOUNT_BH = groupBH.Sum(o => o.AMOUNT) - groupBH.Sum(x => x.TH_AMOUNT ?? 0);
                    rdo.AMOUNT_VP = groupVP.Sum(o => o.AMOUNT) - groupVP.Sum(x => x.TH_AMOUNT ?? 0);
                    rdo.AMOUNT_HP = groupHP.Sum(o => o.AMOUNT) - groupHP.Sum(x => x.TH_AMOUNT ?? 0);
                


                    ListRdo.Add(rdo);
                }

                var GroupbyMaterialIds = listExpMestMaterialView.GroupBy(o => new { o.MATERIAL_TYPE_ID, o.PRICE }).ToList();
                foreach (var group in GroupbyMaterialIds)
                {
                    List<V_HIS_EXP_MEST_MATERIAL> materialsub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                    var impMestMateTHSub = listImpMestMaterialTH.Where(o => group.Select(p => p.ID).ToList().Contains(o.TH_EXP_MEST_MATERIAL_ID ?? 0)).ToList();
                    var groupBH = group.Where(o => o.PATIENT_TYPE_ID == 01 && o.IS_EXPEND != 1).ToList();
                    var groupVP = group.Where(o => o.PATIENT_TYPE_ID != 01 && o.IS_EXPEND != 1).ToList();
                    var groupHP = group.Where(o => o.IS_EXPEND == 1).ToList();
                    
                    
                    Mrs00565RDO rdo = new Mrs00565RDO();
                    rdo.MEDICINE_TYPE_NAME = materialsub.First().MATERIAL_TYPE_NAME;
                    rdo.MEDICINE_TYPE_CODE = materialsub.First().MATERIAL_TYPE_CODE;
                    //PACKAGE_NUMBER = materialsub.First().PACKAGE_NUMBER,
                    rdo.UNIT = materialsub.First().SERVICE_UNIT_NAME;
                    rdo.PRICE = materialsub.First().PRICE??0;
                    rdo.AMOUNT = group.Sum(o => o.AMOUNT);
                    rdo.TT_PRICE = (materialsub.First().PRICE??0) * group.Sum(o => o.AMOUNT);
                    rdo.TYPE = "VẬT TƯ";
                    rdo.AMOUTN_TH = group.Sum(x => x.TH_AMOUNT??0);
                    rdo.AMOUNT_TH = impMestMateTHSub.Sum(x => x.AMOUNT);
                  
                    rdo.AMOUNT_BH = groupBH.Sum(o => o.AMOUNT) - groupBH.Sum(x => x.TH_AMOUNT ?? 0);
                    rdo.AMOUNT_VP = groupVP.Sum(o => o.AMOUNT) - groupVP.Sum(x => x.TH_AMOUNT ?? 0);
                    rdo.AMOUNT_HP = groupHP.Sum(o => o.AMOUNT) - groupHP.Sum(x => x.TH_AMOUNT ?? 0) ;
                    ListRdo.Add(rdo);

                }

                ListRdo = ListRdo.OrderBy(o => o.TYPE).ThenBy(p => p.MEDICINE_TYPE_NAME).ToList();

                Parent = ListRdo.GroupBy(o => o.TYPE).Select(p => p.First()).ToList();
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
                result = false;
            }
            return result;
        }
        private void makeRdo()
        {
            //Danh sach loai nhap, loai xuat

            Dictionary<string, long> dicExpMestType = new Dictionary<string, long>();
            Dictionary<string, long> dicImpMestType = new Dictionary<string, long>();
            RDOImpExpMestTypeContext.Define(ref dicImpMestType, ref dicExpMestType);
            //Danh sach loai SL nhap, loai SL xuat
            PropertyInfo[] piAmount = Properties.Get<Mrs00565RDO>();

            foreach (var item in piAmount)
            {
                if (dicImpMestType.ContainsKey(item.Name))
                {
                    if (!dicImpAmountType.ContainsKey(dicImpMestType[item.Name])) dicImpAmountType[dicImpMestType[item.Name]] = item;
                }
                else if (dicExpMestType.ContainsKey(item.Name))
                {
                    if (!dicExpAmountType.ContainsKey(dicExpMestType[item.Name])) dicExpAmountType[dicExpMestType[item.Name]] = item;
                }
            }

        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00565Filter)this.reportFilter).TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00565Filter)this.reportFilter).TIME_TO));
            dicSingleTag.Add("DEPARTMENT_NAME", string.Join(",", department.Select(o => o.DEPARTMENT_NAME).ToList()));
            dicSingleTag.Add("MEDI_STOCK_NAME", string.Join(",", mediStock.Select(o => o.MEDI_STOCK_NAME).ToList()));

            dicSingleTag.Add("IS_HEIN_BHYT", (castFilter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && castFilter.IS_EXPEND == false) ? "Được BHYT thanh toán" : (castFilter.IS_EXPEND == false) ? "Viện phí không được BHYT thanh toán" : "Hao phí không được BHYT thanh toán");

            objectTag.AddObjectData(store, "Parent", Parent);
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddRelationship(store, "Parent", "Report", "TYPE", "TYPE");
        }

    }
}