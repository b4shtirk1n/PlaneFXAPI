namespace PlaneFX.Tests.Abstractions
{
    public abstract class BaseControllerTest : BaseIntegrationTest
    {
        protected readonly HttpClient client;

        public BaseControllerTest(ApiWebApplicationFactory factory) : base(factory)
        {
            DbContext.Database.EnsureCreated();
            client = factory.CreateClient();
        }

        public new void Dispose()
        {
            client.Dispose();
            base.Dispose();
        }
    }
}