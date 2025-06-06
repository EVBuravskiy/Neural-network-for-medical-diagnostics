namespace VisualInterface
{
    internal static class Program
    {
        public static SystemController SystemController { get; private set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            SystemController = new SystemController();
            Application.Run(new MainForm());
        }
    }
}