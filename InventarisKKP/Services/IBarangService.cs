using InventarisKKP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventarisKKP.Services
{
    /// <summary>
    /// Interface untuk service Barang
    /// </summary>
    public interface IBarangService
    {
        Task<List<Barang>> GetAllBarangsAsync();
        Task<Barang?> GetBarangByIdAsync(int id);
        Task<bool> CreateBarangAsync(Barang barang);
        Task<bool> UpdateBarangAsync(int id, Barang barang);
        Task<bool> DeleteBarangAsync(int id);
        Task<SelectList> GetKategoriDropdownAsync(int? selectedId = null);
        Task EnsureKategoriDataAsync();
    }
}
