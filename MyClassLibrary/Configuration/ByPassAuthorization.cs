using Microsoft.AspNetCore.Authorization;

namespace MyClassLibrary.Configuration
{
	public class ByPassAuthorization : IAuthorizationHandler
	{
		public Task HandleAsync(AuthorizationHandlerContext context)
		{
			foreach (IAuthorizationRequirement requirement in context.PendingRequirements.ToList())
			{
				context.Succeed(requirement); //passes all requirements
			}
			return Task.CompletedTask;
		}
	}
}
