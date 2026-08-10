using Content.Shared.DeviceLinking;
using Robust.Shared.Prototypes;

namespace Content.Shared._Paradise.Electrochromic;

[RegisterComponent, AutoGenerateComponentState]
public sealed partial class ElectrochromicComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool IsOpaque = false;
}
