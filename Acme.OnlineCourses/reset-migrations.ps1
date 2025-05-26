# Remove existing migrations
Remove-Item -Path "Acme.OnlineCourses/Migrations/*.cs" -Force

# Add new migration
dotnet ef migrations add Initial --project Acme.OnlineCourses
 
# Update database
dotnet ef database update --project Acme.OnlineCourses --verbose 