using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Services;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Repositories;
using System.Text.Json;

namespace SampleWebApiAspNetCore.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HoloIDController : ControllerBase
    {
        private readonly IHoloIDRepository _holoIDRepository;
        private readonly IMapper _mapper;
        private readonly ILinkService<HoloIDController> _linkService;

        public HoloIDController(
            IHoloIDRepository holoIDRepository,
            IMapper mapper,
            ILinkService<HoloIDController> linkService)
        {
            _holoIDRepository = holoIDRepository;
            _mapper = mapper;
            _linkService = linkService;
        }

        [HttpGet(Name = nameof(GetAllHoloID))]
        public ActionResult GetAllHoloID(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            List<HoloIDEntity> HoloIDItems = _holoIDRepository.GetAll(queryParameters).ToList();

            var allItemCount = _holoIDRepository.Count();

            var paginationMetadata = new
            {
                totalCount = allItemCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allItemCount)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = _linkService.CreateLinksForCollection(queryParameters, allItemCount, version);
            var toReturn = HoloIDItems.Select(x => _linkService.ExpandSingleHoloIDItem(x, x.Id, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSingleHoloID))]
        public ActionResult GetSingleHoloID(ApiVersion version, int id)
        {
            HoloIDEntity HoloIDItem = _holoIDRepository.GetSingle(id);

            if (HoloIDItem == null)
            {
                return NotFound();
            }

            HoloIDDto item = _mapper.Map<HoloIDDto>(HoloIDItem);

            return Ok(_linkService.ExpandSingleHoloIDItem(item, item.Id, version));
        }

        [HttpPost(Name = nameof(AddHoloID))]
        public ActionResult<HoloIDDto> AddHoloID(ApiVersion version, [FromBody] HoloIDCreateDto holoIDCreateDto)
        {
            if (holoIDCreateDto == null)
            {
                return BadRequest();
            }

            HoloIDEntity toAdd = _mapper.Map<HoloIDEntity>(holoIDCreateDto);

            _holoIDRepository.Add(toAdd);

            if (!_holoIDRepository.Save())
            {
                throw new Exception("Creating a holoIDitem failed on save.");
            }

            HoloIDEntity newHoloIDItem = _holoIDRepository.GetSingle(toAdd.Id);
            HoloIDDto holoIDDto = _mapper.Map<HoloIDDto>(holoIDCreateDto);

            return CreatedAtRoute(nameof(AddHoloID),
                new { version = version.ToString(), id = newHoloIDItem.Id },
                _linkService.ExpandSingleHoloIDItem(holoIDDto, holoIDDto.Id, version));
        }

        [HttpPatch("{id:int}", Name = nameof(PartiallyUpdateHoloID))]
        public ActionResult<HoloIDDto> PartiallyUpdateHoloID(ApiVersion version, int id, [FromBody] JsonPatchDocument<HoloIDUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            HoloIDEntity existingEntity = _holoIDRepository.GetSingle(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            HoloIDUpdateDto holoIDUpdateDto = _mapper.Map<HoloIDUpdateDto>(existingEntity);
            patchDoc.ApplyTo(holoIDUpdateDto);

            TryValidateModel(holoIDUpdateDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(holoIDUpdateDto, existingEntity);
            HoloIDEntity updated = _holoIDRepository.Update(id, existingEntity);

            if (!_holoIDRepository.Save())
            {
                throw new Exception("Updating a holoID failed on save.");
            }

            HoloIDDto holoIDDto = _mapper.Map<HoloIDDto>(updated);

            return Ok(_linkService.ExpandSingleHoloIDItem(holoIDDto, holoIDDto.Id, version));
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(RemoveHoloID))]
        public ActionResult RemoveHoloID(int id)
        {
            HoloIDEntity holoIDItem = _holoIDRepository.GetSingle(id);

            if (holoIDItem == null)
            {
                return NotFound();
            }

            _holoIDRepository.Delete(id);

            if (!_holoIDRepository.Save())
            {
                throw new Exception("Deleting a holoIDitem failed on save.");
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}", Name = nameof(UpdateHoloID))]
        public ActionResult<HoloIDDto> UpdateHoloID(ApiVersion version, int id, [FromBody] HoloIDUpdateDto holoIDUpdateDto)
        {
            if (holoIDUpdateDto == null)
            {
                return BadRequest();
            }

            HoloIDEntity existingHoloIDItem = _holoIDRepository.GetSingle(id);

            if (existingHoloIDItem == null)
            {
                return NotFound();
            }

            _mapper.Map(holoIDUpdateDto, existingHoloIDItem);

            _holoIDRepository.Update(id, existingHoloIDItem);

            if (!_holoIDRepository.Save())
            {
                throw new Exception("Updating a holoIDitem failed on save.");
            }

            HoloIDDto holoIDDto = _mapper.Map<HoloIDDto>(existingHoloIDItem);

            return Ok(_linkService.ExpandSingleHoloIDItem(holoIDDto, holoIDDto.Id, version));
        }

        [HttpGet("GetRandomHoloID", Name = nameof(GetRandomHoloID))]
        public ActionResult GetRandomHoloID()
        {
            ICollection<HoloIDEntity> holoIDItems = _holoIDRepository.GetRandomHoloID();

            IEnumerable<HoloIDDto> dtos = holoIDItems.Select(x => _mapper.Map<HoloIDDto>(x));

            var links = new List<LinkDto>();

            // self 
            links.Add(new LinkDto(Url.Link(nameof(GetRandomHoloID), null), "self", "GET"));

            return Ok(new
            {
                value = dtos,
                links = links
            });
        }
    }
}
