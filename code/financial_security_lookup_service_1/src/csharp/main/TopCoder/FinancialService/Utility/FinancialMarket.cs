// FinancialMarket.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// <para>Represents the supported Financial Markets in this component.
    /// Currently only NYSE, NASDAQ, AMEX financial markets are supported, and as they are all defined
    /// as constant string values, so more financial markets can be added easily.
    /// </para>
    /// </summary>
    ///
    /// <threadsafety>This class is immutable and thread-safe.</threadsafety>
    ///
    /// <author>Standlove</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class FinancialMarket
    {
        /// <summary><para>Represents the NYSE financial market name. </para></summary>
        public const string NYSE = "NYSE";

        /// <summary><para>Represents the NASDAQ financial market name. </para></summary>
        public const string NASDAQ = "NASDAQ";

        /// <summary><para>Represents the AMEX financial market name. </para></summary>
        public const string AMEX = "AMEX";

        /// <summary><para>Empty constructor.</para></summary>
        private FinancialMarket()
        {
        }
    }
}
