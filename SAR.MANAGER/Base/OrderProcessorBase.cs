using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SAR.MANAGER.Base
{
    public class OrderProcessorBase
    {
        protected string defaultField = "MODIFY_TIME";
        protected string defaultDirection = "DESC";
        protected static List<String> listDirection = new List<string>(new string[] { "DESC", "ASC" });

        public string GetOrderDirection(string direction)
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

        public string GetOrderField<T>(string orderField)
        {
            string result = defaultField;
            if (!string.IsNullOrWhiteSpace(orderField))
            {
                try
                {
                    PropertyInfo property = typeof(T).GetProperty(orderField);
                    if (property != null)
                    {
                        result = orderField;
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
