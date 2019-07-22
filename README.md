
# File Processing Data Reader for .Net

This library contains an implementation of System.Data.IDataReader that reads records from a file.
It is used in conjunction with System.Data.SqlClient.SqlBulkCopy to import a file into a destination table.
The FileDataReader is threadsafe and able to read a specific number of records from a starting record number.
This allows multiple threads to process a file that has been sub-divided into *chunks*, in parallel.
Solution includes console application that uses a sample file generator and sample testing code, as well as a sql script to create destination table on local database.

## FileDataReader Class
*Namespace:* FileProc.DataReader

Data reader implementation used by SqlBulkCopy to source data imported into destination table.

#### Constructor Parameters
**filePath** The file path of file to import.  
**fields** An array of field specifications.  
**recordLength** The length of each record.  
**recordStart** Starting record to import. Default to first record.  
**recordCount** Number of records to import. Default to all records.  

## FileImport Class
*Namespace:* FileProc.DataReader

Import file to destination table using FileDataReader.
Sub-divide file into *chunks* to be processed in parallel by multiple threads as required.

#### Constructor Parameters
**filePath** The file path of file to import.  
**fields** An array of field specifications.  
**recordLength** The length of each record.  

#### Import Method Parameters
**connectionString** Connection string to destination database.  
**destinationTable** Name of destination table.  
**threadCount** Number of threads to use. Default to processor count.  

## Field Abstract Class
*Namespace:* FileProc.DataReader  
*Derived:* DateField, DecimalField, LiteralField, NumberField, SequenceField, StringField  

Source record field specification, including database field name, source record position and length.
Fields may be made up of smaller parts from various locations in the source record.

## Test Application
*Namespace:* FileProc

- Open SQLServer Object Explorer in Visual Studio and on (localdb)\MSSQLLocalDB create database called 'fileproc'.
- Open New Query panel and run FileProc.Data.FileProcData.sql to create destination table.
- FileProc.Test class contains methods to create test file and run single and multi threaded tests.
  - Example code showing how to create various fields.
  - Number of records in test file can be modified here.
- Run FileProc console application to create file and run tests.

#### Test Results

*Processor:* Intel(R) Core(TM) i7-7820HQ CPU @ 2.90GHz, 2901 Mhz, 4 Core(s), 8 Logical Processor(s)  
*Record Count:* 500,000  
*Record Length:* 500  

~~~
Test: Create test file with 500000 records.
Duration: 00:00:18.1696518

Test: Import file single threaded
Duration: 00:00:09.9198364

Test: Import file multi threaded
Duration: 00:00:02.5287117
~~~
