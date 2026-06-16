
using Flinty.Assets;
using Flinty.GameMath;
using Flinty.Globals;
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

        public float TickTimer { get; private set; } = 0;

        public int TickIndex { get; private set; } = 0;
        public bool Ticking { get; private set; } = false;

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
            Raylib.SetTargetFPS(60);

            // Initialize systems
            Renderer = new(this);
            Terrain = new(this);

            // Block registring
            BlockRegistry.RegisterNew("soil", "Textures/dirt.png", Color.Brown);
            BlockRegistry.RegisterNew("rock", "Textures/rock.png", new(80, 80, 80));

        }

        public void PreGameLoop()
        {
            Raylib.SetExitKey(KeyboardKey.Null);
            Terrain.Once();
        }

        public void PostGameLoop()
        {
            Terrain.Final();
            TextureRegistry.UnloadAll();

            Raylib.CloseWindow();
        }

        public void Update(float deltaTime)
        {
            TickTimer += deltaTime;
            Ticking = false;

            if (TickTimer > (1f / Preferences.TICK_RATE))
            {
                TickTimer = 0;
                TickIndex++;
                Ticking = true;
                if (TickIndex > Preferences.TICK_RATE) TickIndex = 0;

                Tick();
            }

            Terrain.Update(deltaTime);
        }

        private void Tick()
        {
            
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