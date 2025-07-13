using FluentValidation;
using GHD_WebAPI.Handlers.CommandHandlers.Commands;
using GHD_WebAPI.Handlers.DTOs;
using GHD_WebAPI.Handlers.QueryHandlers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GHD_WebAPI.Controllers
{
    /// <summary>
    /// Controller providing CRUD operatons for products.
    /// </summary>
    /// <remarks>
    /// Initializes dependencies for Controller.
    /// </remarks>
    /// <param name="mediator"></param>
    /// <param name="logger"></param>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IMediator mediator, ILogger<ProductsController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<ProductsController> _logger = logger;

        /// <summary>
        /// Retrieves a list of products based on the provided query parameters.
        /// Query is validated to ensure it meets the required criteria before processing.
        /// </summary>
        /// <param name="productsQuery"></param>
        /// <param name="validator"></param>
        /// <returns></returns>
        [HttpGet("products")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll(
            [FromQuery] ProductsQuery productsQuery,
            [FromServices] IValidator<ProductsQuery> validator)
        {
            try
            {
                var validationResult = await validator.ValidateAsync(productsQuery);

                if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

                var productDtos = await _mediator.Send(productsQuery);

                productDtos = AddSelfLink(productDtos);
                return Ok(productDtos);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a product by its ID.
        /// Query is validated to ensure it meets the required criteria before processing.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="validator"></param>
        /// <returns></returns>
        [HttpGet("product/{id}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> GetById(
            int id,
            [FromServices] IValidator<ProductQuery> validator)
        {
            try
            {
                var productQuery = new ProductQuery { Id = id };
                var validationResult = await validator.ValidateAsync(productQuery);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                var productDto = await _mediator.Send(productQuery);

                if (productDto == null)
                    return NotFound("Product not found.");

                productDto.SelfLink = AddSelfLink(productDto);
                return Ok(productDto);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Creates a new product based on the provided command.
        /// Command is validated to ensure it meets the required criteria before processing.
        /// </summary>
        /// <param name="createProductCommand"></param>
        /// <param name="validator"></param>
        /// <returns></returns>
        [HttpPost("product")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> Create(
            [FromBody] CreateProductCommand createProductCommand,
            [FromServices] IValidator<CreateProductCommand> validator)
        {
            try
            {
                var validationResult = await validator.ValidateAsync(createProductCommand);

                if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

                var (success, productDto) = await _mediator.Send(createProductCommand);

                if (!success || productDto == null) { return Conflict(new { error = "Product already exists." }); }

                productDto.SelfLink = AddSelfLink(productDto);
                return Created(productDto.SelfLink, productDto);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates an existing product based on the provided ID.
        /// Command is validated to ensure it meets the required criteria before processing.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateProductCommand"></param>
        /// <param name="validator"></param>
        /// <returns></returns>
        [HttpPut("product/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateProductCommand updateProductCommand,
            [FromServices] IValidator<UpdateProductCommand> validator)
        {
            try
            {
                var validationResult = await validator.ValidateAsync(updateProductCommand);

                if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

                var (success, error, product) = await _mediator.Send(updateProductCommand);

                if (!success)
                {
                    if (error != null && error.Contains("not found"))
                    {
                        return NotFound(new { error = error });
                    }
                    else if (error != null && error.Contains("already exists"))
                    {
                        return Conflict(new { error = error });
                    }
                }

                product.SelfLink = AddSelfLink(product);
                return Ok(product);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// Command is validated to ensure it meets the required criteria before processing.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deleteProductCommandValidtor"></param>
        /// <returns></returns>
        [HttpDelete("product/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        public async Task<IActionResult> Delete(
            int id,
            [FromServices] IValidator<DeleteProductCommand> deleteProductCommandValidtor)
        {
            try
            {
                var deleteProductCommand = new DeleteProductCommand { Id = id };
                var validationResult = await deleteProductCommandValidtor.ValidateAsync(deleteProductCommand);

                if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

                var result = await _mediator.Send(deleteProductCommand);

                if (!result)
                {
                    return NotFound(new { error = $"Product with ID {id} not found." });
                }

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Return with Hypermedia as the Engine of Application state (HATEOAS).
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns>string</returns>
        private string AddSelfLink(ProductDto productDto)
        {
            return Url.Action(nameof(GetById), new { id = productDto.Id })!;
        }

        /// <summary>
        /// Update list of ProductDtos with Hypermedia as the Engine of Application state (HATEOAS).
        /// </summary>
        /// <param name="productDtos"></param>
        /// <returns></returns>
        private IList<ProductDto> AddSelfLink(IList<ProductDto> productDtos)
        {
            foreach (var productDto in productDtos)
            {
                productDto.SelfLink = AddSelfLink(productDto);
            }

            return productDtos;
        }
    }
}
