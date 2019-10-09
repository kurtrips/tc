// HermesActivityValidator : HermesValidatorBase<HermesActivity>.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using TopCoder.Util.ExceptionManager.SDE;
using HermesNS.TC.Entity.Validation;
using HermesNS.TC.Services.ScheduleItem.Entities;

namespace HermesNS.TC.Services.ScheduleItem.Validators
{
    /// <summary>
    /// <para>This class extends the HermesValidatorBase to validate a HermesActivity.
    /// It will validate that all of the required fields are provided and that no other validation rules are violated,
    /// such as length of text. This may involve using the passed HermesScheduleItemService to check if the entity
    /// has valid fields, or if it is not unique when required.</para>
    /// </summary>
    /// <threadsafety>It is mutable and not thread-safe</threadsafety>
    /// <author>argolite</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [CoverageExcludeAttribute]
    public class HermesActivityValidator : HermesValidatorBase<HermesActivity>
    {
        /// <summary>
        /// <para>HermesScheduleItemService instance that is used to verify the uniqueness or validity of certain
        /// fields in the validated entity.</para>
        /// <para>Set in the constructor to a non-null value, and will never change.</para>
        /// </summary>
        private readonly HermesScheduleItemService hermesScheduleItemService;

        /// <summary>
        /// <para>Creates a new HermesActivityValidator instance for the given entity and service</para>
        /// </summary>
        /// <exception cref="SelfDocumentingException">Wraps ArgumentNullException if either value is null</exception>
        /// <param name="entity">The HermesActivity that is validated</param>
        /// <param name="hermesScheduleItemService">
        /// HermesScheduleItemService instance that is used to verify the uniqueness or validity of certain fields
        /// in the validated entity
        /// </param>
        public HermesActivityValidator(HermesActivity entity, HermesScheduleItemService hermesScheduleItemService)
            : base(entity)
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
                    "HermesNS.TC.Services.ScheduleItem.Validators.HermesActivityValidator.ctor",
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
        /// <item>Abbreviation must be non-null, non-empty and must not be more than 20 characters</item>
        /// <item>Name must be non-null, non-empty and must not be more than 50 characters</item>
        /// <item>There must not be existing HermesActivity which has the same abbreviation as the current
        /// HermesActivity but a different id.</item>
        /// <item>There must not be existing HermesActivity which has the same name as the current
        /// HermesActivity but a different id.</item>
        /// <item>The ActivityType property must not be null and must be an existing ActivityType
        /// in the database.</item>
        /// </list>
        ///
        /// </summary>
        /// <exception cref="SelfDocumentingException">Wraps any exception that may occur.</exception>
        /// <returns>True if the validation succeeded. False otherwise.</returns>
        public override bool Validate()
        {
            bool abbrPassed = true;
            bool namePassed = true;
            HermesActivityType activityType = null;
            IList<HermesActivity> activities = null;
            HermesActivity act = null;

            try
            {
                //Validate the name and abbreviation field
                dataValidationRecords.AddRange(
                    Helper.ValidateAbbreviation(item.Abbreviation, item.GetType().FullName,
                    "HermesActivityValidator", item.Id, out abbrPassed));
                dataValidationRecords.AddRange(
                    Helper.ValidateName(item.Name, item.GetType().FullName,
                    "HermesActivityValidator", item.Id, out namePassed));

                //Check ActivityType for null
                if (!Helper.ValidateNotNull(item.ActivityType, null, false))
                {
                    dataValidationRecords.Add(Helper.CreateDataValidationRecord(
                        item.Id, item.GetType().FullName, "ActivityType",
                        "HermesActivityValidator.MissingActivityType", null));
                }
                //Check if id of ActivityType is actually found.
                else
                {
                    //Get ActivityType by Id.
                    activityType = hermesScheduleItemService.GetActivityType(item.ActivityType.Id);
                    if (!Helper.ValidateNotNull(activityType, null, false))
                    {
                        dataValidationRecords.Add(Helper.CreateDataValidationRecord(
                            item.Id, item.GetType().FullName, "ActivityType",
                            "HermesActivityValidator.IllegalActivityType", new object[] { item.ActivityType.Id }));
                    }
                }

                //Get data from service for checking duplicates only if we need to validate further
                if (namePassed || abbrPassed)
                {
                    //Get all active HermesActivities and check for duplicate Abbreviation and Name
                    activities = hermesScheduleItemService.GetAllActivities(false);
                    foreach (HermesActivity activity in activities)
                    {
                        //FOR SDE!
                        act = activity;

                        if (activity.Abbreviation != null &&
                            activity.Abbreviation.Equals(item.Abbreviation) &&
                            activity.Id != item.Id && abbrPassed)
                        {
                            dataValidationRecords.Add(Helper.CreateDataValidationRecord(
                                item.Id, item.GetType().FullName, "Abbreviation",
                                "HermesActivityValidator.AbbreviationNotUnique",
                                new object[] { item.Abbreviation, activity.Id }));

                            //Dont look for any more duplicate abbreviations
                            abbrPassed = false;
                        }

                        if (activity.Name != null &&
                            activity.Name.Equals(item.Name) &&
                            activity.Id != item.Id && namePassed)
                        {
                            dataValidationRecords.Add(Helper.CreateDataValidationRecord(
                                item.Id, item.GetType().FullName, "Name",
                                "HermesActivityValidator.NameNotUnique",
                                new object[] { item.Name, activity.Id }));

                            //Dont look for any more duplicate names
                            namePassed = false;
                        }
                    }
                }

                //Nothing failed validation then true, else false
                return dataValidationRecords.Count == 0;
            }
            catch (Exception e)
            {
                throw Helper.GetSelfDocumentingException(e, "Unable to perform validation.",
                    "HermesNS.TC.Services.ScheduleItem.Validators.HermesActivityValidator.Validate",
                    new string[] { "hermesScheduleItemService", "item" },
                    new object[] { hermesScheduleItemService, item },
                    new string[0], new object[0],
                    new string[] { "abbrPassed", "namePassed", "activityType", "activities", "act" },
                    new object[] { abbrPassed, namePassed, activityType, activities, act });
            }

        }

    }
}
