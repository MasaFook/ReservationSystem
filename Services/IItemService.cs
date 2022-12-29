using ReservationSystem.Models;

namespace ReservationSystem.Services
{
    public interface IItemService
    {
        public Task<ItemDTO> CreateItemAsync(ItemDTO dto);
        public Task<ItemDTO> GetItemAsync(long id);
        public Task<IEnumerable<ItemDTO>> GetAllItemsAsync();
        public Task<IEnumerable<ItemDTO>> GetAllItemsAsync(String username);
        public Task<IEnumerable<ItemDTO>> QueryItemsAsync(String query);
        public Task<ItemDTO> UpdateItemAsync(ItemDTO item);
        public Task<Boolean> DeleteItemAsync(long id);
    }
}
