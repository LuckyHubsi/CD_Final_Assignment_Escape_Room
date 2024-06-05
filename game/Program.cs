using libs;

class Program
{    
    static void Main(string[] args)
    {
        //Jump point for the goto in the win/lose
        Start:
        if (Console.WindowWidth < 40 || Console.WindowHeight < 12)
        {
            Console.WriteLine("Please resize the console window to at least 40x12 and run the application again.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }

        //Setup
        Console.CursorVisible = false;
        var engine = GameEngine.Instance;
        var inputHandler = InputHandler.Instance;

         // Define the frame rate (e.g., 60 FPS)
        int frameRate = 60;
        TimeSpan frameInterval = TimeSpan.FromSeconds(1.0 / frameRate);
        while (true) {

            engine.Setup();

            
            // Main game loop
            while (true)
            {
                DateTime startTime = DateTime.Now;

                engine.Render();


                if(engine.WinCheck()) {
                    engine.WinLevel();
                    Console.ReadKey(true);
                    goto Start;
                }
                if(engine.LoseCheck()) {
                    engine.LoseLevel();
                    Console.ReadKey(true);
                    goto Start;
                }

                // Wait until the end of the frame
                TimeSpan elapsedTime = DateTime.Now - startTime;
                TimeSpan sleepTime = frameInterval - elapsedTime;
                if (sleepTime > TimeSpan.Zero)
                {
                    Thread.Sleep(sleepTime);
                }
                //Handle keyboard input
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    inputHandler.Handle(keyInfo);
                }
            }
        }
    }
}