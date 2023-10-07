using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IcdVn
{
    public class IcdVnIcd
    {
        public string ICDVN_CODE { get; set; }
        public string ICD_CODE { get; set; }
        public IcdVnIcd()
        { }
        public IcdVnIcd(string ICD_CODE)
        {

            if (ICD_CODE != null)
            {
                this.ICDVN_CODE = (IcdVnStore.GetIcdVn().FirstOrDefault(o => o.ICD_CODE == ICD_CODE) ?? new IcdVnIcd()).ICDVN_CODE;
                if (string.IsNullOrEmpty(this.ICDVN_CODE))
                {
                    this.ICDVN_CODE = (IcdVnStore.GetIcdVn().FirstOrDefault(o =>ICD_CODE.Contains(o.ICD_CODE)) ?? new IcdVnIcd()).ICDVN_CODE;
                }
            }
        }

    }
    public class IcdVnStore
    {
        private static List<IcdVnIcd> IcdVn;
        internal static List<IcdVnIcd> GetIcdVn()
        {
            if (IcdVn == null)
            {
                try
                {
                    string AppPath = HttpRuntime.AppDomainAppPath;
                    IcdVn = File.ReadLines(string.Format(@"{0}\\bin\\IcdVn.txt", AppPath)).Select(o => new IcdVnIcd() { ICD_CODE = o.Split('\t').First(), ICDVN_CODE = o.Split('\t').Last() }).ToList();
                }
                catch (Exception ex)
                {
                    IcdVn = new List<IcdVnIcd>();
                }
            }
            return IcdVn;

        }


    }
}
