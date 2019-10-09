// HermesScheduleItemRequestStatusValidator.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using TopCoder.Util.ExceptionManager.SDE;
using HermesNS.TC.Entity.Validation;
using HermesNS.TC.Services.ScheduleItem.Entities;

namespace HermesNS.TC.Services.ScheduleItem.Validators
{
    /// <summary>
    /// <para>This class extends the HermesValidatorBase to validate a HermesScheduleItemRequestStatus.
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
    public class HermesScheduleItemRequestStatusValidator : HermesValidatorBase<HermesScheduleItemRequestStatus>
    {
        /// <summary>
        /// <para>HermesScheduleItemService instance that is used to verify the uniqueness or validity of certain
        /// fields in the validated entity.</para>
        /// <para>Set in the constructor to a non-null value, and will never change.</para>
        /// </summary>
        private HermesScheduleItemService hermesScheduleItemService;

        /// <summary>
        /// <para>Creates a new HermesScheduleItemRequestStatusValidator instance
        /// for the given entity and service</para>
        /// </summary>
        /// <exception cref="SelfDocumentingException">Wraps ArgumentNullException if either value is null</exception>
        /// <param name="entity">The HermesScheduleItemRequestStatus that is validated</param>
        /// <param name="hermesScheduleItemService">
        /// HermesScheduleItemService instance that is used to verify the uniqueness or validity of certain fields
        /// in the validated entity
        /// </param>
        public HermesScheduleItemRequestStatusValidator(
            HermesScheduleItemRequestStatus entity, HermesScheduleItemService hermesScheduleItemService) : base(entity)
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
                    "HermesNS.TC.Services.ScheduleItem.Validators.HermesScheduleItemRequestStatusValidator.ctor",
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
        /// <item>Description must be non-null, non-empty and must not be more than 50 characters</item>
        /// <item>There must not be existing HermesScheduleItemRequestStatus which has
        /// the same abbreviation as the current HermesScheduleItemRequestStatus but a different id.</item>
        /// <item>There must not be existing HermesScheduleItemRequestStatus which has
        /// the same description as the current HermesScheduleItemRequestStatus but a different id.</item>
        /// </list>
        ///
        /// </summary>
        /// <exception cref="SelfDocumentingException">Wraps any exception that may occur.</exception>
        /// <returns>True if the validation succeeded. False otherwise.</returns>
        public override bool Validate()
        {
            bool abbrPassed = true;
            bool descPassed = true;
            IList<HermesScheduleItemRequestStatus> requestStatuses = null;
            HermesScheduleItemRequestStatus hsirs = null;
            try
            {
                //Validate the Abbreviation property
                dataValidationRecords.AddRange(
                    Helper.ValidateAbbreviation(item.Abbreviation, item.GetType().FullName,
                    "HermesScheduleItemRequestStatusValidator", item.Id, out abbrPassed));

                //Validate the Description property
                dataValidationRecords.AddRange(
                    Helper.ValidateDescription(item.Description, item.GetType().FullName,
                    "HermesScheduleItemRequestStatusValidator", item.Id, out descPassed));

                //Get data from service for checking duplicates only if we need to validate further
                if (abbrPassed || descPassed)
                {
                    //Check duplicates
                    //Get all request statuses:
                    requestStatuses = hermesScheduleItemService.GetAllScheduleItemRequestStatuses();
                    foreach (HermesScheduleItemRequestStatus reqStatus in requestStatuses)
                    {
                        //FOR SDE!
                        hsirs = reqStatus;

                        if (reqStatus.Abbreviation != null &&
                            reqStatus.Abbreviation.Equals(item.Abbreviation) &&
                            reqStatus.Id != item.Id && abbrPassed)
                        {
                            dataValidationRecords.Add(Helper.CreateDataValidationRecord(
                                item.Id, item.GetType().FullName, "Abbreviation",
                                "HermesScheduleItemRequestStatusValidator.AbbreviationNotUnique",
                                new object[] { item.Abbreviation, reqStatus.Id }));

                            //Dont look for any more duplicate abbreviations.
                            abbrPassed = false;
                        }

                        if (reqStatus.Description != null &&
                            reqStatus.Description.Equals(item.Description) &&
                            reqStatus.Id != item.Id && descPassed)
                        {
                            dataValidationRecords.Add(Helper.CreateDataValidationRecord(
                                item.Id, item.GetType().FullName, "Description",
                                "HermesScheduleItemRequestStatusValidator.DescriptionNotUnique",
                                new object[] { item.Description, reqStatus.Id }));

                            //Dont look for any more duplicate descriptions.
                            descPassed = false;
                        }
                    }
                }

                //Nothing failed validation then true, else false
                return dataValidationRecords.Count == 0;
            }
            catch (Exception e)
            {
                throw Helper.GetSelfDocumentingException(e, "Unable to perform validation.",
                    "HermesNS.TC.Services.ScheduleItem.Validators.HermesScheduleItemRequestStatusValidator.Validate",
                    new string[] { "hermesScheduleItemService", "item" },
                    new object[] { hermesScheduleItemService, item },
                    new string[0], new object[0],
                    new string[] { "abbrPassed", "descPassed", "requestStatuses", "hsirs" },
                    new object[] { abbrPassed, descPassed, requestStatuses, hsirs });
            }
        }

    }
}
