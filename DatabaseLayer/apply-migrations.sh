@echo off

#:: Set paths and variables
#SET EF_PROJECT_PATH=../DatabaseLayer         :: Relative path to DatabaseLayer.csproj
#SET STARTUP_PROJECT_PATH=.                   :: Current folder for TradingAPIs.csproj
#SET CONFIGURATION=Release                    :: Configuration (Release or Debug)
#SET DB_CONNECTION_STRING="Host=oregon-postgres.render.com;Port=5432;Database=trade_pgs_db;Username=trade_pgs_db_user;Password=1O4gXP71zC1O8qbo94tJwwGOGT3LwxCT;SslMode=Require"  :: Replace this with your actual connection string
#set CONTEXT_NAME=AppDbContext                       :: Replace with your DbContext name


:: Set paths and variables
set PROJECT_DIR=.
set MIGRATION_NAME=AutoGeneratedMigration
set DB_CONTEXT=AppDbContext
set PIPELINE_OUTPUT_DIR=./Migrations
set DATABASE_LAYER_PROJECT=DatabaseLayer
set STARTUP_PROJECT=TradingAPIs

:: Step 1: Development Environment
cd %PROJECT_DIR%
echo Running Entity Framework updates in Development Environment...

:: Check if there are any pending migrations
echo Checking for pending migrations...
dotnet ef migrations list --project %DATABASE_LAYER_PROJECT% --startup-project %STARTUP_PROJECT% -c %DB_CONTEXT% > nul 2>&1

if %ERRORLEVEL% NEQ 0 (
    echo No migrations found. Adding new migration...
    
    :: Remove any existing migration if necessary (optional)
    if exist "%PROJECT_DIR%\%DATABASE_LAYER_PROJECT%\Migrations\%MIGRATION_NAME%.cs" (
        echo Removing previous migration...
        dotnet ef migrations remove -c %DB_CONTEXT% --project %DATABASE_LAYER_PROJECT% --startup-project %STARTUP_PROJECT% -f
    )
    
    :: Add a new migration
    dotnet ef migrations add %MIGRATION_NAME% -c %DB_CONTEXT% --project %DATABASE_LAYER_PROJECT% --startup-project %STARTUP_PROJECT%

    if %ERRORLEVEL% NEQ 0 (
        echo Failed to add migration. Exiting.
        exit /b 1
    )
) else (
    echo Migrations already applied, skipping migration creation.
)

:: Step 2: Update the Database
echo Applying the migration...
dotnet ef database update -c %DB_CONTEXT% --project %DATABASE_LAYER_PROJECT% --startup-project %STARTUP_PROJECT%

if %ERRORLEVEL% NEQ 0 (
    echo Failed to update the database. Exiting.
    exit /b 1
)

:: Step 3: Git Pipeline Preparation
echo Preparing migrations for Git pipeline...

:: Ensure pipeline output directory exists
if not exist "%PIPELINE_OUTPUT_DIR%" (
    mkdir "%PIPELINE_OUTPUT_DIR%"
)

:: Copy migrations to pipeline directory
xcopy /Y /E "%PROJECT_DIR%\%DATABASE_LAYER_PROJECT%\Migrations" "%PIPELINE_OUTPUT_DIR%"

:: Completion message
echo Entity Framework updates and migrations have been completed.
pause
