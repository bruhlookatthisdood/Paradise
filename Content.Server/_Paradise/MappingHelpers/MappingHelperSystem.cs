using System.Linq;
using Robust.Shared.Map.Components;

namespace Content.Server._Paradise.MappingHelpers;

public sealed partial class MappingHelperSystem : EntitySystem
{
    [Dependency] private SharedMapSystem _mapSystem = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<MappingHelperComponent, MapInitEvent>(GetLocalEntities);
    }

    public IEnumerable<EntityUid> GetLocalEntities(EntityUid uid, MappingHelperComponent component, MapInitEvent args)
    {
        var transform = Transform(uid);
        var gridId = transform.GridUid;
        var coordinates = transform.Coordinates;

        // Is it on the grid? If not, it's probably not what we're looking for.
        if (!TryComp<MapGridComponent>(gridId, out var grid))
            return Enumerable.Empty<EntityUid>();

        return _mapSystem.GetLocal(uid, grid, coordinates);


    }
}
