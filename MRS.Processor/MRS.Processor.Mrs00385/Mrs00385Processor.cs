using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisDepartment;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisMaterialType;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisExpMestReason;
using Inventec.Common.Logging;

namespace MRS.Processor.Mrs00385
{
    class Mrs00385Processor : AbstractProcessor
    {
        Mrs00385Filter castFilter = null;
        Dictionary<string, Mrs00385RDO> dicRdo = new Dictionary<string, Mrs00385RDO>(); // key = MATERIAL_TYPE_id & imp_price
        List<Mrs00385RDO> listRdo = new List<Mrs00385RDO>();
        List<HIS_MATERIAL_TYPE> listMaterialType = new List<HIS_MATERIAL_TYPE>();
        List<long> departmentCls = new List<long>();
        List<long> departmentLs = new List<long>();
        List<long> departmentKKB = new List<long>();
        List<HIS_EXP_MEST_REASON> listExpMestReason = new List<HIS_EXP_MEST_REASON>();
        private long HIS_EXP_MEST_REASON___05 = HisExpMestReasonCFG.HIS_EXP_MEST_REASON___05;
        public Mrs00385Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_EXP_MEST> listExpMest = null;



        public override Type FilterType()
        {
            return typeof(Mrs00385Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                try
                {
                    
                departmentKKB = HisDepartmentCFG.DEPARTMENTs.Where(o=>o.IS_EXAM == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Select(p=>p.ID).ToList();
                departmentLs = HisDepartmentCFG.DEPARTMENTs.Where(o => o.IS_CLINICAL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Select(p => p.ID).ToList();
                departmentCls = HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_CLS;
                }
                catch (Exception)
                {
                    
                }
                castFilter = (Mrs00385Filter)this.reportFilter;
                CommonParam getParam = new CommonParam();
                Inventec.Common.Logging.LogSystem.Info("Mrs00385" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                HisExpMestViewFilterQuery ExpMestViewFilter = new HisExpMestViewFilterQuery();
                ExpMestViewFilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                ExpMestViewFilter.FINISH_TIME_TO = castFilter.TIME_TO;
                ExpMestViewFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                ExpMestViewFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                listExpMest = new HisExpMestManager(getParam).GetView(ExpMestViewFilter);


                HisMaterialTypeFilterQuery MatyFilter = new HisMaterialTypeFilterQuery();
                listMaterialType = new HisMaterialTypeManager().Get(MatyFilter);
                if (castFilter.IS_BUSINESS != null)
                {
                    listMaterialType = listMaterialType.Where(o => (o.IS_BUSINESS??0) ==castFilter.IS_BUSINESS).ToList();
                }
                if (getParam.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00385");
                }
                //Lý do xuất khác
                GetExpMestReason();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetExpMestReason()
        {
            this.listExpMestReason = new HisExpMestReasonManager().Get(new HisExpMestReasonFilterQuery())??new List<HIS_EXP_MEST_REASON>();
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                ProcessExpMest(paramGet);
                if (castFilter.IS_MOBA_ON_TIME.HasValue && castFilter.IS_MOBA_ON_TIME.Value)
                {
                    ProcessImpMest();
                }

                listRdo.AddRange(dicRdo.Values.OrderBy(o => o.MATERIAL_TYPE_CODE).ToList());
                AddInfo(ref listRdo);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
        private void ProcessImpMest()
        {
            try
            {
                CommonParam getParam = new CommonParam();
                HisImpMestFilterQuery impMestFilter = new HisImpMestFilterQuery();
                impMestFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                impMestFilter.IMP_TIME_TO = castFilter.TIME_TO;
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
                        HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
                        impMestMaterialViewFilter.IMP_MEST_IDs = listExpMestDetails.Select(o => o.ID).ToList();
                        ProcessImportData(getParam, impMestMaterialViewFilter);

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

        private void ProcessImportData(CommonParam getParam,  HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter)
        {
            List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new HisImpMestMaterialManager(getParam).GetView(impMestMaterialViewFilter);
            if (listImpMestMaterial != null && listImpMestMaterial.Count > 0)
            {
                Dictionary<long, HIS_MATERIAL_TYPE> dicMaterialType = new Dictionary<long, HIS_MATERIAL_TYPE>();

                var materialTypesub = listMaterialType;
                if (castFilter.IS_BUSINESS.HasValue)
                {
                    if (castFilter.IS_BUSINESS == 1)
                    {
                        materialTypesub = listMaterialType.Where(o => o.IS_BUSINESS == 1).ToList();
                    }
                    else
                    {
                        materialTypesub = listMaterialType.Where(o => o.IS_BUSINESS != 1).ToList();
                    }
                }
                dicMaterialType = materialTypesub.GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First());
                //lọc theo vật tư hóa chất
                if (castFilter.IS_MATERIAL == true && castFilter.IS_CHEMICAL_SUBSTANCE != true)
                {
                    materialTypesub = materialTypesub.Where(o => o.IS_CHEMICAL_SUBSTANCE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                }
                if (castFilter.IS_CHEMICAL_SUBSTANCE == true && castFilter.IS_MATERIAL != true)
                {
                    materialTypesub = materialTypesub.Where(o => o.IS_CHEMICAL_SUBSTANCE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                }
                listImpMestMaterial = listImpMestMaterial.Where(o => materialTypesub.Select(s => s.ID).Contains(o.MATERIAL_TYPE_ID)).ToList();

                

                foreach (var item in listImpMestMaterial)
                {
                    item.IMP_PRICE = item.IMP_PRICE * (item.IMP_VAT_RATIO + 1);
                    string key = item.MATERIAL_TYPE_ID + "_" + item.IMP_PRICE;
                    if (!dicRdo.ContainsKey(key))
                    {
                        dicRdo[key] = new Mrs00385RDO(item.MATERIAL_TYPE_CODE, item.MATERIAL_TYPE_NAME, item.SERVICE_UNIT_NAME, item.IMP_PRICE, item.NATIONAL_NAME);
                        dicRdo[key].IS_CHEMICAL_SUBSTANCE = dicMaterialType.ContainsKey(item.MATERIAL_TYPE_ID)? dicMaterialType[item.MATERIAL_TYPE_ID].IS_CHEMICAL_SUBSTANCE: null;
                        dicRdo[key].MATERIAL_TYPE_CODE = item.MATERIAL_TYPE_CODE;
                        dicRdo[key].MATERIAL_TYPE_NAME = item.MATERIAL_TYPE_NAME;
                        dicRdo[key].SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        if (dicMaterialType.ContainsKey(item.MATERIAL_TYPE_ID))
                        {
                            var parent = listMaterialType.FirstOrDefault(o => o.ID == dicMaterialType[item.MATERIAL_TYPE_ID].PARENT_ID);
                            if (parent != null)
                            {
                                dicRdo[key].PARENT_MATERIAL_TYPE_CODE = parent.MATERIAL_TYPE_CODE;
                                dicRdo[key].PARENT_MATERIAL_TYPE_NAME = parent.MATERIAL_TYPE_NAME;
                            }
                        }
                    }
                    long departmentId = item.REQ_DEPARTMENT_ID ?? 0;
                    if (item.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL) dicRdo[key].AMOUNT_NOI_TRU -= item.AMOUNT;
                    else if (item.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL) dicRdo[key].AMOUNT_HPKP -= item.AMOUNT;
                    else
                    {
                        dicRdo[key].AMOUNT_KHAC_NT -= item.AMOUNT;
                    }
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

                }
            }
        }

        private void AddInfo(ref List<Mrs00385RDO> listRdo)
        {
            foreach (var item in listRdo)
            {
                var materialType = listMaterialType.FirstOrDefault(o => o.MATERIAL_TYPE_CODE == item.MATERIAL_TYPE_CODE);
                if (materialType != null)
                {
                    var parent = listMaterialType.FirstOrDefault(o =>  materialType.PARENT_ID ==o.ID);
                    if (parent != null)
                    {
                        item.PARENT_MATERIAL_TYPE_ID = parent.ID;
                        item.PARENT_MATERIAL_TYPE_CODE = parent.MATERIAL_TYPE_CODE;
                        item.PARENT_MATERIAL_TYPE_NAME = parent.MATERIAL_TYPE_NAME;
                    }
                }
            }
            if(castFilter.PARENT_MATERIAL_TYPE_IDs!=null)
            {
                listRdo = listRdo.Where(o => castFilter.PARENT_MATERIAL_TYPE_IDs.Contains(o.PARENT_MATERIAL_TYPE_ID)).ToList();
            }    
        }


        private void ProcessExpMest(CommonParam paramGet)
        {
            try
            {

                if (listExpMest != null && listExpMest.Count > 0)
                {
                    List<long> listMobaExpMestId = new List<long>();
                    ///Dictionary phuc vu kiem tra xuat cho don vi nao
                    Dictionary<long, long> dicDepartmentByExpMestId = CreateDictionaryDepartmentByExpMestId(listExpMest);
                    ProcessExportData(paramGet, dicDepartmentByExpMestId,listExpMest, ref listMobaExpMestId);
                    int start = 0;
                    int count = listMobaExpMestId.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var listId = listMobaExpMestId.Skip(start).Take(limit).ToList();

                        if ((!castFilter.IS_MOBA_ON_TIME.HasValue) || !castFilter.IS_MOBA_ON_TIME.Value)
                        {
                            ProcessMoveBackData(paramGet, dicDepartmentByExpMestId, listId);
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                dicRdo.Clear();
            }
        }
        private static Dictionary<long, long> CreateDictionaryDepartmentByExpMestId(List<V_HIS_EXP_MEST> listExpMest)
        {
            Dictionary<long, long> dicDepartmentByExpMestId = new Dictionary<long, long>();
            foreach (var item in listExpMest)
            {
                if (!dicDepartmentByExpMestId.ContainsKey(item.ID))
                {
                    dicDepartmentByExpMestId[item.ID] = item.REQ_DEPARTMENT_ID;
                }
            }
            return dicDepartmentByExpMestId;
        }
        private void ProcessMoveBackData(CommonParam getParam, Dictionary<long, long> dicDepartmentByExpMestId, List<long> listExpMestId)
        {
            HisImpMestViewFilterQuery mobaFilter = new HisImpMestViewFilterQuery();
            mobaFilter.MOBA_EXP_MEST_IDs = listExpMestId;
            mobaFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
            List<V_HIS_IMP_MEST> listMobaImpMest = new MOS.MANAGER.HisImpMest.HisImpMestManager(getParam).GetView(mobaFilter);
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

                HisImpMestMaterialViewFilterQuery impMest_MATERIALViewFilter = new HisImpMestMaterialViewFilterQuery();
                impMest_MATERIALViewFilter.IMP_MEST_IDs = listMobaImpMest.Select(o => o.ID).ToList();
                List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(getParam).GetView(impMest_MATERIALViewFilter);
                //lọc theo vật tư hóa chất
                var materialTypeSub = listMaterialType;
                if (castFilter.IS_MATERIAL == true && castFilter.IS_CHEMICAL_SUBSTANCE != true)
                {
                    materialTypeSub = materialTypeSub.Where(o => o.IS_CHEMICAL_SUBSTANCE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                }
                if (castFilter.IS_CHEMICAL_SUBSTANCE == true && castFilter.IS_MATERIAL != true)
                {
                    materialTypeSub = materialTypeSub.Where(o => o.IS_CHEMICAL_SUBSTANCE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                }
                listImpMestMaterial = listImpMestMaterial.Where(o => materialTypeSub.Exists(p => p.ID == o.MATERIAL_TYPE_ID)).ToList();
                if (listImpMestMaterial != null && listImpMestMaterial.Count > 0)
                {
                    foreach (var item in listImpMestMaterial)
                    {
                        if (!dictionaryExpMestIdByImpMestId.ContainsKey(item.IMP_MEST_ID)) continue;
                        long expMestId = dictionaryExpMestIdByImpMestId[item.IMP_MEST_ID];
                        long departmentId = 0;
                        if (dicDepartmentByExpMestId.ContainsKey(expMestId))
                        {
                            departmentId = dicDepartmentByExpMestId[expMestId];
                        }
                        //if (departmentId > 0)
                        {
                            Mrs00385RDO rdo = null;
                            item.IMP_PRICE = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                            string key = item.MATERIAL_TYPE_ID + "_" + item.IMP_PRICE;
                            if (dicRdo.ContainsKey(key))
                            {
                                rdo = dicRdo[key];
                                if (item.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL) dicRdo[key].AMOUNT_NOI_TRU -= item.AMOUNT;
                                else if (item.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL) dicRdo[key].AMOUNT_HPKP -= item.AMOUNT;
                                else
                                {
                                    dicRdo[key].AMOUNT_KHAC_NT -= item.AMOUNT;
                                }
                                if (departmentCls != null && departmentCls.Contains(departmentId))
                                {
                                    rdo.AMOUNT_CLS -= item.AMOUNT;
                                }
                                else if (departmentLs != null && departmentLs.Contains(departmentId))
                                {
                                    rdo.AMOUNT_LS -= item.AMOUNT;
                                }
                                else if (departmentKKB != null && departmentKKB.Contains(departmentId))
                                {
                                    rdo.AMOUNT_KKB -= item.AMOUNT;
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
                            }
                            else
                            {
                                throw new DataMisalignedException("Loi thuat toan hoac CSDL. Co thu hoi nhung khong co xuat kho." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => item), item));
                            }
                        }
                    }
                }
            }
        }

        private void ProcessExportData(CommonParam getParam,Dictionary<long, long> dicDepartmentByExpMestId, List<V_HIS_EXP_MEST> listExpMest,ref List<long> listMobaExpMestId)
        {
            List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();

            ///Dictionary phuc vu kiem tra xuat cho don vi nao
            var listExpMestId = listExpMest.Select(o => o.ID).ToList();
            if (IsNotNullOrEmpty(listExpMestId))
            {
                int start = 0;
                int count = listExpMestId.Count;
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var listId = listExpMestId.Skip(start).Take(limit).ToList();

                    HisExpMestMaterialViewFilterQuery materialFilter = new HisExpMestMaterialViewFilterQuery();
                    materialFilter.EXP_MEST_IDs = listId;
                    materialFilter.IS_EXPORT = true;
                    var listData = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(getParam).GetView(materialFilter);
                    if (IsNotNullOrEmpty(listData))
                    {
                        listExpMestMaterial.AddRange(listData);
                    }

                    start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                }
                listExpMestMaterial = listExpMestMaterial.Where(o => listMaterialType.Exists(p => p.ID == o.MATERIAL_TYPE_ID)).ToList();
                listMobaExpMestId = listExpMestMaterial.Where(o => o.TH_AMOUNT > 0).Select(p => p.EXP_MEST_ID ?? 0).Distinct().ToList();

                //lọc theo vật tư hóa chất
                if (castFilter.IS_MATERIAL == true && castFilter.IS_CHEMICAL_SUBSTANCE != true)
                {
                    listExpMestMaterial = listExpMestMaterial.Where(o => o.IS_CHEMICAL_SUBSTANCE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                }
                if (castFilter.IS_CHEMICAL_SUBSTANCE == true && castFilter.IS_MATERIAL != true)
                {
                    listExpMestMaterial = listExpMestMaterial.Where(o => o.IS_CHEMICAL_SUBSTANCE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                }
                if (listExpMestMaterial != null && listExpMestMaterial.Count > 0)
                {
                    foreach (var item in listExpMestMaterial)
                    {
                        var expMest = listExpMest.FirstOrDefault(o => o.ID == item.EXP_MEST_ID) ?? new V_HIS_EXP_MEST();
                        item.IMP_PRICE = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        string key = item.MATERIAL_TYPE_ID + "_" + item.IMP_PRICE;
                        Mrs00385RDO rdo = null;
                        if (dicRdo.ContainsKey(key))
                        {
                            rdo = dicRdo[key];
                        }
                        else
                        {
                            rdo = new Mrs00385RDO(item.MATERIAL_TYPE_CODE, item.MATERIAL_TYPE_NAME, item.SERVICE_UNIT_NAME, item.IMP_PRICE, item.NATIONAL_NAME);
                            rdo.IS_CHEMICAL_SUBSTANCE = item.IS_CHEMICAL_SUBSTANCE;
                            dicRdo.Add(key,rdo);
                        }

                        if (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK) dicRdo[key].AMOUNT_NGOAITRU += item.AMOUNT;
                        else if (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT) dicRdo[key].AMOUNT_NOI_TRU += item.AMOUNT;
                        else if (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP) dicRdo[key].AMOUNT_HPKP += item.AMOUNT;
                        else if (expMest.EXP_MEST_REASON_ID == HIS_EXP_MEST_REASON___05) dicRdo[key].AMOUNT_XUATXA += item.AMOUNT;
                        else 
                        {
                            dicRdo[key].AMOUNT_KHAC_NT += item.AMOUNT;
                        }
                        if (expMest.EXP_MEST_REASON_ID != null)
                        {
                            var expMestReason = listExpMestReason.FirstOrDefault(o=>o.ID == expMest.EXP_MEST_REASON_ID);
                            if (expMestReason != null)
                            {
                                if (!dicRdo[key].DIC_REASON.ContainsKey(expMestReason.EXP_MEST_REASON_CODE ?? "NONE"))
                                {
                                    dicRdo[key].DIC_REASON.Add(expMestReason.EXP_MEST_REASON_CODE ?? "NONE", item.AMOUNT);
                                }
                                else
                                {
                                    dicRdo[key].DIC_REASON[expMestReason.EXP_MEST_REASON_CODE ?? "NONE"] += item.AMOUNT;
                                }
                            }
                        }
                        long departmentId = 0;
                        if (dicDepartmentByExpMestId.ContainsKey(item.EXP_MEST_ID??0))
                        {
                            departmentId = dicDepartmentByExpMestId[item.EXP_MEST_ID ?? 0];
                        }
                        if (departmentId > 0)
                        {
                            if (departmentCls != null && departmentCls.Contains(departmentId))
                            {
                                rdo.AMOUNT_CLS += item.AMOUNT;
                            }
                            else if (departmentLs != null && departmentLs.Contains(departmentId))
                            {
                                rdo.AMOUNT_LS += item.AMOUNT;
                            }
                            else if (departmentKKB != null && departmentKKB.Contains(departmentId))
                            {
                                rdo.AMOUNT_KKB += item.AMOUNT;
                            }
                            else
                            {
                                rdo.AMOUNT_K += item.AMOUNT;
                            }
                            if (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC)
                            {
                                rdo.AMOUNT_KHAC += item.AMOUNT;
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
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0));
                }

                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0));
                }

                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(o => o.MATERIAL_TYPE_NAME).ToList());

                objectTag.AddObjectData(store, "Departments", HisDepartmentCFG.DEPARTMENTs);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
