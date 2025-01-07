using DynamicQR.Common;
using DynamicQR.Common.Api.Extension;
using DynamicQR.Data;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Metadata;

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

		private static async Task<Results<Ok<Response>, UnauthorizedHttpResult>> Handle(Request request, DynamicQrContext database/**, Jwt jwt**/, CancellationToken cancellationToken)
		{
			var user = await database.Users.SingleOrDefaultAsync(x => x.Username == request.Username && x.Password == request.Password, cancellationToken);

			if (user is null || user.Password != request.Password)
			{
				return TypedResults.Unauthorized();
			}

			//var token = jwt.GenerateToken(user);
			var response = new Response("test"/*token*/);
			return TypedResults.Ok(response);
		}
	}
}
