namespace common;

public class Program
{
    public static void Main(string[] args)
    {
        if (Microsoft.Data.SqlClient.SqlClientFactory.Instance == null)
        {
            Console.WriteLine("Its null!");
        }
    }
}