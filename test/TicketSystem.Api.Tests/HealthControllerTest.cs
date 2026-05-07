using Microsoft.AspNetCore.Mvc;
using TicketSystem.Api.Controllers;

namespace TicketSystem.Api.Tests;

public sealed class HealthControllerTests
{
    [Fact]
    public void Get_ShouldReturnOkResult()
    {
        var controller = new HealthzController();

        var result = controller.Get();

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void Get_ShouldReturnOkStatus()
    {

        var controller = new HealthzController();

        var result = controller.Get();

        var resultAfterAssert = Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(resultAfterAssert.Value);

        var statusProperty = resultAfterAssert.Value.GetType().GetProperty("status");

        Assert.NotNull(statusProperty);

        var statusValue = statusProperty.GetValue(resultAfterAssert.Value);

        Assert.Equal("OK", statusValue);
    }
}
