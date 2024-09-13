using Microsoft.EntityFrameworkCore;
using MyModel_CodeFirst.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();


//1.2.4 在Program.cs內以依賴注入的寫法撰寫讀取連線字串的物件
//      ※注意程式的位置必須要在var builder = WebApplication.CreateBuilder(args);這句之後
builder.Services.AddDbContext<GuestBookContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("GuestBookConnection")));


// Add services to the container.
builder.Services.AddControllersWithViews();


//4.2.7 在Program.cs中註冊及啟用Session
//註冊session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});


var app = builder.Build();



    app.UseSwagger();
    app.UseSwaggerUI();


//1.3.3 在Program.cs撰寫啟用Initializer的程式(要寫在var app = builder.Build();之後)
//※這個Initializer的作用是建立一些初始資料在資料庫中以利測試，所以不一定要有Initializer※
//※注意:初始資料的照片放在SeedSourcePhoto資料夾中※
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}



// Configure the HTTP request pipeline.

//3.4.4 修改 Program.cs中的程式碼，將是否在開發模式下的錯誤處理判斷註解掉
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    //3.4.8 在Program.cs中的程式碼註冊處理HttpNotFound(404)錯誤的Error Handler
    app.UseStatusCodePagesWithReExecute("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


//4.2.7 在Program.cs中註冊及啟用Session
//註冊session
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
