using System.Configuration;

namespace ClearBank.DeveloperTest.Data;

public class DataStoreFactory
{
    private const string BackupDataStoreType = "Backup";
    
    public static IDataStore CreateDataStore()
    {
        var dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];

        if (dataStoreType == BackupDataStoreType)
        {
            return new BackupAccountDataStore();
        }
        
        return new AccountDataStore();
    }
}