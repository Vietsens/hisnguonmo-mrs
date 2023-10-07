using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOS.DAO.Sql
{
    public class ProcessDelegate
    {
        private static Dictionary<long, EditSql> editSql;
        public static Dictionary<long,EditSql> EditSql
        {
            get
            {
                return editSql;
            }
            set
            {
                editSql = value;
            }
        }
    }
}
