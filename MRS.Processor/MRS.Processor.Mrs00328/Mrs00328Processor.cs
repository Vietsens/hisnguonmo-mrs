using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisExpMestMedicine;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisMedicineType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisExpMestReason;
using MOS.MANAGER.HisExpMestType;
using MOS.MANAGER.HisImpMestType;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTreatment;

namespace MRS.Processor.Mrs00328
{
    class Mrs00328Processor : AbstractProcessor
    {
        int countExpMestMedicine = 0; int countImpMestMedicine = 0;
        Mrs00328Filter castFilter = null;
        Dictionary<string, Mrs00328RDO> dicRdo = new Dictionary<string, Mrs00328RDO>(); // key = medicine_type_id & imp_price
        List<Mrs00328RDO> listRdo = new List<Mrs00328RDO>();

        List<long> CHMS_EXP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL
        };
        List<long> departmentCls = new List<long>();
        List<long> departmentLs = new List<long>();
        List<long> departmentKKB = new List<long>();


        List<V_HIS_MEDICINE_TYPE> listMedicineType = new List<V_HIS_MEDICINE_TYPE>();

        List<HIS_EXP_MEST_REASON> listExpMestReason = new List<HIS_EXP_MEST_REASON>();

        List<HIS_EXP_MEST_TYPE> listExpMestType = new List<HIS_EXP_MEST_TYPE>();

        List<HIS_IMP_MEST_TYPE> listImpMestType = new List<HIS_IMP_MEST_TYPE>();

        List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        long expMestReasonId05 = HisExpMestReasonCFG.HIS_EXP_MEST_REASON___05;
        public Mrs00328Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00328Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00328Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau get du lieu V_HIS_MEDI_STOCK, MRS00328 Filter:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                try
                {

                    departmentKKB = HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_KKB;
                    departmentLs = HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_LS;
                    departmentCls = HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_CLS;
                }
                catch (Exception)
                {

                }
                GetMedicineType();
                GetExpMestReason();
                GetExpMestType();
                GetImpMestType();
                ProcessExpMest();
                if (castFilter.IS_MOBA_ON_TIME.HasValue && castFilter.IS_MOBA_ON_TIME.Value)
                {
                    ProcessImpMest();                    
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetMedicineType()
        {
            HisMedicineTypeViewFilterQuery tyFilter = new HisMedicineTypeViewFilterQuery();
            
            listMedicineType = new HisMedicineTypeManager().GetView(tyFilter);
        }

        private void GetExpMestReason()
        {
            HisExpMestReasonFilterQuery tyFilter = new HisExpMestReasonFilterQuery();
            listExpMestReason = new HisExpMestReasonManager().Get(tyFilter);
        }

        private void GetExpMestType()
        {
            HisExpMestTypeFilterQuery tyFilter = new HisExpMestTypeFilterQuery();
            listExpMestType = new HisExpMestTypeManager().Get(tyFilter);
        }

        private void GetImpMestType()
        {
            HisImpMestTypeFilterQuery tyFilter = new HisImpMestTypeFilterQuery();
            listImpMestType = new HisImpMestTypeManager().Get(tyFilter);
        }

        private void ProcessImpMest()
        {
            try
            {
                CommonParam getParam = new CommonParam();
                HisImpMestFilterQuery impMestFilter = new HisImpMestFilterQuery();
                impMestFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                impMestFilter.IMP_TIME_TO = castFilter.TIME_TO;
                impMestFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMestFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                impMestFilter.IMP_MEST_TYPE_IDs = new List<long>()
                {
                    IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BTL,
                    IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DMTL,
                    IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL,
                    IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL,
                    IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL,
                    IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL
                };

                impMestFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;


                List<HIS_IMP_MEST> listImpMest = new HisImpMestManager(getParam).Get(impMestFilter);
                if (castFilter.BRANCH_ID != null)
                {
                    //listTreatment = listTreatment.Where(o => o.BRANCH_ID == castFilter.BRANCH_ID).ToList();
                    //listImpMest = listImpMest.Where(o => listTreatment.Exists(p => p.ID == o.TDL_TREATMENT_ID)).ToList();

                   

                    var departmentIds = (new HisDepartmentManager().Get(new HisDepartmentFilterQuery() { BRANCH_ID = this.castFilter.BRANCH_ID }) ?? new List<HIS_DEPARTMENT>()).Select(o => o.ID).ToList();

                    listImpMest = listImpMest.Where(o => departmentIds.Contains(o.REQ_DEPARTMENT_ID ??0)).ToList();
                        

                }
                if (listImpMest == null || listImpMest.Count == 0)
                {
                    LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => impMestFilter), impMestFilter));
                }

