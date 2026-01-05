using DynamicQR.Authentication.Services;
using DynamicQR.Common;

namespace DynamicQR.Authentication.Endpoints
{
	public class Signup : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app)
		{
			app
				.MapPost("/signup", Handle)
				.WithSummary("Creates new user account")
				.WithRequestValidation<Request>();
		}

		public record Request(string Username, string Password);
		public record Response(string Token);

		public class RequestValidator : AbstractValidator<Request>
		{
			public RequestValidator()
			{
				RuleFor(x => x.Username).NotEmpty();
				RuleFor(x => x.Password).NotEmpty();
			}
		}

		public static async Task<Results<Ok<Response>, ValidationError>> Handle(Request request, DynamicQrContext database, Jwt jwt, CancellationToken cancellationToken)
		{
			var isUsernameTaken = await database.Users
				.AnyAsync(x => x.Username == request.Username, cancellationToken);

			if (isUsernameTaken)
			{
				return new ValidationError("Username is already taken");
			}

			var user = new User
			{
				Username = request.Username,
				Password = request.Password,
				Role=UserRole.User.ToString()
				
			};

			await database.Users.AddAsync(user, cancellationToken);
			await database.SaveChangesAsync(cancellationToken);

			var token = jwt.GenerateToken(user);
			var response = new Response(token);

			return TypedResults.Ok(response);
		}
	}
}
