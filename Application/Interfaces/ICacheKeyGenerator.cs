using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICacheKeyGenerator
    {
        string ForEntity<T>(string id) where T : BaseEntity;
        string ForCollection<T>(string? filter = null) where T : IEnumerable<T>;
    }
}
