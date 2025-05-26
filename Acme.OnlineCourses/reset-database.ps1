# Drop database if exists
dotnet ef database drop --project Acme.OnlineCourses --force
 
# Create database and apply migrations
dotnet ef database update --project Acme.OnlineCourses --verbose 