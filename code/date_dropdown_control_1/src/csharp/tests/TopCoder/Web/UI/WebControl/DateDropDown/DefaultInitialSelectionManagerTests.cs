// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Web.UI.WebControls;
using NUnit.Framework;
using System.Globalization;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// Unit tests for the InitialSelectionManager class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class InitialSelectionManagerTests
    {
        /// <summary>
        /// The InitialSelectionManager instance to use for the tests
        /// </summary>
        private InitialSelectionManager ism;

        /// <summary>
        /// The default date format to use
        /// </summary>
        private const string DateFormat = "MM/dd/yyyy HH:mm";

        /// <summary>
        /// The ListItemCollection instance to use for the tests
        /// </summary>
        ListItemCollection coll;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            ism = new InitialSelectionManager();

            coll = new ListItemCollection();

            //Put some dates in the collection
            DateTime start = new DateTime(2007, 1, 1);
            DateTime end = new DateTime(2009, 1, 1);
            for (DateTime date = start; date <= end; date = date.AddHours(6))
            {
                coll.Add(date.ToString(DateFormat, CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            ism = null;
            coll = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// InitialSelectionManager()
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            Assert.IsNotNull(ism, "Contructor returns null.");
        }

        /// <summary>
        /// Tests the constructor.
        /// InitialSelectionManager(string)
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            ism = new InitialSelectionManager(InitialSelectionManager.SelectClosestNotAfterTimeStamp);
            Assert.IsNotNull(ism, "Contructor returns null.");
        }

        /// <summary>
        /// Tests the constructor.
        /// InitialSelectionManager(string, string)
        /// </summary>
        [Test]
        public void TestConstructor3()
        {
            ism = new InitialSelectionManager(InitialSelectionManager.SelectClosestNotAfterTimeStamp, DateFormat);
            Assert.IsNotNull(ism, "Contructor returns null.");
        }

        /// <summary>
        /// Tests the constructor.
        /// InitialSelectionManager(string, string, string)
        /// </summary>
        [Test]
        public void TestConstructor4()
        {
            ism = new InitialSelectionManager(InitialSelectionManager.SelectClosestNotAfterTimeStamp,
                "08/03/2007 21:45", DateFormat);
            Assert.IsNotNull(ism, "Contructor returns null.");
        }

        /// <summary>
        /// Tests the constructor when initialSelection is null
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFail1()
        {
            ism = new InitialSelectionManager(null, "08/03/2007 21:45", DateFormat);
        }

        /// <summary>
        /// Tests the constructor when timeStamp is null
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFail2()
        {
            ism = new InitialSelectionManager(InitialSelectionManager.SelectClosestNotAfterTimeStamp, null, DateFormat);
        }

        /// <summary>
        /// Tests the constructor when dateFormat is null
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFail3()
        {
            ism = new InitialSelectionManager(InitialSelectionManager.SelectClosestNotAfterTimeStamp,
                "08/03/2007 21:45", null);
        }

        /// <summary>
        /// Tests the DateFormat property.
        /// </summary>
        [Test]
        public void TestDateFormat()
        {
            ism.DateFormat = DateFormat;
            Assert.AreEqual(ism.DateFormat, DateFormat, "Wrong DateFormat implementation.");
        }

        /// <summary>
        /// Tests the InitialSelection property.
        /// </summary>
        [Test]
        public void TestInitialSelection()
        {
            ism.InitialSelection = "abc";
            Assert.AreEqual(ism.InitialSelection, "abc", "Wrong InitialSelection implementation.");
        }

        /// <summary>
        /// Tests the TimeStamp property.
        /// </summary>
        [Test]
        public void TestTimeStamp()
        {
            ism.TimeStamp = "def";
            Assert.AreEqual(ism.TimeStamp, "def", "Wrong TimeStamp implementation.");
        }

        /// <summary>
        /// Tests the DateFormat property when value is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestDateFormatFail1()
        {
            ism.DateFormat = null;
        }

        /// <summary>
        /// Tests the InitialSelection property when value is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestInitialSelectionFail1()
        {
            ism.InitialSelection = null;
        }

        /// <summary>
        /// Tests the TimeStamp property when value is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestTimeStampFail1()
        {
            ism.TimeStamp = null;
        }

        /// <summary>
        /// Tests the SelectItems function when selecting the first list item
        /// </summary>
        [Test]
        public void TestSelectItems1()
        {
            ism.InitialSelection = InitialSelectionManager.SelectFirstListItem;
            ism.DateFormat = DateFormat;

            int selected = ism.SelectItems(coll, false, DateFormat);

            Assert.AreEqual(selected, 1, "Wrong SelectItems implementation.");
            Assert.IsTrue(coll[0].Selected, "Wrong SelectItems implementation.");
        }

        /// <summary>
        /// Tests the SelectItems function when selecting the last list item
        /// </summary>
        [Test]
        public void TestSelectItems2()
        {
            ism.InitialSelection = InitialSelectionManager.SelectLastListItem;
            ism.DateFormat = DateFormat;

            int selected = ism.SelectItems(coll, false, DateFormat);

            Assert.AreEqual(selected, 1, "Wrong SelectItems implementation.");
            Assert.IsTrue(coll[coll.Count - 1].Selected, "Wrong SelectItems implementation.");
        }

        /// <summary>
        /// Tests the SelectItems function when selecting the first list item and collection is empty.
        /// No Exception must be thrown
        /// </summary>
        [Test]
        public void TestSelectItems3()
        {
            coll = new ListItemCollection();
            ism.InitialSelection = InitialSelectionManager.SelectFirstListItem;
            ism.DateFormat = DateFormat;

            int selected = ism.SelectItems(coll, false, DateFormat);

            Assert.AreEqual(selected, 0, "Wrong SelectItems implementation.");
        }

        /// <summary>
        /// Tests the SelectItems function when selecting the last list item and collection is empty.
        /// No Exception must be thrown
        /// </summary>
        [Test]
        public void TestSelectItems4()
        {
            coll = new ListItemCollection();
            ism.InitialSelection = InitialSelectionManager.SelectFirstListItem;
            ism.DateFormat = DateFormat;

            int selected = ism.SelectItems(coll, false, DateFormat);
            Assert.AreEqual(selected, 0, "Wrong SelectItems implementation.");
        }

        /// <summary>
        /// Tests the SelectItems function when selecting the closest to today.
        /// </summary>
        [Test]
        public void TestSelectItems5()
        {
            ism.InitialSelection = InitialSelectionManager.SelectClosestToday;
            ism.DateFormat = DateFormat;

            int selected = ism.SelectItems(coll, false, DateFormat);
            Assert.IsTrue(selected == 1, "Wrong SelectItems implementation.");

            //Make sure that the correct selection has been made
            Assert.IsTrue(ValidateClosestSelection(DateTime.Today, coll, 0), "Wrong SelectItems implementation.");
        }

        /// <summary>
        /// Tests the SelectItems function when selecting the closest not before today.
        /// </summary>
        [Test]
        public void TestSelectItems6()
        {
            ism.InitialSelection = InitialSelectionManager.SelectClosestNotBeforeToday;
            ism.DateFormat = DateFormat;

            int selected = ism.SelectItems(coll, false, DateFormat);
            Assert.IsTrue(selected == 1, "Wrong SelectItems implementation.");

            //Make sure that the correct selection has been made
            Assert.IsTrue(ValidateClosestSelection(DateTime.Today, coll, 1), "Wrong SelectItems implementation.");
        }

        /// <summary>
        /// Tests the SelectItems function when selecting the closest not after today.
        /// </summary>
        [Test]
        public void TestSelectItems7()
        {
            ism.InitialSelection = InitialSelectionManager.SelectClosestNotAfterToday;
            ism.DateFormat = DateFormat;

            int selected = ism.SelectItems(coll, false, DateFormat);
            Assert.IsTrue(selected == 1, "Wrong SelectItems implementation.");

            //Make sure that the correct selection has been made
            Assert.IsTrue(ValidateClosestSelection(DateTime.Today, coll, 2), "Wrong SelectItems implementation.");
        }

        /// <summary>
        /// Tests the SelectItems function when selecting the closest not before timestamp.
        /// </summary>
        [Test]
        public void TestSelectItems11()
        {
            ism.TimeStamp = "09/03/2007 21:45";
            ism.InitialSelection = InitialSelectionManager.SelectClosestNotBeforeTimeStamp;
            ism.DateFormat = DateFormat;

            int selected = ism.SelectItems(coll, false, DateFormat);
            Assert.IsTrue(selected == 1, "Wrong SelectItems implementation.");

            //Make sure that the correct selection has been made
            Assert.IsTrue(ValidateClosestSelection(DateTime.ParseExact(ism.TimeStamp, ism.DateFormat,
                CultureInfo.InvariantCulture), coll, 1), "Wrong SelectItems implementation.");
        }

        /// <summary>
        /// Tests the SelectItems function when selecting the closest not after timestamp.
        /// </summary>
        [Test]
        public void TestSelectItems8()
        {
            ism.TimeStamp = "09/03/2007 21:45";
            ism.InitialSelection = InitialSelectionManager.SelectClosestNotAfterTimeStamp;
            ism.DateFormat = DateFormat;

            int selected = ism.SelectItems(coll, false, DateFormat);
            Assert.IsTrue(selected == 1, "Wrong SelectItems implementation.");

            //Make sure that the correct selection has been made
            Assert.IsTrue(ValidateClosestSelection(DateTime.ParseExact(ism.TimeStamp, ism.DateFormat,
                CultureInfo.InvariantCulture), coll, 2), "Wrong SelectItems implementation.");
        }

        /// <summary>
        /// Tests the SelectItems function when selecting a simple date.
        /// </summary>
        [Test]
        public void TestSelectItems9()
        {
            ism.InitialSelection = "09/03/2007 18:00";
            ism.DateFormat = DateFormat;

            int selected = ism.SelectItems(coll, false, DateFormat);
            Assert.IsTrue(selected == 1, "Wrong SelectItems implementation.");

            //Make sure that the correct selection has been made
            Assert.IsTrue(ValidateClosestSelection(DateTime.ParseExact(ism.InitialSelection,
                ism.DateFormat, CultureInfo.InvariantCulture), coll, 0),
                "Wrong SelectItems implementation.");
        }

        /// <summary>
        /// Tests the SelectItems function when making a composite selection.
        /// </summary>
        [Test]
        public void TestSelectItems10()
        {
            ism.TimeStamp = "08/03/2007 12:05";
            ism.InitialSelection = "09/03/2007 18:00;Select:ClosestNotAfterTimeStamp";
            ism.DateFormat = DateFormat;

            int selected = ism.SelectItems(coll, true, DateFormat);
            Assert.IsTrue(selected == 2, "Wrong SelectItems implementation.");

            foreach (ListItem item in coll)
            {
                //The SelectClosestNotAfterTimeStamp selection
                if (item.Value == "08/03/2007 12:00")
                {
                    Assert.IsTrue(item.Selected, "Wrong SelectItems implementation.");
                }
                //The direct date selection
                else if (item.Value == "09/03/2007 18:00")
                {
                    Assert.IsTrue(item.Selected, "Wrong SelectItems implementation.");
                }
                //No other item must be selected.
                else
                {
                    Assert.IsFalse(item.Selected, "Wrong SelectItems implementation.");
                }
            }
        }

        /// <summary>
        /// Tests the SelectItems function when an item is already selected in the ListItemCollection.
        /// Function must return 0
        /// </summary>
        [Test]
        public void TestSelectItems12()
        {
            ism.TimeStamp = "08/03/2007 12:05";
            ism.InitialSelection = "09/03/2007 18:00;Select:ClosestNotAfterTimeStamp";
            ism.DateFormat = DateFormat;

            coll[0].Selected = true;
            int selected = ism.SelectItems(coll, true, DateFormat);
            Assert.IsTrue(selected == 0, "Wrong SelectItems implementation.");
        }

        /// <summary>
        /// Tests the SelectItems function when an item in ListItemCollection is null.
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSelectItemsFail1()
        {
            ism.InitialSelection = InitialSelectionManager.SelectFirstListItem;
            coll.Add((ListItem)null);
            ism.SelectItems(coll, true, DateFormat);
        }

        /// <summary>
        /// Tests the SelectItems function when InitialSelection is empty but ListItemCollection contains elements.
        /// InitialSelectionInvalidDataException is expected
        /// </summary>
        [Test, ExpectedException(typeof(InitialSelectionInvalidDataException))]
        public void TestSelectItemsFail2()
        {
            ism.SelectItems(coll, true, DateFormat);
        }

        /// <summary>
        /// Tests the SelectItems function when InitialSelection is ClosestXXXTimeStamp but TimeStamp is empty
        /// InitialSelectionInvalidDataException is expected
        /// </summary>
        [Test, ExpectedException(typeof(InitialSelectionInvalidDataException))]
        public void TestSelectItemsFail3()
        {
            ism.InitialSelection = InitialSelectionManager.SelectClosestNotBeforeTimeStamp;
            ism.SelectItems(coll, true, DateFormat);
        }

        /// <summary>
        /// Tests the SelectItems function when date in ListItemCollection is not in correct format
        /// InitialSelectionInvalidDataException is expected
        /// </summary>
        [Test, ExpectedException(typeof(InitialSelectionInvalidDataException))]
        public void TestSelectItemsFail4()
        {
            coll.Add("2007-12-12");
            ism.InitialSelection = InitialSelectionManager.SelectClosestToday;
            ism.SelectItems(coll, true, DateFormat);
        }


        /// <summary>
        /// Validates that a closest single selection has been made in the ListItemCollection.
        /// </summary>
        /// <param name="date">The date to which the selection should be closest</param>
        /// <param name="coll">The ListItemCollection instance</param>
        /// <param name="beforeAfter">
        /// <para>1 means to skip entries that are before date</para>
        /// <para>2 means to skip entries that are after date</para>
        /// <para>0 doen not skip any entries</para>
        /// </param>
        /// <returns>
        /// Truw if the selected item is indeed closest to the date.
        /// False otherwise.
        /// </returns>
        private bool ValidateClosestSelection(DateTime date, ListItemCollection coll, int beforeAfter)
        {
            long min = long.MaxValue;
            foreach (ListItem item in coll)
            {
                //Get the deviation of the selected date
                if (item.Selected)
                {
                    DateTime itemDate = DateTime.ParseExact(item.Value, DateFormat, CultureInfo.InvariantCulture);
                    min = Math.Abs(date.Ticks - itemDate.Ticks);
                    break;
                }
            }

            //Check if any other date is closest
            foreach (ListItem item in coll)
            {
                DateTime itemDate = DateTime.ParseExact(item.Value, DateFormat, CultureInfo.InvariantCulture);

                if (beforeAfter == 1 && itemDate < date)
                {
                    continue;
                }
                else if (beforeAfter == 2 && itemDate > date)
                {
                    continue;
                }

                if (Math.Abs(date.Ticks - itemDate.Ticks) < min)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
