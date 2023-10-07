using Inventec.Common.Logging;
using System;
using System.Collections.Generic;

namespace MRS.Processor.Mrs00105
{
    public class OrderProcessor
    {
        protected const string defaultField = "MODIFY_TIME";
        protected const string defaultDirection = "DESC";
        protected static List<String> listDirection = new List<string>(new string[] { "DESC", "ASC" });

        public static string GetOrderDirection(string direction)
        {
            string result = defaultDirection;
            if (!string.IsNullOrWhiteSpace(direction))
            {
                try
                {
                    if (listDirection.Contains(direction))
                    {
                        result = direction;
                    }
                }
                catch (Exception ex)
                {
                    LogSystem.Error(ex);
                }
            }
            return result;
        }

        public static string GetOrderField<T>(string orderField)
        {
            string result = typeof(T).GetProperties()[0].Name;
            if (!string.IsNullOrWhiteSpace(orderField))
            {
                try
                {
                    if (typeof(T).GetProperty(orderField) != null)
                    {
                        result = orderField;
                    }
                    else if (typeof(T).GetProperty(defaultField) != null)
                    {
                        result = defaultField;
                    }
                }
                catch (Exception ex)
                {
                    LogSystem.Error(ex);
                }
            }
            return result;
        }
    }
}
