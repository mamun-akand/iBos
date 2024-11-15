using CRUD.DBContext;
using CRUD.IRepository;
using CRUD.Repository;
using Microsoft.EntityFrameworkCore;

namespace CRUD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            /* Added Start */

            var connectionString = builder.Configuration.GetConnectionString("CRUDConnectionString"); //added
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString)); //added 
            builder.Services.AddScoped<IOrder, Order>();  //added

            /* Added End */


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
