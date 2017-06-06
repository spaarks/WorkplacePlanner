using System;
using System.Collections.Generic;
using System.Text;
using WorkPlacePlanner.Domain.Dtos.Person;

namespace WorkPlacePlanner.Domain.Services
{
    public interface IPersonService
    {
        void Create(PersonDto data);

        void Delete(int id);

        PersonDto Get(int id);

        ICollection<PersonDto> GetAll();

        void Update(PersonDto data);
    }
}
