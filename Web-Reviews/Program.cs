namespace Web_Reviews
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            builder.Services.AddMvc();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}");

            app.Run();
        }
    }
}
