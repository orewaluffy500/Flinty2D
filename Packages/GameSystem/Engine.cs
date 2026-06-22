
using Flinty.Assets;
using Flinty.GameMath;
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
        public Color BackgroundColor { set; get; }

        public EngineRenderer Renderer { get; }
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

            // Max FPS
            // Raylib.SetTargetFPS(60);

            // Initialize systems
            Renderer = new(this);
            Terrain = new(this);
            Clock = new();

            ModEngine = new(this);
            ModEngine.InitializeSystem();
            ModEngine.InitializeModules();

            // Block registring
            BlockRegistry.RegisterNew("soil", "Textures/dirt.png", Color.Brown);
            BlockRegistry.RegisterNew("rock", "Textures/rock.png", new(80, 80, 80));

            BlockRegistry.SetActive("soil", true);

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


            ModEngine.Callback_Start();
        }

        public void PostGameLoop()
        {
            ModEngine.Callback_End();
            Terrain.Final();
            TextureRegistry.UnloadAll();

            Raylib.CloseWindow();
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