$rg= 'sp-rg-weu'
$server= 'sp-sql-serv'
$location= 'westeurope'
$database= 'sp-sql-db-weu'
$user= 'matlij'
$password= 'myadminpassword321!'

# Write-Output "Connecting user"
# Connect-AzAccount

# Write-Output "Creating $server in $location..."
# az sql server create --location $location --resource-group $rg --name $server --admin-user $user --admin-password $password

# Write-Output "Creating $database on $server..."
# az sql db create --resource-group $rg --server $server --name $database --edition Basic --zone-redundant false

# $myIpAddress= (Invoke-WebRequest -uri "http://ifconfig.me/ip").Content
# New-AzSqlServerFirewallRule -ResourceGroupName $rg -ServerName $server -FirewallRuleName "MattiasL" -StartIpAddress $myIpAddress -EndIpAddress $myIpAddress

Write-Output "Migrating the database"
$connectionString= "Server=tcp:$server.database.windows.net,1433;Initial Catalog=$database;Persist Security Info=False;User ID=$user;Password=$password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
dotnet ef database update --connection $connectionString -p C:\GIT\Mattias\sportplanner\SportPlannerApi\SportPlannerApi.csproj