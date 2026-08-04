using System.Linq;
using Robust.Shared.Map.Components;

namespace Content.Server._Paradise.MappingHelpers;

public sealed partial class MappingHelperSystem : EntitySystem
{
    [Dependency] private SharedMapSystem _mapSystem = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<MappingHelperComponent, MapInitEvent>(OnMappingHelperInit);
    }


    private void OnMappingHelperInit(EntityUid uid, MappingHelperComponent component, MapInitEvent args)
    {
        var transform = Transform(uid);
        var gridId = transform.GridUid;
        var coordinates = transform.Coordinates;

        // Is it on the grid? If not, it's probably not what we're looking for.
        if (!TryComp<MapGridComponent>(gridId, out var grid))
        {
            Log.Warning("Failure at finding grid");
            return;
        }

        var localEntities = _mapSystem.GetLocal(uid, grid, coordinates);
        var amhEvent = new ApplyMappingHelperEvent(localEntities);

        Log.Warning("OnMappingHelperInit finished");
        RaiseLocalEvent(uid, ref amhEvent);
    }
}

[ByRefEvent]
public readonly record struct ApplyMappingHelperEvent(IEnumerable<EntityUid> LocalEntities);
