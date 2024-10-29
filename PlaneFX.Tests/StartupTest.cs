namespace PlaneFX.Tests
{
    public class StartupTest(ApiWebApplicationFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task MakeSU_ClearConfig_ThrowNull()
        {
            configuration[StartupService.TG_ID] = null;
            configuration[StartupService.TG_USERNAME] = null;
            configuration[StartupService.TIME_ZONE] = null;
            configuration[StartupService.TG_API_TOKEN] = null;

            await Assert.ThrowsAsync<NullReferenceException>(startup.MakeSU);
        }

        [Fact]
        public async Task MakeSU_ConfigFillRight_Void()
        {
            configuration[StartupService.TG_ID] = "123456789";
            configuration[StartupService.TG_USERNAME] = "cherkashh";
            configuration[StartupService.TIME_ZONE] = "5";
            configuration[StartupService.TG_API_TOKEN] = "ghfjkdy57494hf";

            await startup.MakeSU();
        }

        [Fact]
        public async Task MakeSU_ConfigFillBad_ThrowFormat()
        {
            configuration[StartupService.TG_ID] = "test";
            configuration[StartupService.TG_USERNAME] = "cherkashh";
            configuration[StartupService.TIME_ZONE] = "test";
            configuration[StartupService.TG_API_TOKEN] = "ghfjkdy57494hf";

            await Assert.ThrowsAsync<FormatException>(startup.MakeSU);
        }

        [Theory]
        [InlineData("")]
        [InlineData("test")]
        public async Task MakeSU_BadTgId_ThrowFormat(string value)
        {
            configuration[StartupService.TG_ID] = value;
            configuration[StartupService.TG_USERNAME] = "cherkashh";
            configuration[StartupService.TIME_ZONE] = "5";
            configuration[StartupService.TG_API_TOKEN] = "ghfjkdy57494hf";

            await Assert.ThrowsAsync<FormatException>(startup.MakeSU);
        }

        [Theory]
        [InlineData("")]
        [InlineData("test")]
        public async Task MakeSU_BadTimeZone_ThrowFormat(string value)
        {
            configuration[StartupService.TG_ID] = "123456789";
            configuration[StartupService.TG_USERNAME] = "cherkashh";
            configuration[StartupService.TIME_ZONE] = value;
            configuration[StartupService.TG_API_TOKEN] = "ghfjkdy57494hf";

            await Assert.ThrowsAsync<FormatException>(startup.MakeSU);
        }
    }
}