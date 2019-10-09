/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using System.IO;
using System.Data;
using NUnit.Framework;
using System.Collections.Generic;
using TopCoder.Data.ConnectionFactory;
using TopCoder.Util.ConfigurationManager;
using HermesNS.TC.Services.GenericNotes;
using HermesNS.TC.Services.ScheduleItem.Entities;

namespace HermesNS.TC.Services.ScheduleItem.Persistence.AccuracyTests
{
    /// <summary>
    /// Some help methods used in the accuracy tests.
    /// </summary>
    ///
    /// <author>zaixiang</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c)2007, TopCoder, Inc. All rights reserved.</copyright>
    internal static class AccuracyTestsHelper
    {
        /// <summary>
        /// <para>
        /// Load configuration files.
        /// </para>
        /// </summary>
        public static void LoadConfiguration()
        {
            ConfigManager cm = ConfigManager.GetInstance();
            cm.Clear(false);
            cm.LoadFile("../../test_files/accuracy/HermesScheduleItemPersistenceProvider.xml");
            cm.LoadFile("../../test_files/accuracy/ConnectionFactory.xml");
        }

        /// <summary>
        /// <para>
        /// Clear configuration files.
        /// </para>
        /// </summary>
        public static void ClearConfiguration()
        {
            ConfigManager.GetInstance().Clear(false);
        }

        /// <summary>
        /// <para>
        /// Clear all data.
        /// </para>
        /// </summary>
        public static void ClearTables()
        {
            using (IDbConnection connection = ConnectionManager.Instance.CreateDefaultPredefinedDbConnection())
            {
                connection.Open();
                ExecuteNonQuery(connection, "../../test_files/accuracy/ClearTestData.sql");
            }
        }

