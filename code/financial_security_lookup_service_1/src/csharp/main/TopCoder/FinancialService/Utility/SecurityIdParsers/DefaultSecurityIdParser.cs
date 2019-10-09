// DefaultSecurityIdParser.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Xml;
using System.Collections.Generic;
using TopCoder.Util.ExceptionManager.SDE;

namespace TopCoder.FinancialService.Utility.SecurityIdParsers
{
    /// <summary>
    /// <para> This class implements the ISecurityIdParser interface,
    /// and it supports 4 types of security identifications: CUSIP, ISIN, SEDOL, and Symbol Ticker.
    /// CUSIP, ISIN and SEDOL can be determined by the length of the characters
    /// and embedded logic.  The class will also validate these IDs against their check digit.  For ticker symbols, the
    /// class tries to determine which market they are in (NYSE, NASDAQ or AMEX),  if it cannot determine, the
    /// component tries to narrow down the potential markets [NASDAQ, AMEX].  For all ticker symbols, it tries
    /// to determine the special code, for example if end with 'X', special code will be MUTUAL FUND.
    /// </para>
    /// </summary>
    ///
    /// <threadsafety>
    /// This class is stateless and thread-safe.
    /// </threadsafety>
    ///
    /// <author>Standlove</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class DefaultSecurityIdParser : ISecurityIdParser
    {
        /// <summary>
        /// Represents the length of an ISIN id.
        /// </summary>
        private const int ISIN_LENGTH = 12;

        /// <summary>
        /// Represents the length of a SEDOL id.
        /// </summary>
        private const int SEDOL_LENGTH = 7;

        /// <summary>
        /// Represents the length of a CUSIP id.
        /// </summary>
        private const int CUSIP_LENGTH = 9;

        /// <summary>
        /// Represents the maximum possible length of a NYSE ticker symbol.
        /// </summary>
        private const int NYSE_MAX_LENGTH = 6;

        /// <summary>
        /// Represents the minimum possible length of a NASDAQ ticker symbol.
        /// </summary>
        private const int NASDAQ_MIN_LENGTH = 4;

        /// <summary>
        /// Represents the maximum possible length of a NASDAQ ticker symbol.
        /// </summary>
        private const int NASDAQ_MAX_LENGTH = 5;

        /// <summary>
        /// Represents the minimum possible length of an AMEX ticker symbol.
        /// </summary>
        private const int AMEX_MIN_LENGTH = 2;

        /// <summary>
        /// Represents the maximum possible length of an AMEX ticker symbol.
        /// </summary>
        private const int AMEX_MAX_LENGTH = 5;

        /// <summary>
        /// The base xpath with which to find the special code for a special code symbol.
        /// </summary>
        public const string SPECIAL_CODE_XPATH = "Root/SpecialCodes";

        /// <summary>
        /// The base xpath with which to find the country name for a country code.
        /// </summary>
        public const string COUNTRIES_XPATH = "Root/Countries";

        /// <summary>
        /// The path of xml file from which to pick up the country codes and special codes.
        /// </summary>
        public const string COUNTRYANDSPECIALCODES_XML = "../../conf/countryAndSpecialCodes.xml";

        /// <summary>
        /// The weights given to each character when calculating the check digit for SEDOL.
        /// </summary>
        private static readonly int[] SEDOL_WEIGHTS = { 1, 3, 1, 7, 3, 9, 1 };

        /// <summary>
        /// <para>Empty constructor.</para>
        /// </summary>
        public DefaultSecurityIdParser()
        {
        }

        /// <summary>
        /// <para>Parse the security id to get its details. The result contains at least the security identifier
        /// type. CUSIP, ISIN and SEDOL can be determined by the length of the characters and embedded logic.  This
        /// method also validates these IDs against their check digit. For ticker symbols, this method tries to
        /// determine which market they are in (NYSE, NASDAQ or AMEX),  if it cannot determine, it tries
        /// to narrow down the potential markets [NASDAQ, AMEX].  For all ticker symbols, it tries to determine
        /// the special code.
        /// </para>
        /// </summary>
        ///
        /// <param name="securityId">the security id to parse.</param>
        ///
        /// <returns>the parsed SecurityIdDetails object.</returns>
        ///
        /// <exception cref="ArgumentNullException">if the given argument is null</exception>
        /// <exception cref="ArgumentException">if the given argument is empty</exception>
        /// <exception cref="InvalidSecurityIdFormatException">If country code or check digit are incorrect.</exception>
        /// <exception cref="SecurityIdParsingException">
        /// If any other error occurs when parsing the security id like non-alphanumeric characters etc.
        /// </exception>
        /// <exception cref="UnknownSecurityIdTypeException">
        /// If the securityId was not any of the CUSIP, SEDOL, ISIN or SymbolTicker.
        /// </exception>
        public SecurityIdDetails Parse(string securityId)
        {
            List<string> potentialMarkets = new List<string>();
            string specialCode = null;
            XmlDocument countryAndSpecialCodes = null;

            Helper.ValidateNotNullNotEmpty(securityId, "securityId");
            try
            {
                //Load xml document containing country codes and special codes
                countryAndSpecialCodes = new XmlDocument();
                countryAndSpecialCodes.Load(COUNTRYANDSPECIALCODES_XML);

                //Convert to uppercase and trim
                securityId = securityId.ToUpper().Trim();

                if (IsIsin(securityId, countryAndSpecialCodes))
                {
                    return new SecurityIdDetails(securityId, SecurityIdType.ISIN);
                }
                else if (IsSedol(securityId))
                {
                    return new SecurityIdDetails(securityId, SecurityIdType.SEDOL);
                }
                else if (IsCusip(securityId))
                {
                    return new SecurityIdDetails(securityId, SecurityIdType.CUSIP);
                }
                else
                {
                    if (IsNyseSymbol(securityId, ref specialCode, countryAndSpecialCodes))
                    {
                        potentialMarkets.Add(FinancialMarket.NYSE);
                    }
                    if (IsNasdaqSymbol(securityId, ref specialCode, countryAndSpecialCodes))
                    {
                        potentialMarkets.Add(FinancialMarket.NASDAQ);
                    }
                    if (IsAmexSymbol(securityId, ref specialCode, countryAndSpecialCodes))
                    {
                        potentialMarkets.Add(FinancialMarket.AMEX);
                    }

                    //If at least one market was found
                    if (potentialMarkets.Count > 0)
                    {
                        return new SymbolTickerSecurityIdDetails(
                            securityId, SecurityIdType.SymbolTicker, potentialMarkets.ToArray(), specialCode);
                    }
                }

                throw new UnknownSecurityIdTypeException(
                        "Unable to determine the type of securityId: " + securityId);
            }
            catch (Exception e)
            {
                throw Helper.GetSelfDocumentingException(e, "Unable to parse security Id.",
                    "TopCoder.FinancialService.Utility.SecurityIdParsers.DefaultSecurityIdParser.Parse",
                    new string[0], new object[0], new string[] { "securityId" }, new object[] { securityId },
                    new string[] { "potentialMarkets", "specialCode", "countryAndSpecialCodes" },
                    new object[] { potentialMarkets, specialCode, countryAndSpecialCodes });
            }

        }

        /// <summary>
        /// Checks whether the given securityId is a valid ISIN code.
        /// Following rules are followed:
        /// a) Must have length 12.
        /// b) Must have only alphanumeric characters.
        /// c) First 2 characters must be letters and must be a valid country code.
        /// d) Last character must be a digit.
        /// e) The actual check digit must equal the calculated check digit.
        /// </summary>
        ///
        /// <param name="securityId">The security id to check</param>
        /// <param name="countryAndSpecialCodes">xmlDocument instance containing country and special codes.</param>
        /// <returns>true if valid ISIN; false if length is not 12.</returns>
        ///
        /// <exception cref="InvalidSecurityIdFormatException">If country code or check digit are incorrect.</exception>
        /// <exception cref="SecurityIdParsingException">
        /// If any other error occurs when parsing the security id like non-alphanumeric characters etc.
        /// </exception>
        private static bool IsIsin(string securityId, XmlDocument countryAndSpecialCodes)
        {
            //Length must be 12 and last character must be a digit.
            if (securityId.Length != ISIN_LENGTH)
            {
                return false;
            }

            //Check digit must be a digit
            if (!char.IsDigit(securityId[ISIN_LENGTH - 1]))
            {
                throw new InvalidSecurityIdFormatException(
                    "Possible Isin securityId must have a numeric check digit.");
            }

            //Must contain only alphanumeric characters
            for (int i = 0; i < ISIN_LENGTH; i++)
            {
                if (!char.IsLetterOrDigit(securityId[i]))
                {
                    throw new SecurityIdParsingException(
                        "Possible Isin securityId must contain alpha-numeric characters only.");
                }
            }

            //First 2 characters must be valid country code
            XmlNode countryNode = countryAndSpecialCodes.SelectSingleNode(
                COUNTRIES_XPATH + "/" + securityId.Substring(0, 2));
            if (countryNode == null)
            {
                throw new InvalidSecurityIdFormatException(
                    "Isin securityId does not have recognizable country code.");
            }

            //Get check digit
            char checkDigit = Helper.GetCheckDigitForIsin(securityId.Remove(ISIN_LENGTH - 1, 1));

            //actual check digit must match
            if (checkDigit != securityId[ISIN_LENGTH - 1])
            {
                throw new InvalidSecurityIdFormatException(
                    "Isin securityId has incorrect check digit.");
            }

            return true;
        }

        /// <summary>
        /// Checks whether the given securityId is a valid SEDOL code.
        /// Following rules are followed:
        /// a) Must have length 7.
        /// b) Must have only alphanumeric characters.
        /// c) Last character must be a digit.
        /// d) The actual check digit must equal the calculated check digit.
        /// e) Cannot contain vowels.
        /// </summary>
        ///
        /// <param name="securityId">the security id to check.</param>
        /// <returns>true if valid SEDOL; false if length is not 7.</returns>
        ///
        /// <exception cref="InvalidSecurityIdFormatException">If check digit is incorrect.</exception>
        /// <exception cref="SecurityIdParsingException">
        /// If any other error occurs when parsing the security id like non-alphanumeric characters, vowels etc.
        /// </exception>
        private static bool IsSedol(string securityId)
        {
            //Length must be 7
            if (securityId.Length != SEDOL_LENGTH)
            {
                return false;
            }

            //last character must be a digit and first character must be a letter.
            if (!char.IsDigit(securityId[SEDOL_LENGTH - 1]))
            {
                throw new InvalidSecurityIdFormatException(
                    "Sedol securityId's last character must be a digit.");
            }

            int sum = 0;
            for (int i = 0; i < SEDOL_LENGTH; i++)
            {
                int numValue;

                if (IsVowel(securityId[i]))
                {
                    throw new SecurityIdParsingException("Sedol security id cannot contain vowels.");
                }

                if (char.IsDigit(securityId[i]))
                {
                    numValue = securityId[i] - '0';
                }
                else if (char.IsLetter(securityId[i]))
                {
                    numValue = securityId[i] - 'A' + 10;
                }
                else
                {
                    throw new SecurityIdParsingException(
                        "Sedol security id must contain only alphanumeric characters.");
                }

                //Multiply by the weight
                numValue *= SEDOL_WEIGHTS[i];

                //Add to overall sum
                sum += numValue;
            }

            //Verify check digit. If total sum is multiple of 10 then it is valid check digit.
            if (sum % 10 != 0)
            {
                throw new InvalidSecurityIdFormatException("Sedol security id's check digit is incorrect.");
            }

            return true;
        }

        /// <summary>
        /// Checks whether the given securityId is a valid CUSIP code.
        /// Following rules are followed:
        /// a) Must have length 9.
        /// b) Must have only alphanumeric characters.
        /// c) The 3rd, 4th, 5th characters must always be digits
        /// d) Last character must be a digit.
        /// e) The actual check digit must equal the calculated check digit.
        /// </summary>
        ///
        /// <param name="securityId">the security id to check.</param>
        /// <returns>true if valid CUSIP; false if length is not 9.</returns>
        ///
        /// <exception cref="InvalidSecurityIdFormatException">If check digit is incorrect.</exception>
        /// <exception cref="SecurityIdParsingException">
        /// If any other error occurs when parsing the security id like non-alphanumeric characters etc.
        /// </exception>
        private static bool IsCusip(string securityId)
        {
            //Length must be 9
            if (securityId.Length != CUSIP_LENGTH)
            {
                return false;
            }

            //Last chacracter must be a digit
            if (!char.IsDigit(securityId[CUSIP_LENGTH - 1]))
            {
                throw new InvalidSecurityIdFormatException(
                    "Possible Cusip securityId must have a numeric check digit.");
            }

            int sum = 0;
            bool multiply = true;
            for (int i = CUSIP_LENGTH - 1; i >= 0; --i)
            {
                int numValue;
                //Get the numeric value of the character
                switch (i)
                {
                    //The 3rd, 4th, 5th characters must always be digits
                    case 2:
                    case 3:
                    case 4:
                        {
                            if (char.IsDigit(securityId[i]))
                            {
                                numValue = securityId[i] - '0';
                            }
                            else
                            {
                                throw new SecurityIdParsingException(
                                    "The 3rd, 4th and 5th characters of a Cusip id must be digits.");
                            }
                            break;
                        }
                    default:
                        {
                            if (char.IsLetter(securityId[i]))
                            {
                                numValue = securityId[i] - 'A' + 10;
                            }
                            else if (char.IsDigit(securityId[i]))
                            {
                                numValue = securityId[i] - '0';
                            }
                            else
                            {
                                throw new SecurityIdParsingException(
                                    "Cusip security id must contain only alphanumeric characters.");
                            }
                            break;
                        }
                }

                //Alternate between 1 and 2 starting with 1 for the check digit.
                multiply = !multiply;
                numValue *= (multiply == true ? 2 : 1);

                //Add to overall sum
                sum += AddDigits(numValue);
            }

            //total sum (which includes the check digit) must be a multiple of 10
            if (sum % 10 != 0)
            {
                throw new InvalidSecurityIdFormatException("Check digit of Cusip security id is wrong.");
            }

            return true;
        }

        /// <summary>
        /// Checks whether the given securityId can be valid NYSE symbol.
        /// Following rules are followed:
        /// a) Must have length from 1 to 6.
        /// b) Must have only alphanumeric characters.
        /// c) A dot ('.') if present, must be the second last character of the string
        /// </summary>
        ///
        /// <param name="securityId">the security id to check.</param>
        /// <param name="countryAndSpecialCodes">xmlDocument instance containing country and special codes.</param>
        /// <param name="specialCode">Ref parameter for holding the special code if found.</param>
        /// <returns>true if valid NYSE symbol; false if any condition mentioned above is not true</returns>
        private static bool IsNyseSymbol(string securityId, ref string specialCode,
            XmlDocument countryAndSpecialCodes)
        {
            //Must be 1 to 6 chars long
            if (securityId.Length > NYSE_MAX_LENGTH)
            {
                return false;
            }

            for (int i = 0; i < securityId.Length; i++)
            {
                //Must contain letter or digits only
                if (char.IsLetterOrDigit(securityId[i]))
                {
                    continue;
                }
                //Can contain a dot but must be second last character of the string.
                //Last character must be a letter
                else if (securityId[i] == '.' && i == securityId.Length - 2 &&
                    char.IsLetter(securityId[securityId.Length - 1]))
                {
                    specialCode = GetSpecialCodeName(securityId[securityId.Length - 1], countryAndSpecialCodes);
                }
                //Any other situation except the 2 mentioned above means that it is not an NYSE symbol
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks whether the given securityId can be valid NASDAQ symbol.
        /// Following rules are followed:
        /// a) Must have length from 4 to 5.
        /// b) Must have only alphanumeric characters.
        /// c) If length is 5 then last character  must be a letter and should be a valid special code.
        /// </summary>
        ///
        /// <param name="securityId">the security id to check.</param>
        /// <param name="countryAndSpecialCodes">xmlDocument instance containing country and special codes.</param>
        /// <param name="specialCode">Ref parameter for holding the special code if found.</param>
        /// <returns>true if valid NASDAQ symbol; false if any condition mentioned above is not true</returns>
        private static bool IsNasdaqSymbol(string securityId, ref string specialCode,
            XmlDocument countryAndSpecialCodes)
        {
            //Must be 4 or 5 chars long
            if (securityId.Length < NASDAQ_MIN_LENGTH || securityId.Length > NASDAQ_MAX_LENGTH)
            {
                return false;
            }

            //Must contain letter or digits only
            for (int i = 0; i < securityId.Length; i++)
            {
                if (!char.IsLetterOrDigit(securityId[i]))
                {
                    return false;
                }
            }

            //Assign special code if length is 5. Last character must be a letter.
            if (securityId.Length == NASDAQ_MAX_LENGTH)
            {
                //If length is 5 and last character is not a letter then return false
                if (char.IsLetter(securityId[NASDAQ_MAX_LENGTH - 1]))
                {
                    specialCode = GetSpecialCodeName(securityId[NASDAQ_MAX_LENGTH - 1], countryAndSpecialCodes);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks whether the given securityId can be valid AMEX symbol.
        /// Following rules are followed:
        /// a) Must have length from 2 to 5.
        /// b) Must have only alphanumeric characters.
        /// c) A dot ('.') if present, must be the second last character of the string
        /// d) Code of length 2 or 3 must not contain dots.
        /// e) Code of length 4 or 5 must contain dots.
        /// f) If dot is found, then last character  must be a letter and should be a valid special code.
        /// </summary>
        ///
        /// <param name="securityId">the security id to check.</param>
        /// <param name="countryAndSpecialCodes">xmlDocument instance containing country and special codes.</param>
        /// <param name="specialCode">Ref parameter for holding the special code if found.</param>
        /// <returns>true if valid AMEX symbol; false if any condition mentioned above is not true</returns>
        private static bool IsAmexSymbol(string securityId, ref string specialCode,
            XmlDocument countryAndSpecialCodes)
        {
            //Total length between 2 and 5
            if (securityId.Length < AMEX_MIN_LENGTH || securityId.Length > AMEX_MAX_LENGTH)
            {
                return false;
            }

            //For symbols of length 2 or 3, no dot must be present
            if (securityId.Length < 4)
            {
                if (securityId.Contains("."))
                {
                    return false;
                }
            }

            //For symbols of length 4 or 5, dot must be present.
            if (securityId.Length > 3)
            {
                if (!securityId.Contains("."))
                {
                    return false;
                }
            }

            for (int i = 0; i < securityId.Length; i++)
            {
                //Must contain letter or digits only
                if (char.IsLetterOrDigit(securityId[i]))
                {
                    continue;
                }
                //Can contain a dot but must be second last character of the string
                //Last character must be a letter.
                else if (securityId[i] == '.' && i == securityId.Length - 2
                    && char.IsLetter(securityId[securityId.Length - 1]))
                {
                    specialCode = GetSpecialCodeName(securityId[securityId.Length - 1], countryAndSpecialCodes);
                }
                //Any other situation except the 2 mentioned above means that it is not an AMEX symbol
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Adds the digits of a number.
        /// </summary>
        /// <param name="number">The number</param>
        /// <returns>The added digits.</returns>
        private static int AddDigits(int number)
        {
            int ret = 0;

            //Add the digits
            while (number > 0)
            {
                ret += number % 10;
                number /= 10;
            }

            return ret;
        }

        /// <summary>
        /// Checks whether a character is a vowel.
        /// </summary>
        /// <param name="c">Character to check</param>
        /// <returns>true if vowel; false otherwise</returns>
        private static bool IsVowel(char c)
        {
            if (c == 'A' || c == 'E' || c == 'I' || c == 'O' || c == 'U')
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Reads the special code from xml file and returns its human friendly name.
        /// </summary>
        /// <param name="code">The special code for which to find the name.</param>
        /// <param name="countryAndSpecialCodes">XmlDocument containing the special codes</param>
        /// <returns>human friendly name of special code if code is found: null otherwise.</returns>
        private static string GetSpecialCodeName(char code, XmlDocument countryAndSpecialCodes)
        {
            //Get the special code (last character of string) and get its name from xml.
            XmlNode specialCodeNode = countryAndSpecialCodes.SelectSingleNode(
                SPECIAL_CODE_XPATH + "/" + code);

            //If the node was not found return null.
            if (specialCodeNode == null)
            {
                return null;
            }
            //Assign the name to the ref specialCode parameter.
            else
            {
                return specialCodeNode.InnerText;
            }
        }
    }
}
