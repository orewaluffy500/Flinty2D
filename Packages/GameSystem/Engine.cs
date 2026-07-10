
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


        public static readonly Color BACKGROUND_CLASSIC = new(20, 20, 40);
        public static readonly Color BACKGROUND_COSMIC = new(16, 10, 28);
        public static readonly Color BACKGROUND_NATURAL = new(10, 31, 20);


        public Engine(string _caption, int _w, int _h)
        {
            // Initialize window preferences
            Width = _w;
            Height = _h;
            Caption = _caption;
            BackgroundColor = BACKGROUND_NATURAL;


            // Initialize window
            Raylib.SetTraceLogLevel(TraceLogLevel.Error);

            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
            Raylib.InitWindow(Width, Height, Caption);


            GameLogger.InfoLog("Game", $"Initialized Window: {_caption} {_w}x{_h}");

            // Max FPS
            Raylib.SetTargetFPS(MaximumFPS);
            GameLogger.InfoLog("Game", $"Set maximum FPS to {MaximumFPS}");

            Terrain = new(this);
            GameLogger.InfoLog("Game", "Initialized Terrain");

            Clock = new(this);
            GameLogger.InfoLog("Game", "Initialized Clock");

            ModEngine = new(this);

            InitializeAll();
        }

        private void InitializeAll()
        {
            // Initialize systems
            EngineRenderer.Init();
            GameLogger.InfoLog("Game", "Initialized Renderer");

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
           
            FireGameEvent("start");
        }

        public void PostGameLoop()
        {
            FireGameEvent("end");
            Terrain.Final();
            TextureRegistry.UnloadAll();

            Raylib.CloseWindow();
            GameLogger.InfoLog("Game", "Exiting...");
        }

        public void Update(float deltaTime)
        {
            Clock.Update(deltaTime);

            Terrain.Update(deltaTime);
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










        public List<object> FireGameEvent(string name, params object[] args)
        {
            return ModEngine.FireCallback($"{EventCategories.GLOBAL_GAME_EVENTS}.{name}", args);
        }

        public List<object> FirePlayerEvent(string name, params object[] args)
        {
            return ModEngine.FireCallback($"{EventCategories.PLAYER_EVENTS}.{name}", args);
        }

        public List<object> FireClockEvent(string name, params object[] args)
        {
            return ModEngine.FireCallback($"{EventCategories.CLOCK_EVENTS}.{name}", args);
        }
        
        public List<object> FireBlockEvent(string event_name, string block_type, params object[] args)
        {
            var results_for_all = ModEngine.FireCallback($"{EventCategories.BLOCK_EVENTS}.{EventCategories.ALL_BLOCKS}.{block_type}", args);
            var results_for_exclusive = ModEngine.FireCallback($"{EventCategories.BLOCK_EVENTS}.{event_name}.{block_type}", args);

            return new(results_for_all.Concat(results_for_exclusive)); // Combine both results
        }
        
    }
}