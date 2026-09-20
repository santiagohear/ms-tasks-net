using Application.Users.GetUsers;
using Application.Users.Shared;
using MediatR;

namespace Domain.Test.Handler.GetUsers
{
    public class GetUsersQueryTests
    {
        [Fact]
        public void GetUsersQuery_ShouldImplementIRequestOfUserDtoCollection()
        {
            var query = new GetUsersQuery();

            Assert.IsAssignableFrom<IRequest<IEnumerable<UserDto>>>(query);
        }

        [Fact]
        public void GetUsersQuery_MultipleInstances_ShouldBeEqual()
        {
            var query1 = new GetUsersQuery();
            var query2 = new GetUsersQuery();

            Assert.Equal(query1, query2);
            Assert.Equal(query1.GetHashCode(), query2.GetHashCode());
        }

        [Fact]
        public void GetUsersQuery_ToString_ShouldContainTypeName()
        {
            var query = new GetUsersQuery();

            Assert.Contains("GetUsersQuery", query.ToString());
        }
    }
}
