/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */
using System;
using NUnit.Framework;
using TopCoder.FinancialService.Utility.SecurityDataCombiners;

namespace TopCoder.FinancialService.Utility.StressTests
{
    /// <summary>
    /// <para>
    /// This class contains stress test cases for <see cref="DefaultSecurityDataCombiner"/>.
    /// The DefaultSecurityIdParser is complemently immutable and is thread safe.
    /// The stress test cases will mainly focus on the performance.
    /// </para>
    /// </summary>
    ///
    /// <author>crazypigs</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (C) 2007 TopCoder Inc., All Rights Reserved.</copyright>
    [TestFixture]
    public class DefaultSecurityDataCombinerStressTests
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
        /// Represents the <see cref="DefaultSecurityDataCombiner"/> instance to test.
        /// </para>
        /// </summary>
        private DefaultSecurityDataCombiner combiner;

        /// <summary>
        /// <para>
        /// Represents the first security data to combine.
        /// </para>
        /// </summary>
        private SecurityData first;

        /// <summary>
        /// <para>
        /// Represents the second security data to combine.
        /// </para>
        /// </summary>
        private SecurityData second;


        /// <summary>
        /// <para>
        /// Sets up environment for test.
        /// </para>
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            combiner = new DefaultSecurityDataCombiner();
            first = new SecurityData("037833100", "c1", new string[] {"931142103", "J0176K103"});
            second = new SecurityData("931142103", "c1", new string[] {"J0176K103", "G4770P115"});
        }

        /// <summary>
        /// <para>
        /// Clears up environment after test.
        /// </para>
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            combiner = null;
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
        /// Benchmarks the performance of <c>Combine(SecurityData firstSecurityData,
        /// SecurityData secondSecurityData)</c>.
        /// </para>
        /// </summary>
        [Test]
        public void BenchmarkCombinePerformance()
        {
            SecurityData result;
            Start();
            for (int i = 0; i < ITERATION; i++)
            {
                result = combiner.Combine(first, second);
                Assert.IsNotNull(result, "the DefaultSecurityDataCombiner.Combine is wrong.");
                Assert.AreEqual(result.Id, first.Id, "the DefaultSecurityDataCombiner.Combine is wrong.");
                Assert.AreEqual(result.CompanyName, first.CompanyName,
                                "the DefaultSecurityDataCombiner.Combine is wrong.");
                Assert.AreEqual(4, result.ReferenceIds.Length, "the DefaultSecurityDataCombiner.Combine is wrong.");
            }
            Stop("Run DefaultSecurityDataCombiner.Combine() ");
        }
    }
}