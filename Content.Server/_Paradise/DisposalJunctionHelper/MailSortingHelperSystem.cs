using Content.Server._Paradise.MailSortingHelper;

namespace Content.Server._Paradise.DisposalJunctionHelper;

public sealed class ExampleSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MailSortingHelperComponent, ComponentInit>(OnInit);
    }

    private void OnInit(EntityUid uid, MailSortingHelperComponent component, ComponentInit args)
    {
        // runs when the component is first added to an entity
    }
}
