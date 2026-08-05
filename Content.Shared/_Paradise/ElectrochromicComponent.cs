namespace Content.Shared._Paradise;

[RegisterComponent, AutoGenerateComponentState]
public sealed partial class ElectrochromicComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool IsOpaque = false;

}
