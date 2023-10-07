using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;

namespace MRS.Processor.Mrs00505
{
    class Mrs00505Processor : AbstractProcessor
    {
        Mrs00505Filter castFilter = null;

        public Mrs00505Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<Mrs00505RDO> listRdo = new List<Mrs00505RDO>();

        HIS_DEPARTMENT listDepartments = new HIS_DEPARTMENT();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterials = new List<V_HIS_EXP_MEST_MATERIAL>();

        long yearReport = DateTime.Now.Year;

        public override Type FilterType()
        {
            return typeof(Mrs00505Filter);
        }

        protected override bool GetData()
        {
            bool valid = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00505Filter)this.reportFilter;
                // getData
                yearReport = IsNotNull(castFilter.CREATE_TIME) ? Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.CREATE_TIME.Value).Value.Year : DateTime.Now.Year;

                listDepartments = new HisDepartmentManager(paramGet).GetById(castFilter.DEPARTMENT_ID ?? 0) ?? new HIS_DEPARTMENT();

                HisMediStockViewFilterQuery mediStockFilter = new HisMediStockViewFilterQuery();
                mediStockFilter.DEPARTMENT_ID = castFilter.DEPARTMENT_ID;
                var listMediStocks = new HisMediStockManager(paramGet).GetView(mediStockFilter) ?? new List<V_HIS_MEDI_STOCK>();
                if (listMediStocks != null && castFilter.DEPARTMENT_IDs != null)
                {
                    listMediStocks = listMediStocks.Where(o => castFilter.DEPARTMENT_IDs.Contains(o.DEPARTMENT_ID)).ToList();
                }

                HisExpMestViewFilterQuery expMestFilter = new HisExpMestViewFilterQuery();
                expMestFilter.FINISH_TIME_FROM = castFilter.TIME_FROM < (long)Convert.ToDecimal(yearReport + "0101000000") ? (long)Convert.ToDecimal(yearReport + "0101000000") : castFilter.TIME_FROM;
                expMestFilter.FINISH_TIME_TO = castFilter.TIME_TO > (long)Convert.ToDecimal(yearReport + "1231235959") ? (long)Convert.ToDecimal(yearReport + "1231235959") : castFilter.TIME_TO;
                expMestFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                expMestFilter.EXP_MEST_TYPE_IDs = new List<long>()
                {
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP
                };
                expMestFilter.REQ_DEPARTMENT_ID = castFilter.DEPARTMENT_ID;
                expMestFilter.REQ_DEPARTMENT_IDs = castFilter.DEPARTMENT_IDs;
                var listExpMests = new HisExpMestManager(paramGet).GetView(expMestFilter) ?? new List<V_HIS_EXP_MEST>();

                HisExpMestViewFilterQuery expMest2Filter = new HisExpMestViewFilterQuery();
                expMest2Filter.FINISH_TIME_FROM = castFilter.TIME_FROM < (long)Convert.ToDecimal(yearReport + "0101000000") ? (long)Convert.ToDecimal(yearReport + "0101000000") : castFilter.TIME_FROM;
                expMest2Filter.FINISH_TIME_TO = castFilter.TIME_TO > (long)Convert.ToDecimal(yearReport + "1231235959") ? (long)Convert.ToDecimal(yearReport + "1231235959") : castFilter.TIME_TO;
                expMestFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                expMest2Filter.EXP_MEST_TYPE_IDs = new List<long>()
                {
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL
                };
                expMest2Filter.IMP_MEDI_STOCK_IDs = listMediStocks.Select(s => s.ID).ToList();
                listExpMests.AddRange(new HisExpMestManager(paramGet).GetView(expMest2Filter) ?? new List<V_HIS_EXP_MEST>());

