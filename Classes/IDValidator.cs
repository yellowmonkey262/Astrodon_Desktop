using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Astrodon
{
    /// <summary>
    /// Validates a South African identity number.
    /// </summary>
    public class IDValidator
    {
        // constants
        const int VALID_LENGTH = 13;
        const int CONTROL_DIGIT_LOCATION = 12;
        const int CONTROL_DIGIT_CHECK_VALUE = 10;
        const int CONTROL_DIGIT_CHECK_EXCEPTION_VALUE = 9;
        const string REGEX_ID_PATTERN = "(?<Year>[0-9][0-9])(?<Month>([0][1-9])|([1][0-2]))(?<Day>([0-2][0-9])|([3][0-1]))(?<Gender>[0-9])(?<Series>[0-9]{3})(?<Citizenship>[0-9])(?<Uniform>[0-9])(?<Control>[0-9])";
        const bool VALID = true;
        const bool INVALID = false;

        // member variables
        private string id;

        // constructor
        public IDValidator(string id_)
        {
            id = id_;
        }

        public int GetAge()
        {
            if (isValid())
            {
                DateTime birthDate = DateTime.ParseExact(id.Substring(0, 2) + "/" + id.Substring(2, 2) + "/" + id.Substring(4, 2), "yy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                int years = DateTime.Now.Year - birthDate.Year;

                if (years < 0)
                {
                    birthDate = DateTime.ParseExact("19" + id.Substring(0, 2) + "/" + id.Substring(2, 2) + "/" + id.Substring(4, 2), "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                    years = DateTime.Now.Year - birthDate.Year;
                }

                if (DateTime.Now.Month < birthDate.Month ||
                    (DateTime.Now.Month == birthDate.Month &&
                    DateTime.Now.Day < birthDate.Day))
                    years--;

                return years;
            }
            else
            {
                throw new Exception("Invalid ID");
            }
        }

        // SA citizen check
        public bool IsSACitizen()
        {
            if (isValid())
            {
                if (int.Parse(id.Substring(10, 1)) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new Exception("Invalid ID");
            }
        }

        // gender check
        public bool IsFemale()
        {
            if (isValid())
            {
                if (int.Parse(id.Substring(6, 1)) < 5)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new Exception("Invalid ID");
            }
        }

        // get date of birth
        public string GetDateOfBirth()
        {
            if (isValid())
            {
                DateTime date = DateTime.ParseExact(id.Substring(0, 2) + "/" + id.Substring(2, 2) + "/" + id.Substring(4, 2), "yy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);

                int years = DateTime.Now.Year - date.Year;

                if (years < 0)
                {
                    date = DateTime.ParseExact("19" + id.Substring(0, 2) + "/" + id.Substring(2, 2) + "/" + id.Substring(4, 2), "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                }

                return date.ToShortDateString();
            }
            else
            {
                throw new Exception("Invalid ID");
            }
        }

        public DateTime GetDateOfBirthAsDateTime()
        {
            if (isValid())
            {
                DateTime date = DateTime.ParseExact(id.Substring(0, 2) + "/" + id.Substring(2, 2) + "/" + id.Substring(4, 2), "yy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);

                int years = DateTime.Now.Year - date.Year;

                if (years < 0)
                {
                    date = DateTime.ParseExact("19" + id.Substring(0, 2) + "/" + id.Substring(2, 2) + "/" + id.Substring(4, 2), "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                }

                return date;
            }
            else
            {
                throw new Exception("Invalid ID");
            }
        }



        // check whether ID number is valid
        public bool isValid()
        {
            // assume that the id number is invalid
            bool isValidPattern = false;
            bool isValidLength = false;
            bool isValidControlDigit = false;

            // check length
            if (id.Length == VALID_LENGTH)
            {
                isValidLength = true;
            }

            // match regex pattern, only if length is valid
            if (isValidLength)
            {
                Regex idPattern = new Regex(REGEX_ID_PATTERN);

                if (idPattern.IsMatch(id))
                {
                    //00 will slip through the regex and checksum
                    if (id.Substring(2, 2) != "00" && id.Substring(4, 2) != "00")
                    {
                        isValidPattern = true;
                    }
                }
            }



            // check control digit, only if previous validations passed
            if (isValidLength && isValidPattern)
            {
                int a = 0;
                int b = 0;
                int c = 0;
                int cDigit = -1;
                int tmp = 0;
                StringBuilder even = new StringBuilder();
                string evenResult = null;

                // sum odd digits
                for (int i = 0; i < VALID_LENGTH - 1; i = i + 2)
                {
                    a = a + int.Parse(id[i].ToString());
                }

                // build a string containing even digits
                for (int i = 1; i < VALID_LENGTH - 1; i = i + 2)
                {
                    even.Append(id[i]);
                }
                // multipy by 2
                tmp = int.Parse(even.ToString()) * 2;
                // convert to string again
                evenResult = tmp.ToString();
                // sum the digits in evenResult
                for (int i = 0; i < evenResult.Length; i++)
                {
                    b = b + int.Parse(evenResult[i].ToString());
                }

                c = a + b;

                cDigit = CONTROL_DIGIT_CHECK_VALUE - int.Parse(c.ToString()[1].ToString());
                if (cDigit == int.Parse(id[CONTROL_DIGIT_LOCATION].ToString()))
                {
                    isValidControlDigit = true;
                }
                else
                {
                    if (cDigit > CONTROL_DIGIT_CHECK_EXCEPTION_VALUE)
                    {
                        if (0 == int.Parse(id[CONTROL_DIGIT_LOCATION].ToString()))
                        {
                            isValidControlDigit = true;
                        }
                    }
                }
            }

            // final check
            if (isValidLength && isValidPattern && isValidControlDigit)
            {
                return VALID;
            }
            else
            {
                return INVALID;
            }
        }
    }
}