                if (listImpMest != null && listImpMest.Count > 0)
                {
                    int start = 0;
                    int count = listImpMest.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<HIS_IMP_MEST> listExpMestDetails = listImpMest.Skip(start).Take(limit).ToList();
                        HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                        impMestMedicineViewFilter.IMP_MEST_IDs = listExpMestDetails.Select(o => o.ID).ToList();
                        ProcessImportData(getParam, impMestMedicineViewFilter);

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
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

        private void ProcessImportData(CommonParam getParam, HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter)
        {
            List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new HisImpMestMedicineManager(getParam).GetView(impMestMedicineViewFilter);
            countExpMestMedicine += listImpMestMedicine.Count;
            if (listImpMestMedicine != null && listImpMestMedicine.Count > 0)
            {
                Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();

                var medicineTypesub = listMedicineType;
                if (castFilter.IS_BUSINESS.HasValue)
                {
                    if (castFilter.IS_BUSINESS == 1)
                    {
                        medicineTypesub = listMedicineType.Where(o => o.IS_BUSINESS == 1).ToList();
                    }
                    else
                    {
                        medicineTypesub = listMedicineType.Where(o => o.IS_BUSINESS != 1).ToList();
                    }
                }
                dicMedicineType = medicineTypesub.GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First());
                listImpMestMedicine = listImpMestMedicine.Where(o => medicineTypesub.Select(s => s.ID).Contains(o.MEDICINE_TYPE_ID)).ToList();


                foreach (var item in listImpMestMedicine)
                {
                    item.IMP_PRICE = item.IMP_PRICE * (item.IMP_VAT_RATIO + 1);
                    string key = item.MEDICINE_TYPE_ID + "_" + item.IMP_PRICE;
                    if (!dicRdo.ContainsKey(key)) dicRdo[key] = new Mrs00328RDO(item);
                    dicRdo[key].MEDICINE_TYPE_CODE = item.MEDICINE_TYPE_CODE;
                    dicRdo[key].MEDICINE_TYPE_NAME = item.MEDICINE_TYPE_NAME;
                    dicRdo[key].SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                    if (dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID))
                    {
                        dicRdo[key].CONCENTRA = dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID) ? dicMedicineType[item.MEDICINE_TYPE_ID].CONCENTRA : "";
                        dicRdo[key].MEDICINE_USE_FORM_NAME = dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID) ? dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_USE_FORM_NAME : "";
                        var parent = listMedicineType.FirstOrDefault(o => o.ID == dicMedicineType[item.MEDICINE_TYPE_ID].PARENT_ID);
                        if (parent != null)
                        {
                            dicRdo[key].PARENT_MEDICINE_TYPE_CODE = parent.MEDICINE_TYPE_CODE;
                            dicRdo[key].PARENT_MEDICINE_TYPE_NAME = parent.MEDICINE_TYPE_NAME;
                        }
                    }


                    dicRdo[key].PRICE = item.PRICE ?? 0;
                    if (item.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL) dicRdo[key].AMOUNT_NOI_TRU -= item.AMOUNT;
                    else if (item.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL) dicRdo[key].AMOUNT_HPKP -= item.AMOUNT;
                    else
                    {
                        dicRdo[key].AMOUNT_KHAC -= item.AMOUNT;
                    }
                    long departmentId = item.REQ_DEPARTMENT_ID ?? 0;
                    if (departmentCls != null && departmentCls.Contains(departmentId))
                    {
                        dicRdo[key].AMOUNT_CLS -= item.AMOUNT;
                    }
                    else if (departmentLs != null && departmentLs.Contains(departmentId))
                    {
                        dicRdo[key].AMOUNT_LS -= item.AMOUNT;
                        
                    }
                    else if (departmentKKB != null && departmentKKB.Contains(departmentId))
                    {
                        dicRdo[key].AMOUNT_KKB -= item.AMOUNT;
                    }


