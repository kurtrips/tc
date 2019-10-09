/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */
using System;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.StressTests
{
    /// <summary>
    /// <para>
    /// This class contains stress test cases for <see cref="FinancialSecurityManagerBuilder"/>.
    /// </para>
    /// </summary>
    ///
    /// <author>crazypigs</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (C) 2007 TopCoder Inc., All Rights Reserved.</copyright>
    [TestFixture]
    public class FinancialSecurityManagerBuilderStressTests
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
        public void BenchmarkBuildFinancialSecurityManager()
        {
            Start();
            for (int i = 0; i < ITERATION; i++)
            {
                FinancialSecurityManager manager =
                    FinancialSecurityManagerBuilder.BuildFinancialSecurityManager
                        (StressTestHelper.BuildConfiguration());
                Assert.IsNotNull(manager, "the BuildFinancialSecurityManager() is wrong.");
            }
            Stop("Run FinancialSecurityManagerBuilder.BuildFinancialSecurityManager() ");
        }
    }
}