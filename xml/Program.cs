using LinkedIn_XML.Services;

namespace LinkedIn_XML;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddControllers();
        builder.Services.AddSingleton<IDataFileService,DataFileService>();
        builder.Services.AddSingleton<IBlobDataFileService, BlobDataFileService>();

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();

    }
}
