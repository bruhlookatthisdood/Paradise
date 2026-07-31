namespace Content.Server.AccessHelper._Paradise
{
    public sealed class AccessHelperSystem : EntitySystem
    {
        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<AccessHelperComponent, MapInitEvent>(OnMapInit);
        }
    }
}
