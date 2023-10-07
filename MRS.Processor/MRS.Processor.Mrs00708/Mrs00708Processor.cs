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
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.MANAGER.Manager;
using ACS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00708
{
    public class Mrs00708Processor : AbstractProcessor
    {
        private Mrs00708Filter filter;
        List<Mrs00708RDO> listTreatment = new List<Mrs00708RDO>();
        List<Mrs00708RDO> listRdo = new List<Mrs00708RDO>();
        List<long> FormIds = new List<long>();
        List<ACS_USER> Users = new List<ACS_USER>();

        //List<string> listKey = new List<string>();
       
           
        CommonParam paramGet = new CommonParam(); 
        public Mrs00708Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00708Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            filter = (Mrs00708Filter)reportFilter; 
            try
            {
                AcsUserFilterQuery Userfilter = new AcsUserFilterQuery();
                Users = new AcsUserManager(paramGet).Get<List<ACS_USER>>(Userfilter);
                listTreatment = new MRS.Processor.Mrs00708.ManagerSql().GetRdo(filter) ?? new List<Mrs00708RDO>();
                foreach (var item in listTreatment)
                {
                    if (!string.IsNullOrWhiteSpace(item.JSON_FORM_ID))
                    {
                        string[] formIdStr = item.JSON_FORM_ID.Split(',');
                        foreach (var formId in formIdStr)
                        {
                            long id = 0;
                            if (Int64.TryParse(formId, out id))
                            {
                                FormIds.Add(id);
                                Mrs00708RDO rdo = new Mrs00708RDO();
                                rdo.TREATMENT_CODE = item.TREATMENT_CODE;
                                rdo.TDL_PATIENT_CODE = item.TDL_PATIENT_CODE;
                                rdo.TDL_PATIENT_ADDRESS = item.TDL_PATIENT_ADDRESS;
                                rdo.TDL_PATIENT_NAME = item.TDL_PATIENT_NAME;
                                rdo.TDL_PATIENT_DOB = item.TDL_PATIENT_DOB;
                                rdo.FORM_ID = id;
                                rdo.DIC_FORM_DATA = new Dictionary<string, string>();
                                listRdo.Add(rdo);
                            }

                        }
                    }
                }
                FormIds = FormIds.Distinct().ToList();

                
                
              
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

                if (IsNotNullOrEmpty(listRdo))
                {
                    if (FormIds != null && FormIds.Count > 0)
                    {
                        var skip = 0;
                        while (FormIds.Count - skip > 0)
                        {
                            var limit = FormIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var listFormDataSub = new MRS.Processor.Mrs00708.ManagerSql().GetFormData(filter, limit) ?? new List<FormData>();
                            var listFormSub = new MRS.Processor.Mrs00708.ManagerSql().GetForm(filter, limit) ?? new List<Form>();
                            var listRdoLocal = listRdo.Where(o => FormIds.Contains(o.FORM_ID)).ToList();
                            foreach (var item in listRdoLocal)
                            {
                                var formDataSub = listFormDataSub.Where(o => o.FORM_ID == item.FORM_ID).ToList();
                                var form= listFormSub.FirstOrDefault(o => o.ID == item.FORM_ID);
                                if (form != null)
                                {
                                    item.FORM_CREATOR = form.CREATOR;
                                    var user = Users.FirstOrDefault(o => o.LOGINNAME == form.CREATOR);
                                    if (user != null)
                                    {
                                        item.FORM_CREATE_USERNAME = user.USERNAME;
                                    }
                                    item.FORM_CREATE_TIME = form.CREATE_TIME;
                                }

                                if (formDataSub.Count == 0)
                                {
                                    item.FORM_ID = 0;
                                }
                                else
                                {
                                    foreach (var data in formDataSub)
                                    {
                                        if (!item.DIC_FORM_DATA.ContainsKey(data.KEY))
                                        {
                                            item.DIC_FORM_DATA.Add(data.KEY, data.VALUE);
                                            //if (!this.listKey.Contains(data.KEY))
                                            //{
                                            //    this.listKey.Add(data.KEY);
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
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
            objectTag.AddObjectData(store, "Report", listRdo.Where(o => o.FORM_ID > 0).ToList());
        }

       
    }
}
