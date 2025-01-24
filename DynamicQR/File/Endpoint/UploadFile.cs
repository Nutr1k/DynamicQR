using DynamicQR.Authentication.Services;
using DynamicQR.Common;
using DynamicQR.Common.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;

namespace DynamicQR.File.Endpoint
{
    public class UploadFile : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapPost("/UploadFile", Handle)
                .WithRequestValidation<Request>()
				.DisableAntiforgery();

		}

        public record Request(IFormFile file);

		public class RequestValidator : AbstractValidator<Request>
		{
			public RequestValidator()
			{
                RuleFor(x => x.file.Length).GreaterThan(0).LessThan(10*1024*1024);
			}
		}

		public static async Task<Ok> Handle([FromForm] Request request, DynamicQrContext database,ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            using (var ms = new MemoryStream())
            {
                await request.file.CopyToAsync(ms,cancellationToken);
                var fileBytes = ms.ToArray();

				var file = new FileQr
				{
					File = fileBytes,
					UserId = claimsPrincipal.GetUserId()
				};
				await database.FileQrs.AddAsync(file,cancellationToken);
			}
            await database.SaveChangesAsync(cancellationToken);
            return TypedResults.Ok();
        }
        
    }
}
