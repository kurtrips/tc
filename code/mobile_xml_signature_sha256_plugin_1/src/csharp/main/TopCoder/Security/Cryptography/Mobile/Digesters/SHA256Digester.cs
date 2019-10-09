// SHA256Digester.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
// @author TCSDEVELOPER

using System;

namespace TopCoder.Security.Cryptography.Mobile.Digesters
{
    /// <summary>
    /// <p>This is an implementation of IDigester interface and provides a SHA-256 digester which
    /// provides a 256 bit digest from the input data.</p>
    /// <p><b>Thread Safety: </b>This is a thread-safe implementation, as it has no mutable state.</p>
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    /// <remarks>
    /// Throughout the documentation, "NIST SHA Reference" is used to refer to
    /// this file http://csrc.nist.gov/publications/fips/fips180-2/fips180-2withchangenotice.pdf
    /// </remarks>
    /// <example>
    /// ProcessRegistry pr = new ProcessRegistry("SomeNamespaceUsingSHA256AsDigester");
    /// IDigester digester = pr.GetDigesterInstance();
    /// byte[] digest = digester.Digest(UTF8Encoding.UTF8.GetBytes("HelloWorld"));
    /// </example>
    public class SHA256Digester : IDigester
    {
        /// <summary>
        /// Digest exception message
        /// </summary>
        private const string DIGEST_EX_MSG = "Digest could not be calculated. Please see inner exception for details";

        /// <summary>
        /// Used for calculation of the padded message length.
        /// </summary>
        private const int Int63 = 0x3f;

        /// <summary>
        /// Bit one followed by 7 zeroes. Used for padding message.
        /// </summary>
        private const int Int128 = 0x80;

        /// <summary>
        /// This method take a byte array, pads it according to the
        /// specifications in NIST SHA Reference and passes the padded message
        /// to the CalculateHash function which in turn returns a byte array containing
        /// the hash of the given message.
        /// </summary>
        /// <param name="message">Message to be hashed as byte array</param>
        /// <returns>Message hash value as byte array</returns>
        /// <exception cref="DigesterException">If there is an issue with producing the digest.</exception>
        /// <exception cref="ArgumentNullException">If message parameter is null</exception>
        public byte[] Digest(byte[] message)
        {
            ExceptionHelper.ValidateNotNull(message, "message");

            try
            {
                // Determine length of message in 512 bit (i.e 64 byte) blocks
                int len512 = message.Length / 64;

                // Determine space required for padding
                int padBase;
                if ((message.Length & Int63) < 56)
                {
                    len512++;
                    padBase = 56;
                }
                else
                {
                    len512 += 2;
                    padBase = 120;
                }

                // Create message padding
                byte[] pad = new byte[padBase - (message.Length & Int63)];

                //Add bit 1 followed by 7 0 bits
                //No need to add zeroes to the rest of the pad as C# arrays have default values
                pad[0] = 0x80;

                //Total number of bytes in the padded message is the number of 512 bit blocks
                //multiplied by the number of bytes in a 512 bit block (64)
                byte[] paddedMessage = new byte[len512 * 64];

                // Assemble padded message
                //First, copy the message itself to the padded message
                Array.Copy(message, 0, paddedMessage, 0, message.Length);

                //Next, copy the bit 1 followed by requisite number of 0 bits to the padded message
                Array.Copy(pad, 0, paddedMessage, message.Length, pad.Length);

                //Finally, copy the bit representation of the original message length to the padded message
                // Determine total message length in bits
                long lengthInBits = (long)message.Length * 8;
                byte[] arrLengthInBits = BitConverter.GetBytes(lengthInBits);
                Array.Reverse(arrLengthInBits);
                Array.Copy(arrLengthInBits, 0, paddedMessage, message.Length + pad.Length, 8);

                // Call CalculateHash to hash the now padded message
                return CalculateHash(len512, paddedMessage);
            }
            catch (Exception e)
            {
                throw new DigesterException(DIGEST_EX_MSG, e);
            }
        }

