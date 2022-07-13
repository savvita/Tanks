using Client.Controller;
using Client.Model;
using Client.View;

namespace Client
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            //TankController controller = new TankController(new SpriteImageModel(
            //    Properties.Resources.Tank,
            //    new ImageBounds(0, 564, 350, 258), 
            //    new ImageBounds(0, 354, 350, 210), 
            //    new ImageBounds(0, 0, 211, 350), 
            //    new ImageBounds(211, 0, 211, 350)));

            FieldController controller = new FieldController();


            Application.Run(new GameForm(controller));
        }
    }
}