// SecurityIdType.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// <para> Represents the supported security id types in this component. Currently only CUSIP, ISIN, SEDOL,
    /// SymbolTicker security id types are supported,  and as they are all defined as constant string values,
    /// so more security id types can be added easily.</para>
    /// </summary>
    ///
    /// <threadsafety>This class is immutable and thread-safe.</threadsafety>
    ///
    /// <author>Standlove</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class SecurityIdType
    {
        /// <summary>
        /// <para>Represents the CUSIP security id type. It refers to 9-character alphanumeric security identifier type.
        /// The 9th digit is an automatically generated check digit using the "Modulus 10 Double Add Double" technique.
        /// </para>
        /// </summary>
        public const string CUSIP = "CUSIP";

        /// <summary>
        /// <para>Represents the ISIN security id type. It refers to 12-character alphanumeric security identifier type.
        /// Identifier of this type consists of three parts: a two letter country code, a nine character alpha-numeric
        /// national security identifier, and a single check digit. The country code is the ISO 3166-1 alpha-2 code for
        /// the country of issue. The algorithm to calculate the check digit is based on "Modulus 10 Double Add Double".
        /// </para>
        /// </summary>
        public const string ISIN = "ISIN";

        /// <summary>
        /// <para>Represents the SEDOL security id type. It refers to 7-character alphanumeric security identifier type.
        /// Identifier of this type consists of two parts: a six-place alphanumeric code and a trailing check digit. The
        /// check digit is modulo 10 of a weighted sum of the first six digits.</para>
        /// </summary>
        public const string SEDOL = "SEDOL";

        /// <summary>
        /// <para>
        /// Represents the Symbol Ticker security id type. Identifier of this type may consist of letters, numbers or
        /// a combination of both. In this component, we only care about NASDAQ, NYSE and AMEX security
        /// identifiers.</para>
        /// </summary>
        public const string SymbolTicker = "SymbolTicker";

        /// <summary><para>Empty constructor.</para></summary>
        private SecurityIdType()
        {
        }
    }
}
