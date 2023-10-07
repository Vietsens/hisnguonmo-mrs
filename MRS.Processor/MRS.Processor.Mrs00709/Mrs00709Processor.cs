using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using Inventec.Common.DateTime; 
using MRS.MANAGER.Config; 
using FlexCel.Report; 
 
using MOS.MANAGER.HisImpMest; 
using MOS.MANAGER.HisMediStock; 
using MOS.MANAGER.HisMedicineType; 
using MOS.MANAGER.HisMedicineTypeAcin; 
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisSupplier;
using MOS.MANAGER.HisMedicineBean; 

namespace MRS.Processor.Mrs00709
{
    public class Mrs00709Processor : AbstractProcessor
    {
        private Mrs00709Filter filter;
        List<Mrs00709RDO> listNCCImp = new List<Mrs00709RDO>();
        List<Mrs00709RDO> listNCCImpMema = new List<Mrs00709RDO>();
        List<Mrs00709RDO> listExpMema = new List<Mrs00709RDO>();
        List<Mrs00709RDO> listData = new List<Mrs00709RDO>();

        //List<string> listKey = new List<string>();
       
           
        CommonParam paramGet = new CommonParam(); 
        public Mrs00709Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00709Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            filter = (Mrs00709Filter)reportFilter; 
            try
            {
                listData = new MRS.Processor.Mrs00709.ManagerSql().GetMedicine(filter) ?? new List<Mrs00709RDO>();
                var groupLevel1 = listData.GroupBy(g => new { g.MEMA_ID, g.EXP_DATE, g.SERVICE_CODE, g.TYPE, g.IMP_DATE, g.SUPPLIER_CODE, g.TDL_BID_NUMBER, g.TDL_BID_YEAR, g.DOCUMENT_DATE, g.DOCUMENT_NUMBER }).ToList();
                foreach (var item in groupLevel1)
                {
                    Mrs00709RDO rdo = new Mrs00709RDO();
                    rdo.MEMA_ID = item.Key.MEMA_ID;
                    rdo.EXP_DATE = item.Key.EXP_DATE;
                    rdo.SERVICE_CODE = item.Key.SERVICE_CODE;
                    rdo.SERVICE_NAME = item.First().SERVICE_NAME;
                    rdo.TYPE = item.Key.TYPE;
                    rdo.IMP_DATE = item.Key.IMP_DATE;
                    rdo.SUPPLIER_CODE = item.Key.SUPPLIER_CODE;
                    rdo.SUPPLIER_NAME = item.First().SUPPLIER_NAME;
                    rdo.TDL_BID_NUMBER = item.Key.TDL_BID_NUMBER;
                    rdo.TDL_BID_YEAR = item.Key.TDL_BID_YEAR;
                    rdo.DOCUMENT_DATE = item.Key.DOCUMENT_DATE;
                    rdo.DOCUMENT_NUMBER = item.Key.DOCUMENT_NUMBER;
                    rdo.IMP_AMOUNT = item.Sum(s => s.IMP_AMOUNT ?? 0);
                    rdo.AMOUNT = item.First().AMOUNT ?? 0;
                    rdo.SERVICE_UNIT_CODE = item.First().SERVICE_UNIT_CODE;
                    rdo.SERVICE_UNIT_NAME = item.First().SERVICE_UNIT_NAME;
                    rdo.MEMA_ID = item.First().MEMA_ID;
                    rdo.EXP_AMOUNT = item.Sum(s => s.EXP_AMOUNT ?? 0);
                    //rdo.END_AMOUNT = rdo.IMP_AMOUNT + rdo.AMOUNT - rdo.EXP_AMOUNT;
                    var DIC_EXP_DEPARTMENT = new Dictionary<string, decimal>();
                    foreach (var dc in item)
                    {
                        if (DIC_EXP_DEPARTMENT.ContainsKey(dc.DEPARTMENT_CODE??"0"))
                        {
                            DIC_EXP_DEPARTMENT[dc.DEPARTMENT_CODE ?? "0"] += dc.EXP_AMOUNT ?? 0;
                        }
                        else
                        {
                            DIC_EXP_DEPARTMENT.Add(dc.DEPARTMENT_CODE ?? "0", dc.EXP_AMOUNT ?? 0);
                        }
                    }
                    rdo.JSON_EXP_DEPARTMENT = "["+Newtonsoft.Json.JsonConvert.SerializeObject(DIC_EXP_DEPARTMENT)+"]";
                    listExpMema.Add(rdo);
                }
                listData = new MRS.Processor.Mrs00709.ManagerSql().GetMaterial(filter) ?? new List<Mrs00709RDO>();
                var groupLevel2 = listData.GroupBy(g => new { g.MEMA_ID, g.EXP_DATE, g.SERVICE_CODE, g.TYPE, g.IMP_DATE, g.SUPPLIER_CODE, g.TDL_BID_NUMBER, g.TDL_BID_YEAR, g.DOCUMENT_DATE, g.DOCUMENT_NUMBER }).ToList();
                foreach (var item in groupLevel2)
                {
                    Mrs00709RDO rdo = new Mrs00709RDO();
                    rdo.MEMA_ID = item.Key.MEMA_ID;
                    rdo.EXP_DATE = item.Key.EXP_DATE;
                    rdo.SERVICE_CODE = item.Key.SERVICE_CODE;
                    rdo.SERVICE_NAME = item.First().SERVICE_NAME;
                    rdo.TYPE = item.Key.TYPE;
                    rdo.IMP_DATE = item.Key.IMP_DATE;
                    rdo.SUPPLIER_CODE = item.Key.SUPPLIER_CODE;
                    rdo.SUPPLIER_NAME = item.First().SUPPLIER_NAME;
                    rdo.TDL_BID_NUMBER = item.Key.TDL_BID_NUMBER;
                    rdo.TDL_BID_YEAR = item.Key.TDL_BID_YEAR;
                    rdo.DOCUMENT_DATE = item.Key.DOCUMENT_DATE;
                    rdo.DOCUMENT_NUMBER = item.Key.DOCUMENT_NUMBER;
                    rdo.IMP_AMOUNT = item.Sum(s => s.IMP_AMOUNT ?? 0);
                    rdo.AMOUNT = item.First().AMOUNT ?? 0;
                    rdo.SERVICE_UNIT_CODE = item.First().SERVICE_UNIT_CODE;
                    rdo.SERVICE_UNIT_NAME = item.First().SERVICE_UNIT_NAME;
                    rdo.MEMA_ID = item.First().MEMA_ID;
                    rdo.EXP_AMOUNT = item.Sum(s => s.EXP_AMOUNT ?? 0);
                    //rdo.END_AMOUNT = rdo.IMP_AMOUNT + rdo.AMOUNT - rdo.EXP_AMOUNT;
                    var DIC_EXP_DEPARTMENT = new Dictionary<string, decimal>();
                    foreach (var dc in item)
                    {
                        if (DIC_EXP_DEPARTMENT.ContainsKey(dc.DEPARTMENT_CODE ?? "0"))
                        {
                            DIC_EXP_DEPARTMENT[dc.DEPARTMENT_CODE ?? "0"] += dc.EXP_AMOUNT ?? 0;
                        }
                        else
                        {
                            DIC_EXP_DEPARTMENT.Add(dc.DEPARTMENT_CODE ?? "0", dc.EXP_AMOUNT ?? 0);
                        }
                    }
                    rdo.JSON_EXP_DEPARTMENT = "[" + Newtonsoft.Json.JsonConvert.SerializeObject(DIC_EXP_DEPARTMENT) + "]";
                    listExpMema.Add(rdo);
                }
                listData.Clear();
                listData = null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 

                result = false; 
            }
            return result; 
        }
        
