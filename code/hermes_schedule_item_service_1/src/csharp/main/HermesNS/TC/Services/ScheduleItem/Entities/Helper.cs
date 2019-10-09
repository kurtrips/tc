// Helper.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using TopCoder.Util.ExceptionManager.SDE;
using HermesNS.TC.Entity.Validation;
using HermesNS.TC.Services.AuditTrail;
using HermesNS.TC.Services.GenericNotes.Entities;
using TopCoder.Util.Indexing;
using TopCoder.Services.WCF.ScheduleItem.Entities;
using TopCoder.Services.WCF.GenericNotes.Entities;

namespace HermesNS.TC.Services.ScheduleItem.Entities
{
    /// <summary>
    /// <para>
    /// This class exposes static helper functions which helps improve code readability and reduces code redundancy.
    /// </para>
    /// </summary>
    ///
    /// <threadsafety>This class is stateless and thread-safe.</threadsafety>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [CoverageExcludeAttribute]
    internal static class Helper
    {
        /// <summary>
        /// Checks whether an object is null or not
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <param name="name">The name of the object</param>
        /// <param name="throwError">Whether to throw error if obj is null.</param>
        /// <exception cref="ArgumentNullException">If object is null</exception>
        /// <returns>true if object is not null, else false</returns>
        internal static bool ValidateNotNull(object obj, string name, bool throwError)
        {
            if (obj == null && throwError)
            {
                throw new ArgumentNullException(name, name + " must not be null.");
            }
            return obj != null;
        }

        /// <summary>
        /// Checks whether an string is empty or not (after trimming).
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <param name="name">The name of the string</param>
        /// <param name="throwError">Whether to throw error if str is empty.</param>
        /// <exception cref="ArgumentException">If string is empty</exception>
        /// <returns>true if string is not empty when trimmed, false otherwise</returns>
        internal static bool ValidateNotEmpty(string str, string name, bool throwError)
        {
            if (str != null && str.Trim().Equals(String.Empty) && throwError)
            {
                throw new ArgumentException(name + " must not be empty.", name);
            }
            return !(str != null && str.Trim().Equals(string.Empty));
        }

        /// <summary>
        /// Checks whether an string is neither empty nor null.
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <param name="name">The name of the string</param>
        /// <exception cref="ArgumentException">If string is empty</exception>
        /// <exception cref="ArgumentNullException">If string is null</exception>
        /// <param name="throwError">Whether to throw error if str is empty or null.</param>
        /// <returns>true if string is not null and not empty when trimmed, false otherwise.</returns>
        internal static bool ValidateNotNullNotEmpty(string str, string name, bool throwError)
        {
            return ValidateNotNull(str, name, throwError) && ValidateNotEmpty(str, name, throwError);
        }

        /// <summary>
        /// Wraps the given exception into a SelfDocumentingException instance after adding the instance variables,
        /// method parameters and local variables to it
        /// </summary>
        /// <param name="e">The exception to wrap.</param>
        /// <param name="message">The message for the SelfDocumentingException</param>
        /// <param name="methodName">The fully qualified method name from where the exception is thrown.</param>
        /// <param name="instanceVarsNames">The names of the instance variables.</param>
        /// <param name="instanceVars">The values of the instance variables at time of exception.</param>
        /// <param name="parameterVarsNames">The names of the method parameters.</param>
        /// <param name="parameterVars">The values of the method parameters at time of exception.</param>
        /// <param name="localVarsNames">The names of the local variables.</param>
        /// <param name="localVars">The values of the local variables at time of exception.</param>
        /// <returns>The formed SelfDocumentingException instance.</returns>
        internal static SelfDocumentingException GetSelfDocumentingException(
            Exception e, string message, string methodName, string[] instanceVarsNames, object[] instanceVars,
            string[] parameterVarsNames, object[] parameterVars, string[] localVarsNames, object[] localVars)
        {
            //Wrap only if it is not already of type SelfDocumentingException
            SelfDocumentingException sde = null;
            if (e is SelfDocumentingException)
            {
                sde = (SelfDocumentingException)e;
            }
            else
            {
                sde = new SelfDocumentingException(message, e);
            }

            MethodState ms = sde.PinMethod(methodName, e.StackTrace);

            //Add instance variables, method parameters and local variables
            for (int i = 0; i < instanceVarsNames.Length; i++)
            {
                //Ignore if unable to add object to MethodState.
                //This is a bug with SDE component. For ex. an XmlDocument instance cant be added.
                try
                {
                    ms.AddInstanceVariable(instanceVarsNames[i], instanceVars[i]);
                }
                catch { }
            }
            for (int i = 0; i < parameterVarsNames.Length; i++)
            {
                //Ignore if unable to add object to MethodState.
                //This is a bug with SDE component. For ex. an XmlDocument instance cant be added.
                try
                {
                    ms.AddMethodParameter(parameterVarsNames[i], parameterVars[i]);
                }
                catch { }
            }
            for (int i = 0; i < localVarsNames.Length; i++)
            {
                //Ignore if unable to add object to MethodState.
                //This is a bug with SDE component. For ex. an XmlDocument instance cant be added.
                try
                {
                    ms.AddLocalVariable(localVarsNames[i], localVars[i]);
                }
                catch { }
            }

            ms.Lock();
            return sde;
        }

