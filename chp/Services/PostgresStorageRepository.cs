using Npgsql;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class PostgresStorageRepository : IStorageRepository
{
    private readonly string _connectionString;

    public PostgresStorageRepository(string connectionString) {
        _connectionString = connectionString;
    }

    public void Store(object data)
    {
        StoreHeartRate(data);
    }
    
    private void StoreHeartRate(object data) 
    {
        var json = JsonConvert.SerializeObject(data);
        JObject jsonObject = JObject.Parse(json);

        var heartrate = jsonObject.Value<int>("heartrate");
        var datetime = jsonObject.Value<double>("datetime");

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            
            using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "INSERT INTO salesforce.test__c (mykey__c, myvalue__c) VALUES (@p1, @p2)";
                command.Parameters.AddWithValue("p1", datetime);
                command.Parameters.AddWithValue("p2", heartrate);
                command.ExecuteNonQuery();
            }
        }
    }
}