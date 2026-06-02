using System;
using System.Threading;
using Microsoft.Win32;
using System.Security.Principal;
class Program
{
    static string subKeyPath = @"SOFTWARE\Microsoft\WindowsUpdate\Orchestrator";
    static string valueName = "ShutdownFlyoutOptions";
    public static class SecurityHelper
    {
        // C# 10 expression-bodied property
        public static bool IsAdministrator =>
            new WindowsPrincipal(WindowsIdentity.GetCurrent())
                .IsInRole(WindowsBuiltInRole.Administrator);
    }

    static void Main()
    {
        if (SecurityHelper.IsAdministrator == false)
        {
            Console.WriteLine("The application is not running as privileged. aborting.");
            Thread.Sleep(3000);
            Environment.Exit(1);
        }
        Console.CancelKeyPress += (sender, e) => {
            Console.WriteLine("\nOperation aborted.");
            // Allow the application to exit cleanly right now
            e.Cancel = false;
        };

        Console.WriteLine("Software by ookkn or omer");
        Console.WriteLine("This will remove Windows updates forcing you to update instead of shutdown");

        for (int i = 5; i > 0; i--)
        {
            Console.WriteLine($"IN {i} SECONDS FLYOUT WILL BE DESTROYED. PRESS CTRL+C TO CANCEL");
            Thread.Sleep(1000); // Wait 1 second
        }

        Console.WriteLine("Starting. Pray that is not patched");
        fuckwindows();
    }
    static void fuckwindows()
    {
        Console.WriteLine("qqq");
        using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
        using (RegistryKey key = baseKey.OpenSubKey(subKeyPath, true))
        {
            if (key != null && key.GetValue(valueName) != null)
            {
                key.SetValue(valueName, 1, RegistryValueKind.DWord);
                Console.WriteLine("Registry value updated successfully!");
            }
            else
            {
                Console.WriteLine("Error!!! Could not find orchestrators FlyoutOptions");
            }
        }
    }
}