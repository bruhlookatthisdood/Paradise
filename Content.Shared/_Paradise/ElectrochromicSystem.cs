namespace Content.Shared._Paradise;

public sealed class ElectrochromicSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ElectrochromicComponent, ComponentInit>(OnInit);
    }

    private void OnInit(EntityUid uid, ElectrochromicComponent component, ComponentInit args)
    {

    }
}
