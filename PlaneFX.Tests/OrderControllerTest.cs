namespace PlaneFX.Tests
{
    public class OrderControllerTest(ApiWebApplicationFactory factory) : BaseControllerTest(factory)
    {
        [Fact]
        public async Task GET_OrderById_Ok_NotNull()
        {
            var response = await client.GetAsync("/api/Order/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(await response.Content.ReadFromJsonAsync<OrderResponse>());
        }
    }
}