using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExternalServiceAdapters
{
    public class SendSMSToFileAdapter : ISendSMSAdapter
    {
        private const string OutputFileDateFormat = "yyyy-MM-dd_HH_mm_ss";
        private const string OutputFileNameFormat = "{0}_outgoing_sms_{1}.log";

        private string OutputFilePath;

        public SendSMSToFileAdapter(string outputPath)
        {
            this.OutputFilePath = outputPath;
        }

        public bool SendSMS(string from, string to, string message)
        {
            this.WriteSMSToFile(from, to, message);

            return true;
        }

        private void WriteSMSToFile(string from, string to, string message)
        {
            string filePath = BuildFullPath();

            WriteSMSToFile(filePath, from, to, message);
        }

        private string BuildFullPath()
        {
            string folder = this.OutputFilePath;
            string fileName = string.Format(OutputFileNameFormat, DateTime.UtcNow.ToString(OutputFileDateFormat), Guid.NewGuid());
            string fullPath = System.IO.Path.Combine(folder, fileName);

            return fullPath;
        }

        private void WriteSMSToFile(string filePath, string from, string to, string message)
        {
            string[] lines = BuildLinesToWrite(from, to, message);

            System.IO.File.WriteAllLines(filePath, lines);
        }

        private string[] BuildLinesToWrite(string from, string to, string message)
        {
            string[] lines = 
            {
                "From: " + from,
                "To: " + to,
                "Message: " + message
            };

            return lines;
        }

    }
}