        protected override bool ProcessData()
        {
            var result = true; 
            try
            {

                if (IsNotNullOrEmpty(listExpMema))
                {
                   
                }

                
            }
            catch (Exception ex)
            {
                result = false; 
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return result; 
        }

       
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", filter.TIME_FROM);
            dicSingleTag.Add("TIME_TO", filter.TIME_TO);
            objectTag.AddObjectData(store, "ExpMemas", listExpMema.OrderBy(o => o.EXP_DATE).ToList());
            objectTag.AddObjectData(store, "NCCImpMemas", listExpMema.GroupBy(o => new { o.SERVICE_CODE, o.TYPE, o.SUPPLIER_CODE, o.TDL_BID_NUMBER, o.TDL_BID_YEAR, o.DOCUMENT_NUMBER, o.DOCUMENT_DATE, o.IMP_DATE, o.MEMA_ID }).Select(p => p.First()).OrderBy(q => q.SERVICE_NAME).ToList());
            objectTag.AddObjectData(store, "NCCImps", listExpMema.GroupBy(o => new { o.SUPPLIER_CODE, o.TDL_BID_NUMBER, o.TDL_BID_YEAR, o.DOCUMENT_NUMBER, o.DOCUMENT_DATE, o.IMP_DATE }).Select(p => p.First()).OrderBy(q=>q.IMP_DATE).ToList());

            string[] key = new string[] { "SUPPLIER_CODE", "TDL_BID_NUMBER", "TDL_BID_YEAR", "DOCUMENT_NUMBER", "DOCUMENT_DATE", "IMP_DATE" };

            objectTag.AddRelationship(store, "NCCImps", "NCCImpMemas", key, key);

            string[] key1 = new string[] { "SUPPLIER_CODE", "TDL_BID_NUMBER", "TDL_BID_YEAR", "DOCUMENT_NUMBER", "DOCUMENT_DATE", "IMP_DATE", "TYPE", "SERVICE_CODE", "MEMA_ID" };

            objectTag.AddRelationship(store, "NCCImpMemas", "ExpMemas", key1, key1);
        }

       
    }
}
