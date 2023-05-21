using ProyectoPresupuesto.Services;
using ProyectoPresupuesto.Validations;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRepositoryAccountTypes, RepositoryAccountTypes>();
builder.Services.AddTransient<IRepeatedAccountType, RepeatedAccountType>();
builder.Services.AddTransient<IUsers, Users>();
builder.Services.AddTransient<IrepositoryAccounts ,RepositoryAccounts>();
builder.Services.AddTransient<IrepositoryCategories, RepositoryCategories>();
builder.Services.AddTransient<IrepositoryTransactions, RepositoryTransactions>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
