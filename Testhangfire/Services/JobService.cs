namespace Testhangfire.Services
{
    public interface IJobService
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task ProcessDataAsync(string data);
    }

    public class JobService : IJobService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            // Simulate email sending delay
            await Task.Delay(2000);

            Console.WriteLine($"[Email Sent] To: {email}, Subject: {subject}, Message: {message}");
        }

        public async Task ProcessDataAsync(string data)
        {
            // Simulate data processing delay
            await Task.Delay(3000);

            Console.WriteLine($"[Data Processed] Data: {data}");
        }
    }
}
