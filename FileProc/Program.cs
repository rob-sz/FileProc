namespace FileProc
{
    class Program
    {
        static void Main(string[] args)
        {
            // Fastest import approach:
            //  - Create table without indexing or primary key
            //      - Create local db. Server: (localdb)\MSSQLLocalDB; Database: fileproc
            //      - Run FileProc.Data.FileProcData.sql to create table on fileproc database
            //  - Import file multi threaded
            //  - Apply any indexing to table after importing

            // Uses entity framework to create database and table, however this will create
            //  table with index on Id column, which slows down the import somewhat.
            //Test.SetupDatabase();

            Test.CreateTestFile();

            Test.ImportFileSingleThreaded();
            Test.ImportFileMultiThreaded();
        }
    }
}
