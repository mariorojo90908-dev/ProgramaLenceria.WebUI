using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramaLenceria.Application.Common.Interfaces;
using ProgramaLenceria.Domain.Entities;

namespace ProgramaLenceria.Application.Services;

public class ClienteService(IRepository<Cliente> clienteRepository) : IClienteService
{
    private readonly IRepository<Cliente> _clienteRepository = clienteRepository;

    public async Task<IEnumerable<Cliente>> ObtenerTodosAsync()
    {
        return await _clienteRepository.GetAllAsync();
    }

    public async Task<Cliente?> ObtenerPorIdAsync(int id)
    {
        return await _clienteRepository.GetByIdAsync(id);
    }

    public async Task CrearClienteAsync(Cliente cliente)
    {
        ValidarCliente(cliente);
        await _clienteRepository.AddAsync(cliente);
    }

    public async Task ActualizarClienteAsync(Cliente cliente)
    {
        ValidarCliente(cliente);
        await _clienteRepository.UpdateAsync(cliente);
    }

    public async Task EliminarClienteAsync(int id)
    {
        await _clienteRepository.DeleteAsync(id);
    }

    private static void ValidarCliente(Cliente cliente)
    {
        if (string.IsNullOrWhiteSpace(cliente.Nombre))
            throw new ArgumentException("El Nombre del cliente no puede estar vacío.");
    }
}
