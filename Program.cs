using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace MediaCloudUploaderFormApp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, args) => ShowError(args.Exception);
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                ShowError(args.ExceptionObject as Exception);

            bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent())
            .IsInRole(WindowsBuiltInRole.Administrator);

            if (!isAdmin)
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.UseShellExecute = true;
                    startInfo.WorkingDirectory = Environment.CurrentDirectory;
                    startInfo.FileName = Application.ExecutablePath;
                    startInfo.Verb = "runas";

                    Process adminProcess = Process.Start(startInfo);
                    Application.Exit();
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Uygulama yönetici haklarıyla başlatılamadı: " + ex.Message,
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            Application.Run(new MultiCloudUploaderForm());
        }

        private static void ShowError(Exception ex)
        {
            MessageBox.Show($"Kritik hata: {ex?.ToString()}", "Çökme", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(1);
        }
    }

}
