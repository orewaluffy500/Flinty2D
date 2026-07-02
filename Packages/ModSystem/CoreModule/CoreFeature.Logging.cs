namespace Flinty.ModSystem;


public class CoreLoggingModule : ICoreFeature
{
    public CoreLoggingModule(CoreHelperModule coreHelperModule) : base(coreHelperModule)
    {
    }

    public override void Create()
    {
        string id = $"{ModEngine.GAME_CORE_MASTER_MODULE_NAME}.logging";

        Engine.Lua.DoString(@$"
            {id} = {{}}

            function {id}.info(msg)
                NATIVE.out.info(msg)
            end
            
            function {id}.error(label, msg)
                NATIVE.out.error(label, msg)
            end

            function {id}.warn(label, msg)
                NATIVE.out.warning(label, msg)
            end
        ", "create_logger_methods");   
    }
}