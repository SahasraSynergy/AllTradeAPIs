using System;
using System.Diagnostics;
using System;
using System.Diagnostics;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
namespace DatabaseLayer
{
    public class ShellScriptExecutor
    {
        static string scriptPath = 
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "apply-migrations.bat" : "apply-migrations.sh";

        // Construct the script for both platforms
       static string windowsScript = "cd .. && cd .\\DatabaseLayer && " + scriptPath;  // For Windows (using && to run multiple commands)
       static string linuxScript = "cd .. && cd ./DatabaseLayer && ./" + scriptPath;   // For Linux/MacOS (using && to run multiple commands)

        // Determine the correct script to use based on the platform
       static string command = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? windowsScript : linuxScript;

        static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        static bool IsLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        static bool IsMac() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        public ShellScriptExecutor() {
            RunBatchScript();
            if (IsWindows())
            {
                Console.WriteLine("Running on Windows - Executing batch file...");
                //RunBatchScript();
            }
            else if (IsLinux() || IsMac())
            {
                Console.WriteLine("Running on Linux/Mac - Executing shell script...");
               // RunShellScript();
            }
            else
            {
                Console.WriteLine("Unsupported OS");
            }
        }
        static void RunBatchScript()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = IsWindows() ? "cmd.exe" : "/bin/bash",
                    Arguments = IsWindows() ? $"/C {command}" : $"-c \"{command}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                using (Process process = Process.Start(startInfo))
                {
                    if (process != null)
                    {
                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();
                        Console.WriteLine(output);

                        if (!string.IsNullOrWhiteSpace(error))
                        {
                            Console.WriteLine($"Error: {error}");
                        }

                        // Wait for the process to exit with a timeout
                        if (!process.WaitForExit(10000)) // Wait 10 seconds
                        {
                            Console.WriteLine("Process timed out. Killing the process.");
                            process.Kill();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing batch file: {ex.Message}");
            }
        }

        // Method to execute a Windows batch file
        //static void RunBatchScript()
        //{
        //    try
        //    {

        //        ProcessStartInfo startInfo = new ProcessStartInfo
        //        {
        //            FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "cmd.exe" : "/bin/bash", // Use cmd.exe for Windows and bash for Linux/MacOS
        //            Arguments = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? $"/C {command}" : $"-c \"{command}\"", // Run the command in Windows or Linux
        //            RedirectStandardOutput = true,
        //            RedirectStandardError = true,
        //            UseShellExecute = false,
        //            CreateNoWindow = true,
        //        };

        //        try
        //        {
        //            // Start the process to run the script
        //            // Execute the process
        //            using (Process process = Process.Start(startInfo))
        //            {
        //                if (process != null)
        //                {
        //                    string output = process.StandardOutput.ReadToEnd();
        //                    Console.WriteLine(output);
        //                    process.WaitForExit();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("An error occurred: " + ex.Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error executing batch file: " + ex.Message);
        //    }
        //}

    }

}