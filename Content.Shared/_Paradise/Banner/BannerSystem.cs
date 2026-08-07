using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Tools.Components;
using Robust.Shared.Spawners;
using Robust.Shared.Timing;

namespace Content.Shared._Paradise.Banner;

public sealed partial class BannerSystem : EntitySystem
{
    [Dependency] private SharedAppearanceSystem _appearanceSystem = default!;
    [Dependency] private IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<BannerComponent, UseInHandEvent>(OnUseInHand);
        SubscribeLocalEvent<BannerComponent, InteractUsingEvent>(OnInteractUsing);
    }

    private void OnUseInHand(EntityUid uid, BannerComponent component, UseInHandEvent args)
    {
        // Makes sure we only run this once when we use in hand
        if (!_timing.IsFirstTimePredicted)
            return;

        // Basically saying set us to the opposite state of whatever we are now, then set new state as our visual
        component.State = component.State == BannerVisualsState.Rolled ? BannerVisualsState.Unrolled : BannerVisualsState.Rolled;
        _appearanceSystem.SetData(uid, BannerVisuals.State, component.State);
        args.Handled = true;
    }

    private void OnInteractUsing(EntityUid uid, BannerComponent component, InteractUsingEvent args)
    {
        // Are we an object with the WelderComponent, if so are we enabled?
        if (!TryComp<WelderComponent>(args.Used, out var weldcomp)|| !weldcomp.Enabled)
            return;

        // Burn baby burn (Set our visual to burning)
        component.Burning = true;
        _appearanceSystem.SetData(uid, BannerVisuals.Burning, component.Burning);

        Timer.Spawn(TimeSpan.FromSeconds(6), () =>
        {
            // Did we get destroyed since we were set on fire?
            if (!Exists(uid))
                return;

            // Grab our coordinates
            var xform = Transform(uid);
            var coordinates = xform.Coordinates;

            // Spawn ash and destroy our banner. Rest in peace.
            Spawn("Ash", coordinates);
            QueueDel(uid);
        });


        args.Handled = true;
    }
}
