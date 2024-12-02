using CisReg_Website.Domain;
using CisReg_Website.Models;
using NuGet.Protocol.Core.Types;

namespace CisReg_Website.Repositories;

public class HallRepository(ApplicationDbContext context)
{
  private readonly ApplicationDbContext _context = context;

  public IEnumerable<Hall> GetAll()
  {
    return [.. _context.Hall.Where(u => u.Permission == Permissions.Hall)];
  }
}