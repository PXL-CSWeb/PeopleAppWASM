using PeopleApp.Models;
using PeopleApp.Shared.Api.Models;
using Riok.Mapperly.Abstractions;

namespace PeopleApp.Mappers
{
    [Mapper]
    public partial class PersonMapper
    {
        [MapProperty(nameof(@Person.Location.State), nameof(PersonOutputModel.LocationName))]
        public partial PersonOutputModel ToOutputModel(Person person);

        public partial IEnumerable<PersonOutputModel> ToOutputList(IEnumerable<Person> people);
    }
}
