﻿using System.Globalization;
using System.Windows.Data;

namespace GraphLabs.Common.Helpers
{
    /// <summary> Binging, работающий в текущей культуре </summary>
    public class CultureAwareBinding : Binding
    {
        /// <summary> Ctor. </summary>
        public CultureAwareBinding()
        {
            ConverterCulture = CultureInfo.CurrentCulture;
        }
    }
}
