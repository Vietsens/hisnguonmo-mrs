using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00007
{
    public class Mrs00007Processor : AbstractProcessor
    {
        Mrs00007Filter castFilter = null;
        Dictionary<string, Mrs00007RDO> dicRdo = new Dictionary<string, Mrs00007RDO>(); // key = medicine_type_id & imp_price
        List<Mrs00007RDO> listRdo = new List<Mrs00007RDO>();
        List<Mrs00007RDO> listRdo1 = new List<Mrs00007RDO>();
        List<long> LIST_DEPARTMENT_ID__GROUP_CLS = new List<long>();
        List<long> LIST_DEPARTMENT_ID__GROUP_KKB = new List<long>();
        List<long> LIST_DEPARTMENT_ID__GROUP_LS = new List<long>();

        long PatientTypeIdBHYT = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
        long PatientTypeIdFEE = HisPatientTypeCFG.PATIENT_TYPE_ID__FEE;
        long PatientTypeIdIS_FREE = HisPatientTypeCFG.PATIENT_TYPE_ID__IS_FREE;

        Dictionary<long, V_HIS_EXP_MEST_MEDICINE> dicExpMestMedicine = new Dictionary<long, V_HIS_EXP_MEST_MEDICINE>();

        public Mrs00007Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00007Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                try
                {
                    LIST_DEPARTMENT_ID__GROUP_CLS = MRS.MANAGER.Config.HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_CLS;
                    LIST_DEPARTMENT_ID__GROUP_KKB = MRS.MANAGER.Config.HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_KKB;
                    LIST_DEPARTMENT_ID__GROUP_LS = MRS.MANAGER.Config.HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_LS;

                }
                catch { }
                castFilter = ((Mrs00007Filter)this.reportFilter);
                ProcessExpMest(); //cac phieu xuat
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessExpMest()
        {
            try
            {
                CommonParam getParam = new CommonParam();
                //List<V_HIS_IMP_MEST> ListImp = new List<V_HIS_IMP_MEST>();

                List<V_HIS_IMP_MEST_MEDICINE> ListImpMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();

                HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                expMestMedicineViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                expMestMedicineViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
                expMestMedicineViewFilter.IS_EXPORT = true;
                expMestMedicineViewFilter.REQ_DEPARTMENT_IDs = castFilter.REQ_DEPARTMENT_IDs;
                expMestMedicineViewFilter.MEDICINE_TYPE_IDs = castFilter.MEDICINE_TYPE_IDs;
                expMestMedicineViewFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                expMestMedicineViewFilter.EXP_MEST_TYPE_IDs = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,// xuat don thuoc                       
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT,
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP,
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__PL
                    };

                var listExpMestMedicine = new HisExpMestMedicineManager(getParam).GetView(expMestMedicineViewFilter);
                dicExpMestMedicine = listExpMestMedicine.ToDictionary(o => o.ID);
                //Get hồ sơ điều trị
                //dicTreatment = GetTreatment(listExpMestMedicine);

                int start = 0;
                int count = listExpMestMedicine.Count();

                if (listExpMestMedicine != null)
                {
                    if (listExpMestMedicine != null && listExpMestMedicine.Count > 0)
                    {

                        foreach (var item in listExpMestMedicine)
                        {
                            string key = item.MEDICINE_TYPE_ID + "_" + item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);

                            Mrs00007RDO rdo = null;
                            if (dicRdo.ContainsKey(key))
                            {
                                rdo = dicRdo[key];
                            }
                            else
                            {
                                rdo = new Mrs00007RDO(item.MEDICINE_TYPE_CODE, item.MEDICINE_TYPE_NAME, item.SERVICE_UNIT_NAME, item.CONCENTRA, item.IMP_PRICE * (1 + item.IMP_VAT_RATIO));
                                rdo.REQ_DEPARTMENT_ID = item.REQ_DEPARTMENT_ID;
                                rdo.MEDICINE_GROUP_CODE = !string.IsNullOrEmpty(item.MEDICINE_GROUP_CODE) ? item.MEDICINE_GROUP_CODE : "NTK";
                                rdo.MEDICINE_GROUP_NAME = !string.IsNullOrEmpty(item.MEDICINE_GROUP_NAME) ? item.MEDICINE_GROUP_NAME : "Nhóm thuốc khác";
                            }


                            if (LIST_DEPARTMENT_ID__GROUP_CLS != null && LIST_DEPARTMENT_ID__GROUP_CLS.Contains(item.REQ_DEPARTMENT_ID))
                            {
                                rdo.AMOUNT_CLS += item.AMOUNT;
                            }
                            else if (LIST_DEPARTMENT_ID__GROUP_LS != null && LIST_DEPARTMENT_ID__GROUP_LS.Contains(item.REQ_DEPARTMENT_ID))
                            {
                                rdo.AMOUNT_LS += item.AMOUNT;
                            }
                            else if (LIST_DEPARTMENT_ID__GROUP_KKB != null && LIST_DEPARTMENT_ID__GROUP_KKB.Contains(item.REQ_DEPARTMENT_ID))
                            {
                                rdo.AMOUNT_KKB += item.AMOUNT;
                            }
                            else
                            {
                                rdo.AMOUNT_K += item.AMOUNT;
                            }
                            if (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK)
                            {
                                rdo.OUT_EXP_AMOUNT += item.AMOUNT;
                            }
                            else if (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT)
                            {
                                rdo.IN_EXP_AMOUNT += item.AMOUNT;
                            }
                            else if (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT)
                            {
                                rdo.USE_EXP_AMOUNT += item.AMOUNT;
                            }
                            else
                            {
                                rdo.OTHER_AMOUNT += item.AMOUNT;
                            }
                            if (item.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP)
                            {
                                if (item.PATIENT_TYPE_ID == PatientTypeIdBHYT)
                                {
                                    rdo.AMOUNT_BHYT += item.AMOUNT;
                                    //rdo.AMOUNT_BHYT_HT += impMestMedicine.AMOUNT;
                                }
                                else if (item.PATIENT_TYPE_ID == PatientTypeIdFEE)
                                {
                                    rdo.AMOUNT_VP += item.AMOUNT;
                                    //rdo.AMOUNT_VP_HT += impMestMedicine.AMOUNT;
                                }
                                else if (item.PATIENT_TYPE_ID == PatientTypeIdIS_FREE)
                                {
                                    rdo.AMOUNT_FREE += item.AMOUNT;
                                    //rdo.AMOUNT_FREE_HT += impMestMedicine.AMOUNT;
                                }

                            }
                            else
                            {
                                rdo.AMOUNT_HPKP += item.AMOUNT;
                                //rdo.AMOUNT_HPKP_HT += impMestMedicine.AMOUNT;
                            }
                           
                            if (!dicRdo.ContainsKey(key))
                            {
                                dicRdo[key] = rdo;
                            }
                        }



                        var skip = 0;
                        var mobaExpMestIds = listExpMestMedicine.Where(o => o.TH_AMOUNT > 0).Select(p => p.EXP_MEST_ID ?? 0).Distinct().ToList();
                        while (mobaExpMestIds.Count - skip > 0)
                        {
                            var listIds = mobaExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            HisImpMestViewFilterQuery mobaFilter = new HisImpMestViewFilterQuery();
                            mobaFilter.MOBA_EXP_MEST_IDs = listIds;
                            mobaFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                            ProcessMoveBackData(getParam, mobaFilter);

                        }

                    }
                }

                if (getParam.HasException)
                {
                    LogSystem.Debug("Co exception tai DAOGET trong qua trinh tong hop du lieu.");
                    //throw new DataMisalignedException("Co exception tai DAOGET trong qua trinh tong hop du lieu."); 
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                dicRdo.Clear();
            }
        }

        //private Dictionary<long, HIS_TREATMENT> GetTreatment(List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine)
        //{
        //    Dictionary<long, HIS_TREATMENT> result = new Dictionary<long, HIS_TREATMENT>();
        //    var treatmentIds = listExpMestMedicine.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
        //    if (IsNotNullOrEmpty(treatmentIds))
        //    {
        //        var skip = 0;
        //        while (treatmentIds.Count - skip > 0)
        //        {
        //            var listIDs = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
        //            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
        //            HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
        //            treatmentFilter.IDs = listIDs;
        //            var treatment = new HisTreatmentManager().Get(treatmentFilter);
        //            if (treatment == null)
        //                Inventec.Common.Logging.LogSystem.Error("treatment is null");
        //            else
        //            {
        //                var listExpMest = listExpMestMedicine.GroupBy(o => o.EXP_MEST_ID ?? 0).ToList();
        //                foreach (var item in listExpMest)
        //                {
        //                    if (!result.ContainsKey(item.Key))
        //                    {
        //                        result.Add(item.Key, treatment.FirstOrDefault(o => o.ID == item.First().TDL_TREATMENT_ID) ?? new HIS_TREATMENT());
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    return result;
        //}

        private void ProcessMoveBackData(CommonParam getParam, HisImpMestViewFilterQuery mobaFilter)
        {
            var listMobaImpMest = new HisImpMestManager(getParam).GetView(mobaFilter);
            if (listMobaImpMest != null && listMobaImpMest.Count > 0)
            {
                Dictionary<long, long> dictionaryExpMestIdByImpMestId = new Dictionary<long, long>();
                foreach (var item in listMobaImpMest)
                {
                    if (!dictionaryExpMestIdByImpMestId.ContainsKey(item.ID))
                    {
                        dictionaryExpMestIdByImpMestId[item.ID] = item.MOBA_EXP_MEST_ID ?? 0;
                    }
                }
                List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
                HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                impMestMedicineViewFilter.IMP_MEST_IDs = listMobaImpMest.Select(o => o.ID).ToList();
                var impMestMedicines = new HisImpMestMedicineManager(getParam).GetView(impMestMedicineViewFilter);
                if (impMestMedicines != null && impMestMedicines.Count > 0)
                {
                    listImpMestMedicine.AddRange(impMestMedicines);
                }

                if (listImpMestMedicine == null || listImpMestMedicine.Count == 0)
                {
                    LogSystem.Info("p10 -> ProcessMoveBackData");

                }
                if (listImpMestMedicine != null && listImpMestMedicine.Count > 0)
                {
                    foreach (var item in listImpMestMedicine)
                    {
                        if (!dictionaryExpMestIdByImpMestId.ContainsKey(item.IMP_MEST_ID)) continue;
                        long expMestId = dictionaryExpMestIdByImpMestId[item.IMP_MEST_ID];

                        Mrs00007RDO rdo = null;
                        string key = item.MEDICINE_TYPE_ID + "_" + item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        if (dicRdo.ContainsKey(key))
                        {
                            rdo = dicRdo[key];
                            if (LIST_DEPARTMENT_ID__GROUP_CLS != null && LIST_DEPARTMENT_ID__GROUP_CLS.Contains(rdo.REQ_DEPARTMENT_ID))
                            {
                                rdo.AMOUNT_CLS -= item.AMOUNT;
                            }
                            else if (LIST_DEPARTMENT_ID__GROUP_LS != null && LIST_DEPARTMENT_ID__GROUP_LS.Contains(rdo.REQ_DEPARTMENT_ID))
                            {
                                rdo.AMOUNT_LS -= item.AMOUNT;
                            }
                            else if (LIST_DEPARTMENT_ID__GROUP_KKB != null && LIST_DEPARTMENT_ID__GROUP_KKB.Contains(rdo.REQ_DEPARTMENT_ID))
                            {
                                rdo.AMOUNT_KKB -= item.AMOUNT;
                            }
                            else
                            {
                                rdo.AMOUNT_K -= item.AMOUNT;
                            }
                            if (item.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL)
                            {
                                rdo.IN_EXP_AMOUNT -= item.AMOUNT;
                            }
                            else if (item.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL)
                            {
                                rdo.USE_EXP_AMOUNT -= item.AMOUNT;
                            }
                            else
                            {
                                rdo.OTHER_AMOUNT -= item.AMOUNT;
                            }
                            if (dicExpMestMedicine.ContainsKey(item.TH_EXP_MEST_MEDICINE_ID ?? 0))
                            {
                                var exmm = dicExpMestMedicine[item.TH_EXP_MEST_MEDICINE_ID ?? 0]??new V_HIS_EXP_MEST_MEDICINE();
                                if (exmm.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP)
                                {
                                    if (exmm.PATIENT_TYPE_ID == PatientTypeIdBHYT)
                                    {
                                        rdo.AMOUNT_BHYT_HT += item.AMOUNT;
                                    }
                                    else if (exmm.PATIENT_TYPE_ID == PatientTypeIdFEE)
                                    {
                                        rdo.AMOUNT_VP_HT += item.AMOUNT;
                                    }
                                    else if (exmm.PATIENT_TYPE_ID == PatientTypeIdIS_FREE)
                                    {
                                        rdo.AMOUNT_FREE_HT += item.AMOUNT;
                                    }

                                }
                                else
                                {
                                    rdo.AMOUNT_HPKP_HT += item.AMOUNT;
                                }
                            }

                        }
                        else
                        {
                            throw new DataMisalignedException("Loi thuat toan hoac CSDL. Co thu hoi nhung khong co xuat kho." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => item), item));
                        }
                    }
                }
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                listRdo.AddRange(dicRdo.Values.OrderBy(o => o.MEDICINE_TYPE_CODE).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (this.castFilter.TIME_FROM > 0)
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.castFilter.TIME_FROM ?? 0));
                if (this.castFilter.TIME_TO > 0)
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.castFilter.TIME_TO ?? 0));

                objectTag.AddObjectData(store, "Report", listRdo);
                objectTag.AddObjectData(store, "Report1", listRdo);
                objectTag.AddObjectData(store, "MedicineGroup", listRdo.GroupBy(p => p.MEDICINE_GROUP_CODE).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "MedicineGroup", "Report1", "MEDICINE_GROUP_CODE", "MEDICINE_GROUP_CODE");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
