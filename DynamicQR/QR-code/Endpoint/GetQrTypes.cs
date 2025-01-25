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
			.MapGet("/types", Handle)
			.WithSummary("Get all QR type");

		}

		public record Response(int Id,string type);
		
		private static async Task<Results<Ok<List<Response>>, NotFound>> Handle(DynamicQrContext database, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
		{
			var types= await database.TypeQrs
				.Select(t => new Response
				(
					t.Id,
					t.Type
				))
				.ToListAsync(cancellationToken);

			return types is null
				? TypedResults.NotFound()
				: TypedResults.Ok(types);
		}
	}
}
