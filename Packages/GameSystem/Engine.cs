
using Flinty.Assets;
using Flinty.Globals;
using Flinty.ModSystem;
using Flinty.World;
using Raylib_cs;

namespace Flinty.GameSystem
{

    public class Engine
    {
        public int Width { get; }
        public int Height { get; }
        public string Caption { get; }

        public int MaximumFPS { get; } = 60;

        public Color BackgroundColor { set; get; }

        public Terrain Terrain { get; }

        public Clock Clock { get; }

        public ModEngine ModEngine { get; }



        public Engine(string _caption, int _w, int _h)
        {
            // Initialize window preferences
            Width = _w;
            Height = _h;
            Caption = _caption;
            BackgroundColor = new(20, 20, 40);


            // Initialize window
            Raylib.SetTraceLogLevel(TraceLogLevel.Error);

            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
            Raylib.InitWindow(Width, Height, Caption);

            
            GameLogger.InfoLog("Game", $"Initialized Window: {_caption} {_w}x{_h}");

            // Max FPS
            Raylib.SetTargetFPS(MaximumFPS);
            GameLogger.InfoLog("Game", $"Set maximum FPS to {MaximumFPS}");

            // Initialize systems
            EngineRenderer.Init();
            GameLogger.InfoLog("Game", "Initialized Renderer");

            Terrain = new(this);
            GameLogger.InfoLog("Game", "Initialized Terrain");

            Clock = new();
            GameLogger.InfoLog("Game", "Initialized Clock");

            ModEngine = new(this);
            ModEngine.InitializeSystem();
            ModEngine.InitializeModules();
            GameLogger.InfoLog("Game", "Initialize Mod engine.");

            // Block registring
            BlockRegistry.RegisterNew("soil", "Textures/dirt.png", Color.Brown);
            BlockRegistry.RegisterNew("rock", "Textures/rock.png", new(80, 80, 80));

            // Load Scripts
            
            string modFolder = "data/Scripts";

            if (!Directory.Exists(modFolder)) Directory.CreateDirectory(modFolder);

            foreach (string file in Directory.GetFiles(modFolder))
            {
                ModEngine.LoadScript(file);
            }

        }

        public void PreGameLoop()
        {
            Raylib.SetExitKey(KeyboardKey.Null);
            Terrain.Once();

            ModEngine.RunQueuedMods();
            ModEngine.Callback_Start();
        }

        public void PostGameLoop()
        {
            ModEngine.Callback_End();
            Terrain.Final();
            TextureRegistry.UnloadAll();

            Raylib.CloseWindow();
            GameLogger.InfoLog("Game", "Exiting...");
        }

        public void Update(float deltaTime)
        {
            Clock.Update(deltaTime);

            Terrain.Update(deltaTime);

            if (Clock.IsTicking)
            {
                ModEngine.Callback_Tick();
            }
        }

        public void Draw()
        {
            Terrain.Draw();
        }

        public void Start()
        {
            PreGameLoop();

            while (!Raylib.WindowShouldClose())
            {   
                // Update
                Update(Raylib.GetFrameTime());


                // Begin frame
                Raylib.BeginDrawing();
                Raylib.ClearBackground(BackgroundColor);

                Draw();

                Raylib.EndDrawing();
            }

            PostGameLoop();
        }
    }

}