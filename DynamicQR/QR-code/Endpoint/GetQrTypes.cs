using DynamicQR.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;
using System;
using DynamicQR.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DynamicQR.QR_code
{
	public class GetQrTypes : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app)
		{
			app
			.MapPost("/types", Handle)
			.WithSummary("Get all QR type");

		}

		public record Response(int Id,string type);
		
		private static async Task<Results<Ok<Response>, NotFound>> Handle(DynamicQrContext database, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
		{
			var post= await database.TypeQrs
				.Select(t => new Response
				(
					t.Id,
					t.Type
				))
				.SingleOrDefaultAsync(cancellationToken);
			
			return post is null
				? TypedResults.NotFound()
				: TypedResults.Ok(post);
		}
	}
}
