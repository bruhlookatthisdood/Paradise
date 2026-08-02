using System.Diagnostics.CodeAnalysis;
using Content.Shared.Doors.Components;
using Content.Shared.Doors.Systems;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Server._Paradise.DoorBoltHelper;

public sealed partial class DoorBoltHelperSystem : EntitySystem
{
    [Dependency] private SharedDoorSystem _doorSystem = default!;
    [Dependency] private SharedMapSystem _mapSystem = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DoorBoltHelperComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(EntityUid uid, DoorBoltHelperComponent component, MapInitEvent args)
    {
        var transform = Transform(uid);
        var coordinates = transform.Coordinates;
        var gridId = transform.GridUid;

        if (!FindBoltableDoor(gridId, coordinates, out var door))
        {
            Log.Warning($"Door Bolt Helper ({uid}) was unable to find boltable door at {coordinates}.");
            QueueDel(uid);
            return;
        }

        _doorSystem.ForceSetBoltsDown(door.Value);
        QueueDel(uid);
    }

    private bool FindBoltableDoor(EntityUid? gridId,
        EntityCoordinates coordinates,
        [NotNullWhen(true)] out Entity<DoorBoltComponent>? door)
    {
        door = null;

        if (!TryComp<MapGridComponent>(gridId, out var grid))
            return false;

        foreach (var entityUid in _mapSystem.GetLocal(gridId.Value, grid, coordinates))
        {
            if (!TryComp<DoorBoltComponent>(entityUid, out var doorBoltComponent))
                continue;

            door = (entityUid, doorBoltComponent);

            return true;
        }

        return false;
    }
}
