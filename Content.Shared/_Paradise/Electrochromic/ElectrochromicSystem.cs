using Content.Shared.DeviceLinking;
using Content.Shared.DeviceLinking.Events;
using Content.Shared.DeviceNetwork.Events;

namespace Content.Shared._Paradise.Electrochromic;

public sealed partial class ElectrochromicSystem : EntitySystem
{
    [Dependency] private OccluderSystem _occluderSystem = default!;
    [Dependency] private SharedDeviceLinkSystem _deviceLinkSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ElectrochromicComponent, SignalReceivedEvent>(OnSignalReceived);
    }

    private void OnSignalReceived(Entity<ElectrochromicComponent> entity, ref SignalReceivedEvent args)
    {
        Log.Warning("SignalRecieved!!!!!!!!");

        if(!TryComp<OccluderComponent>(entity, out var occluder))
            return;

        // Flip our IsOpaque value
        entity.Comp.IsOpaque = !entity.Comp.IsOpaque;
        _occluderSystem.SetEnabled(entity.Owner, entity.Comp.IsOpaque, occluder);
        Dirty(entity);
    }


}
