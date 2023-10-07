using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisMaterialType;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00037
{
    public class Mrs00037Processor : AbstractProcessor
    {
        Mrs00037Filter castFilter = null;
        Dictionary<string, Mrs00037RDO> dicRdo = new Dictionary<string, Mrs00037RDO>(); // key = MATERIAL_TYPE_id & imp_price
        List<Mrs00037RDO> listRdo = new List<Mrs00037RDO>();
        List<V_HIS_MATERIAL_TYPE> ListMaterialType = new List<V_HIS_MATERIAL_TYPE>();

        public Mrs00037Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00037Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00037Filter)this.reportFilter);
                LoadDataToRam();
                ProcessDepaExpMest();

                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void LoadDataToRam()
        {
            try
            {
                ListMaterialType = new HisMaterialTypeManager().GetView(new HisMaterialTypeViewFilterQuery());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                if (IsNotNullOrEmpty(dicRdo))
                {
                    listRdo.AddRange(dicRdo.Values.OrderBy(o => o.MATERIAL_TYPE_CODE).ToList());
                }
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessDepaExpMest()
        {
            try
            {
                CommonParam param = new CommonParam();
                HisExpMestViewFilterQuery depaExpMestFilter = new HisExpMestViewFilterQuery();
                depaExpMestFilter.FINISH_DATE_FROM = castFilter.TIME_FROM;
                depaExpMestFilter.FINISH_DATE_TO = castFilter.TIME_TO;
                depaExpMestFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                var listDepaExpMest = new HisExpMestManager(param).GetView(depaExpMestFilter);
                if (listDepaExpMest != null && listDepaExpMest.Count > 0)
                {
                    Dictionary<long, long> dicDepartmentByExpMestId = new Dictionary<long, long>();
                    foreach (var item in listDepaExpMest)
                    {
                        if (dicDepartmentByExpMestId.ContainsKey(item.ID))
                        {
                            throw new DataMisalignedException("CSDL co bug, co 2 DEPA_EXP_MEST cung chung 1 EXP_MEST_ID. Bao cao se tra lai sai du lieu. EXP_MEST_ID=" + item.ID);
                        }
                        else
                        {
                            dicDepartmentByExpMestId[item.ID] = item.REQ_DEPARTMENT_ID;
                        }
                    }

                    int start = 0;
                    int count = listDepaExpMest.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<V_HIS_EXP_MEST> depaExpMests = listDepaExpMest.Skip(start).Take(limit).ToList();
                        HisExpMestMaterialViewFilterQuery expMestMateFilter = new HisExpMestMaterialViewFilterQuery();
                        expMestMateFilter.EXP_MEST_IDs = depaExpMests.Select(s => s.ID).ToList();
                        ProcessExportMaterial(param, expMestMateFilter, dicDepartmentByExpMestId);

                        HisImpMestViewFilterQuery mobaImpMestFilter = new HisImpMestViewFilterQuery();
                        mobaImpMestFilter.MOBA_EXP_MEST_IDs = depaExpMests.Select(s => s.ID).ToList();
                        ProcessMoveBackMaterial(param, mobaImpMestFilter, dicDepartmentByExpMestId);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
                if (param.HasException)
                {
                    throw new DataMisalignedException("Co exception tai DAOGET trong qua trinh tong hop du lieu.");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                dicRdo.Clear();
            }
        }



        private void ProcessExportMaterial(CommonParam param, HisExpMestMaterialViewFilterQuery expMestFilter, Dictionary<long, long> dicDepaByExpMestId)
        {
            try
            {
                var listExpMestMate = new HisExpMestMaterialManager(param).GetView(expMestFilter);
                if (listExpMestMate != null && listExpMestMate.Count > 0)
                {
                    foreach (var item in listExpMestMate)
                    {
                        var materialType = ListMaterialType.FirstOrDefault(f => f.ID == item.MATERIAL_TYPE_ID);
                        if (materialType != null && materialType.IS_CHEMICAL_SUBSTANCE == 1)
                        {
                            string key = item.MATERIAL_TYPE_ID + "_" + item.IMP_PRICE;
                            Mrs00037RDO rdo = null;
                            if (dicRdo.ContainsKey(key))
                            {
                                rdo = dicRdo[key];
                            }
                            else
                            {
                                rdo = new Mrs00037RDO(item.MATERIAL_TYPE_CODE, item.MATERIAL_TYPE_NAME, item.SERVICE_UNIT_NAME, item.IMP_PRICE);
                            }
                            long departmentId = 0;
                            dicDepaByExpMestId.TryGetValue(item.EXP_MEST_ID ?? 0, out departmentId);
                            if (departmentId > 0)
                            {
                                if (HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_CLS.Contains(departmentId))
                                {
                                    rdo.AMOUNT_CLS += item.AMOUNT;
                                }
                                else if (HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_LS.Contains(departmentId))
                                {
                                    rdo.AMOUNT_LS += item.AMOUNT;
                                }
                                else if (HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_KKB.Contains(departmentId))
                                {
                                    rdo.AMOUNT_KKB += item.AMOUNT;
                                }
                            }
                            if (!dicRdo.ContainsKey(key))
                            {
                                dicRdo[key] = rdo;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessMoveBackMaterial(CommonParam param, HisImpMestViewFilterQuery mobaViewFilter, Dictionary<long, long> dicDepaByExpMestId)
        {
            try
            {
                mobaViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                var listMobaImpMest = new HisImpMestManager(param).GetView(mobaViewFilter);
                if (listMobaImpMest != null && listMobaImpMest.Count > 0)
                {
                    Dictionary<long, long> dicExpMestIdByImpMestId = new Dictionary<long, long>();
                    foreach (var item in listMobaImpMest)
                    {
                        if (dicExpMestIdByImpMestId.ContainsKey(item.ID))
                        {
                            throw new DataMisalignedException("CSDL co bug, co 2 MOBA_IMP_MEST cung chung 1 IMP_MEST_ID. Bao cao se tra lai sai du lieu. IMP_MEST_ID=" + item.ID);
                        }
                        else
                        {
                            dicExpMestIdByImpMestId[item.ID] = item.MOBA_EXP_MEST_ID ?? 0;
                        }
                    }

                    int start = 0;
                    int count = listMobaImpMest.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<V_HIS_IMP_MEST> mobaImpMests = listMobaImpMest.Skip(start).Take(limit).ToList();

                        HisImpMestMaterialViewFilterQuery impMestMateFilter = new HisImpMestMaterialViewFilterQuery();
                        impMestMateFilter.IMP_MEST_IDs = mobaImpMests.Select(s => s.ID).ToList();
                        var listImpMestMate = new HisImpMestMaterialManager(param).GetView(impMestMateFilter);
                        if (listImpMestMate != null && listImpMestMate.Count > 0)
                        {
                            foreach (var item in listImpMestMate)
                            {
                                var materialType = ListMaterialType.FirstOrDefault(f => f.ID == item.MATERIAL_TYPE_ID);
                                if (materialType != null && materialType.IS_CHEMICAL_SUBSTANCE == 1)
                                {
                                    long expMestId = dicExpMestIdByImpMestId[item.IMP_MEST_ID];
                                    long departmentId = 0;
                                    dicDepaByExpMestId.TryGetValue(expMestId, out departmentId);
                                    if (departmentId > 0)
                                    {
                                        Mrs00037RDO rdo = null;
                                        string key = item.MATERIAL_TYPE_ID + "_" + item.IMP_PRICE;
                                        if (dicRdo.ContainsKey(key))
                                        {
                                            rdo = dicRdo[key];
                                            if (MRS.MANAGER.Config.HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_CLS.Contains(departmentId))
                                            {
                                                rdo.AMOUNT_CLS -= item.AMOUNT;
                                            }
                                            else if (MRS.MANAGER.Config.HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_LS.Contains(departmentId))
                                            {
                                                rdo.AMOUNT_LS -= item.AMOUNT;
                                            }
                                            else if (MRS.MANAGER.Config.HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_KKB.Contains(departmentId))
                                            {
                                                rdo.AMOUNT_KKB -= item.AMOUNT;
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
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                }

                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                }

                objectTag.AddObjectData(store, "Report", listRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
