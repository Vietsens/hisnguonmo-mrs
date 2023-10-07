using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMedicineType;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00036
{
    public class Mrs00036Processor : AbstractProcessor
    {
        Mrs00036Filter castFilter = null;
        Dictionary<string, Mrs00036RDO> dicRdo = new Dictionary<string, Mrs00036RDO>(); // key = medicine_type_id & imp_price
        List<Mrs00036RDO> listRdo = new List<Mrs00036RDO>();
        private List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>();

        public Mrs00036Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00036Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00036Filter)this.reportFilter);
                LoadDataToRam();
                ProcessExpMest();

                result = true;
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
                if (IsNotNullOrEmpty(dicRdo))
                {
                    listRdo.AddRange(dicRdo.Values.OrderBy(o => o.MEDICINE_TYPE_CODE).ToList());
                }
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
                CommonParam param = new CommonParam();
                HisExpMestViewFilterQuery depaFilter = new HisExpMestViewFilterQuery();
                depaFilter.FINISH_DATE_FROM = castFilter.TIME_FROM;
                depaFilter.FINISH_DATE_TO = castFilter.TIME_TO;
                depaFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                var listDepaExpMest = new HisExpMestManager(param).GetView(depaFilter);
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

                        HisExpMestMedicineViewFilterQuery medicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                        medicineViewFilter.EXP_MEST_IDs = depaExpMests.Select(s => s.ID).ToList();
                        medicineViewFilter.IS_EXPORT = true;
                        ProcessExportMedicine(param, medicineViewFilter, dicDepartmentByExpMestId);

                        HisImpMestViewFilterQuery mobaFilter = new HisImpMestViewFilterQuery();
                        mobaFilter.MOBA_EXP_MEST_IDs = depaExpMests.Select(s => s.ID).ToList();
                        ProcessMoveBackMedicine(param, mobaFilter, dicDepartmentByExpMestId);

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

        private void ProcessExportMedicine(CommonParam param, HisExpMestMedicineViewFilterQuery medicineViewFilter, Dictionary<long, long> dicDepaByExpMestId)
        {
            try
            {
                var listExpMestMedi = new HisExpMestMedicineManager(param).GetView(medicineViewFilter);
                if (listExpMestMedi != null && listExpMestMedi.Count > 0)
                {
                    foreach (var item in listExpMestMedi)
                    {
                        var medicineType = ListMedicineType.FirstOrDefault(f => f.ID == item.MEDICINE_TYPE_ID);
                        if (medicineType != null && medicineType.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__KS)
                        {
                            string key = item.MEDICINE_TYPE_ID + "_" + item.IMP_PRICE;
                            Mrs00036RDO rdo = null;
                            if (dicRdo.ContainsKey(key))
                            {
                                rdo = dicRdo[key];
                            }
                            else
                            {
                                rdo = new Mrs00036RDO(item.MEDICINE_TYPE_CODE, item.MEDICINE_TYPE_NAME, item.SERVICE_UNIT_NAME, item.IMP_PRICE);
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

        private void ProcessMoveBackMedicine(CommonParam param, HisImpMestViewFilterQuery mobaFilter, Dictionary<long, long> dicDepaByExpMestId)
        {
            try
            {
                mobaFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                var listMobaImpMest = new HisImpMestManager(param).GetView(mobaFilter);
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

                        HisImpMestMedicineViewFilterQuery impMestMediFilter = new HisImpMestMedicineViewFilterQuery();
                        impMestMediFilter.IMP_MEST_IDs = mobaImpMests.Select(s => s.ID).ToList();
                        var listImpMestMedi = new HisImpMestMedicineManager(param).GetView(impMestMediFilter);
                        if (listImpMestMedi != null && listImpMestMedi.Count > 0)
                        {
                            foreach (var item in listImpMestMedi)
                            {
                                var medicineType = ListMedicineType.FirstOrDefault(f => f.ID == item.MEDICINE_TYPE_ID);
                                if (medicineType != null && medicineType.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__KS)
                                {
                                    long expMestId = dicExpMestIdByImpMestId[item.IMP_MEST_ID];
                                    long departmentId = 0;
                                    dicDepaByExpMestId.TryGetValue(expMestId, out departmentId);
                                    if (departmentId > 0)
                                    {
                                        Mrs00036RDO rdo = null;
                                        string key = item.MEDICINE_TYPE_ID + "_" + item.IMP_PRICE;
                                        if (dicRdo.ContainsKey(key))
                                        {
                                            rdo = dicRdo[key];
                                            if (HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_CLS.Contains(departmentId))
                                            {
                                                rdo.AMOUNT_CLS -= item.AMOUNT;
                                            }
                                            else if (HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_LS.Contains(departmentId))
                                            {
                                                rdo.AMOUNT_LS -= item.AMOUNT;
                                            }
                                            else if (HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_KKB.Contains(departmentId))
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

        private void LoadDataToRam()
        {
            try
            {
                ListMedicineType = new HisMedicineTypeManager().GetView(new HisMedicineTypeViewFilterQuery());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListMedicineType.Clear();
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                }
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
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
