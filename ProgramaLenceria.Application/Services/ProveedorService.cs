using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramaLenceria.Application.Common.Interfaces;
using ProgramaLenceria.Domain.Entities;

namespace ProgramaLenceria.Application.Services;

public class ProveedorService(IRepository<Proveedor> proveedorRepository) : IProveedorService
{
    private readonly IRepository<Proveedor> _proveedorRepository = proveedorRepository;

    public async Task<IEnumerable<Proveedor>> ObtenerTodosAsync()
    {
        return await _proveedorRepository.GetAllAsync();
    }

    public async Task<Proveedor?> ObtenerPorIdAsync(int id)
    {
        return await _proveedorRepository.GetByIdAsync(id);
    }

    public async Task CrearProveedorAsync(Proveedor proveedor)
    {
        ValidarProveedor(proveedor);
        await _proveedorRepository.AddAsync(proveedor);
    }

    public async Task ActualizarProveedorAsync(Proveedor proveedor)
    {
        ValidarProveedor(proveedor);
        await _proveedorRepository.UpdateAsync(proveedor);
    }

    public async Task EliminarProveedorAsync(int id)
    {
        await _proveedorRepository.DeleteAsync(id);
    }

    private static void ValidarProveedor(Proveedor proveedor)
    {
        if (string.IsNullOrWhiteSpace(proveedor.RazonSocial))
            throw new ArgumentException("La Razón Social o Nombre del Proveedor no puede estar vacío.");
    }
}