        /// <summary>
        /// Creates a new DataValidationRecord given the values of properties to set.
        /// </summary>
        /// <param name="objectId">The id of the entity for which the record is being created.</param>
        /// <param name="entityFullName">The full name of the entity</param>
        /// <param name="validatedPropertyName">The property which has failed validation.</param>
        /// <param name="validationFormatStringResourceName">A string describing the nature of the failure.</param>
        /// <param name="validationFormatStringData">
        /// Array of objects containing the objects related to the failure. </param>
        /// <returns>The created DataValidationRecord</returns>
        internal static DataValidationRecord CreateDataValidationRecord(
            string objectId, string entityFullName, string validatedPropertyName,
            string validationFormatStringResourceName, object[] validationFormatStringData)
        {
            DataValidationRecord dvr = new DataValidationRecord();

            //Set the properties
            dvr.ObjectId = objectId;
            dvr.EntityFullName = entityFullName;
            dvr.ValidatedPropertyName = validatedPropertyName;
            dvr.ValidationFormatStringResourceName = validationFormatStringResourceName;
            if (validationFormatStringData != null)
            {
                dvr.ValidationFormatStringData = validationFormatStringData;
            }

            return dvr;
        }

        /// <summary>
        /// Checks if a type is numeric.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if it is numeric type; false otherwise</returns>
        internal static bool IsNumeric(Type type)
        {
            return (type.Equals(typeof(int)) || type.Equals(typeof(double)) || type.Equals(typeof(decimal))
                || type.Equals(typeof(short)) || type.Equals(typeof(long)) || type.Equals(typeof(float))
                || type.Equals(typeof(Int16)) || type.Equals(typeof(Int64)) || type.Equals(typeof(UInt16))
                || type.Equals(typeof(UInt32)) || type.Equals(typeof(UInt64)) || type.Equals(typeof(ulong))
                || type.Equals(typeof(ushort)));
        }

        /// <summary>
        /// Validates the abbreviation property of the entity.
        /// </summary>
        /// <param name="abbreviation">The value of abbreviation property.</param>
        /// <param name="entityFullName">The full name of teh entity</param>
        /// <param name="validatorName">The name of the validator class.</param>
        /// <param name="objectId">The id of the entity</param>
        /// <param name="abbrPassed">Whether the validation passed or not.</param>
        /// <returns>A list containing a list of validation records if any.</returns>
        internal static IList<DataValidationRecord> ValidateAbbreviation(
            string abbreviation, string entityFullName, string validatorName, string objectId, out bool abbrPassed)
        {
            IList<DataValidationRecord> ret = new List<DataValidationRecord>();

            //Check null or empty
            if (!ValidateNotNullNotEmpty(abbreviation, null, false))
            {
                ret.Add(CreateDataValidationRecord(objectId, entityFullName, "Abbreviation",
                    validatorName + ".MissingAbbreviation", null));
            }
            //Check length
            else if (abbreviation.Length > 20)
            {
                ret.Add(CreateDataValidationRecord(objectId, entityFullName,
                    "Abbreviation", validatorName + ".AbbreviationTooLong", new object[] { abbreviation }));
            }

            abbrPassed = ret.Count == 0;
            return ret;
        }

