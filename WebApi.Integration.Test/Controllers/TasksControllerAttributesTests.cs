using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;

namespace WebApi.Integration.Test.Controllers
{
    public class TasksControllerAttributesTests
    {
        [Fact]
        public void Controller_ShouldHaveApiControllerAndRouteAttributes()
        {
            var type = typeof(TasksController);

            Assert.NotNull(type.GetCustomAttribute<ApiControllerAttribute>());
            var route = type.GetCustomAttribute<RouteAttribute>();
            Assert.NotNull(route);
            Assert.Equal("api/tasks", route!.Template);
        }

        [Fact]
        public void CreateAsync_ShouldHaveHttpPostAndResponseAttributes()
        {
            var method = typeof(TasksController).GetMethod("CreateAsync");

            Assert.NotNull(method);
            Assert.NotNull(method!.GetCustomAttribute<HttpPostAttribute>());
            var responseCodes = method.GetCustomAttributes<ProducesResponseTypeAttribute>().Select(x => x.StatusCode).ToList();
            Assert.Contains(StatusCodes.Status201Created, responseCodes);
            Assert.Contains(StatusCodes.Status422UnprocessableEntity, responseCodes);

            var parameter = method.GetParameters().Single();
            Assert.Contains(parameter.GetCustomAttributes(), attr => attr.GetType().Name == "ValidateAttribute");
        }

        [Fact]
        public void GetAsync_ShouldHaveHttpGetAndProducesResponseType()
        {
            var method = typeof(TasksController).GetMethod("GetAsync");

            Assert.NotNull(method);
            Assert.NotNull(method!.GetCustomAttribute<HttpGetAttribute>());
            var response = method.GetCustomAttributes<ProducesResponseTypeAttribute>().Single();
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Fact]
        public void UpdateStatusAsync_ShouldHaveHttpPutRouteAndValidation()
        {
            var method = typeof(TasksController).GetMethod("UpdateStatusAsync");

            Assert.NotNull(method);
            var httpPut = method!.GetCustomAttribute<HttpPutAttribute>();
            Assert.NotNull(httpPut);
            Assert.Equal("{id:long}/status", httpPut!.Template);

            var responseCodes = method.GetCustomAttributes<ProducesResponseTypeAttribute>().Select(x => x.StatusCode).ToList();
            Assert.Contains(StatusCodes.Status200OK, responseCodes);
            Assert.Contains(StatusCodes.Status422UnprocessableEntity, responseCodes);

            var requestParameter = method.GetParameters().Single(x => x.Name == "request");
            Assert.Contains(requestParameter.GetCustomAttributes(), attr => attr.GetType().Name == "ValidateAttribute");
        }
    }
}
