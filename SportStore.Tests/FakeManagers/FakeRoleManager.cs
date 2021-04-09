using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;

namespace SportStore.Tests.FakeManagers
{
    public class FakeRoleManager : RoleManager<IdentityRole<int>>
    {
        public FakeRoleManager() : base(new Mock<IRoleStore<IdentityRole<int>>>().Object,
                                        new List<IRoleValidator<IdentityRole<int>>> { new Mock<IRoleValidator<IdentityRole<int>>>().Object },
                                        new Mock<ILookupNormalizer>().Object,
                                        new Mock<IdentityErrorDescriber>().Object,
                                        new Mock<ILogger<RoleManager<IdentityRole<int>>>>().Object)
        { }
    }
}