        /// <summary>
        /// Validates the name property of the entity.
        /// </summary>
        /// <param name="name">The value of name property.</param>
        /// <param name="entityFullName">The full name of the entity</param>
        /// <param name="validatorName">The name of the validator class.</param>
        /// <param name="objectId">The id of the entity</param>
        /// <param name="namePassed">Whether the validation passed or not.</param>
        /// <returns>A list containing a list of validation records if any.</returns>
        internal static IList<DataValidationRecord> ValidateName(
            string name, string entityFullName, string validatorName, string objectId, out bool namePassed)
        {
            IList<DataValidationRecord> ret = new List<DataValidationRecord>();

            if (!ValidateNotNullNotEmpty(name, null, false))
            {
                ret.Add(CreateDataValidationRecord(objectId, entityFullName, "Name",
                    validatorName + ".MissingName", null));
            }
            else if (name.Length > 50)
            {
                ret.Add(CreateDataValidationRecord(
                    objectId, entityFullName, "Name", validatorName + ".NameTooLong", new object[] { name }));
            }

            namePassed = ret.Count == 0;
            return ret;
        }

        /// <summary>
        /// Validates the description property of the entity.
        /// </summary>
        /// <param name="description">The value of description property.</param>
        /// <param name="entityFullName">The full name of the entity</param>
        /// <param name="validatorName">The name of the validator class.</param>
        /// <param name="objectId">The id of the entity</param>
        /// <param name="descPassed">Whether the validation passed or not.</param>
        /// <returns>A list containing a list of validation records if any.</returns>
        internal static IList<DataValidationRecord> ValidateDescription(
            string description, string entityFullName, string validatorName, string objectId, out bool descPassed)
        {
            IList<DataValidationRecord> ret = new List<DataValidationRecord>();

            if (!ValidateNotNullNotEmpty(description, null, false))
            {
                ret.Add(CreateDataValidationRecord(objectId, entityFullName, "Description",
                    validatorName + ".MissingDescription", null));
            }
            else if (description.Length > 50)
            {
                ret.Add(CreateDataValidationRecord(objectId, entityFullName, "Description",
                    validatorName + ".DescriptionTooLong", new object[] { description }));
            }

            descPassed = ret.Count == 0;
            return ret;
        }

