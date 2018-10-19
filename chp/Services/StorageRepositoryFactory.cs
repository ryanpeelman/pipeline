using System;

public class StorageRepositoryFactory 
{
    private static readonly Lazy<StorageRepositoryFactory> _lazyInstance = new Lazy<StorageRepositoryFactory>(() => new StorageRepositoryFactory());

    public static StorageRepositoryFactory Instance 
    {
        get
        {
            return _lazyInstance.Value;
        }
    }

    private StorageRepositoryFactory() 
    {
        // place for instance initialization code
    }

    public IStorageRepository Create(StoreParametersPayload parameters) 
    {
        if(parameters.Storage == "postgres") 
        {
            return new PostgresStorageRepository(parameters.ConnectionString);
        }
        
        return null;
    }
}