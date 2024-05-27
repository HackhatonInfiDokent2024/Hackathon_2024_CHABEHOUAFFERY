using Hackathon_2024_INFISOFTWARE.DataAccessLayer.DbContext;
using Hackathon_2024_INFISOFTWARE.WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services au conteneur
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("DATABASE"));
builder.Services.RegisterServices();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Construire l'application
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();


