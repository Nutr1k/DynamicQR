using DynamicQR.Authentication.Services;
using DynamicQR.Common;


namespace DynamicQR.Authentication.Endpoints
{
	public class Login : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app)
		{
			app
			   .MapPost("/login", Handle)
			   .WithSummary("Logs in a user")
			   .WithRequestValidation<Request>();
		}

		//Запрос
		public record Request(string Username, string Password);

		//Ответ
		public record Response(string Token);

		//FluentValidation
		public class RequestValidator : AbstractValidator<Request>
		{
			public RequestValidator()
			{
				RuleFor(x => x.Username).NotEmpty();
				RuleFor(x => x.Password).NotEmpty();
			}
		}

		//Тип Results<Ok, UnauthorizedHttpResult> в ASP.NET Core позволяет указать, что метод может возвращать один из двух типов
		private static async Task<Results<Ok<Response>, UnauthorizedHttpResult>> Handle(Request request, DynamicQrContext database,Jwt jwt, CancellationToken cancellationToken)
		{
			var user = await database.Users.SingleOrDefaultAsync(x => x.Username == request.Username && x.Password == request.Password, cancellationToken);

			if (user is null || user.Password != request.Password)
			{
				return TypedResults.Unauthorized();
			}
			
			var token=jwt.GenerateToken(user);
			var response=new Response(token);
			return TypedResults.Ok(response);
		}
	}
}
