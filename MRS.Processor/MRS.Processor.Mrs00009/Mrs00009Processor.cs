using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00009
{
    public class Mrs00009Processor : AbstractProcessor
    {
        Mrs00009Filter castFilter = null;
        Dictionary<string, Mrs00009RDO> dicRdo = new Dictionary<string, Mrs00009RDO>(); // key = MATERIAL_TYPE_id & imp_price
        List<Mrs00009RDO> listRdo = new List<Mrs00009RDO>();
        List<long> LIST_DEPARTMENT_ID__GROUP_CLS = new List<long>();
        List<long> LIST_DEPARTMENT_ID__GROUP_KKB = new List<long>();
        List<long> LIST_DEPARTMENT_ID__GROUP_LS = new List<long>();
        public Mrs00009Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00009Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00009Filter)this.reportFilter);
                try
                {
                    LIST_DEPARTMENT_ID__GROUP_CLS=MRS.MANAGER.Config.HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_CLS ;
                    LIST_DEPARTMENT_ID__GROUP_KKB=MRS.MANAGER.Config.HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_KKB ;
                    LIST_DEPARTMENT_ID__GROUP_LS=MRS.MANAGER.Config.HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_LS ;
                   
                }
                catch { }
                ProcessExpMest();

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
                HisExpMestViewFilterQuery depaFilter = new HisExpMestViewFilterQuery();
                depaFilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                depaFilter.FINISH_TIME_TO = castFilter.TIME_TO;
                depaFilter.REQ_DEPARTMENT_IDs = castFilter.REQ_DEPARTMENT_IDs;
                depaFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                depaFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                depaFilter.EXP_MEST_TYPE_IDs = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,// xuat don thuoc                       
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT,
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP,
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__PL
                    };
                List<V_HIS_EXP_MEST> listDepaExpMest = new HisExpMestManager(getParam).GetView(depaFilter);

                ///Dictionary phuc vu kiem tra xuat cho don vi nao
                Dictionary<long, long> dicDepartmentByExpMestId = CreateDictionaryDepartmentByExpMestId(listDepaExpMest);

                if (listDepaExpMest != null && listDepaExpMest.Count > 0)
                {
                    HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
                    expMestMaterialViewFilter.EXP_MEST_IDs = listDepaExpMest.Select(o => o.ID).ToList();
                    ProcessExportData(getParam, expMestMaterialViewFilter, dicDepartmentByExpMestId);

                    HisImpMestViewFilterQuery mobaFilter = new HisImpMestViewFilterQuery();
                    mobaFilter.MOBA_EXP_MEST_IDs = listDepaExpMest.Select(o => o.ID).ToList();
                    ProcessMoveBackData(getParam, dicDepartmentByExpMestId, mobaFilter);
                }

                if (getParam.HasException)
                {
                    throw new DataMisalignedException("Co exception tai DAOGET trong qua trinh tong hop du lieu.");
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                dicRdo.Clear();
            }
        }

        private void ProcessMoveBackData(CommonParam getParam, Dictionary<long, long> dicDepartmentByExpMestId, HisImpMestViewFilterQuery mobaFilter)
        {
            mobaFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
            List<V_HIS_IMP_MEST> listMobaImpMest = new HisImpMestManager(getParam).GetView(mobaFilter);
            if (mobaFilter != null && mobaFilter.MOBA_EXP_MEST_IDs != null)
            {
                var impMestIds = mobaFilter.MOBA_EXP_MEST_IDs.ToList();
                var skip = 0;
                while (impMestIds.Count - skip > 0)
                {
                    var Ids = impMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestViewFilterQuery ImpMestFilter = mobaFilter;
                    ImpMestFilter.MOBA_EXP_MEST_IDs = Ids;
                    var listMobaImpMestSub = new HisImpMestManager(param).GetView(ImpMestFilter);
                    if (listMobaImpMestSub != null)
                    {
                        listMobaImpMest.AddRange(listMobaImpMestSub);
                    }
                }
            }
            if (listMobaImpMest != null && listMobaImpMest.Count > 0)
            {
                Dictionary<long, long> dictionaryExpMestIdByImpMestId = new Dictionary<long, long>();
                foreach (var item in listMobaImpMest)
                {
                    if (dictionaryExpMestIdByImpMestId.ContainsKey(item.ID))
                    {
                        dictionaryExpMestIdByImpMestId[item.ID] = item.MOBA_EXP_MEST_ID ?? 0;
                    }
                }

                HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
                impMestMaterialViewFilter.IMP_MEST_IDs = listMobaImpMest.Select(o => o.ID).ToList();
                List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new HisImpMestMaterialManager(getParam).GetView(impMestMaterialViewFilter);
                if (listImpMestMaterial != null && listImpMestMaterial.Count > 0)
                {
                    foreach (var item in listImpMestMaterial)
                    {
                        if (!dictionaryExpMestIdByImpMestId.ContainsKey(item.IMP_MEST_ID))
                        {
                            continue;
                        }
                        long expMestId = dictionaryExpMestIdByImpMestId[item.IMP_MEST_ID];
                        long departmentId = 0;
                        dicDepartmentByExpMestId.TryGetValue(expMestId, out departmentId);
                        if (departmentId > 0)
                        {
                            Mrs00009RDO rdo = null;
                            string key = item.MATERIAL_TYPE_ID + "_" + item.IMP_PRICE;
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

        private void ProcessExportData(CommonParam getParam, HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter, Dictionary<long, long> dicDepartmentByExpMestId)
        {
            List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
            if (expMestMaterialViewFilter != null &&expMestMaterialViewFilter.EXP_MEST_IDs!=null)
            {
                var expMestIds = expMestMaterialViewFilter.EXP_MEST_IDs.ToList();
                var skip = 0;
                while (expMestIds.Count - skip > 0)
                {
                    var Ids = expMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisExpMestMaterialViewFilterQuery ExpMestMaterialFilter = expMestMaterialViewFilter;
                    ExpMestMaterialFilter.EXP_MEST_IDs = Ids;
                    var listExpMestMaterialSub = new HisExpMestMaterialManager(param).GetView(ExpMestMaterialFilter);
                    if (listExpMestMaterialSub != null)
                    {
                        listExpMestMaterial.AddRange(listExpMestMaterialSub);
                    }
                }
            }
            if (listExpMestMaterial != null && listExpMestMaterial.Count > 0)
            {
                foreach (var item in listExpMestMaterial)
                {
                    string key = item.MATERIAL_TYPE_ID + "_" + item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    Mrs00009RDO rdo = null;
                    if (dicRdo.ContainsKey(key))
                    {
                        rdo = dicRdo[key];
                    }
                    else
                    {
                        rdo = new Mrs00009RDO(item.MATERIAL_TYPE_CODE, item.MATERIAL_TYPE_NAME, item.SERVICE_UNIT_NAME, item.IMP_PRICE * (1 + item.IMP_VAT_RATIO));
                        rdo.REQ_DEPARTMENT_ID = item.REQ_DEPARTMENT_ID;
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


                    if (!dicRdo.ContainsKey(key))
                    {
                        dicRdo[key] = rdo;
                    }
                }
            }
        }

        private static Dictionary<long, long> CreateDictionaryDepartmentByExpMestId(List<V_HIS_EXP_MEST> listDepaExpMest)
        {
            Dictionary<long, long> dicDepartmentByExpMestId = new Dictionary<long, long>();
            foreach (var item in listDepaExpMest)
            {
                if (!dicDepartmentByExpMestId.ContainsKey(item.ID))
                {
                    dicDepartmentByExpMestId[item.ID] = item.REQ_DEPARTMENT_ID;
                }
            }
            return dicDepartmentByExpMestId;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(dicRdo))
                {
                    listRdo.AddRange(dicRdo.Values.OrderBy(o => o.MATERIAL_TYPE_CODE).ToList());
                }
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
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0));
                }

                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0));
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