        /// <summary>
        /// Gets a list of records signifying the properties that are different between the old and new entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity</typeparam>
        /// <param name="newEntity">The new entity. This can never be null.</param>
        /// <param name="oldEntity">The old entity. This may be null.</param>
        /// <param name="newEntityId">The id of the new entity. This may be null.</param>
        /// <returns>
        /// A list of records signifying the properties that are different between the old and new entity.
        /// </returns>
        internal static IList<HermesAuditRecord> GetAuditRecords<TEntity>(
            ISearchable<TEntity> newEntity, ISearchable<TEntity> oldEntity, string newEntityId)
            where TEntity : ISearchable<TEntity>
        {
            IList<HermesAuditRecord> ret = new List<HermesAuditRecord>();

            //If it is a new entity, return just 1 record
            if (oldEntity == null)
            {
                HermesAuditRecord auditRecord = new HermesAuditRecord();
                auditRecord.Message = typeof(TEntity).Name + ".CreatedAudit";
                auditRecord.EntityId = newEntityId;
                ret.Add(auditRecord);
                return ret;
            }

            //Check each property
            foreach (string key in newEntity.Values.Keys)
            {
                object newVal = newEntity.Values[key].Value;
                object oldVal = oldEntity.Values[key].Value;

                if (newVal == null && oldVal == null)
                {
                    continue;
                }

                //Get the type of the property
                Type propertyType = oldVal == null ? newVal.GetType() : oldVal.GetType();

                //Property of type GenericNote, we compare the description
                if (propertyType.Equals(typeof(HermesGenericNote)))
                {
                    newVal = newVal == null ? null : (newVal as HermesGenericNote).Description;
                    oldVal = oldVal == null ? null : (oldVal as HermesGenericNote).Description;
                    propertyType = typeof(string);
                }
                //Property of type HermesActivity, we compare the name
                else if (propertyType.Equals(typeof(HermesActivity)))
                {
                    newVal = newVal == null ? null : (newVal as HermesActivity).Name;
                    oldVal = oldVal == null ? null : (oldVal as HermesActivity).Name;
                    propertyType = typeof(string);
                }
                //Property of type HermesActivity, we compare the name
                else if (propertyType.Equals(typeof(HermesActivityType)))
                {
                    newVal = newVal == null ? null : (newVal as HermesActivityType).Name;
                    oldVal = oldVal == null ? null : (oldVal as HermesActivityType).Name;
                    propertyType = typeof(string);
                }
                //Property of type HermesActivity, we compare the name
                else if (propertyType.Equals(typeof(HermesActivityGroup)))
                {
                    newVal = newVal == null ? null : (newVal as HermesActivityGroup).Name;
                    oldVal = oldVal == null ? null : (oldVal as HermesActivityGroup).Name;
                    propertyType = typeof(string);
                }
                //Property of type HermesScheduleItemRequestStatus, we compare the Description
                else if (propertyType.Equals(typeof(HermesScheduleItemRequestStatus)))
                {
                    newVal = newVal == null ? null : (newVal as HermesScheduleItemRequestStatus).Description;
                    oldVal = oldVal == null ? null : (oldVal as HermesScheduleItemRequestStatus).Description;
                    propertyType = typeof(string);
                }
                //Property of type HermesScheduleItemRequestStatus, we compare the name
                else if (propertyType.Equals(typeof(HermesScheduleItemStatus)))
                {
                    newVal = newVal == null ? null : (newVal as HermesScheduleItemStatus).Description;
                    oldVal = oldVal == null ? null : (oldVal as HermesScheduleItemStatus).Description;
                    propertyType = typeof(string);
                }

                //Create record if the properties are not equal.
                if ((propertyType.IsValueType || propertyType.Equals(typeof(string))) &&
                    !object.Equals(oldVal, newVal))
                {
                    //Create the HermesAuditRecord and set EntityId
                    HermesAuditRecord auditRec = new HermesAuditRecord();
                    auditRec.EntityId = newEntityId;
                    auditRec.Message = typeof(TEntity).Name + "." + ToCamelCase(key) + "Audit";

                    //int, decimal, double etc.
                    if (IsNumeric(propertyType))
                    {
                        auditRec.NumericValue1 = Decimal.Parse(oldVal.ToString());
                        auditRec.NumericValue2 = Decimal.Parse(newVal.ToString());
                    }
                    //string
                    else if (propertyType.Equals(typeof(string)))
                    {
                        auditRec.TextValue1 = (string)oldVal;
                        auditRec.TextValue2 = (string)newVal;
                    }
                    //DateTime
                    else if (propertyType.Equals(typeof(DateTime)))
                    {
                        auditRec.NumericValue1 = new Decimal(((DateTime)oldVal).ToBinary());
                        auditRec.NumericValue2 = new Decimal(((DateTime)newVal).ToBinary());
                    }
                    //bool, char
                    else if (propertyType.IsValueType)
                    {
                        auditRec.TextValue1 = oldVal.ToString();
                        auditRec.TextValue2 = newVal.ToString();
                    }

                    ret.Add(auditRec);
                }
            }

            return ret;
        }

        /// <summary>
        /// Converts the first character of the string to upper case.
        /// </summary>
        /// <param name="str">The input string</param>
        /// <returns>The converted string</returns>
        private static string ToCamelCase(string str)
        {
            string ret = new string(char.ToUpper(str[0]), 1);
            ret += str.Remove(0, 1);
            return ret;
        }
    }
}
