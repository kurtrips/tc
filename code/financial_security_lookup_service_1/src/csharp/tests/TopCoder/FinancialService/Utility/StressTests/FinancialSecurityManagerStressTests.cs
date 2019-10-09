/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */
using System;
using System.Collections.Generic;
using NUnit.Framework;
using TopCoder.Cache;
using TopCoder.FinancialService.Utility.SecurityDataCombiners;
using TopCoder.FinancialService.Utility.SecurityIdParsers;

namespace TopCoder.FinancialService.Utility.StressTests
{
    /// <summary>
    /// <para>
    /// This class contains stress test cases for <see cref="FinancialSecurityManager"/>.
    /// </para>
    /// </summary>
    ///
    /// <author>crazypigs</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (C) 2007 TopCoder Inc., All Rights Reserved.</copyright>
    [TestFixture]
    public class FinancialSecurityManagerStressTests
    {
        /// <summary>
        /// <p>
        /// The number to repeat a single task.
        /// </p>
        /// </summary>
        private const int ITERATION = 5000;

        /// <summary>
        /// <p>
        /// The tick count for the current watch.
        /// </p>
        /// </summary>
        private long start = 0;

        /// <summary>
        /// <para>
        /// Represents the <see cref="FinancialSecurityManager"/> instance to test.
        /// </para>
        /// </summary>
        private FinancialSecurityManager manager;

        /// <summary>
        /// <para>
        /// Sets up environment for test.
        /// </para>
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            IDictionary<string, ISecurityLookupService> securityLookupServices =
                new Dictionary<string, ISecurityLookupService>();
            securityLookupServices[SecurityIdType.CUSIP] = new CustomSecurityLookupService();
            securityLookupServices[SecurityIdType.ISIN] = new CustomSecurityLookupService();
            securityLookupServices[SecurityIdType.SEDOL] = new CustomSecurityLookupService();
            securityLookupServices[SecurityIdType.SymbolTicker] =
                new CustomSecurityLookupService();

            ICache cache = new SimpleCache();
            manager = new FinancialSecurityManager(new DefaultSecurityIdParser(),
                                                   securityLookupServices,
                                                   new DefaultSecurityDataCombiner(),
                                                   true, false, cache);
        }

        /// <summary>
        /// <para>
        /// Clears up environment after test.
        /// </para>
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            manager = null;
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
        /// Benchmarks the performance of <c>ConvertFromCUSIPToISIN(string cusipSecurityId)</c>.
        /// </para>
        /// </summary>
        [Test]
        public void BenchmarkConvertFromCUSIPToISIN()
        {
            Start();
            for (int i = 0; i < ITERATION; i++)
            {
                Assert.AreEqual("US0378331005", manager.ConvertFromCUSIPToISIN("037833100"));
            }
            Stop("Run FinancialSecurityManager.ConvertFromCUSIPToISIN() ");
        }

        /// <summary>
        /// <para>
        /// Benchmarks the performance of <c>ConvertFromISINToCUSIP(string isinSecurityId)</c>.
        /// </para>
        /// </summary>
        [Test]
        public void BenchmarkConvertFromISINToCUSIP()
        {
            Start();
            for (int i = 0; i < ITERATION; i++)
            {
                Assert.AreEqual("037833100", manager.ConvertFromISINToCUSIP("US0378331005"));
            }
            Stop("Run FinancialSecurityManager.ConvertFromISINToCUSIP() ");
        }

        /// <summary>
        /// <para>
        /// Benchmarks the performance of <c>Lookup(string securityId)</c>.
        /// </para>
        /// </summary>
        [Test]
        public void BenchmarkLookup()
        {
            Start();
            for (int i = 0; i < ITERATION; i++)
            {
                SecurityData data = manager.Lookup("A");
                // the result should be SecurityData("A", "company1", new string[] { "A","B","C","D" })
                Assert.AreEqual("A", data.Id, "the Lookup is wrong.");
                Assert.AreEqual("company1", data.CompanyName, "the Lookup is wrong.");
                Assert.AreEqual(4, data.ReferenceIds.Length, "the Lookup is wrong.");
            }
            Stop("Run FinancialSecurityManager.Lookup() ");
        }
    }
}