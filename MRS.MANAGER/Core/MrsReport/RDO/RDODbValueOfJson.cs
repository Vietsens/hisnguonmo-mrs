using FlexCel.Report;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Core.MrsReport.RDO
{
    public class RDODbValueOfJson : TFlexCelUserFunction
    {

        object result = null;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");


            try
            {
                string KeyGet = Convert.ToString(parameters[0]);
                if (KeyGet.StartsWith("[") && KeyGet.EndsWith("]"))
                {
                    Newtonsoft.Json.Linq.JArray data = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JArray>(KeyGet);
                    if (data.Count>0)
                    {
                        int row = data.Count-1;// data.Rows.Count;
                        if (parameters.Length > 2)
                        {
                            string index = Convert.ToString(parameters[1]);
                            int id = Int32.Parse(index);
                            if (row > id)
                            {
                                row = id;
                            }
                            result = data[row][Convert.ToString(parameters[2])];
                        }
                        
                    }

                }
                else if (KeyGet.StartsWith("{") && KeyGet.EndsWith("}"))
                    {
                        Dictionary<string, object> data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(KeyGet);
                        if (data.Count > 0)
                        {
                            int row = data.Count - 1;// data.Rows.Count;
                            if (parameters.Length > 2)
                            {
                                string index = Convert.ToString(parameters[1]);
                                int id = Int32.Parse(index);
                                if (row > id)
                                {
                                    row = id;
                                }
                                if (data.ContainsKey(Convert.ToString(parameters[2])))
                                {
                                    result = data[Convert.ToString(parameters[2])];
                                }
                                else
                                {
                                    result =null;
                                }
                                
                            }

                        }
                    }


            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }

            return result;
        }
    }
}
