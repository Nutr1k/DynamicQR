using Chirper.Common.Api.Filters;
using Chirper.Data.Types;
using DynamicQR.Common.Api.Filters;
using DynamicQR.Data;
using System;

namespace DynamicQR.Common.Api.Extensions
{
	public static class RouteHandlerBuilderValidationExtensions
	{
		public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
		{
			return builder
				.AddEndpointFilter<RequestValidationFilter<TRequest>>()//Добавляем валидацию данных
				.ProducesValidationProblem();
		}

		/// <summary>
		/// Добавляет фильтр проверки запроса в обработчик маршрута, чтобы гарантировать, что текущий <seealso cref="ClaimsPrincipal"/> владеет <typeparamref name="TEntity"/> с идентификатором, возвращаемым <paramref name="idSelector"/>.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TRequest"></typeparam>
		/// <param name="builder"></param>
		/// <param name="idSelector">Функция, которая выбирает свойство <c>Id</c> из <typeparamref name="TRequest"/></param>.
		/// <returns><see cref="RouteHandlerBuilder"/>, который можно использовать для дальнейшей настройки конечной точки.</returns>
		public static RouteHandlerBuilder WithEnsureUserOwnsEntity<TEntity, TRequest>(this RouteHandlerBuilder builder, Func<TRequest, int> idSelector) where TEntity : class, IEntity, IOwnedEntity
		{
			return builder
				.AddEndpointFilterFactory((endpointFilterFactoryContext, next) => async context =>
				{
					var db = context.HttpContext.RequestServices.GetRequiredService<DynamicQrContext>();
					var filter = new EnsureUserOwnsEntityFilter<TRequest, TEntity>(db, idSelector);
					return await filter.InvokeAsync(context, next);
				})
				.ProducesProblem(StatusCodes.Status404NotFound)
				.Produces(StatusCodes.Status403Forbidden);
		}

		/// <summary>
		/// Добавляет фильтр проверки запроса в обработчик маршрута, чтобы гарантировать существование <typeparamref name="TEntity"/> с идентификатором, возвращаемым <paramref name="idSelector"/>.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TRequest"></typeparam>
		/// <param name="builder"></param>
		/// <param name="idSelector">Функция, которая выбирает свойство <c>Id</c> из <typeparamref name="TRequest"/></param>
		/// <returns><see cref="RouteHandlerBuilder"/>, который можно использовать для дальнейшей настройки конечной точки.</returns>
		public static RouteHandlerBuilder WithEnsureEntityExists<TEntity, TRequest>(this RouteHandlerBuilder builder, Func<TRequest, int?> idSelector) where TEntity : class, IEntity
		{
			return builder
				.AddEndpointFilterFactory((endpointFilterFactoryContext, next) => async context =>
				{
					//.GetRequiredService<T> используется для извлечения зарегистрированной зависимости (сервиса) из контейнера внедрения зависимостей (DI).
					var db = context.HttpContext.RequestServices.GetRequiredService<DynamicQrContext>();
					var filter = new EnsureEntityExistsFilter<TRequest, TEntity>(db, idSelector);
					return await filter.InvokeAsync(context, next);
				})
				.ProducesProblem(StatusCodes.Status404NotFound);
		}
	}
}
