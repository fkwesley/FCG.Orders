using API.Models;
using Application.DTO.Order;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #region GETS
        /// <summary>
        /// Returns all orders registered.
        /// </summary>
        /// <returns>List of Orders</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Returms a order by id.
        /// </summary>
        /// <returns>Object User</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult GetById(int id)
        {
            var order = _orderService.GetOrderById(id);
            return Ok(order);
        }
        #endregion

        #region POST
        /// <summary>
        /// Add a order.
        /// </summary>
        /// <returns>Object order added</returns>
        [HttpPost(Name = "Order")]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult Add([FromBody] AddOrderRequest orderRequest)
        {
            var createdOrder = _orderService.AddOrder(orderRequest);
            return CreatedAtAction(nameof(GetById), new { id = createdOrder.OrderId }, createdOrder);
        }
        #endregion

        #region PUT
        /// <summary>
        /// Update a order.
        /// </summary>
        /// <returns>Object Order updated</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult Update(int id, [FromBody] UpdateOrderRequest orderRequest)
        {
            orderRequest.OrderId = id; 

            var updated = _orderService.UpdateOrder(orderRequest);
            return Ok(updated);
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Delete a order.
        /// </summary>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(int id)
        {
            _orderService.DeleteOrder(id);
            return NoContent();
        }
        #endregion
    }
}
