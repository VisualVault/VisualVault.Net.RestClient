// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonWebToken.cs" company="Auersoft">
//   Copyright (c) Auersoft. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace VVRestApi.Common.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    ///     The json web token.
    /// </summary>
    public class JsonWebToken
    {
        #region Static Fields

        /// <summary>
        ///     The hash algorithms.
        /// </summary>
        private static readonly Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>> HashAlgorithms;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="JsonWebToken" /> class.
        /// </summary>
        static JsonWebToken()
        {
            HashAlgorithms = new Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>> { { JwtHashAlgorithm.HS256, (key, value) =>
                {
                    using (var sha = new HMACSHA256(key))
                    {
                        return sha.ComputeHash(value);
                    }
                } 
                }, 
                                                                                              { JwtHashAlgorithm.HS384, (key, value) =>
                                                                                                  {
                                                                                                      using (var sha = new HMACSHA384(key))
                                                                                                      {
                                                                                                          return sha.ComputeHash(value);
                                                                                                      }
                                                                                                  } }, 
                                                                                              { JwtHashAlgorithm.HS512, (key, value) =>
                                                                                                  {
                                                                                                      using (var sha = new HMACSHA512(key))
                                                                                                      {
                                                                                                          return sha.ComputeHash(value);
                                                                                                      }
                                                                                                  } } };
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The decode.
        /// </summary>
        /// <param name="token">
        ///     The token.
        /// </param>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string Decode(string token, string key)
        {
            return Decode(token, key, true);
        }

        /// <summary>
        ///     The decode.
        /// </summary>
        /// <param name="token">
        ///     The token.
        /// </param>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="verify">
        ///     The verify.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        /// <exception cref="ApplicationException">
        /// </exception>
        public static string Decode(string token, string key, bool verify)
        {
            string[] parts = token.Split('.');
            string header = parts[0];
            string payload = parts[1];
            byte[] crypto = Base64UrlDecode(parts[2]);

            string headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
            JObject headerData = JObject.Parse(headerJson);
            string payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(payload));
            JObject payloadData = JObject.Parse(payloadJson);

            if (verify)
            {
                byte[] bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", payload));
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                var algorithm = (string)headerData["alg"];

                byte[] signature = HashAlgorithms[GetHashAlgorithm(algorithm)](keyBytes, bytesToSign);
                string decodedCrypto = Convert.ToBase64String(crypto);
                string decodedSignature = Convert.ToBase64String(signature);

                if (decodedCrypto != decodedSignature)
                {
                    throw new ApplicationException(string.Format("Invalid signature. Expected {0} got {1}", decodedCrypto, decodedSignature));
                }
            }

            return payloadData.ToString();
        }

        /// <summary>
        ///     The encode.
        /// </summary>
        /// <param name="payload">
        ///     The payload.
        /// </param>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string Encode(object payload, string key, JwtHashAlgorithm algorithm)
        {
            return Encode(payload, Encoding.UTF8.GetBytes(key), algorithm);
        }

        /// <summary>
        ///     The encode.
        /// </summary>
        /// <param name="payload">
        ///     The payload.
        /// </param>
        /// <param name="keyBytes">
        ///     The key bytes.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string Encode(object payload, byte[] keyBytes, JwtHashAlgorithm algorithm)
        {
            var segments = new List<string>();
            var header = new { alg = algorithm.ToString(), typ = "JWT" };

            byte[] headerBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header, Formatting.None));
            byte[] payloadBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload, Formatting.None));

            segments.Add(Base64UrlEncode(headerBytes));
            segments.Add(Base64UrlEncode(payloadBytes));

            string stringToSign = string.Join(".", segments.ToArray());

            byte[] bytesToSign = Encoding.UTF8.GetBytes(stringToSign);

            byte[] signature = HashAlgorithms[algorithm](keyBytes, bytesToSign);
            segments.Add(Base64UrlEncode(signature));

            return string.Join(".", segments.ToArray());
        }

        /// <summary>
        /// </summary>
        /// <param name="iss">
        ///     The "iss" (issuer) claim  identifies the principal that issued the JWT.  The processing of this claim is generally application specific.  The "iss" value is case sensitive.
        /// </param>
        /// <param name="additionalValues"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GenerateDefaultPayload(string loginToken, string issuer,  Dictionary<string, object> additionalValues)
        {
            return GeneratePayload(DateTime.Now.AddMinutes(20), DateTime.Now.AddMinutes(-10), DateTime.Now, issuer, "http://VisualVault/api", loginToken, Guid.NewGuid().ToString(), "JWT", additionalValues);
        }

        /// <summary>
        ///     Populates a JWT dictionary with non-null values
        /// </summary>
        /// <param name="exp">
        ///     The "exp" (expiration time) claim identifies the expiration time on  or after which the token MUST NOT be accepted for processing.
        /// </param>
        /// <param name="nbf">
        ///     The "nbf" (not before) claim identifies the time before which the token MUST NOT be accepted for processing.
        /// </param>
        /// <param name="iat">
        ///     The "iat" (issued at) claim identifies the time at which the JWT was issued.  This claim can be used to determine the age of the token.
        /// </param>
        /// <param name="iss">
        ///     The "iss" (issuer) claim  identifies the principal that issued the JWT.  The processing of this claim is generally application specific.  The "iss" value is case sensitive.
        /// </param>
        /// <param name="aud">
        ///     The "aud" (audience) claim identifies the audience that the JWT is intended for.  The principal intended to process the JWT MUST be identified with the value of the audience claim.  If the principal processing the claim does not identify itself with the identifier in the "aud" claim value then the JWT MUST be rejected.
        /// </param>
        /// <param name="prn">
        ///     The "prn" (principal) claim identifies the subject of the JWT.
        /// </param>
        /// <param name="jti">
        ///     The "jti" (JWT ID) claim provides a unique identifier for the JWT. The identifier value MUST be assigned in a manner that ensures that there is a negligible probability that the same value will be accidentally assigned to a different data object.  The "jti" claim can be used to prevent the JWT from being replayed.
        /// </param>
        /// <param name="typ">
        ///     The "typ" (type) claim is used to declare a type for the contents of this JWT Claims Set. The "typ" value is case sensitive. it is RECOMMENDED  that its value be either "JWT" or  "http://openid.net/specs/jwt/1.0"
        /// </param>
        /// <returns>
        ///     The <see cref="Dictionary{TKey,TValue}" />.
        /// </returns>
        public static Dictionary<string, object> GeneratePayload(DateTime? exp, DateTime? nbf, DateTime? iat, string iss, string aud, string prn, string jti, string typ, Dictionary<string, object> additionalValues)
        {
            var tokenObject = new Dictionary<string, object>();

            if (exp.HasValue)
            {
                tokenObject.Add("exp", GetDateSeconds(exp.Value));
            }

            if (nbf.HasValue)
            {
                tokenObject.Add("nbf", GetDateSeconds(nbf.Value));
            }

            if (iat.HasValue)
            {
                tokenObject.Add("iat", GetDateSeconds(iat.Value));
            }

            if (!string.IsNullOrWhiteSpace(iss))
            {
                tokenObject.Add("iss", iss);
            }

            if (!string.IsNullOrWhiteSpace(aud))
            {
                tokenObject.Add("aud", aud);
            }

            if (!string.IsNullOrWhiteSpace(prn))
            {
                tokenObject.Add("prn", prn);
            }

            if (!string.IsNullOrWhiteSpace(jti))
            {
                tokenObject.Add("jti", jti);
            }

            if (!string.IsNullOrWhiteSpace(typ))
            {
                tokenObject.Add("typ", typ);
            }

            if (additionalValues != null)
            {
                foreach (string key in additionalValues.Keys)
                {
                    if (!tokenObject.ContainsKey(key))
                    {
                        object value = additionalValues[key];
                        if (value != null)
                        {
                            if (value is DateTime)
                            {
                                var dateValue = (DateTime)value;
                                tokenObject.Add(key, GetDateSeconds(dateValue));
                            }
                            else if (value is string)
                            {
                                var strValue = value as string;
                                if (!string.IsNullOrWhiteSpace(strValue))
                                {
                                    tokenObject.Add(key, strValue);
                                }
                            }
                        }
                    }
                }
            }

            return tokenObject;
        }

        #endregion

        // from JWT spec
        #region Methods

        /// <summary>
        ///     The base 64 url decode.
        /// </summary>
        /// <param name="input">
        ///     The input.
        /// </param>
        /// <returns>
        ///     The <see cref="byte[]" />.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        private static byte[] Base64UrlDecode(string input)
        {
            string output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4)
            {
                // Pad with trailing '='s
                case 0:
                    break; // No pad chars in this case
                case 2:
                    output += "==";
                    break; // Two pad chars
                case 3:
                    output += "=";
                    break; // One pad char
                default:
                    throw new Exception("Illegal base64url string!");
            }

            byte[] converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }

        /// <summary>
        ///     The base 64 url encode.
        /// </summary>
        /// <param name="input">
        ///     The input.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        private static string Base64UrlEncode(byte[] input)
        {
            string output = Convert.ToBase64String(input);
            output = output.Split('=')[0]; // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }

        /// <summary>
        ///     Convert to RFC 3339 format
        /// </summary>
        /// <param name="dateTime">
        ///     The date Time.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        private static string GetDateSeconds(DateTime dateTime)
        {
            var utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTime);
            string totalSeconds = ((int)utcDateTime.Subtract(utc0).TotalSeconds).ToString();

            return totalSeconds;
        }

        /// <summary>
        ///     The get hash algorithm.
        /// </summary>
        /// <param name="algorithm">
        ///     The algorithm.
        /// </param>
        /// <returns>
        ///     The <see cref="JwtHashAlgorithm" />.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        private static JwtHashAlgorithm GetHashAlgorithm(string algorithm)
        {
            switch (algorithm)
            {
                case "HS256":
                    return JwtHashAlgorithm.HS256;
                case "HS384":
                    return JwtHashAlgorithm.HS384;
                case "HS512":
                    return JwtHashAlgorithm.HS512;
                default:
                    throw new InvalidOperationException("Algorithm not supported.");
            }
        }

        #endregion
    }
}