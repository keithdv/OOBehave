namespace OOBehave.Newtonsoft.Json
{
    public interface INewtonsoftJsonSerializer
    {
        T Deserialize<T>(string json);
        string Serialize(object target);
    }
}