                var skip = 0;
                while (listExpMests.Count - skip > 0)
                {
                    var listIDs = listExpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisExpMestMaterialViewFilterQuery expMestMaterialFilter = new HisExpMestMaterialViewFilterQuery();
                    expMestMaterialFilter.EXP_MEST_IDs = listIDs.Select(s => s.ID).ToList();
                    expMestMaterialFilter.IS_EXPORT = true;
                    listExpMestMaterials.AddRange(new HisExpMestMaterialManager(paramGet).GetView(expMestMaterialFilter) ?? new List<V_HIS_EXP_MEST_MATERIAL>());
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }
        private bool IsBed(V_HIS_SERE_SERV o)
        {
            return o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G;
        }
        protected override bool ProcessData()
        {
            bool valid = true;
            try
            {
                listRdo.Clear();

                //process
                if (IsNotNullOrEmpty(listExpMestMaterials))
                {
                    foreach (var expMestMaterial in listExpMestMaterials)
                    {
                        var rdo = new Mrs00505RDO();
                        rdo.MATERIAL_TYPE_ID = expMestMaterial.MATERIAL_TYPE_ID;
                        rdo.MATERIAL_TYPE_CODE = expMestMaterial.MATERIAL_TYPE_CODE;
                        rdo.MATERIAL_TYPE_NAME = expMestMaterial.MATERIAL_TYPE_NAME;

                        rdo.IMP_PRICE = expMestMaterial.IMP_PRICE;
                        rdo.SERVICE_UNIT_NAME = expMestMaterial.SERVICE_UNIT_NAME;

                        if (expMestMaterial.EXP_TIME >= (long)Convert.ToDecimal(yearReport + "0101000000") && expMestMaterial.EXP_TIME <= (long)Convert.ToDecimal(yearReport + "0131235959"))
                            rdo.AMOUNT_01 = expMestMaterial.AMOUNT;
                        else if (expMestMaterial.EXP_TIME >= (long)Convert.ToDecimal(yearReport + "0201000000") && expMestMaterial.EXP_TIME <= (long)Convert.ToDecimal(yearReport + "0229235959"))
                            rdo.AMOUNT_02 = expMestMaterial.AMOUNT;
                        else if (expMestMaterial.EXP_TIME >= (long)Convert.ToDecimal(yearReport + "0301000000") && expMestMaterial.EXP_TIME <= (long)Convert.ToDecimal(yearReport + "0331235959"))
                            rdo.AMOUNT_03 = expMestMaterial.AMOUNT;
                        else if (expMestMaterial.EXP_TIME >= (long)Convert.ToDecimal(yearReport + "0401000000") && expMestMaterial.EXP_TIME <= (long)Convert.ToDecimal(yearReport + "0430235959"))
                            rdo.AMOUNT_04 = expMestMaterial.AMOUNT;
                        else if (expMestMaterial.EXP_TIME >= (long)Convert.ToDecimal(yearReport + "0501000000") && expMestMaterial.EXP_TIME <= (long)Convert.ToDecimal(yearReport + "0531235959"))
                            rdo.AMOUNT_05 = expMestMaterial.AMOUNT;
                        else if (expMestMaterial.EXP_TIME >= (long)Convert.ToDecimal(yearReport + "0601000000") && expMestMaterial.EXP_TIME <= (long)Convert.ToDecimal(yearReport + "0630235959"))
                            rdo.AMOUNT_06 = expMestMaterial.AMOUNT;
                        else if (expMestMaterial.EXP_TIME >= (long)Convert.ToDecimal(yearReport + "0701000000") && expMestMaterial.EXP_TIME <= (long)Convert.ToDecimal(yearReport + "0731235959"))
                            rdo.AMOUNT_07 = expMestMaterial.AMOUNT;
                        else if (expMestMaterial.EXP_TIME >= (long)Convert.ToDecimal(yearReport + "0801000000") && expMestMaterial.EXP_TIME <= (long)Convert.ToDecimal(yearReport + "0831235959"))
                            rdo.AMOUNT_08 = expMestMaterial.AMOUNT;
                        else if (expMestMaterial.EXP_TIME >= (long)Convert.ToDecimal(yearReport + "0901000000") && expMestMaterial.EXP_TIME <= (long)Convert.ToDecimal(yearReport + "0930235959"))
                            rdo.AMOUNT_09 = expMestMaterial.AMOUNT;
                        else if (expMestMaterial.EXP_TIME >= (long)Convert.ToDecimal(yearReport + "1001000000") && expMestMaterial.EXP_TIME <= (long)Convert.ToDecimal(yearReport + "1031235959"))
                            rdo.AMOUNT_10 = expMestMaterial.AMOUNT;
                        else if (expMestMaterial.EXP_TIME >= (long)Convert.ToDecimal(yearReport + "1101000000") && expMestMaterial.EXP_TIME <= (long)Convert.ToDecimal(yearReport + "1130235959"))
                            rdo.AMOUNT_11 = expMestMaterial.AMOUNT;
                        else if (expMestMaterial.EXP_TIME >= (long)Convert.ToDecimal(yearReport + "1201000000") && expMestMaterial.EXP_TIME <= (long)Convert.ToDecimal(yearReport + "1231235959"))
                            rdo.AMOUNT_12 = expMestMaterial.AMOUNT;
                        listRdo.Add(rdo);
                    }

                    listRdo = listRdo.GroupBy(g => new { g.MATERIAL_TYPE_ID, g.IMP_PRICE }).Select(s => new Mrs00505RDO()
                    {
                        MATERIAL_TYPE_ID = s.First().MATERIAL_TYPE_ID,
                        MATERIAL_TYPE_CODE = s.First().MATERIAL_TYPE_CODE,
                        MATERIAL_TYPE_NAME = s.First().MATERIAL_TYPE_NAME,

                        IMP_PRICE = s.First().IMP_PRICE,
                        SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,

                        AMOUNT_01 = s.Sum(su => su.AMOUNT_01),
                        AMOUNT_02 = s.Sum(su => su.AMOUNT_02),
                        AMOUNT_03 = s.Sum(su => su.AMOUNT_03),
                        AMOUNT_04 = s.Sum(su => su.AMOUNT_04),
                        AMOUNT_05 = s.Sum(su => su.AMOUNT_05),
                        AMOUNT_06 = s.Sum(su => su.AMOUNT_06),
                        AMOUNT_07 = s.Sum(su => su.AMOUNT_07),
                        AMOUNT_08 = s.Sum(su => su.AMOUNT_08),
                        AMOUNT_09 = s.Sum(su => su.AMOUNT_09),
                        AMOUNT_10 = s.Sum(su => su.AMOUNT_10),
                        AMOUNT_11 = s.Sum(su => su.AMOUNT_11),
                        AMOUNT_12 = s.Sum(su => su.AMOUNT_12)
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("CREATE_TIME", IsNotNull(castFilter.CREATE_TIME) ? Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Now) : (long)Convert.ToDecimal(yearReport + "1231235959"));

                if (IsNotNull(listDepartments))
                {
                    dicSingleTag.Add("DEPARTMENT_NAME", listDepartments.DEPARTMENT_NAME);
                    dicSingleTag.Add("DEPARTMENT_CODE", listDepartments.DEPARTMENT_CODE);
                }
                objectTag.AddObjectData(store, "Rdo", listRdo.OrderBy(s => s.MATERIAL_TYPE_NAME).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

}
