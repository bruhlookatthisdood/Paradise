using System.Diagnostics.CodeAnalysis;
using Content.Shared.Access.Components;
using Content.Shared.Access.Systems;
using Content.Shared.Doors.Components;
using Robust.Shared.Containers;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Server.AccessHelper._Paradise
{
    public sealed partial class AccessHelperSystem : EntitySystem
    {
        [Dependency] private SharedMapSystem _mapSystem = default!;
        [Dependency] private SharedContainerSystem _containerSystem = default!;
        [Dependency] private AccessReaderSystem _accessReaderSystem = default!;
        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<AccessHelperComponent, MapInitEvent>(OnMapInit);
        }

        private void OnMapInit(Entity<AccessHelperComponent> entity, ref MapInitEvent args)
        {
            var transform = Transform(entity.Owner);
            var coordinates = transform.Coordinates;
            var gridId = transform.GridUid;

            // Is there an airlock on the same grid as the helper?
            if (!FindAirlock(gridId, coordinates, out var airlock))
            {
                Log.Warning($"Access Helper (Uid {entity.Owner}) was placed on a gridId with no airlock at {Transform(entity.Owner).Coordinates}!");
                QueueDel(entity.Owner);
                return;
            }

            // Does our airlock have an AccessComponent inside it?
            if (!GetAccessComponent(airlock.Value.Owner, out var accessReader))
            {
                Log.Warning($"Access Helper (Uid {entity.Owner}) was placed on top of {airlock.Value.Owner} at {Transform(entity.Owner).Coordinates} which has no AccessReader inside it.");
                QueueDel(entity.Owner);
                return;
            }

            // Is the YML for the helper null?
            if (entity.Comp.Access == null)
            {
                Log.Warning($"Access Helper(Uid {entity.Owner}) access is set to null! Try checking the YML file?");
                QueueDel(entity.Owner);
                return;
            }
            // Attempts to add the access to the accessReader, then queues marker for deletion.
            _accessReaderSystem.TryAddAccess(accessReader.Value, entity.Comp.Access.Value);
            QueueDel(entity.Owner);
        }

        // Do we have an airlock on our grid?
        private bool FindAirlock(EntityUid? gridId, EntityCoordinates coordinates, [NotNullWhen(true)] out Entity<AirlockComponent>? airlock)
        {
            airlock = null;
            // Is it on the grid? If not, it's probably not an airlock.
            if (!TryComp<MapGridComponent>(gridId, out var grid))
                return false;
            // Checks for airlock component, returns true if found and combines airlock the entityUid and component in a tuple.
            foreach (var entityUid in _mapSystem.GetLocal(gridId.Value, grid, coordinates))
            {
                if (TryComp<AirlockComponent>(entityUid, out var airlockComp))
                {
                    airlock = (entityUid, airlockComp);
                    return true;
                }
            }
            return false;
        }

        // Get the access component from inside the door
        private bool GetAccessComponent(EntityUid airlockUid, [NotNullWhen(true)] out Entity<AccessReaderComponent>? accessReader)
        {
            accessReader = null;
            // Checks if the airlock has a container, returns true if found
            if (!_containerSystem.TryGetContainer(airlockUid, "board", out var container))
                return false;
            /* Searches every entityUid in the airlock's container. When it finds an entity with the AccessReaderComponent, it stops
               and combines the entityUid and component in a tuple.*/
            foreach (var entityUid in container.ContainedEntities)
            {
                if (TryComp<AccessReaderComponent>(entityUid, out var reader))
                {
                    accessReader = (entityUid, reader);
                    return true;
                }
            }

            return false;
        }
    }
}