        /// <summary>
        /// SHA-256 Hash function. This method calculates the hash of a properly padded messages.
        /// </summary>
        /// <param name="blocks">Number of 512-bit message segments</param>
        /// <param name="paddedMessage">Padded message to be hashed as byte array</param>
        /// <returns>Hash value calculated of a padded message as byte array</returns>
        private byte[] CalculateHash( int blocks, byte[] paddedMessage )
        {
            // Represents the initial hash values used by SHA-256 algorithm.
            // These are the H constants as described NIST SHA Reference section 2.2.1
            uint[] initialHashes = new uint[]  {  0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a,
                                           0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19 };

            // Represents the constants used by SHA 256 for the hash computation.
            // The constant at index t is the one used for iteration t of the hash computation.
            // These are the K constants as described NIST SHA Reference section 2.2.1
            uint[] sha256Constants = new uint[] {
                                           0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5,
                                           0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
                                           0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3,
                                           0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
                                           0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc,
                                           0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
                                           0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7,
                                           0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
                                           0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13,
                                           0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
                                           0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3,
                                           0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
                                           0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5,
                                           0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
                                           0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208,
                                           0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2 };

            //Working variables.
            //These are a,b,c,....,h variables defined in NIST SHA Reference
            uint[] workingVars = new uint[8];

            // Intermediate hash values
            // These are the H variables as defined in section 2.2.1 of NIST SHA Reference
            uint[] interHash = new uint[8];

            // Scheduled W values as defined in section 2.2.1 of NIST SHA Reference
            uint[] scheduledValues = new uint[64];

            // Final hash byte array. Final hash value to return.
            byte[] hash = new byte[32];

            // Used to correct for endian
            byte[] endian = new byte[4];

            // Set Initial hash values
            for (int i = 0; i < 8; i++)
            {
                interHash[i] = initialHashes[i];
            }

            // Perform hash operation.
            //This confirms to Section 6.2.2 of NIST SHA Reference
            for( int i = 0; i < blocks; i++ )
            {
                // Prepare the message schedule.
                for( int t = 0; t < 64; t++ )
                {
                    if( t < 16 )
                    {
                        for( int j = 0; j < 4; j++ )
                            endian[j] = paddedMessage[i * 64 + t * 4 + 3 - j];
                        scheduledValues[t] = BitConverter.ToUInt32(endian, 0);
                    }
                    else
                    {
                        scheduledValues[t] = (uint)(Sigma(3, scheduledValues[t - 2]) + scheduledValues[t - 7]
                            + Sigma(2, scheduledValues[t - 15]) + scheduledValues[t - 16]);
                    }
                }

                //Initialize the eight working variables
                for (int j = 0; j < 8; j++)
                {
                    workingVars[j] = interHash[j];
                }

                //Perform main hash loop
                for( int t = 0; t < 64; t++ )
                {

                    uint temp1 = workingVars[7] + Sigma(1, workingVars[4]) +
                            Ch(workingVars[4], workingVars[5], workingVars[6]) +
                            sha256Constants[t] + scheduledValues[t];
                    uint temp2 = Sigma(0, workingVars[0]) + Maj(workingVars[0], workingVars[1], workingVars[2]);
                    workingVars[7] = workingVars[6];
                    workingVars[6] = workingVars[5];
                    workingVars[5] = workingVars[4];
                    workingVars[4] = workingVars[3] + temp1;
                    workingVars[3] = workingVars[2];
                    workingVars[2] = workingVars[1];
                    workingVars[1] = workingVars[0];
                    workingVars[0] = temp1 + temp2;
                }

                //Compute the intermediate hash values
                for (int j = 0; j < 8; j++)
                {
                    interHash[j] = workingVars[j] + interHash[j];
                }
            }

            // Copy Intermediate results to final hash array
            for( int i = 0; i < 8; i++)
            {
                endian = BitConverter.GetBytes(interHash[i]);
                Array.Reverse(endian);
                Array.Copy( endian, 0, hash, i*4, 4 );
            }

            return hash;
        }

        /// <summary>
        /// This is one of the six logical SHA256 functions as defined in section 4.1.2 of NIST SHA Reference
        /// </summary>
        /// <param name="x">The first 32 bit word</param>
        /// <param name="y">The second 32 bit word</param>
        /// <param name="z">The third 32 bit word</param>
        /// <returns>Ch Function return value</returns>
        private uint Ch(uint x, uint y, uint z)
        {
            return (x & y) ^ ((~x) & z);
        }

        /// <summary>
        /// This is one of the six logical SHA256 functions as defined in section 4.1.2 of NIST SHA Reference
        /// </summary>
        /// <param name="x">The first 32 bit word</param>
        /// <param name="y">The second 32 bit word</param>
        /// <param name="z">The third 32 bit word</param>
        /// <returns>Maj Function return value</returns>
        private uint Maj(uint x, uint y, uint z)
        {
            return (x & y) ^ (x & z) ^ (y & z);
        }

        /// <summary>
        /// Performs the SHA-256 shifting and rotating functions.
        /// These functions are defined in Section 4.1.2 of NIST SHA Reference
        /// </summary>
        /// <param name="i">Integer indicating which function to perform</param>
        /// <param name="x">Operand</param>
        /// <returns>Result of rotation/shift</returns>
        private uint Sigma( int i, uint x )
        {
            uint temp = 0;
            switch( i )
            {
                //Equation 4.4 defined in Section 4.1.2 of NIST SHA Reference
                case 0:
                    temp = (x>>2) | (x<<30);
                    temp = temp ^ ( (x>>13) | (x<<19) );
                    temp = temp ^ ( (x>>22) | (x<<10) );
                    break;
                //Equation 4.5 defined in Section 4.1.2 of NIST SHA Reference
                case 1:
                    temp = (x>>6) | (x<<26);
                    temp = temp ^ ( (x>>11) | (x<<21) );
                    temp = temp ^ ( (x>>25) | (x<<7) );
                    break;
                //Equation 4.6 defined in Section 4.1.2 of NIST SHA Reference
                case 2:
                    temp = (x>>7) | (x<<25);
                    temp = temp ^ ( (x>>18) | (x<<14) );
                    temp = temp ^ (x>>3);
                    break;
                //Equation 4.7 defined in Section 4.1.2 of NIST SHA Reference
                case 3:
                    temp = (x>>17) | (x<<15);
                    temp = temp ^ ( (x>>19) | (x<<13) );
                    temp = temp ^ (x>>10);
                    break;
            }
            return temp;
        }
    }
}