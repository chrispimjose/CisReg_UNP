using CisReg_Website.Domain;
using CisReg_Website.Models;
using NuGet.Protocol.Core.Types;

namespace CisReg_Website.Repositories;

public class HallRepository(ApplicationDbContext context)
{
  private readonly ApplicationDbContext _context = context;

  public IEnumerable<UserHall> GetAll()
  {
    return [.. _context.UserHall.Where(u => u.Permission == Permissions.UserHall)];
  }
}