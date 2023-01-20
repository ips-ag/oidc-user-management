dotnet ef migrations add CreateIdentitySchema -c ApplicationDbContext -o EntityFramework/Migrations --startup-project ../IPS.UserManagement --verbose
dotnet ef migrations remove -c ApplicationDbContext --startup-project ../IPS.UserManagement --verbose
dotnet ef database update -c ApplicationDbContext --startup-project ../IPS.UserManagement --verbose
