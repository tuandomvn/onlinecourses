# Drop database completely
dotnet ef database drop --project Acme.OnlineCourses --force

# Remove existing migrations
Remove-Item -Path "Acme.OnlineCourses/Migrations/*.cs" -Force

# Add new migration
dotnet ef migrations add Initial --project Acme.OnlineCourses

# Create database and apply migrations
dotnet ef database update --project Acme.OnlineCourses --verbose 