using MyExtensions;
using System.Data;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace MyExtensions

{
    public static class ConsoleExtensions
    {

        /// <summary>
        /// Returns a string from a Console Message and Read Line.
        /// </summary>
        /// <param name="message">The message you want to ask to prompt user input:</param>
        public static string RequestString(this string message)
        {
            return RequestUserInput(message);
        }

        /// <summary>
        /// Return string from a Console Message and Read Line and Validates for empty string.
        /// </summary>
        /// <param name="message">The message you want to ask to prompt user input:</param>
        /// <param name="acceptNothing">true will allow use to enter blank string false will prevent this.</param>
        /// <returns></returns>
        public static string RequestString(this string message,bool acceptNothing)
        {
           return RequestUserInput(message,acceptNothing);       
        }


        /// <summary>
        /// Request integer through Console Message and vaildates for empty string and non-integers.
        /// </summary>
        /// <param name="message">The message you to ask to prompt user input</param>
        /// <returns></returns>
        public static int RequestInteger(this string message)
        {
            bool isValid;
            int output;
            do
            {   
                isValid = int.TryParse(RequestUserInput(message), out output);
                if (!isValid)
                {
                    Console.WriteLine("You answer needs to be a whole number. Please try again.");
                } 

            } while (!isValid);

            return output;
        }


        /// <summary>
        /// Request integer through Console Message and vaildates for empty string and non-integers.
        /// </summary>
        /// <param name="message">The message you to ask to prompt user input</param>
        /// <returns></returns>
        public static DateTime RequestDate(this string message)
        {
            bool isValid;
            DateTime output;
            do
            {
                isValid = DateTime.TryParseExact(RequestUserInput($"{message} (dd/MM/yy)"),"dd/MM/yy",CultureInfo.InvariantCulture,DateTimeStyles.None, out output);
                if (!isValid)
                {
                    Console.WriteLine("You answer needs to be a date in the format indicated above. Please try again.");
                }

            } while (!isValid);

            return output;
        }

        public static DateTime RequestDateTime(this string message)
        {
            bool isValid;
            DateTime output;
            do
            {
                isValid = DateTime.TryParseExact(RequestUserInput($"{message} (dd/MM/yy hh:mm)"), "dd/MM/yy hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out output);
                if (!isValid)
                {
                    Console.WriteLine("You answer needs to be a date in the format indicated above. Please try again.");
                }

            } while (!isValid);

            return output;
        }


        public static bool RequestBool(this string message)
        {
            bool? output;
            do
            {
                output = RequestUserInput(message).ToBool();
                
                if (output == null)
                {
                    Console.WriteLine("You answer needs to something like y/n, true/flase, yes/no, 1/0 etc. Please try again.");
                }

            } while (output == null);

            return (bool)output;
        }




        private static string RequestUserInput(string message, bool acceptNothing=false)
        {
            string output = "";
            do
            {
                Console.Write(message);
                output = Console.ReadLine() ?? "";
                if (output == "" && !acceptNothing) Console.Write("Your answer cannot be blank. Please try again.");


            } while (output == "" && !acceptNothing);

            return output;
        }

    }
}
      