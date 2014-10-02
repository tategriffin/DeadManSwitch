using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExternalServiceAdapters
{
    public class SendEmailToFileAdapter : ISendEmailAdapter
    {
        private const string OutputFileDateFormat = "yyyy-MM-dd_HH_mm_ss";
        private const string OutputFileNameFormat = "{0}_outgoing_email_{1}.log";

        private string OutputFilePath;

        public SendEmailToFileAdapter(string outputPath)
        {
            this.OutputFilePath = outputPath;
        }

        public bool SendEmail(string from, string to, string subject, string body)
        {
            this.SendEmailToFile(from, to, subject, body);

            return true;
        }

        private void SendEmailToFile(string from, string to, string subject, string body)
        {
            string filePath = BuildFullPath();

            WriteEmailToFile(filePath, from, to, subject, body);
        }

        private string BuildFullPath()
        {
            string folder = this.OutputFilePath;
            string fileName = string.Format(OutputFileNameFormat, DateTime.UtcNow.ToString(OutputFileDateFormat), Guid.NewGuid());
            string fullPath = System.IO.Path.Combine(folder, fileName);

            return fullPath;
        }

        private void WriteEmailToFile(string filePath, string from, string to, string subject, string body)
        {
            string[] lines = BuildLinesToWrite(from, to, subject, body);

            System.IO.File.WriteAllLines(filePath, lines);
        }

        private string[] BuildLinesToWrite(string from, string to, string subject, string body)
        {
            string[] lines = 
            {
                "From: " + from,
                "To: " + to,
                "Subject: " + subject,
                "Body: " + body
            };

            return lines;
        }

    }
}