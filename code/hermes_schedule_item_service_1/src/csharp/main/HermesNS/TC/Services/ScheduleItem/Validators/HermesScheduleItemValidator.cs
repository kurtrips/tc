// HermesScheduleItemValidator : HermesValidatorBase<HermesScheduleItem>.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using TopCoder.Util.ExceptionManager.SDE;
using HermesNS.TC.Entity.Validation;
using HermesNS.TC.Services.ScheduleItem.Entities;

namespace HermesNS.TC.Services.ScheduleItem.Validators
{
    /// <summary>
    /// <para>This class extends the HermesValidatorBase to validate a HermesScheduleItem.
    /// It will validate that all of the required fields are provided and that no other validation
    /// rules are violated, such as length of text. This may involve using the passed HermesScheduleItemService
    /// to check if the entity has valid fields, or if it is not unique when required.</para>
    /// </summary>
    /// <threadsafety>It is mutable and not thread-safe</threadsafety>
    /// <author>argolite</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [CoverageExcludeAttribute]
    public class HermesScheduleItemValidator : HermesValidatorBase<HermesScheduleItem>
    {
        /// <summary>
        /// <para>HermesScheduleItemService instance that is used to verify the uniqueness or validity of certain
        /// fields in the validated entity.</para>
        /// <para>Set in the constructor to a non-null value, and will never change.</para>
        /// </summary>
        private readonly HermesScheduleItemService hermesScheduleItemService;

        /// <summary>
        /// <para>Creates a new HermesScheduleItemValidator instance
        /// for the given entity and service</para>
        /// </summary>
        /// <exception cref="SelfDocumentingException">Wraps ArgumentNullException if either value is null</exception>
        /// <param name="entity">The HermesScheduleItem that is validated</param>
        /// <param name="hermesScheduleItemService">
        /// HermesScheduleItemService instance that is used to verify the uniqueness or validity of certain fields
        /// in the validated entity
        /// </param>
        public HermesScheduleItemValidator(
            HermesScheduleItem entity, HermesScheduleItemService hermesScheduleItemService) : base(entity)
        {
            try
            {
                Helper.ValidateNotNull(entity, "entity", true);
                Helper.ValidateNotNull(hermesScheduleItemService, "hermesScheduleItemService", true);

                this.hermesScheduleItemService = hermesScheduleItemService;
            }
            catch (Exception e)
            {
                throw Helper.GetSelfDocumentingException(e, e.Message,
                    "HermesNS.TC.Services.ScheduleItem.Validators.HermesScheduleItemValidator.ctor",
                    new string[] { "hermesScheduleItemService" }, new object[] { this.hermesScheduleItemService },
                    new string[] { "entity", "hermesScheduleItemService" },
                    new object[] { entity, hermesScheduleItemService },
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// <para>Validates the entity for required fields and other validation rules,
        /// using the HermesScheduleItemService to query for data.</para>
        ///
        /// Validates the following rules:
        /// <list type="bullet">
        /// <item>Activity property must be non-null and must be an existing HermesActivity in the database</item>
        /// <item>ScheduleItemStatus property must be non-null and
        /// must be an existing HermesScheduleItemStatus in the database</item>
        /// <item>ScheduleItemRequestStatus property must be non-null and
        /// must be an existing HermesScheduleItemRequestStatus in the database</item>
        /// </list>
        ///
        /// </summary>
        /// <exception cref="SelfDocumentingException">Wraps any exception that may occur.</exception>
        /// <returns>True if the validation succeeded. False otherwise.</returns>
        public override bool Validate()
        {
            HermesActivity activity = null;
            HermesScheduleItemStatus status = null;
            HermesScheduleItemRequestStatus reqSstatus = null;
            try
            {
                //Check Activity for null
                if (!Helper.ValidateNotNull(item.Activity, null, false))
                {
                    dataValidationRecords.Add(Helper.CreateDataValidationRecord(
                        item.Id, item.GetType().FullName, "Activity",
                        "HermesScheduleItemValidator.MissingActivity", null));
                }
                //Check if id of Activity is actually found.
                else
                {
                    //Get ActivityType by Id.
                    activity = hermesScheduleItemService.GetActivity(item.Activity.Id);
                    if (!Helper.ValidateNotNull(activity, null, false))
                    {
                        dataValidationRecords.Add(Helper.CreateDataValidationRecord(
                            item.Id, item.GetType().FullName, "Activity",
                            "HermesActivityValidator.IllegalActivity", new object[] { item.Activity.Id }));
                    }
                }

                //Check ScheduleItemStatus for null
                if (!Helper.ValidateNotNull(item.ScheduleItemStatus, null, false))
                {
                    dataValidationRecords.Add(Helper.CreateDataValidationRecord(
                        item.Id, item.GetType().FullName, "ScheduleItemStatus",
                        "HermesScheduleItemValidator.MissingScheduleItemStatus", null));
                }
                //Check if id of ScheduleItemStatus is actually found.
                else
                {
                    //Get ActivityType by Id.
                    status = hermesScheduleItemService.GetScheduleItemStatus(item.ScheduleItemStatus.Id);
                    if (!Helper.ValidateNotNull(status, null, false))
                    {
                        dataValidationRecords.Add(Helper.CreateDataValidationRecord(
                            item.Id, item.GetType().FullName, "ScheduleItemStatus",
                            "HermesActivityValidator.IllegalScheduleItemStatus",
                            new object[] { item.ScheduleItemStatus.Id }));
                    }
                }

                //Check ScheduleItemRequestStatus for null
                if (!Helper.ValidateNotNull(item.ScheduleItemRequestStatus, null, false))
                {
                    dataValidationRecords.Add(Helper.CreateDataValidationRecord(
                        item.Id, item.GetType().FullName, "ScheduleItemRequestStatus",
                        "HermesScheduleItemValidator.ScheduleItemRequestStatus", null));
                }
                //Check if id of ScheduleItemRequestStatus is actually found.
                else
                {
                    //Get ActivityType by Id.
                    reqSstatus =
                        hermesScheduleItemService.GetScheduleItemRequestStatus(item.ScheduleItemRequestStatus.Id);
                    if (!Helper.ValidateNotNull(reqSstatus, null, false))
                    {
                        dataValidationRecords.Add(Helper.CreateDataValidationRecord(
                            item.Id, item.GetType().FullName, "ScheduleItemRequestStatus",
                            "HermesActivityValidator.IllegalScheduleItemRequestStatus",
                            new object[] { item.ScheduleItemRequestStatus.Id }));
                    }
                }

                return dataValidationRecords.Count == 0;
            }
            catch (Exception e)
            {
                throw Helper.GetSelfDocumentingException(e, "Unable to perform validation.",
                    "HermesNS.TC.Services.ScheduleItem.Validators.HermesActivityValidator.Validate",
                    new string[] { "hermesScheduleItemService", "item" },
                    new object[] { hermesScheduleItemService, item },
                    new string[0], new object[0],
                    new string[] { "activity", "status", "reqSstatus" },
                    new object[] { activity, status, reqSstatus });
            }
        }

    }
}
