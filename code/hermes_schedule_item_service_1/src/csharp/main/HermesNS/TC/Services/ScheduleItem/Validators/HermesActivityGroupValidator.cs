// HermesActivityGroupValidator : HermesValidatorBase<HermesActivityGroup>.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using TopCoder.Util.ExceptionManager.SDE;
using HermesNS.TC.Entity.Validation;
using HermesNS.TC.Services.ScheduleItem.Entities;

namespace HermesNS.TC.Services.ScheduleItem.Validators
{
    /// <summary>
    /// <para>Extends the HermesValidatorBase to validate a HermesActivityGroup. It validates that all of the required
    /// fields are provided and that no other validation rules are violated, such as length of text.
    /// This may involve using the passed HermesScheduleItemService to check if the entity has valid fields,
    /// or if it is not unique when required.</para>
    /// </summary>
    /// <threadsafety>It is mutable and not thread-safe</threadsafety>
    /// <author>argolite</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [CoverageExcludeAttribute]
    public class HermesActivityGroupValidator : HermesValidatorBase<HermesActivityGroup>
    {
        /// <summary>
        /// <para>HermesScheduleItemService instance that is used to verify the uniqueness or validity of certain
        /// fields in the validated entity.</para>
        /// <para>Set in the constructor to a non-null value, and will never change.</para>
        /// </summary>
        private readonly HermesScheduleItemService hermesScheduleItemService;

        /// <summary>
        /// <para>Creates a new HermesActivityGroupValidator instance for the given entity and service</para>
        /// </summary>
        /// <exception cref="SelfDocumentingException">Wraps ArgumentNullException if either value is null</exception>
        /// <param name="entity">The HermesActivityGroup that is validated</param>
        /// <param name="hermesScheduleItemService">
        /// HermesScheduleItemService instance that is used to verify the uniqueness or validity of certain fields
        /// in the validated entity.</param>
        public HermesActivityGroupValidator(
            HermesActivityGroup entity, HermesScheduleItemService hermesScheduleItemService) : base(entity)
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
                    "HermesNS.TC.Services.ScheduleItem.Validators.HermesActivityGroupValidator.ctor",
                    new string[] { "hermesScheduleItemService" }, new object[] { this.hermesScheduleItemService },
                    new string[] { "entity", "hermesScheduleItemService" },
                    new object[] { entity, hermesScheduleItemService },
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// <para>
        /// Validates the entity for required fields and other validation rules, using the HermesScheduleItemService
        /// to query for data.
        /// </para>
        /// Validates the following rules:
        /// <list type="bullet">
        /// <item>Abbreviation must be non-null, non-empty and must not be more than 20 characters</item>
        /// <item>Name must be non-null, non-empty and must not be more than 50 characters</item>
        /// <item>There must not be existing HermesActivityGroup which has the same abbreviation as the current
        /// HermesActivityGroup but a different id.</item>
        /// <item>There must not be existing HermesActivityGroup which has the same name as the current
        /// HermesActivityGroup but a different id.</item>
        /// </list>
        ///
        /// </summary>
        /// <exception cref="SelfDocumentingException">Wraps any exception that may occur.</exception>
        /// <returns>True if the validation succeeded. False otherwise.</returns>
        public override bool Validate()
        {
            bool abbrPassed = true;
            bool namePassed = true;
            IList<HermesActivityGroup> activityGroups = null;
            HermesActivityGroup acg = null;
            try
            {
                //Validate abbreviation
                dataValidationRecords.AddRange(Helper.ValidateAbbreviation(
                    item.Abbreviation, item.GetType().FullName, "HermesActivityGroupValidator",
                    item.Id, out abbrPassed));

                //Validate name
                dataValidationRecords.AddRange(Helper.ValidateName(
                    item.Name, item.GetType().FullName, "HermesActivityGroupValidator", item.Id, out namePassed));

                //Get HermesActivityGroups only if we need to validate further
                if (namePassed || abbrPassed)
                {
                    //Get all HermesActivityGroups
                    activityGroups = hermesScheduleItemService.GetAllActivityGroups();
                    //Validate for duplicates
                    foreach (HermesActivityGroup activityGroup in activityGroups)
                    {
                        //For SDE
                        acg = activityGroup;

                        if (activityGroup.Abbreviation != null &&
                            activityGroup.Abbreviation.Equals(item.Abbreviation) &&
                            activityGroup.Id != item.Id && abbrPassed)
                        {
                            dataValidationRecords.Add(Helper.CreateDataValidationRecord(
                                item.Id, item.GetType().FullName, "Abbreviation",
                                "HermesActivityGroupValidator.AbbreviationNotUnique",
                                new object[] { item.Abbreviation, activityGroup.Id }));

                            //Dont look for any more duplicate abbreviations
                            abbrPassed = false;
                        }

                        if (activityGroup.Name != null &&
                            activityGroup.Name.Equals(item.Name) &&
                            activityGroup.Id != item.Id && namePassed)
                        {
                            dataValidationRecords.Add(Helper.CreateDataValidationRecord(
                                item.Id, item.GetType().FullName, "Name",
                                "HermesActivityGroupValidator.NameNotUnique",
                                new object[] { item.Name, activityGroup.Id }));

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
                    "HermesNS.TC.Services.ScheduleItem.Validators.HermesActivityGroupValidator.Validate",
                    new string[] { "hermesScheduleItemService", "item" },
                    new object[] { hermesScheduleItemService, item },
                    new string[0], new object[0],
                    new string[] { "abbrPassed", "namePassed", "activityGroups", "acg" },
                    new object[] { abbrPassed, namePassed, activityGroups, acg });
            }
        }
    }
}
