using Microsoft.AspNetCore.Mvc;
using PeopleApp.Mappers;
using PeopleApp.Models;
using PeopleApp.Services;
using PeopleApp.Shared.Api.Models;

namespace PeopleApp.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {        
        private readonly IPersonRepository _personRepo;
        public PeopleController(IPersonRepository personRepo)
        {
            _personRepo = personRepo;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Person> people = _personRepo.GetAll();
            var mapper = new PersonMapper();
            var models = mapper.ToOutputList(people);
            return Ok(models);
        }
        [HttpGet("{id}")]
        public IActionResult GetDetails(long id)
        {
            Person person = _personRepo.GetById(id);
            var mapper = new PersonMapper();
            var model = mapper.ToOutputModel(person);
            return Ok(model);
        }
        [HttpPost]
        public IActionResult AddPerson(PersonEditModel model)
        {
            Person person = new Person
            {
                Firstname = model.Firstname,
                Surname = model.Surname,
                DepartmentId = model.DepartmentId,
                LocationId = model.LocationId
            };
            _personRepo.Add(person);

            //make sure department and location are loaded
            person = _personRepo.GetById(person.Id);
            var mapper = new PersonMapper();
            var outputModel = mapper.ToOutputModel(person);
            return CreatedAtAction("GetDetails", new { id = person.Id }, outputModel);
        }
        [HttpPut("{id}")]
        public IActionResult UpdatePerson(int id, PersonEditModel model)
        {
            Person person = _personRepo.GetById(id);
            if (person == null)
            {
                return NotFound();
            }

            person.Firstname = model.Firstname;
            person.Surname = model.Surname;
            person.DepartmentId = model.DepartmentId;
            person.LocationId = model.LocationId;

            _personRepo.Update(person);
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult DeletePerson(int id)
        {
            Person person = _personRepo.GetById(id);
            if (person == null)
            {
                return NotFound();
            }
            _personRepo.Delete(person);
            return Ok();
        }

    }
}
