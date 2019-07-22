using FileProc.Data;
using FileProc.DataReader;
using FileProc.DataReader.Sampler;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace FileProc
{
    public static class Test
    {
        #region Private Members

        private static string FilePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                @"FileProcData.txt");

        private static int RecordLength = 500;
        private static long RecordCount = 500000;

        private static void CleanMessage(StringBuilder fieldValue)
        {
            // do fast char swap: bad chars for space
            for (int i = 0; i < fieldValue.Length; i++)
            {
                char ch = fieldValue[i];

                if (ch == 20) continue; // space
                if (ch >= 48 && ch <= 57) continue; // number
                if (ch >= 65 && ch <= 90) continue; // uppercase
                if (ch >= 97 && ch <= 122) continue; // lowercase

                fieldValue[i] = ' ';
            }
        }

        private static Field[] Fields
        {
            get
            {
                var bankNumberParts = new[]
                {
                    new FieldPart(205, 4),
                    new FieldPart(210, 4)
                };
                var accountBalanceParts = new[]
                {
                    new FieldPart(204, 1),
                    new FieldPart(195, 9)
                };

                return new Field[]
                {
                    new SequenceField("Id"),
                    new NumberField("CustomerNumber", 0, 10),
                    new StringField("CustomerName", 10, 30),
                    new StringField("CustomerSurname", 40, 30),
                    new StringField("AddressStreet1", 70, 30),
                    new StringField("AddressStreet2", 100, 30),
                    new StringField("AddressStreet3", 130, 30),
                    new StringField("AddressState", 160, 10),
                    new StringField("AddressPostcode", 170, 5),
                    new StringField("AccountNumber", 175, 20),
                    new DecimalField("AccountBalance", accountBalanceParts, 2),
                    new StringField("BankNumber", bankNumberParts),
                    new DateField("DateOfBirth", 215, 8, "ddMMyyyy"),
                    new StringField("Message", 223, 255) { Action = CleanMessage },
                    new LiteralField("SourceCode", "XKJ127J")
                };
            }
        }

        #endregion

        #region Public Methods

        public static void SetupDatabase()
        {
            StartTest(string.Format("Setup database. ({0}; {1})", 
                FileProcDbContext.ServerName, FileProcDbContext.DatabaseName));

            new FileProcDbContext().EnsureDatabaseCreated(true);

            StopTest();
        }
        public static void CreateTestFile()
        {
            StartTest(string.Format("Create test file with {0} records.", RecordCount));

            new SampleFile().Create(FilePath, RecordLength, RecordCount, Fields);

            StopTest();
        }

        public static void ImportFileSingleThreaded()
        {
            StartTest("Import file single threaded");

            new FileImport(FilePath, Fields, RecordLength).Import(
                FileProcDbContext.ConnectionString,
                FileProcDbContext.FileProcDataTable, 1);

            StopTest();
        }

        public static void ImportFileMultiThreaded()
        {
            StartTest("Import file multi threaded");

            new FileImport(FilePath, Fields, RecordLength).Import(
                FileProcDbContext.ConnectionString,
                FileProcDbContext.FileProcDataTable);

            StopTest();
        }

        #endregion

        #region Private Methods

        private static DateTime startTime;
        private static void StartTest(string title)
        {
            startTime = DateTime.Now;
            Console.WriteLine(string.Format("Test: {0}", title));
            Console.WriteLine(string.Format("Start: {0:dd MMM yyyy HH:mm:ss.fff}", startTime));
        }

        private static DateTime stopTime;
        private static void StopTest()
        {
            stopTime = DateTime.Now;
            Console.WriteLine(string.Format("Stop: {0:dd MMM yyyy HH:mm:ss.fff}", stopTime));
            Console.WriteLine(string.Format("Duration: {0}", stopTime - startTime));
            Console.WriteLine();
            Console.WriteLine("Press 'Enter' to continue...");
            Console.ReadLine(); // pause
        }

        #endregion
    }
}
