﻿using System;
using System.Reflection;

namespace SDA.EFMODEL.Decorator
{
    public class CreatorDecorator
    {
        public static void Set<RAW>(RAW raw, string creator)
        {
            try
            {
                PropertyInfo piCreator = typeof(RAW).GetProperty("CREATOR");
                PropertyInfo piModifier = typeof(RAW).GetProperty("MODIFIER");

                piCreator.SetValue(raw, creator);
                piModifier.SetValue(raw, creator);
            }
            catch (Exception)
            {
                
            }
        }
    }
}
