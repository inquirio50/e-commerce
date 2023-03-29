using AutoMapper;
using DataAccess.DTO;
using DataAccess.DTO.OrderDetailsDTO;
using DataAccess.DTO.OrdersDTO;
using DataAccess.DTO.ProductDTO;
using DataAccess.Repository;
using Domain.Contract;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace E_Commerce_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IBaseContract<Order> _orderRepository;
        private readonly IBaseContract2<OrderDetails> _detailsRepository;
        private readonly IMapper _mapper;

        public OrdersController(IBaseContract<Order> orderRepository, IBaseContract2<OrderDetails> detailsRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _detailsRepository = detailsRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrdersCreateDTO order)
        {
            try
            {
                if (order != null)
                {
                    var map = _mapper.Map<Order>(order);
                    await _orderRepository.AddAsync(map);

                    var mappedOrders = _mapper.Map<OrdersGetDTO>(map);
                    return CreatedAtAction(nameof(GetOrderByID), new { id = map.OrderId }, mappedOrders);
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetOrderByID(int id)
        {
            try
            {
                var check = await _orderRepository.GetByIdAsync(id);
                if (check != null)
                {
                    var order = _mapper.Map<OrdersGetDTO>(check);
                    return Ok(order);
                }
                else
                {
                    return BadRequest("An error has occured while trying to get this order");
                }
            }
            catch
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var query = await _orderRepository.GetAllAsync();
                var order = _mapper.Map<List<OrdersGetDTO>>(query);
                return Ok(order);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        //[HttpGet]
        //[Route("{filter}/Filter-By-OrderId")]
        //public async Task<IActionResult> GetOrderssByFiltering(int OrderId)
        //{
        //    try
        //    {
        //        Expression<Func<Order, bool>> filter = r => r.OrderId == OrderId;

        //        var filteredOrders = await _orderRepository.GetFiltered(filter);
        //        if (filteredOrders != null)
        //        {
        //            var mapFilter = _mapper.Map<OrdersGetDTO>(filteredOrders);
        //            return Ok(mapFilter);
        //        }
        //        else
        //        {
        //            return BadRequest("Please input a filter condition");
        //        }
        //    }
        //    catch
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateOrder([FromBody] OrdersCreateDTO order, int id)
        {
            try
            {

                var update = _mapper.Map<Order>(order);
                update.OrderId = id;
                if (update != null)
                {
                    await _orderRepository.UpdateAsync(update);
                    var mappedOrder = _mapper.Map<OrdersGetDTO>(update);
                    return CreatedAtAction(nameof(GetOrderByID), new { id = update.OrderId }, mappedOrder);
                }
                else
                {
                    return BadRequest($"Order with Id {id} doesn't exist");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var order = await _orderRepository.DeleteAsync(id);
                if (order != null)
                {
                    return Ok("Successfully Deleted");
                }
                else
                {
                    return NotFound($"Order with Id {id} doesn't exist");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpPost]
        [Route("{orderId}/orderDetails")]
        public async Task<IActionResult> AddOrderDetails([FromBody]OrderDetailsCreateDTO orderDetails, int orderId)
        {
            try
            {
                if (orderDetails != null)
                {
                    var map = _mapper.Map<OrderDetails>(orderDetails);
                    map.OrderId = orderId;
                    await _detailsRepository.AddNestedAsync(map, orderId);
                    var mapDetails = _mapper.Map<OrderDetailsGetDTO>(map);
                    return CreatedAtAction(nameof(GetOrderDetailsById), new { orderId = orderId, orderDetailsId = mapDetails.OrderDetailId }, mapDetails);
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet]
        [Route("{orderId}/OrderDetails/{orderDetailsId}")]
        public async Task<IActionResult> GetOrderDetailsById(int orderId, int orderDetailsId)
        {
            try
            {
                var idCheck = await _detailsRepository.GetNestedByIdAsync(orderId, orderDetailsId);
                if (idCheck != null)
                {
                    var details = _mapper.Map<OrderDetailsGetDTO>(idCheck);
                    return Ok(details);
                }
                else
                {
                    return NotFound($"Details of this Order with Id {orderId} doesn't exist");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("{orderId}/OrderDetails")]
        public async Task<IActionResult> GetAllOrderDetails(int orderId)
        {
            try
            {
                var orders = await _detailsRepository.GetAllNestedAsync(orderId);
                if (orders != null)
                {
                    var details = _mapper.Map<List<OrderDetailsGetDTO>>(orderId);
                    return Ok(details);
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("{orderId}/OrderDetails/{detailsId}")]
        public async Task<IActionResult> DeleteOrderDetails(int orderId, int detailsId)
        {
            try
            {
                var review = await _detailsRepository.DeleteNestedAsync(orderId, detailsId);
                if (review != null)
                {
                    return Ok("Successfully Deleted");
                }
                else
                {
                    return NotFound($"Details with Id {detailsId} does not exist");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("{orderId}/OrderDetails/{orderDetailsId}")]
        public async Task<IActionResult> UpdateOrderDetais(int orderId, [FromBody] OrderDetailsCreateDTO details)
        {
            try
            {
                var map = _mapper.Map<OrderDetails>(details);
                map.OrderId = orderId;
                if (map != null)
                {
                    await _detailsRepository.UpdateNestedAsync(map, orderId);
                    return Ok("Successfully updated");
                }
                else
                {
                    return NoContent();
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
