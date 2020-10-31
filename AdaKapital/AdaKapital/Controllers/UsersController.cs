using AdaKapital.DTOs.UserModel;
using AdaKapital.Entities;
using AdaKapital.Entities.EntityDb;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaKapital.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UsersController(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserModelDTO>>> Get() 
        {
            var user = await _context.userModels.ToListAsync();
            return _mapper.Map<List<UserModelDTO>>(user);
        }

        [HttpGet("{id}", Name = "getUser")]
        public async Task<ActionResult<UserModelDTO>> Get(int id) 
        {
            var user = await _context.userModels.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return _mapper.Map<UserModelDTO>(user);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserCreatedDTO userCreatedDTO)
        {
            var user = _mapper.Map<UserModel>(userCreatedDTO);
            user.CreatedDate = DateTime.Now;
            _context.Add(user);
            await _context.SaveChangesAsync();
            var userDTO = _mapper.Map<UserModelDTO>(user);
            return new CreatedAtRouteResult("getUser", new { id = user.Id }, userDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UserCreatedDTO userCreatedDTO)
        {
            var user = await _context.userModels.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            userCreatedDTO.CreatedDate = user.CreatedDate;
            user = _mapper.Map(userCreatedDTO, user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<UserPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var user = await _context.userModels.FirstOrDefaultAsync(x => x.Id == id);
            user.ModifieldDate = DateTime.Now;
            if (user == null)
            {
                return NotFound();
            }
            
            var userDTO = _mapper.Map<UserPatchDTO>(user);
            patchDocument.ApplyTo(userDTO, ModelState);
            var IsValid = TryValidateModel(userDTO);
            
            if (!IsValid)
            {
                return BadRequest(ModelState);
            }
            _mapper.Map(userDTO, user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _context.userModels.AnyAsync(x => x.Id == id);
            if (!user)
            {
                return NotFound();
            }

            _context.Remove(new UserModel() { Id = id });
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
