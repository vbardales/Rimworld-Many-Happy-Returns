using RimWorks.Pickle;

namespace ManyHappyReturns.PickleSteps
{
    /// <summary>
    /// Picks scenario roles from the fixture's own eligible colonists, by position rather than by
    /// name: the exact roster of whichever save is staged is not something this suite controls, and
    /// a name that happens not to be on the map would fail every scenario with the same unhelpful
    /// "no pawn named ..." rather than telling the truth about what is missing.
    /// </summary>
    [PickleSteps]
    public class RoleSteps
    {
        [Given("the celebrant is the map's first eligible colonist")]
        public void PickCelebrant(PickleContext ctx) =>
            ctx.Set(new Driver.CelebrantRole { NameKey = Driver.NameKeyOf(Driver.Colonists(ctx, 1)[0]) });

        [Given("the wisher is the map's second eligible colonist")]
        public void PickWisher(PickleContext ctx) =>
            ctx.Set(new Driver.WisherRole { NameKey = Driver.NameKeyOf(Driver.Colonists(ctx, 2)[1]) });

        /// <summary>
        /// Only used where a scenario needs the population count itself, not a specific pawn: the
        /// tenure and psychopath exemptions below need nobody named, just enough colonists present.
        /// </summary>
        [Given("at least a third eligible colonist is present")]
        public void RequireThird(PickleContext ctx) => Driver.Colonists(ctx, 3);
    }
}
