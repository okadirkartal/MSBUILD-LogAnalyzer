using System;
using System.IO;

namespace LogManager.Core.BLL
{
    public class Utility
    {

        private static readonly string dbDirectory= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        public static void CheckDatabaseDirectory()
        {
           try
            {
                if (!Directory.Exists(dbDirectory))
                    _ = Directory.CreateDirectory(dbDirectory);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public static void DeleteDatabaseDirectory()
        {
            try
            {
                Directory.Delete(dbDirectory, true);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