        /// <summary>
        /// <para>
        /// Execute the query.
        /// </para>
        /// </summary>
        /// <param name="connection">
        /// Db connection.
        /// </param>
        /// <param name="fileName">
        /// The name of the file conatining the statements.
        /// </param>
        private static void ExecuteNonQuery(IDbConnection connection, string fileName)
        {
            string fileContent = File.ReadAllText(fileName);
            string[] statements = fileContent.Split(';');

            using (IDbCommand command = connection.CreateCommand())
            {
                foreach (string commandString in statements)
                {
                    command.CommandText = commandString;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Validates HermesActivity.
        /// </summary>
        /// <param name="first">The first entity.</param>
        /// <param name="second">>The second entity.</param>
        internal static void VerifyHermesActivity(HermesActivity first, HermesActivity second)
        {
            Assert.AreEqual(first.Abbreviation, second.Abbreviation, "Abbreviation is wrong.");
            Assert.AreEqual(first.DefaultDuration, second.DefaultDuration, "DefaultDuration is wrong.");
            Assert.AreEqual(first.DefaultExpireDays, second.DefaultExpireDays, "DefaultExpireDays is wrong.");
            Assert.AreEqual(first.DefaultStartTime, second.DefaultStartTime, "DefaultStartTime is wrong.");
            Assert.AreEqual(first.Enabled, second.Enabled, "Enabled is wrong.");
            Assert.AreEqual(first.ExclusiveFlag, second.ExclusiveFlag, "ExclusiveFlag is wrong.");
            Assert.AreEqual(first.Id, second.Id, "Id is wrong.");
            Assert.AreEqual(first.LastModifiedBy, second.LastModifiedBy, "LastModifiedBy is wrong.");
            Assert.AreEqual(first.LastModifiedDate.Date, second.LastModifiedDate.Date, "LastModifiedDate is wrong.");
            Assert.AreEqual(first.Name, second.Name, "Name is wrong.");
            Assert.AreEqual(first.WorkDayAmount, second.WorkDayAmount, "WorkDayAmount is wrong.");
            VerifyHermesActivityType(first.ActivityType, second.ActivityType);
        }

        /// <summary>
        /// Validates HermesActivityType.
        /// </summary>
        /// <param name="first">The first entity.</param>
        /// <param name="second">>The second entity.</param>
        internal static void VerifyHermesActivityType(HermesActivityType first, HermesActivityType second)
        {
            Assert.AreEqual(first.Abbreviation, second.Abbreviation, "Abbreviation is wrong.");
            Assert.AreEqual(first.Id, second.Id, "Id is wrong.");
            Assert.AreEqual(first.LastModifiedBy, second.LastModifiedBy, "LastModifiedBy is wrong.");
            Assert.AreEqual(first.LastModifiedDate.Date, second.LastModifiedDate.Date, "LastModifiedDate is wrong.");
            Assert.AreEqual(first.Name, second.Name, "Name is wrong.");
            VerifyHermesActivityGroup(first.ActivityGroup, second.ActivityGroup);
        }

        /// <summary>
        /// Validates HermesActivityGroup.
        /// </summary>
        /// <param name="first">The first entity.</param>
        /// <param name="second">>The second entity.</param>
        internal static void VerifyHermesActivityGroup(HermesActivityGroup first, HermesActivityGroup second)
        {
            Assert.AreEqual(first.Abbreviation, second.Abbreviation, "Abbreviation is wrong.");
            Assert.AreEqual(first.Id, second.Id, "Id is wrong.");
            Assert.AreEqual(first.LastModifiedBy, second.LastModifiedBy, "LastModifiedBy is wrong.");
            Assert.AreEqual(first.LastModifiedDate.Date, second.LastModifiedDate.Date, "LastModifiedDate is wrong.");
            Assert.AreEqual(first.Name, second.Name, "Name is wrong.");
        }

        /// <summary>
        /// Validates HermesScheduleItem.
        /// </summary>
        /// <param name="first">The first entity.</param>
        /// <param name="second">>The second entity.</param>
        internal static void VerifyHermesScheduleItem(HermesScheduleItem first, HermesScheduleItem second)
        {
            Assert.AreEqual(first.Id, second.Id, "Id is wrong.");
            Assert.AreEqual(first.LastModifiedBy, second.LastModifiedBy, "LastModifiedBy is wrong.");
            Assert.AreEqual(first.LastModifiedDate.Date, second.LastModifiedDate.Date, "LastModifiedDate is wrong.");
            Assert.AreEqual(first.Duration, second.Duration, "Duration is wrong.");
            Assert.AreEqual(first.ExceptionFlag, second.ExceptionFlag, "ExceptionFlag is wrong.");
            Assert.AreEqual(first.ExpirationDate.Date, second.ExpirationDate.Date, "LastModifiedDate is wrong.");
            Assert.AreEqual(first.Version, second.Version, "Version is wrong.");
            Assert.AreEqual(first.WorkDate.Date, second.WorkDate.Date, "WorkDate is wrong.");
            Assert.AreEqual(first.WorkDayAmount, second.WorkDayAmount, "WorkDayAmount is wrong.");
            Assert.AreEqual(first.Note.Id, second.Note.Id, "Note.Id is wrong.");

            VerifyHermesActivity(first.Activity, second.Activity);
            VerifyHermesScheduleItemRequestStatus(first.ScheduleItemRequestStatus,
                second.ScheduleItemRequestStatus);
            VerifyHermesScheduleItemStatus(first.ScheduleItemStatus, second.ScheduleItemStatus);
        }

        /// <summary>
        /// Validates HermesScheduleItemRequestStatus.
        /// </summary>
        /// <param name="first">The first entity.</param>
        /// <param name="second">>The second entity.</param>
        internal static void VerifyHermesScheduleItemRequestStatus(HermesScheduleItemRequestStatus first,
            HermesScheduleItemRequestStatus second)
        {
            Assert.AreEqual(first.Abbreviation, second.Abbreviation, "Abbreviation is wrong.");
            Assert.AreEqual(first.Id, second.Id, "Id is wrong.");
            Assert.AreEqual(first.LastModifiedBy, second.LastModifiedBy, "LastModifiedBy is wrong.");
            Assert.AreEqual(first.LastModifiedDate.Date, second.LastModifiedDate.Date, "LastModifiedDate is wrong.");
            Assert.AreEqual(first.Description, second.Description, "Description is wrong.");
        }

        /// <summary>
        /// Validates HermesScheduleItemStatus.
        /// </summary>
        /// <param name="first">The first entity.</param>
        /// <param name="second">>The second entity.</param>
        internal static void VerifyHermesScheduleItemStatus(HermesScheduleItemStatus first,
            HermesScheduleItemStatus second)
        {
            Assert.AreEqual(first.Abbreviation, second.Abbreviation, "Abbreviation is wrong.");
            Assert.AreEqual(first.Id, second.Id, "Id is wrong.");
            Assert.AreEqual(first.LastModifiedBy, second.LastModifiedBy, "LastModifiedBy is wrong.");
            Assert.AreEqual(first.LastModifiedDate.Date, second.LastModifiedDate.Date, "LastModifiedDate is wrong.");
            Assert.AreEqual(first.Description, second.Description, "Description is wrong.");
        }

        /// <summary>
        /// Creates HermesActivity.
        /// </summary>
        internal static HermesActivity CreateHermesActivity()
        {
            HermesActivity entity = new HermesActivity();
            entity.Abbreviation = "Abbreviation 1";
            entity.Id = Guid.NewGuid().ToString();
            entity.LastModifiedBy = "zaixiang";
            entity.LastModifiedDate = DateTime.Now;
            entity.Name = "HermesActivityType";
            entity.ActivityType = CreateHermesActivityType();
            entity.DefaultDuration = 100;
            entity.DefaultExpireDays = 1001;
            entity.DefaultStartTime = 100;
            entity.Enabled = false;
            entity.ExclusiveFlag = true;
            entity.WorkDayAmount = 9;
            return entity;
        }

        /// <summary>
        /// Creates HermesActivityType.
        /// </summary>
        internal static HermesActivityType CreateHermesActivityType()
        {
            HermesActivityType entity = new HermesActivityType();
            entity.Abbreviation = "Abbreviation 1";
            entity.Id = Guid.NewGuid().ToString();
            entity.LastModifiedBy = "zaixiang";
            entity.LastModifiedDate = DateTime.Now;
            entity.Name = "HermesActivityType";
            entity.ActivityGroup = CreateHermesActivityGroup();
            return entity;
        }

        /// <summary>
        /// Creates HermesActivityGroup.
        /// </summary>
        internal static HermesActivityGroup CreateHermesActivityGroup()
        {
            HermesActivityGroup entity = new HermesActivityGroup();
            entity.Abbreviation = "Abbreviation 1";
            entity.Id = Guid.NewGuid().ToString();
            entity.LastModifiedBy = "zaixiang";
            entity.LastModifiedDate = DateTime.Now;
            entity.Name = "HermesActivityGroup";
            return entity;
        }

        /// <summary>
        /// Creates HermesScheduleItem.
        /// </summary>
        internal static HermesScheduleItem CreateHermesScheduleItem()
        {
            HermesScheduleItem entity = new HermesScheduleItem();
            entity.Activity = CreateHermesActivity();
            entity.Duration = 99;
            entity.ExceptionFlag = 'Y';
            entity.ExpirationDate = DateTime.Now;
            entity.Id = Guid.NewGuid().ToString();
            entity.LastModifiedBy = "zaixiang";
            entity.LastModifiedDate = DateTime.Now;
            entity.ScheduleItemRequestStatus = CreateHermesScheduleItemRequestStatus();
            entity.ScheduleItemStatus = CreateHermesScheduleItemStatus();
            entity.Version = 2;
            entity.WorkDate = DateTime.Now;
            entity.WorkDayAmount = 87;
            entity.Note = new HermesGenericNote();
            entity.Note.Id = Guid.NewGuid().ToString();
            return entity;
        }

        /// <summary>
        /// Creates HermesScheduleItemRequestStatus.
        /// </summary>
        internal static HermesScheduleItemRequestStatus CreateHermesScheduleItemRequestStatus()
        {
            HermesScheduleItemRequestStatus entity = new HermesScheduleItemRequestStatus();
            entity.Abbreviation = "Abbreviation 2";
            entity.Description = "Description 111";
            entity.Id = Guid.NewGuid().ToString();
            entity.LastModifiedBy = "zaixiang";
            entity.LastModifiedDate = DateTime.Now;
            return entity;
        }

        /// <summary>
        /// Creates HermesScheduleItemStatus.
        /// </summary>
        internal static HermesScheduleItemStatus CreateHermesScheduleItemStatus()
        {
            HermesScheduleItemStatus entity = new HermesScheduleItemStatus();
            entity.Abbreviation = "Abbreviation 2";
            entity.Description = "Description 111";
            entity.Id = Guid.NewGuid().ToString();
            entity.LastModifiedBy = "zaixiang";
            entity.LastModifiedDate = DateTime.Now;
            return entity;
        }
    }
}
