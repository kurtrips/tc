// HermesActivityTypeValidator : HermesValidatorBase<HermesActivityType>.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using TopCoder.Util.ExceptionManager.SDE;
using HermesNS.TC.Entity.Validation;
using HermesNS.TC.Services.ScheduleItem.Entities;

namespace HermesNS.TC.Services.ScheduleItem.Validators
{
    /// <summary>
    /// <para>This class extends the HermesValidatorBase to validate a HermesActivityType.
    /// It validates that all of the required fields are provided and that no other validation rules are violated,
    /// such as length of text. This may involve using the passed HermesScheduleItemService to check if the entity
    /// has valid fields, or if it is not unique when required.</para>
    /// </summary>
    /// <threadsafety>It is mutable and not thread-safe</threadsafety>
    /// <author>argolite</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [CoverageExcludeAttribute]
    public class HermesActivityTypeValidator : HermesValidatorBase<HermesActivityType>
    {
        /// <summary>
        /// <para>HermesScheduleItemService instance that is used to verify the uniqueness or validity of certain
        /// fields in the validated entity.</para>
        /// <para>Set in the constructor to a non-null value, and will never change.</para>
        /// </summary>
        private readonly HermesScheduleItemService hermesScheduleItemService;

        /// <summary>
        /// <para>Creates a new HermesActivityTypeValidator instance for the given entity and service</para>
        /// </summary>
        /// <exception cref="SelfDocumentingException">Wraps ArgumentNullException if either value is null</exception>
        /// <param name="entity">The HermesActivityType that is validated</param>
        /// <param name="hermesScheduleItemService">
        /// HermesScheduleItemService instance that is used to verify the uniqueness or validity of certain fields
        /// in the validated entity
        /// </param>
        public HermesActivityTypeValidator(
            HermesActivityType entity, HermesScheduleItemService hermesScheduleItemService) : base(entity)
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
                    "HermesNS.TC.Services.ScheduleItem.Validators.HermesActivityTypeValidator.ctor",
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
        /// <item>There must not be existing HermesActivityType which has the same abbreviation as the current
        /// HermesActivityType but a different id.</item>
        /// <item>There must not be existing HermesActivityType which has the same name as the current
        /// HermesActivityType but a different id.</item>
        /// <item>The ActivityGroup property must not be null and must be an existing ActivityGroup
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
            HermesActivityGroup activityGroup = null;
            IList<HermesActivityType> activityTypes = null;
            HermesActivityType act = null;

            try
            {
                //Validate abbreviation
                dataValidationRecords.AddRange(Helper.ValidateAbbreviation(
                    item.Abbreviation, item.GetType().FullName, "HermesActivityTypeValidator",
                    item.Id, out abbrPassed));

                //Validate name
                dataValidationRecords.AddRange(Helper.ValidateName(
                    item.Name, item.GetType().FullName, "HermesActivityTypeValidator", item.Id, out namePassed));

                //Validate ActivityGroup
                if (!Helper.ValidateNotNull(item.ActivityGroup, null, false))
                {
                    dataValidationRecords.Add(Helper.CreateDataValidationRecord(
                        item.Id, item.GetType().FullName, "ActivityGroup",
                        "HermesActivityTypeValidator.MissingActivityGroup", null));
                }
                else
                {
                    activityGroup = hermesScheduleItemService.GetActivityGroup(item.ActivityGroup.Id);
                    if (!Helper.ValidateNotNull(activityGroup, null, false))
                    {
                        dataValidationRecords.Add(Helper.CreateDataValidationRecord(
                            item.Id, item.GetType().FullName, "ActivityGroup",
                            "HermesActivityTypeValidator.IllegalActivityGroup",
                            new object[] { item.ActivityGroup.Id }));
                    }
                }

                //Get data from service for checking duplicates only if we need to validate further
                if (namePassed || abbrPassed)
                {
                    //Get all HermesActivityGroups:
                    activityTypes = hermesScheduleItemService.GetAllActivityTypes();
                    foreach (HermesActivityType activityType in activityTypes)
                    {
                        //FOR SDE!
                        act = activityType;

                        if (activityType.Abbreviation != null &&
                            activityType.Abbreviation.Equals(item.Abbreviation) &&
                            activityType.Id != item.Id && abbrPassed)
                        {
                            dataValidationRecords.Add(Helper.CreateDataValidationRecord(
                                item.Id, item.GetType().FullName, "Abbreviation",
                                "HermesActivityTypeValidator.AbbreviationNotUnique",
                                new object[] { item.Abbreviation, activityType.Id }));

                            //Dont look for any more duplicate abbreviations
                            abbrPassed = false;
                        }

                        if (activityType.Name != null &&
                            activityType.Name.Equals(item.Name) &&
                            activityType.Id != item.Id && namePassed)
                        {
                            dataValidationRecords.Add(Helper.CreateDataValidationRecord(
                                item.Id, item.GetType().FullName, "Name",
                                "HermesActivityTypeValidator.NameNotUnique",
                                new object[] { item.Name, activityType.Id }));

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
                    "HermesNS.TC.Services.ScheduleItem.Validators.HermesActivityTypeValidator.Validate",
                    new string[] { "hermesScheduleItemService", "item" },
                    new object[] { hermesScheduleItemService, item },
                    new string[0], new object[0],
                    new string[] { "abbrPassed", "namePassed", "activityGroup", "activityTypes", "act" },
                    new object[] { abbrPassed, namePassed, activityGroup, activityTypes, act });
            }
        }

    }
}
