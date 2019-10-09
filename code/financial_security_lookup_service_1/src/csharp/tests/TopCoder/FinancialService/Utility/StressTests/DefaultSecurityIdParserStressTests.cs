/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */
using System;
using System.Collections.Generic;
using NUnit.Framework;
using TopCoder.FinancialService.Utility.SecurityIdParsers;

namespace TopCoder.FinancialService.Utility.StressTests
{
    /// <summary>
    /// <para>
    /// This class contains stress test cases for <see cref="DefaultSecurityIdParser"/>.
    /// The DefaultSecurityIdParser is complemently immutable and is thread safe.
    /// The stress test cases will mainly focus on the performance.
    /// </para>
    /// </summary>
    ///
    /// <author>crazypigs</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (C) 2007 TopCoder Inc., All Rights Reserved.</copyright>
    [TestFixture]
    public class DefaultSecurityIdParserStressTests
    {
        /// <summary>
        /// <p>
        /// The number to repeat a single task.
        /// </p>
        /// </summary>
        private const int ITERATION = 50;

        /// <summary>
        /// <p>
        /// The tick count for the current watch.
        /// </p>
        /// </summary>
        private long start = 0;

        /// <summary>
        /// <para>
        /// Represents the <see cref="DefaultSecurityIdParser"/> instance to test.
        /// </para>
        /// </summary>
        private DefaultSecurityIdParser parser;

        /// <summary>
        /// <para>
        /// Represents a set to security ids to test.
        /// The key is the security id; the value represents its type.
        /// </para>
        /// </summary>
        private IDictionary<string, string> securityIds;


        /// <summary>
        /// <para>
        /// Sets up environment for test.
        /// </para>
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            parser = new DefaultSecurityIdParser();
            securityIds = new Dictionary<string, string>();
            // add some CUSIP ids
            securityIds.Add("037833100", SecurityIdType.CUSIP);
            securityIds.Add("931142103", SecurityIdType.CUSIP);
            securityIds.Add("J0176K103", SecurityIdType.CUSIP);
            securityIds.Add("G4770P115", SecurityIdType.CUSIP);

            // add some ISIN
            securityIds.Add("US0378331005", SecurityIdType.ISIN);
            securityIds.Add("AU0000XVGZA3", SecurityIdType.ISIN);
            securityIds.Add("GB0002634946", SecurityIdType.ISIN);
            securityIds.Add("DE000A0BL849", SecurityIdType.ISIN);

            // add some SEDOL
            securityIds.Add("B1F3M59", SecurityIdType.SEDOL);
            securityIds.Add("B06G5D9", SecurityIdType.SEDOL);
            securityIds.Add("B1H54P7", SecurityIdType.SEDOL);

            // add some SymbolTicker
            securityIds.Add("AGC", SecurityIdType.SymbolTicker);
            securityIds.Add("A", SecurityIdType.SymbolTicker);
            securityIds.Add("BRK.B", SecurityIdType.SymbolTicker);
            securityIds.Add("MSFT ", SecurityIdType.SymbolTicker);
        }

        /// <summary>
        /// <para>
        /// Clears up environment after test.
        /// </para>
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            parser = null;
        }

        /// <summary>
        /// <para>
        /// Start the watch.
        /// </para>
        /// </summary>
        private void Start()
        {
            start = Environment.TickCount;
        }

        /// <summary>
        /// <para>
        /// Stop the watch and output information to console.
        /// </para>
        /// </summary>
        /// <param name="action">The action performed.</param>
        private void Stop(string action)
        {
            Console.WriteLine(string.Format("{0} {1} times took {2}ms.",
                                            action, ITERATION, Environment.TickCount - start));
        }


        /// <summary>
        /// <para>
        /// Benchmarks the performance of <c>Parse(string securityId)</c>.
        /// </para>
        /// </summary>
        [Test]
        public void BenchmarkParsePerformance()
        {
            IList<string> keys = new List<string>(securityIds.Keys);
            Start();
            for (int i = 0; i < ITERATION; i++)
            {
                string key = keys[i % keys.Count];
                SecurityIdDetails detail = parser.Parse(key);
                Assert.IsNotNull(detail, "the Parse() is wrong.");
                Assert.AreEqual(detail.Type, securityIds[key], "the Parse() is wrong.");
            }
            Stop("Run DefaultSecurityIdParser.Parse() ");
        }
    }
}