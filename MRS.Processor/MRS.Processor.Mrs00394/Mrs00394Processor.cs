using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
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

namespace MRS.Processor.Mrs00394
{
    class Mrs00394Processor : AbstractProcessor
    {
        Mrs00394Filter castFilter = null;
        Dictionary<long, Mrs00394RDO> dicRdo = new Dictionary<long, Mrs00394RDO>();
        List<V_HIS_EXP_MEST> listPrescription = null;
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();

        public Mrs00394Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00394Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00394Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Info("MRS00394: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisExpMestViewFilterQuery presFilter = new HisExpMestViewFilterQuery();
                presFilter.FINISH_DATE_FROM = castFilter.TIME_FROM;
                presFilter.FINISH_DATE_TO = castFilter.TIME_TO;
                presFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                presFilter.EXP_MEST_TYPE_IDs = new List<long>
                {
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT
                };
                listPrescription = new HisExpMestManager(paramGet).GetView(presFilter);
                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua tring lay du lieu MRS00394");
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
                if (IsNotNullOrEmpty(listPrescription))
                {
                    CommonParam paramGet = new CommonParam();
                    var listTreatmentId = listPrescription.Select(s => s.TDL_TREATMENT_ID.Value).Distinct().ToList();
                    int start = 0;
                    int count = listTreatmentId.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var listId = listTreatmentId.Skip(start).Take(limit).ToList();
                        HisPatientTypeAlterViewFilterQuery patyAlterFilter = new HisPatientTypeAlterViewFilterQuery();
                        patyAlterFilter.TREATMENT_IDs = listId;
                        patyAlterFilter.ORDER_DIRECTION = "DESC";
                        patyAlterFilter.ORDER_FIELD = "LOG_TIME";
                        var listPatientType = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patyAlterFilter);
                        if (paramGet.HasException)
                        {
                            throw new Exception("Co exception xay ra tai DAOGET trong qua tring lay du lieu MRS00394");
                        }

                        if (IsNotNullOrEmpty(listPatientType))
                        {
                            var Groups = listPatientType.GroupBy(g => g.TREATMENT_ID).ToList();
                            foreach (var group in Groups)
                            {
                                var currentPaty = group.ToList<V_HIS_PATIENT_TYPE_ALTER>().First();
                                if (currentPaty.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && currentPaty.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                                {
                                    dicPatientTypeAlter[currentPaty.TREATMENT_ID] = currentPaty;
                                }
                            }
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }

                    this.ProcessDataDetail();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        void ProcessDataDetail()
        {
            foreach (var pres in listPrescription)
            {
                if (!dicPatientTypeAlter.ContainsKey(pres.TDL_TREATMENT_ID.Value) || !pres.FINISH_TIME.HasValue)
                    continue;
                Mrs00394RDO rdo = null;
                long ExpData = Convert.ToInt64(pres.FINISH_TIME.Value.ToString().Substring(0, 8));
                if (dicRdo.ContainsKey(ExpData))
                {
                    rdo = dicRdo[ExpData];
                    rdo.AMOUNT++;
                }
                else
                {
                    rdo = new Mrs00394RDO();
                    rdo.AMOUNT++;
                    rdo.EXP_DATE = ExpData;
                    rdo.EXP_TIME = pres.FINISH_TIME.Value;
                    rdo.EXP_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(pres.FINISH_TIME.Value);
                    dicRdo[ExpData] = rdo;
                }
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                objectTag.AddObjectData(store, "Report", dicRdo.Select(s => s.Value).OrderBy(o => o.EXP_TIME).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
