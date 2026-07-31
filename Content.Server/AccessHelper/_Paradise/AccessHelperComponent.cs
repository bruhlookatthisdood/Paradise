using Robust.Shared.Prototypes;
using Content.Shared.Access;

namespace Content.Server.AccessHelper._Paradise
{
    /// <summary>
    /// Used by mapping access helpers
    /// </summary>
    [RegisterComponent, EntityCategory("Mapping")]
    public sealed partial class AccessHelperComponent : Component
    {
        [DataField("access")]
        public ProtoId<AccessLevelPrototype>? Access { get; set; }
    }
}
