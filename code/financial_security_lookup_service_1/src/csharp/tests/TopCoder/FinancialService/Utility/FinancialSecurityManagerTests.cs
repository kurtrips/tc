// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Collections.Generic;
using NUnit.Framework;
using TopCoder.Util.ExceptionManager.SDE;
using TopCoder.Cache;
using TopCoder.FinancialService.Utility.SecurityDataCombiners;
using TopCoder.FinancialService.Utility.SecurityIdParsers;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// Unit tests for the FinancialSecurityManager class.
    /// </summary>
    /// <remarks>There are no tests for the Parse method as it delegates to DefaultSecurityIdParser's Parse method
    /// which is tested separately.</remarks>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class FinancialSecurityManagerTests
    {
        /// <summary>
        /// The FinancialSecurityManager instance to use for the tests.
        /// </summary>
        FinancialSecurityManager fsm;

        /// <summary>
        /// A dictionary holding the security id types to ISecurityLookupService
        /// objects mappings.
        /// </summary>
        IDictionary<string, ISecurityLookupService> securityLookupServices;

        /// <summary>
        /// The DefaultSecurityDataCombiner instance to use for creating the FinancialSecurityManager instance.
        /// </summary>
        DefaultSecurityDataCombiner combiner;

        /// <summary>
        /// The SimpleCache instance to use for creating the FinancialSecurityManager instance.
        /// </summary>
        SimpleCache cache;

        /// <summary>
        /// The DefaultSecurityIdParser instance to use for creating the FinancialSecurityManager instance.
        /// </summary>
        DefaultSecurityIdParser parser;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // create a dictionary holding the security id types to ISecurityLookupService
            // objects mappings
            securityLookupServices = new Dictionary<string, ISecurityLookupService>();
            securityLookupServices[SecurityIdType.CUSIP] = new CustomSecurityLookupService();
            securityLookupServices[SecurityIdType.ISIN] = new CustomSecurityLookupService();
            securityLookupServices[SecurityIdType.SEDOL] = new CustomSecurityLookupService();
            securityLookupServices[SecurityIdType.SymbolTicker] = new CustomSecurityLookupService();

            //Create DefaultSecurityDataCombiner
            combiner = new DefaultSecurityDataCombiner();

            //Create cache
            cache = new SimpleCache();

            //Create DefaultSecurityIdParser
            parser = new DefaultSecurityIdParser();

            fsm = new FinancialSecurityManager(parser, securityLookupServices, combiner,
                false, true, cache);

        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            fsm = null;
            securityLookupServices = null;
            combiner = null;
            parser = null;
            cache = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// Checks the values of the private fields. Also checks that shallow copy was correctly made.
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            Assert.AreEqual(UnitTestHelper.GetPrivateFieldValue(fsm, "securityIdParser"), parser,
                "Incorrect constructor implementation.");
            Assert.AreEqual(UnitTestHelper.GetPrivateFieldValue(fsm, "recursiveLookup"), false,
                "Incorrect constructor implementation.");
            Assert.AreEqual(UnitTestHelper.GetPrivateFieldValue(fsm, "referenceLookup"), true,
                "Incorrect constructor implementation.");
            Assert.AreEqual(UnitTestHelper.GetPrivateFieldValue(fsm, "securityDataCache"), cache,
                "Incorrect constructor implementation.");
            Assert.AreEqual(UnitTestHelper.GetPrivateFieldValue(fsm, "securityDataCombiner"), combiner,
                "Incorrect constructor implementation.");

            //The securityLookupServices instances must not be equal as shallow copy should be created.
            Assert.AreNotEqual(UnitTestHelper.GetPrivateFieldValue(fsm, "securityLookupServices"),
                securityLookupServices, "Incorrect constructor implementation.");
        }

        /// <summary>
        /// Tests the constructor overload.
        /// Checks the values of the private fields. Also checks that shallow copy was correctly made.
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            fsm = new FinancialSecurityManager(securityLookupServices, combiner, false, true, cache);

            Assert.AreEqual(UnitTestHelper.GetPrivateFieldValue(fsm, "recursiveLookup"), false,
                "Incorrect constructor implementation.");
            Assert.AreEqual(UnitTestHelper.GetPrivateFieldValue(fsm, "referenceLookup"), true,
                "Incorrect constructor implementation.");
            Assert.AreEqual(UnitTestHelper.GetPrivateFieldValue(fsm, "securityDataCache"), cache,
                "Incorrect constructor implementation.");
            Assert.AreEqual(UnitTestHelper.GetPrivateFieldValue(fsm, "securityDataCombiner"), combiner,
                "Incorrect constructor implementation.");

            //The securityLookupServices instances must not be equal as shallow copy should be created.
            Assert.AreNotEqual(UnitTestHelper.GetPrivateFieldValue(fsm, "securityLookupServices"),
                securityLookupServices, "Incorrect constructor implementation.");

            Assert.AreNotEqual(UnitTestHelper.GetPrivateFieldValue(fsm, "securityIdParser"), parser,
                "Incorrect constructor implementation.");
        }

        /// <summary>
        /// Tests the constructor for failure when securityIdParser is null.
        /// SelfDocumentingException is expected with innerException as ArgumentNullException
        /// </summary>
        [Test]
        public void TestConstructorFail1()
        {
            try
            {
                fsm = new FinancialSecurityManager(null, securityLookupServices, combiner, false, true, cache);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ArgumentNullException, "ArgumentNullException is expected.");
            }
        }

        /// <summary>
        /// Tests the constructor for failure when securityLookupServices is null.
        /// SelfDocumentingException is expected with innerException as ArgumentNullException
        /// </summary>
        [Test]
        public void TestConstructorFail2()
        {
            try
            {
                fsm = new FinancialSecurityManager(parser, null, combiner, false, true, cache);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ArgumentNullException, "ArgumentNullException is expected.");
            }
        }

        /// <summary>
        /// Tests the constructor for failure when combiner is null.
        /// SelfDocumentingException is expected with innerException as ArgumentNullException
        /// </summary>
        [Test]
        public void TestConstructorFail3()
        {
            try
            {
                fsm = new FinancialSecurityManager(parser, securityLookupServices, null, false, true, cache);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ArgumentNullException, "ArgumentNullException is expected.");
            }
        }

        /// <summary>
        /// Tests the constructor for failure when cache is null.
        /// SelfDocumentingException is expected with innerException as ArgumentNullException
        /// </summary>
        [Test]
        public void TestConstructorFail4()
        {
            try
            {
                fsm = new FinancialSecurityManager(parser, securityLookupServices, combiner, false, true, null);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ArgumentNullException, "ArgumentNullException is expected.");
            }
        }

        /// <summary>
        /// Tests the constructor for failure when securityLookupServices is empty.
        /// SelfDocumentingException is expected with innerException as ArgumentException
        /// </summary>
        [Test]
        public void TestConstructorFail5()
        {
            try
            {
                securityLookupServices.Clear();
                fsm = new FinancialSecurityManager(parser, securityLookupServices, combiner, false, true, cache);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ArgumentException, "ArgumentException is expected.");
            }
        }

        /// <summary>
        /// Tests the constructor for failure when securityLookupServices has an empty key.
        /// SelfDocumentingException is expected with innerException as ArgumentException
        /// </summary>
        [Test]
        public void TestConstructorFail6()
        {
            try
            {
                securityLookupServices.Add("         ", new CustomSecurityLookupService());
                fsm = new FinancialSecurityManager(parser, securityLookupServices, combiner, false, true, cache);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ArgumentException, "ArgumentException is expected.");
            }
        }

        /// <summary>
        /// Tests the constructor for failure when securityLookupServices has an null value.
        /// SelfDocumentingException is expected with innerException as ArgumentException
        /// </summary>
        [Test]
        public void TestConstructorFail7()
        {
            try
            {
                securityLookupServices.Add("abcd", null);
                fsm = new FinancialSecurityManager(parser, securityLookupServices, combiner, false, true, cache);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ArgumentException, "ArgumentException is expected.");
            }
        }

        /// <summary>
        /// Tests the Lookup method when referenceLookup is true and security is being looked up for first time.
        /// The securityId -> referenceIds relationship here is:
        /// A -> B C
        /// B -> D
        /// C ->
        /// D -> B
        /// SecurityData Lookup(string securityId)
        /// </summary>
        [Test]
        public void TestLookupReference1()
        {
            //Lookup using A
            SecurityData securityData = fsm.Lookup("A");

            //Returned data must have reference ids A,B,C
            Assert.IsTrue(UnitTestHelper.AreReferenceIdsEqual(
                securityData.ReferenceIds, new string[] { "A", "B", "C" }),
                "Wrong Lookup implementation.");

            ICache cache = (ICache) UnitTestHelper.GetPrivateFieldValue(fsm, "securityDataCache");
            UnitTestHelper.CheckSecurityDataRecord(cache, "A", new string[] { "A", "B", "C" }, true);
            UnitTestHelper.CheckSecurityDataRecord(cache, "B", new string[] { "A", "B", "C" }, false);
            UnitTestHelper.CheckSecurityDataRecord(cache, "C", new string[] { "A", "B", "C" }, false);
        }

        /// <summary>
        /// Tests the Lookup method when referenceLookup is true and security is already in cache
        /// but has not been looked up.
        /// The securityId -> referenceIds relationship here is:
        /// A -> B C
        /// B -> D
        /// C ->
        /// D -> B
        /// SecurityData Lookup(string securityId)
        /// </summary>
        [Test]
        public void TestLookupReference2()
        {
            //Lookup using A first
            TestLookupReference1();

            //Now lookup using B
            SecurityData securityData = fsm.Lookup("B");

            //Returned data must have reference ids A,B,C,D
            Assert.IsTrue(UnitTestHelper.AreReferenceIdsEqual(
                securityData.ReferenceIds, new string[] { "A", "B", "C", "D" }),
                "Wrong Lookup implementation.");

            ICache cache = (ICache)UnitTestHelper.GetPrivateFieldValue(fsm, "securityDataCache");
            UnitTestHelper.CheckSecurityDataRecord(cache, "A", new string[] { "A", "B", "C", "D" }, true);
            UnitTestHelper.CheckSecurityDataRecord(cache, "B", new string[] { "A", "B", "C", "D" }, true);
            UnitTestHelper.CheckSecurityDataRecord(cache, "C", new string[] { "A", "B", "C", "D" }, false);
            UnitTestHelper.CheckSecurityDataRecord(cache, "D", new string[] { "A", "B", "C", "D" }, false);
        }

        /// <summary>
        /// Tests the Lookup method when referenceLookup is true securities have cyclic reference.
        /// Thsi is just to demonstarte that no error is thrown.
        /// The securityId -> referenceIds relationship here is:
        /// X -> Y, X
        /// Y -> X
        /// SecurityData Lookup(string securityId)
        /// </summary>
        [Test]
        public void TestLookupReference3()
        {
            //Lookup using X
            SecurityData securityData = fsm.Lookup("X");

            //Returned data must have reference ids X,Y
            Assert.IsTrue(UnitTestHelper.AreReferenceIdsEqual(
                securityData.ReferenceIds, new string[] { "X", "Y" }),
                "Wrong Lookup implementation.");
        }

        /// <summary>
        /// Tests the Lookup method when referenceLookup is true. Here first it is looked up using C the by A.
        /// The securityId -> referenceIds relationship here is:
        /// A -> B C
        /// B -> D
        /// C ->
        /// D -> B
        /// SecurityData Lookup(string securityId)
        /// </summary>
        [Test]
        public void TestLookupReference4()
        {
            //Lookup using C first
            fsm.Lookup("C");

            //Now lookup using A
            SecurityData securityData = fsm.Lookup("A");

            //Returned data must have reference ids A,B,C
            Assert.IsTrue(UnitTestHelper.AreReferenceIdsEqual(
                securityData.ReferenceIds, new string[] { "A", "B", "C" }),
                "Wrong Lookup implementation.");

            ICache cache = (ICache)UnitTestHelper.GetPrivateFieldValue(fsm, "securityDataCache");
            UnitTestHelper.CheckSecurityDataRecord(cache, "A", new string[] { "A", "B", "C" }, true);
            UnitTestHelper.CheckSecurityDataRecord(cache, "B", new string[] { "A", "B", "C" }, false);
            UnitTestHelper.CheckSecurityDataRecord(cache, "C", new string[] { "A", "B", "C" }, true);
        }

        /// <summary>
        /// Tests the Lookup method when referenceLookup is true and security has already been looked up.
        /// The enrty in the cache should be returned.
        /// The securityId -> referenceIds relationship here is:
        /// A -> B C
        /// B -> D
        /// C ->
        /// D -> B
        /// SecurityData Lookup(string securityId)
        /// </summary>
        [Test]
        public void TestLookupReference5()
        {
            //Lookup using A first
            TestLookupReference1();

            //Now lookup using A
            SecurityData securityData = fsm.Lookup("A");

            //Both should be pointing to the same object
            ICache cache = (ICache)UnitTestHelper.GetPrivateFieldValue(fsm, "securityDataCache");
            Assert.AreEqual(UnitTestHelper.GetPrivateFieldValue(cache["A"], "securityData"), securityData,
                "Wrong Lookup implementation.");
        }

        /// <summary>
        /// Tests the Lookup method when recursiveLookup is true and security is being looked up for first time.
        /// The securityId -> referenceIds relationship here is:
        /// A -> B C
        /// B -> D
        /// C ->
        /// D -> B
        /// SecurityData Lookup(string securityId)
        /// </summary>
        [Test]
        public void TestLookupRecursive1()
        {
            fsm = new FinancialSecurityManager(securityLookupServices, combiner, true, false, cache);

            //Lookup using A
            SecurityData securityData = fsm.Lookup("A");

            //Returned data must have reference ids A,B,C,D
            Assert.IsTrue(UnitTestHelper.AreReferenceIdsEqual(
                securityData.ReferenceIds, new string[] { "A", "B", "C", "D" }),
                "Wrong Lookup implementation.");

            ICache cacheField = (ICache)UnitTestHelper.GetPrivateFieldValue(fsm, "securityDataCache");
            UnitTestHelper.CheckSecurityDataRecord(cacheField, "A", new string[] { "A", "B", "C", "D" }, true);
            UnitTestHelper.CheckSecurityDataRecord(cacheField, "B", new string[] { "A", "B", "C", "D" }, true);
            UnitTestHelper.CheckSecurityDataRecord(cacheField, "C", new string[] { "A", "B", "C", "D" }, true);
            UnitTestHelper.CheckSecurityDataRecord(cacheField, "D", new string[] { "A", "B", "C", "D" }, true);
        }

        /// <summary>
        /// Tests the Lookup method when recursiveLookup is true and security is being looked up for first time.
        /// This tests checks if this function runs well if cyclic references are present.
        /// The securityId -> referenceIds relationship here is:
        /// X -> Y X
        /// Y -> X
        /// SecurityData Lookup(string securityId)
        /// </summary>
        [Test]
        public void TestLookupRecursive2()
        {
            fsm = new FinancialSecurityManager(securityLookupServices, combiner, true, false, cache);

            //Lookup using X
            SecurityData securityData = fsm.Lookup("X");

            //Returned data must have reference ids X,Y
            Assert.IsTrue(UnitTestHelper.AreReferenceIdsEqual(
                securityData.ReferenceIds, new string[] { "X", "Y" }),
                "Wrong Lookup implementation.");
        }

        /// <summary>
        /// Tests the Lookup method when recursiveLookup is true and security has already been looked up.
        /// The returned securityData must be the same one as that in cache.
        /// The securityId -> referenceIds relationship here is:
        /// X -> Y X
        /// Y -> X
        /// SecurityData Lookup(string securityId)
        /// </summary>
        [Test]
        public void TestLookupRecursive3()
        {
            fsm = new FinancialSecurityManager(securityLookupServices, combiner, true, false, cache);

            //Lookup using X
            fsm.Lookup("X");

            //Lookup again using X
            SecurityData sdx = fsm.Lookup("X");

            //Lookup using Y
            SecurityData sdy = fsm.Lookup("Y");

            //The returned securityData for X must be the same one as that in cache.
            ICache cacheField = (ICache)UnitTestHelper.GetPrivateFieldValue(fsm, "securityDataCache");
            Assert.AreEqual(UnitTestHelper.GetPrivateFieldValue(cacheField["X"], "securityData"), sdx,
                "Wrong Lookup implementation.");
            Assert.AreEqual(UnitTestHelper.GetPrivateFieldValue(cacheField["Y"], "securityData"), sdy,
                "Wrong Lookup implementation.");
        }

        /// <summary>
        /// Tests the Lookup method when recursiveLookup and referenceLookup are false
        /// and security is being looked up for first time.
        /// The securityId -> referenceIds relationship here is:
        /// A -> B C
        /// B -> D
        /// C ->
        /// D -> B
        /// SecurityData Lookup(string securityId)
        /// </summary>
        [Test]
        public void TestLookupSimple1()
        {
            fsm = new FinancialSecurityManager(securityLookupServices, combiner, false, false, cache);

            //Lookup using A
            SecurityData securityData = fsm.Lookup("A");

            //Returned data must have reference ids A,B,C
            Assert.IsTrue(UnitTestHelper.AreReferenceIdsEqual(
                securityData.ReferenceIds, new string[] { "A", "B", "C" }),
                "Wrong Lookup implementation.");

            ICache cacheField = (ICache)UnitTestHelper.GetPrivateFieldValue(fsm, "securityDataCache");
            UnitTestHelper.CheckSecurityDataRecord(cacheField, "A", new string[] { "A", "B", "C" }, true);
            UnitTestHelper.CheckSecurityDataRecord(cacheField, "B", new string[] { "A", "B", "C" }, false);
            UnitTestHelper.CheckSecurityDataRecord(cacheField, "C", new string[] { "A", "B", "C" }, false);
        }

        /// <summary>
        /// Tests the Lookup method for failure when securityIdDetails is null
        /// SecurityData Lookup(SecurityIdDetails securityIdDetails)
        /// SelfDocumentingException is expected with innerException as ArgumentNullException
        /// </summary>
        [Test]
        public void TestLookupFail1()
        {
            try
            {
                fsm.Lookup((SecurityIdDetails)null);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ArgumentNullException, "ArgumentNullException is expected.");
            }
        }

        /// <summary>
        /// Tests the Lookup method for failure when securityId is null
        /// SecurityData Lookup(string securityId)
        /// SelfDocumentingException is expected with innerException as ArgumentNullException
        /// </summary>
        [Test]
        public void TestLookupFail2()
        {
            try
            {
                fsm.Lookup((string)null);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ArgumentNullException, "ArgumentNullException is expected.");
            }
        }

        /// <summary>
        /// Tests the Lookup method for failure when securityId is empty string.
        /// SecurityData Lookup(string securityId)
        /// SelfDocumentingException is expected with innerException as ArgumentException
        /// </summary>
        [Test]
        public void TestLookupFail3()
        {
            try
            {
                fsm.Lookup("       ");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ArgumentException, "ArgumentException is expected.");
            }
        }

        /// <summary>
        /// Tests the Lookup method for failure when securityId has no corresponding lookup service
        /// SecurityData Lookup(string securityId)
        /// NoSuchSecurityLookupServiceException is expected
        /// </summary>
        [Test, ExpectedException(typeof(NoSuchSecurityLookupServiceException))]
        public void TestLookupFail4()
        {
            securityLookupServices.Remove(SecurityIdType.SymbolTicker);
            fsm = new FinancialSecurityManager(securityLookupServices, combiner, true, false, cache);
            fsm.Lookup("A");
        }

        /// <summary>
        /// Tests the Lookup method for failure when securityId can not identified.
        /// SecurityData Lookup(string securityId)
        /// UnknownSecurityIdTypeException is expected
        /// </summary>
        [Test, ExpectedException(typeof(UnknownSecurityIdTypeException))]
        public void TestLookupFail5()
        {
            fsm.Lookup("ASJKFHDJKSJA*&^&*HDJ*");
        }

        /// <summary>
        /// Tests the Lookup method for failure when securityId can not identified.
        /// SecurityData Lookup(string securityId)
        /// UnknownSecurityIdTypeException is expected
        /// </summary>
        [Test, ExpectedException(typeof(UnknownSecurityIdTypeException))]
        public void TestLookupFail6()
        {
            fsm.Lookup("ASJKFHDJKSJA*&^&*HDJ*");
        }

        /// <summary>
        /// Tests the Lookup method for failure when securityId has invalid format (wrong check digit)
        /// SecurityData Lookup(string securityId)
        /// InvalidSecurityIdFormatException is expected
        /// </summary>
        [Test, ExpectedException(typeof(InvalidSecurityIdFormatException))]
        public void TestLookupFail7()
        {
            fsm.Lookup("J0176K102");
        }

        /// <summary>
        /// Tests the Lookup method for failure when securityId could not be looked up because the securityId
        /// could not be found as a valid company by lookup service.
        /// SecurityData Lookup(string securityId)
        /// SecurityLookupException is expected
        /// </summary>
        [Test, ExpectedException(typeof(SecurityLookupException))]
        public void TestLookupFail8()
        {
            fsm.Lookup("P");
        }

        /// <summary>
        /// Tests the Lookup method for failure when securityId could not be looked up because service is down.
        /// SecurityData Lookup(string securityId)
        /// InvalidSecurityIdFormatException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ServiceNotAvailableException))]
        public void TestLookupFail9()
        {
            fsm.Lookup("ZZZ");
        }

        /// <summary>
        /// Tests the Parse method for accuracy.
        /// </summary>
        [Test]
        public void TestParse()
        {
            SecurityIdDetails sid = fsm.Parse("A");
            Assert.AreEqual(sid.Id, "A");
            Assert.AreEqual(sid.Type, SecurityIdType.SymbolTicker);
        }

        /// <summary>
        /// Tests the ConvertFromCUSIPToISIN method.
        /// string ConvertFromCUSIPToISIN(string cusipSecurityId)
        /// </summary>
        [Test]
        public void TestConvertFromCUSIPToISINAccuracy1()
        {
            string isin = fsm.ConvertFromCUSIPToISIN("459056DG9");
            Assert.AreEqual(isin, "US459056DG91", "Wrong conversion of Cusip to ISIN");
        }

        /// <summary>
        /// Tests the ConvertFromCUSIPToISIN method when the passed in Cusip has lowercase characters.
        /// string ConvertFromCUSIPToISIN(string cusipSecurityId)
        /// </summary>
        [Test]
        public void TestConvertFromCUSIPToISINAccuracy2()
        {
            string isin = fsm.ConvertFromCUSIPToISIN("459056dg9");
            Assert.AreEqual(isin, "US459056DG91", "Wrong conversion of Cusip to ISIN");
        }

        /// <summary>
        /// Tests the ConvertFromCUSIPToISIN method for failure when securityId is null
        /// string ConvertFromCUSIPToISIN(string cusipSecurityId)
        /// SelfDocumentingException is expected with innerException as ArgumentNullException
        /// </summary>
        [Test]
        public void TestConvertFromCUSIPToISINFail1()
        {
            try
            {
                fsm.ConvertFromCUSIPToISIN(null);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ArgumentNullException, "ArgumentNullException is expected.");
            }
        }

        /// <summary>
        /// Tests the ConvertFromCUSIPToISIN method for failure when securityId is empty string.
        /// string ConvertFromCUSIPToISIN(string cusipSecurityId)
        /// SelfDocumentingException is expected with innerException as ArgumentException
        /// </summary>
        [Test]
        public void TestConvertFromCUSIPToISINFail2()
        {
            try
            {
                fsm.ConvertFromCUSIPToISIN("       ");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ArgumentException, "ArgumentException is expected.");
            }
        }

        /// <summary>
        /// Tests the ConvertFromCUSIPToISIN method for failure when securityId is not cusip but instead a sedol.
        /// string ConvertFromCUSIPToISIN(string cusipSecurityId)
        /// InvalidSecurityIdTypeException is expected
        /// </summary>
        [Test, ExpectedException(typeof(InvalidSecurityIdTypeException))]
        public void TestConvertFromCUSIPToISINFail3()
        {
            fsm.ConvertFromCUSIPToISIN("B1F3M59");
        }

        /// <summary>
        /// Tests the ConvertFromCUSIPToISIN method for failure when securityId is not identified at all.
        /// string ConvertFromCUSIPToISIN(string cusipSecurityId)
        /// UnknownSecurityIdTypeException is expected
        /// </summary>
        [Test, ExpectedException(typeof(UnknownSecurityIdTypeException))]
        public void TestConvertFromCUSIPToISINFail4()
        {
            fsm.ConvertFromCUSIPToISIN("COLD!!!!TURKEY");
        }

        /// <summary>
        /// Tests the ConvertFromCUSIPToISIN method for failure when securityId has wrong format for
        /// Cusip (wrong check digit)
        /// string ConvertFromCUSIPToISIN(string cusipSecurityId)
        /// InvalidSecurityIdFormatException is expected
        /// </summary>
        [Test, ExpectedException(typeof(InvalidSecurityIdFormatException))]
        public void TestConvertFromCUSIPToISINFail5()
        {
            fsm.ConvertFromCUSIPToISIN("459056DG0");
        }

        /// <summary>
        /// Tests the ConvertFromISINToCUSIP method.
        /// string ConvertFromISINToCUSIP(string isinSecurityId)
        /// </summary>
        [Test]
        public void TestConvertFromISINToCUSIPAccuracy1()
        {
            string cusip = fsm.ConvertFromISINToCUSIP("US459056DG91");
            Assert.AreEqual(cusip, "459056DG9", "Wrong ConvertFromISINToCUSIP implementation.");
        }

        /// <summary>
        /// Tests the ConvertFromISINToCUSIP method for failure when isinSecurityId is null
        /// string ConvertFromISINToCUSIP(string isinSecurityId)
        /// SelfDocumentingException is expected with innerException as ArgumentNullException
        /// </summary>
        [Test]
        public void TestConvertFromISINToCUSIPFail1()
        {
            try
            {
                fsm.ConvertFromISINToCUSIP(null);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ArgumentNullException, "ArgumentNullException is expected.");
            }
        }

        /// <summary>
        /// Tests the ConvertFromISINToCUSIP method for failure when isinSecurityId is empty.
        /// string ConvertFromISINToCUSIP(string isinSecurityId)
        /// SelfDocumentingException is expected with innerException as ArgumentException
        /// </summary>
        [Test]
        public void TestConvertFromISINToCUSIPFail2()
        {
            try
            {
                fsm.ConvertFromISINToCUSIP("    ");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ArgumentException, "ArgumentException is expected.");
            }
        }

        /// <summary>
        /// Tests the ConvertFromISINToCUSIP method for failure when isinSecurityId is not an isin but sedol.
        /// string ConvertFromISINToCUSIP(string isinSecurityId)
        /// InvalidSecurityIdTypeException is expected
        /// </summary>
        [Test, ExpectedException(typeof(InvalidSecurityIdTypeException))]
        public void TestConvertFromISINToCUSIPFail3()
        {
            fsm.ConvertFromISINToCUSIP("B1F3M59");
        }

        /// <summary>
        /// Tests the ConvertFromISINToCUSIP method for failure when isinSecurityId does not start with US
        /// string ConvertFromISINToCUSIP(string isinSecurityId)
        /// InvalidSecurityIdTypeException is expected
        /// </summary>
        [Test, ExpectedException(typeof(InvalidSecurityIdTypeException))]
        public void TestConvertFromISINToCUSIPFail4()
        {
            fsm.ConvertFromISINToCUSIP("BG459056DG98");
        }

        /// <summary>
        /// Tests the ConvertFromISINToCUSIP method for failure when underlying cusip id is has wrong check digit.
        /// string ConvertFromISINToCUSIP(string isinSecurityId)
        /// InvalidSecurityIdTypeException is expected
        /// </summary>
        [Test, ExpectedException(typeof(InvalidSecurityIdTypeException))]
        public void TestConvertFromISINToCUSIPFail5()
        {
            fsm.ConvertFromISINToCUSIP("US459056DG26");
        }

        /// <summary>
        /// Tests the ConvertFromISINToCUSIP method for failure when isin has invalid format (wrong check digit)
        /// string ConvertFromISINToCUSIP(string isinSecurityId)
        /// InvalidSecurityIdFormatException is expected
        /// </summary>
        [Test, ExpectedException(typeof(InvalidSecurityIdFormatException))]
        public void TestConvertFromISINToCUSIPFail6()
        {
            fsm.ConvertFromISINToCUSIP("US459056DG90");
        }
    }
}