                    var type = listImpMestType.FirstOrDefault(o => o.ID == item.IMP_MEST_TYPE_ID);
                    if (type != null)
                    {
                        if (!dicRdo[key].DIC_MOBA_IMT.ContainsKey(type.IMP_MEST_TYPE_CODE ?? ""))
                        {
                            dicRdo[key].DIC_MOBA_IMT[type.IMP_MEST_TYPE_CODE ?? ""] = item.AMOUNT;
                        }
                        else
                        {
                            dicRdo[key].DIC_MOBA_IMT[type.IMP_MEST_TYPE_CODE ?? ""] += item.AMOUNT;
                        }

                    }  

                }
            }
        }

        private void ProcessExpMest()
        {
            try
            {
                CommonParam getParam = new CommonParam();
                HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery();
                expMestFilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                expMestFilter.FINISH_TIME_TO = castFilter.TIME_TO;
                expMestFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMestFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                expMestFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                List<HIS_EXP_MEST> listExpMest = new HisExpMestManager(getParam).Get(expMestFilter);

                if (castFilter.BRANCH_ID != null)
                {
                    //listTreatment = listTreatment.Where(o => o.BRANCH_ID == castFilter.BRANCH_ID).ToList();
                    //listExpMest = listExpMest.Where(o => listTreatment.Exists(p => p.ID == o.TDL_TREATMENT_ID)).ToList();

                   
                        var departmentIds = (new HisDepartmentManager().Get(new HisDepartmentFilterQuery() { BRANCH_ID = this.castFilter.BRANCH_ID }) ?? new List<HIS_DEPARTMENT>()).Select(o => o.ID).ToList();

                        listExpMest = listExpMest.Where(o => departmentIds.Contains(o.REQ_DEPARTMENT_ID)).ToList();
                    

                }
                listExpMest = listExpMest.Where(o => !this.CHMS_EXP_MEST_TYPE_IDs.Contains(o.EXP_MEST_TYPE_ID)).ToList();
                Inventec.Common.Logging.LogSystem.Info("listExpMest" + listExpMest.Count);
                if (listExpMest == null || listExpMest.Count == 0)
                {
                    LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => expMestFilter), expMestFilter));
                }

                if (listExpMest != null && listExpMest.Count > 0)
                {
                    int start = 0;
                    int count = listExpMest.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<HIS_EXP_MEST> listExpMestDetails = listExpMest.Skip(start).Take(limit).ToList();
                        HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                        expMestMedicineViewFilter.EXP_MEST_IDs = listExpMestDetails.Select(o => o.ID).ToList();
                        expMestMedicineViewFilter.IS_EXPORT = true;
                        ProcessExportData(getParam, expMestMedicineViewFilter, listExpMestDetails);
                        if ((!castFilter.IS_MOBA_ON_TIME.HasValue) || !castFilter.IS_MOBA_ON_TIME.Value)
                        {
                            ProcessMoveBackData(getParam, listExpMestDetails);
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
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

        private void ProcessExportData(CommonParam getParam, HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter, List<HIS_EXP_MEST> listExpMestDetails)
        {
            List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(getParam).GetView(expMestMedicineViewFilter);
            countExpMestMedicine += listExpMestMedicine.Count;
            if (listExpMestMedicine != null && listExpMestMedicine.Count > 0)
            {
                //listExpMestMedicine = listExpMestMedicine.Where(o => !this.CHMS_EXP_MEST_TYPE_IDs.Contains(o.EXP_MEST_TYPE_ID)).ToList();
                Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();

                var medicineTypesub = listMedicineType;
                if (castFilter.IS_BUSINESS.HasValue)
                {
                    if (castFilter.IS_BUSINESS == 1)
                    {
                        medicineTypesub = listMedicineType.Where(o => o.IS_BUSINESS == 1).ToList();
                    }
                    else
                    {
                        medicineTypesub = listMedicineType.Where(o => o.IS_BUSINESS != 1).ToList();
                    }
                }

                dicMedicineType = medicineTypesub.GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First());
                listExpMestMedicine = listExpMestMedicine.Where(o => medicineTypesub.Select(s => s.ID).Contains(o.MEDICINE_TYPE_ID)).ToList();

                foreach (var item in listExpMestMedicine)
                {
                    var expMest = listExpMestDetails.FirstOrDefault(o => o.ID == item.EXP_MEST_ID) ?? new HIS_EXP_MEST();

                    item.IMP_PRICE = item.IMP_PRICE * (item.IMP_VAT_RATIO + 1);
                    string key = item.MEDICINE_TYPE_ID + "_" + item.IMP_PRICE;
                    if (!dicRdo.ContainsKey(key)) dicRdo[key] = new Mrs00328RDO(item);
                    dicRdo[key].MEDICINE_TYPE_CODE = item.MEDICINE_TYPE_CODE;
                    dicRdo[key].MEDICINE_TYPE_NAME = item.MEDICINE_TYPE_NAME;
                    dicRdo[key].SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;

                    if (dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID))
                    {
                        dicRdo[key].CONCENTRA = dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID) ? dicMedicineType[item.MEDICINE_TYPE_ID].CONCENTRA : "";
                        dicRdo[key].MEDICINE_USE_FORM_NAME = dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID) ? dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_USE_FORM_NAME : "";

                        dicRdo[key].MEDICINE_GROUP_CODE = dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_GROUP_CODE;
                        dicRdo[key].MEDICINE_GROUP_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_GROUP_NAME;
                        var parent = listMedicineType.FirstOrDefault(o => o.ID == dicMedicineType[item.MEDICINE_TYPE_ID].PARENT_ID);
                        if (parent != null)
                        {
                            dicRdo[key].PARENT_MEDICINE_TYPE_CODE = parent.MEDICINE_TYPE_CODE;
                            dicRdo[key].PARENT_MEDICINE_TYPE_NAME = parent.MEDICINE_TYPE_NAME;
                        }
                    }

                    dicRdo[key].PRICE = item.PRICE ?? 0;
                    if (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK) dicRdo[key].AMOUNT_NGOAITRU += item.AMOUNT;
                    else if (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT) dicRdo[key].AMOUNT_NOI_TRU += item.AMOUNT;
                    else if (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP) dicRdo[key].AMOUNT_HPKP += item.AMOUNT;
                    else if (expMest.EXP_MEST_REASON_ID == expMestReasonId05) dicRdo[key].AMOUNT_XUATXA += item.AMOUNT;
                    else if (expMest.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__TNCC) dicRdo[key].AMOUNT_TNCC += item.AMOUNT;
                    else
                    {
                        dicRdo[key].AMOUNT_KHAC += item.AMOUNT;
                    }

                    if (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC)
                    {
                        dicRdo[key].AMOUNT_XUAT_KHAC += item.AMOUNT;
                        var reason = listExpMestReason.FirstOrDefault(o => o.ID == expMest.EXP_MEST_REASON_ID);
                        if(reason !=null)
                        {
                            if (!dicRdo[key].DIC_REASON.ContainsKey(reason.EXP_MEST_REASON_CODE ?? ""))
                            {
                                dicRdo[key].DIC_REASON[reason.EXP_MEST_REASON_CODE ?? ""] = item.AMOUNT;
                            }
                            else
                            {
                                dicRdo[key].DIC_REASON[reason.EXP_MEST_REASON_CODE ?? ""] += item.AMOUNT;
                            }

                        }

                    }

                    var type = listExpMestType.FirstOrDefault(o => o.ID == expMest.EXP_MEST_TYPE_ID);
                    if (type != null)
                    {
                        if (!dicRdo[key].DIC_EXP_MEST_TYPE.ContainsKey(type.EXP_MEST_TYPE_CODE ?? ""))
                        {
                            dicRdo[key].DIC_EXP_MEST_TYPE[type.EXP_MEST_TYPE_CODE ?? ""] = item.AMOUNT;
                        }
                        else
                        {
                            dicRdo[key].DIC_EXP_MEST_TYPE[type.EXP_MEST_TYPE_CODE ?? ""] += item.AMOUNT;
                        }

                    }  
                    long departmentId = item.REQ_DEPARTMENT_ID;
                    if (departmentId > 0)
                    {

                        if (departmentCls != null && departmentCls.Contains(departmentId))
                        {
                            dicRdo[key].AMOUNT_CLS += item.AMOUNT;
                        }
                        else if (departmentLs != null && departmentLs.Contains(departmentId))
                        {
                            dicRdo[key].AMOUNT_LS += item.AMOUNT;
                        }
                        else if (departmentKKB != null && departmentKKB.Contains(departmentId))
                        {
                            dicRdo[key].AMOUNT_KKB += item.AMOUNT;
                        }
                        else
                        {
                            dicRdo[key].AMOUNT_K += item.AMOUNT;
                        }
                        var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId);
                        if (department != null)
                        {
                            if (!dicRdo[key].DIC_REQ_DEPARTMENT.ContainsKey(department.DEPARTMENT_CODE ?? "NONE"))
                            {
                                dicRdo[key].DIC_REQ_DEPARTMENT.Add(department.DEPARTMENT_CODE ?? "NONE", item.AMOUNT);
                            }
                            else
                            {
                                dicRdo[key].DIC_REQ_DEPARTMENT[department.DEPARTMENT_CODE ?? "NONE"] += item.AMOUNT;
                            }
                        }
                    }
                }
            }
        }

        private void ProcessMoveBackData(CommonParam getParam, List<HIS_EXP_MEST> listExpMestDetails)
        {
            try
            {
                HisImpMestViewFilterQuery mobaFilter = new HisImpMestViewFilterQuery();
                mobaFilter.MOBA_EXP_MEST_IDs = listExpMestDetails.Select(o => o.ID).ToList();
                mobaFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST> listMobaImpMest = new HisImpMestManager(getParam).GetView(mobaFilter);
                if (listMobaImpMest != null && listMobaImpMest.Count > 0)
                {
                    Dictionary<long, long> dicExp = listExpMestDetails.GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First().EXP_MEST_TYPE_ID);
                    Dictionary<long, long> dicExpMestTypeOfMobaImpMest = new Dictionary<long, long>();
                    dicExpMestTypeOfMobaImpMest = listMobaImpMest.ToDictionary(o => o.ID, o => dicExp[o.MOBA_EXP_MEST_ID.Value]);

                    List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
                    int start = 0;
                    int count = listMobaImpMest.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<MOS.EFMODEL.DataModels.V_HIS_IMP_MEST> listImpMestDetails = listMobaImpMest.Skip(start).Take(limit).ToList();

                        HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                        impMestMedicineViewFilter.IMP_MEST_IDs = listImpMestDetails.Select(o => o.ID).ToList();
                        var impMestMedicines = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(getParam).GetView(impMestMedicineViewFilter);
                        if (impMestMedicines != null && impMestMedicines.Count > 0)
                        {
                            listImpMestMedicine.AddRange(impMestMedicines);
                            countImpMestMedicine += listImpMestMedicine.Count;
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }

                    if (listImpMestMedicine == null || listImpMestMedicine.Count == 0)
                    {
                        LogSystem.Info("p10 -> ProcessMoveBackData");
                        LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => mobaFilter), mobaFilter));
                    }

                    if (listImpMestMedicine != null && listImpMestMedicine.Count > 0)
                    {
                        foreach (var item in listImpMestMedicine)
                        {
                            long expMestTypeId = dicExpMestTypeOfMobaImpMest[item.IMP_MEST_ID];
                            //long departmentId = 0;
                            //if (departmentId > 0)
                            //{
                            Mrs00328RDO rdo = null;
                            item.IMP_PRICE = item.IMP_PRICE * (item.IMP_VAT_RATIO + 1);
                            string key = item.MEDICINE_TYPE_ID + "_" + item.IMP_PRICE;
                            if (dicRdo.ContainsKey(key))
                            {
                                rdo = dicRdo[key];
                                if (expMestTypeId == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK)
                                {
                                    rdo.AMOUNT_NGOAITRU -= item.AMOUNT;
                                }
                                else if (expMestTypeId == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT)
                                {
                                    rdo.AMOUNT_NOI_TRU -= item.AMOUNT;
                                }
                                else if (expMestTypeId == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP)
                                {
                                    rdo.AMOUNT_HPKP -= item.AMOUNT;
                                }
                                else
                                {
                                    rdo.AMOUNT_KHAC -= item.AMOUNT;
                                }
                                long departmentId = item.REQ_DEPARTMENT_ID ?? 0;
                                if (departmentId > 0)
                                {
                                    if (departmentCls != null && departmentCls.Contains(departmentId))
                                    {
                                        dicRdo[key].AMOUNT_CLS += item.AMOUNT;
                                    }
                                    else if (departmentLs != null && departmentLs.Contains(departmentId))
                                    {
                                        dicRdo[key].AMOUNT_LS += item.AMOUNT;
                                    }
                                    else if (departmentKKB != null && departmentKKB.Contains(departmentId))
                                    {
                                        dicRdo[key].AMOUNT_KKB += item.AMOUNT;
                                    }
                                    else
                                    {
                                        dicRdo[key].AMOUNT_K += item.AMOUNT;
                                    }

                                }
                                var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId);
                                if (department != null)
                                {
                                    if (!dicRdo[key].DIC_REQ_DEPARTMENT.ContainsKey(department.DEPARTMENT_CODE ?? "NONE"))
                                    {
                                        dicRdo[key].DIC_REQ_DEPARTMENT.Add(department.DEPARTMENT_CODE ?? "NONE", -item.AMOUNT);
                                    }
                                    else
                                    {
                                        dicRdo[key].DIC_REQ_DEPARTMENT[department.DEPARTMENT_CODE ?? "NONE"] -= item.AMOUNT;
                                    }
                                }


                                

                                var type = listImpMestType.FirstOrDefault(o => o.ID == item.IMP_MEST_TYPE_ID);
                                if (type != null)
                                {
                                    if (!dicRdo[key].DIC_MOBA_IMT.ContainsKey(type.IMP_MEST_TYPE_CODE ?? ""))
                                    {
                                        dicRdo[key].DIC_MOBA_IMT[type.IMP_MEST_TYPE_CODE ?? ""] = item.AMOUNT;
                                    }
                                    else
                                    {
                                        dicRdo[key].DIC_MOBA_IMT[type.IMP_MEST_TYPE_CODE ?? ""] += item.AMOUNT;
                                    }

                                }  
                            }
                            else
                            {
                                throw new DataMisalignedException("Loi thuat toan hoac CSDL. Co thu hoi nhung khong co xuat kho." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => item), item));
                            }
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                listRdo.AddRange(dicRdo.Values.OrderBy(o => o.MEDICINE_TYPE_NAME).ToList());
                listRdo = listRdo.Where(o => o.AMOUNT_HPKP != 0 || o.AMOUNT_KHAC != 0 || o.AMOUNT_XUATXA != 0 || o.AMOUNT_NGOAITRU != 0 || o.AMOUNT_NOI_TRU != 0).ToList();
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
            if (castFilter.TIME_FROM > 0)
            {
                dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
            }

            if (castFilter.TIME_TO > 0)
            {
                dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
            }

            objectTag.AddObjectData(store, "Medicine", listRdo.OrderBy(o => o.MEDICINE_TYPE_NAME).ToList());
            objectTag.AddObjectData(store, "Departments", HisDepartmentCFG.DEPARTMENTs);
        }
    }
}
