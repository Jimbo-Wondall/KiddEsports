using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace KiddEsports
{
    public class FileManager
    {
        static CurrentTime dateTime = new CurrentTime();

        // Gets and sets the location of the desktop filepath as a string
        static string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);


        public static void CreateReport(string reportType, IEnumerable<string> strings)
        {
            // Joins the report type and current time to create a unique filename for the exported file
            string filePath = desktopPath + @$"\{reportType} {dateTime}.csv";

            // Creates a file at the specified filepath with the string list provided
            File.WriteAllLines(filePath, strings);

            // Shows a message saying what was created and where
            MessageBox.Show($"{reportType} created at {filePath}", "New report created", MessageBoxButton.OK);
        }
    }
    public class CurrentTime
    {
        DateTime Time = DateTime.Now;

        public override string ToString()
        {
            return (Time.ToString("HH:mm:ss") + Time.ToString("MM/dd/yyyy")).Replace("/", "").Replace(":", "");
        }
    }
}
