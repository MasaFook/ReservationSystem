using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Middleware;
using ReservationSystem.Models;
using ReservationSystem.Services;

namespace ReservationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        
        private readonly IItemService _service;
        private readonly IUserAuthenticationService _authenticationService;

        public ItemsController(ReservationContext context, IItemService service, IUserAuthenticationService authenticationService)
        {
           
            _service = service;
            _authenticationService = authenticationService;
        }

        // GET: api/Items
        /// <summary>
        /// Gets all items in the system
        /// </summary>
        /// <returns>List of items</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetItems()
        {

            return Ok(await _service.GetAllItemsAsync());
        }

        // GET: api/Items/user/username
        /// <summary>
        /// Gets all items in the system
        /// </summary>
        /// <returns>List of items</returns>
        [HttpGet("user/{username}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetItems(String username)
        {

            return Ok(await _service.GetAllItemsAsync(username));
        }

        //GET: api/Items/query
        ///<summary>
        /// Gets all items in the system matching given query
        /// </summary>
        /// <returns>List of items</returns>
        [HttpGet("{query}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> QueryItems(String query)
        {
            return Ok(await _service.QueryItemsAsync(query));
        }




        // GET: api/Items/5
        /// <summary>
        /// Gets a single item based on id
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns>A single item</returns>
        /// <response code="200">Return an item</response>
        /// <response code="404">Item not found</response>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<ItemDTO>> GetItem(long id)
        {
            var item = await _service.GetItemAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: api/Items/5

        /// <summary>
        /// Gets a single item based on id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutItem(long id, ItemDTO item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            //Tarkista, onko oikeus muokata

            bool isAllowed = await _authenticationService.IsAllowed(this.User.FindFirst(ClaimTypes.Name).Value, item);

            if (!isAllowed)
            {
                return Unauthorized();
            }

            ItemDTO updatedItem = await _service.UpdateItemAsync(item);

            if(updatedItem != null)
            {
                return NotFound();
            }
            return NoContent();

        }

        // POST: api/Items
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ItemDTO>> PostItem(ItemDTO item)
        {
            bool isAllowed = await _authenticationService.IsAllowed(this.User.FindFirst(ClaimTypes.Name).Value, item);

            if (!isAllowed)
            {
                return Unauthorized();
            }

            ItemDTO newItem = await _service.CreateItemAsync(item);
            if (newItem == null)
            {
                return Problem();
            }

            return CreatedAtAction("GetItem", new { id = newItem.Id }, newItem);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteItem(long id)
        {
            // Tarkista onko oikeus
            ItemDTO item = new ItemDTO();
            item.Id = id;
            bool isAllowed = await _authenticationService.IsAllowed(this.User.FindFirst(ClaimTypes.Name).Value, item);

            if(!isAllowed)
            {
                return Unauthorized();
            }

            if(await _service.DeleteItemAsync(id))
            {
                return Ok();
            }

            return NotFound();
        }

    }
}

/* Turhia??
private bool ItemExists(long id)
{
    return _context.Items.Any(e => e.Id == id);
}

private Item DTOToItem(ItemDTO dto)
{
    Item newItem = new Item();
    newItem.Name = dto.Name;
    newItem.Description = dto.Description;

    User owner = _context.Users.Where(x => x.UserName == dto.Owner).FirstOrDefault();

    if (owner != null)
    {
        newItem.Owner = owner;
    }
    newItem.Images = dto.Images;
    newItem.accesCount = 0;
    return newItem;
}

private ItemDTO ItemToDTO(Item item)
{
    ItemDTO dto = new ItemDTO();
    dto.Id = item.Id;
    dto.Name = item.Name;
    dto.Description = item.Description;
    dto.Images = item.Images;
    if (item.Owner != null)
    {
        dto.Owner = item.Owner.UserName;
    }
    return dto;
}
*/