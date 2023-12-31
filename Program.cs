using App.Models;
using App.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectMVC.ExtendMethod;
using ProjectMVC.Models;

var builder = WebApplication.CreateBuilder(args);

//kết nối dbcontext mysql
var connectionString = builder.Configuration.GetConnectionString("AppMvcConnectionString");
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseMySQL(connectionString)
);

builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();

// đăng ký dịch vụ mail
var mailSetting = builder.Configuration.GetSection("MailSettings");
//đăng ký cấu hình của 1 lớp
builder.Services.Configure<MailSettings>(mailSetting);
// đăng ký dịch vụ SendMailService
builder.Services.AddSingleton<IEmailSender, SendMailService>();


//đăng ký dịch vụ Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();



builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount = true;
});


builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Login";
    options.LogoutPath = "/Logout";
    options.AccessDeniedPath = "/KhongDuocTruyCap";
});

// cấu hình đăng nhập dịch vụ ngoài
builder.Services.AddAuthentication()
                .AddGoogle(options => {
                    var gconfig = builder.Configuration.GetSection("Authentication:Google");
                    options.ClientId = gconfig["ClientId"];
                    options.ClientSecret = gconfig["ClientSecret"];

                    options.CallbackPath = "/dang-nhap-tu-google";
                })
                .AddFacebook(options => {
                    var fconfig = builder.Configuration.GetSection("Authentication:Facebook");
                    options.AppId = fconfig["AppId"];
                    options.AppSecret = fconfig["AppSecret"];

                    options.CallbackPath = "/dang-nhap-tu-facebook";
                });



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

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
app.AddStatusCodePage(); // tùy biến respone lỗi : 400 - 599

app.UseRouting();

app.UseAuthentication(); //xác định danh tính
app.UseAuthorization(); // xác định quyền truy cập

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
