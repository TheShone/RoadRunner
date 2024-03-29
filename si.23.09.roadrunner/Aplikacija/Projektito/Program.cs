var builder = WebApplication.CreateBuilder(args);
var config= builder.Configuration;
// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Context>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProbaCS"));
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


builder.Services.AddAuthentication(x=>
{
    x.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme=JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(x=>
{
    x.SaveToken=true;
    x.TokenValidationParameters=new TokenValidationParameters
    {
            ValidIssuer=config["Jwt:Issuer"],
            ValidAudience=config["Jwt:Audience"],
            IssuerSigningKey=new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
            ValidateIssuer=true,
            ValidateAudience=true,
            ValidateLifetime=true,
            ValidateIssuerSigningKey=true,
            ClockSkew=TimeSpan.Zero
    };
}).AddCookie("default");

builder.Services.AddAuthorization();

builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();

builder.Services.AddSignalR();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(options => 
{
    options.WithOrigins("http://localhost:3000")
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials();
});

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    // ...

    endpoints.MapHub<NotificationsHub>("/notificationHub"); // Replace with your actual hub class and desired path
});

app.MapControllers();


app.Run();