using FlexCel.Report;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Core.MrsReport.RDO
{
    public class RDOCustomerFuncGroupSum
    {
        public static List<R> GroupSum<T, R>(Dictionary<IGrouping<T, R>, R> p, List<string> keySums)
        {
            List<R> result = new List<R>();
            string errorField = "";
            try
            {
                var group = p.Keys;
                Decimal sum = 0;

                List<R> listSub = new List<R>();
                PropertyInfo[] pi = typeof(R).GetProperties();
                foreach (var item in group)
                {
                    listSub = item.ToList<R>();
                    R Rdo = p[item];
                    bool hide = true;
                    foreach (var field in pi)
                    {

                        errorField = field.Name;

                        if (keySums != null && keySums.Exists(o => field.Name.Contains(o)))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(Rdo, sum);
                        }
                        else if (FirstValue(listSub, field) != null)
                        {

                            field.SetValue(Rdo, field.GetValue(FirstValue(listSub, field)));
                        }

                    }
                    if (!hide) result.Add(Rdo);

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                LogSystem.Info(errorField);
                return new List<R>();
            }
            return result;
        }

        private static R FirstValue<R>(List<R> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault();
        }

    }
}
