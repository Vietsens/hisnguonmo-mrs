using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00302
{
    class Mrs00302Processor : AbstractProcessor
    {
        Mrs00302Filter Mrs00302Filter = null;
        List<MOS.EFMODEL.DataModels.V_HIS_EXP_MEST> listPrescription = null;
        List<Mrs00302RDO> list1 = new List<Mrs00302RDO>();
        List<List<V_HIS_EXP_MEST>> listCode = new List<List<V_HIS_EXP_MEST>>();

        public Mrs00302Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00302Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.Mrs00302Filter = (Mrs00302Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu MRS00302: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => Mrs00302Filter), Mrs00302Filter));
                CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 

                HisExpMestViewFilterQuery prescriptionFilter = new HisExpMestViewFilterQuery();
                prescriptionFilter.FINISH_TIME_FROM = Mrs00302Filter.TIME_FROM;
                prescriptionFilter.FINISH_TIME_TO = Mrs00302Filter.TIME_TO;
                prescriptionFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                prescriptionFilter.MEDI_STOCK_ID = Mrs00302Filter.EXP_MEDI_STOCK_ID;
                prescriptionFilter.EXP_MEST_TYPE_IDs = new List<long>()
                     {
                         IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
                         IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT
                     };
                listPrescription = new HisExpMestManager().GetView(prescriptionFilter);

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00302");
                }
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
                if (listPrescription != null && listPrescription.Count > 0)
                {

                    listPrescription = listPrescription.GroupBy(o => new { o.TDL_TREATMENT_CODE }).Select(o => o.First()).ToList();
                    var listOrder = listPrescription.OrderBy(o => o.TDL_TREATMENT_CODE).ToList();

                    int count = listPrescription.Count;
                    int split = count / 6;
                    int size = count % 6;

                    for (int i = 0; i < 6; i++)
                    {
                        if (i != 5)
                        {
                            listCode.Add(listOrder.GetRange(0, (size <= 0 ? split : split + 1)));
                            listOrder.RemoveRange(0, (size <= 0 ? split : split + 1));
                            size--;
                        }
                        else
                            listCode.Add(listOrder);

                        if (listCode[i].Count < listCode[0].Count)
                        {
                            listCode[i].Add(new V_HIS_EXP_MEST());
                        }
                    }

                    for (int i = 0; i < listCode[0].Count; i++)
                    {
                        Mrs00302RDO a = new Mrs00302RDO();
                        a.TREATMENT_CODE1 = listCode[0][i].TDL_TREATMENT_CODE;
                        a.TREATMENT_CODE2 = listCode[1][i].TDL_TREATMENT_CODE;
                        a.TREATMENT_CODE3 = listCode[2][i].TDL_TREATMENT_CODE;
                        a.TREATMENT_CODE4 = listCode[3][i].TDL_TREATMENT_CODE;
                        a.TREATMENT_CODE5 = listCode[4][i].TDL_TREATMENT_CODE;
                        a.TREATMENT_CODE6 = listCode[5][i].TDL_TREATMENT_CODE;

                        a.TREATMENT_CODE_NUMBER1 = Convert.ToInt64(listCode[0][i].TDL_TREATMENT_CODE);
                        a.TREATMENT_CODE_NUMBER2 = Convert.ToInt64(listCode[1][i].TDL_TREATMENT_CODE);
                        a.TREATMENT_CODE_NUMBER3 = Convert.ToInt64(listCode[2][i].TDL_TREATMENT_CODE);
                        a.TREATMENT_CODE_NUMBER4 = Convert.ToInt64(listCode[3][i].TDL_TREATMENT_CODE);
                        a.TREATMENT_CODE_NUMBER5 = Convert.ToInt64(listCode[4][i].TDL_TREATMENT_CODE);
                        a.TREATMENT_CODE_NUMBER6 = Convert.ToInt64(listCode[5][i].TDL_TREATMENT_CODE);
                        list1.Add(a);
                    }
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
                if (Mrs00302Filter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(Mrs00302Filter.TIME_FROM));
                }

                if (Mrs00302Filter.TIME_TO > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(Mrs00302Filter.TIME_TO));
                }
                objectTag.AddObjectData(store, "list1", list1);
                dicSingleTag.Add("COUNT_TREAT", listPrescription.Count);
                store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
