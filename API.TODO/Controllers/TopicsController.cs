﻿using APP.TODO.Features.Topics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.TODO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TopicsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Topics
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IQueryable<TopicQueryResponse> query = await _mediator.Send(new TopicQueryRequest());
            List<TopicQueryResponse> list = await query.ToListAsync();
            if (list.Count > 0) // list.Any()
                return Ok(list);
            return NoContent();
        }

        // GET: api/Topics/1
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = await _mediator.Send(new TopicQueryRequest());
            var item = await query.SingleOrDefaultAsync(t => t.Id == id);
            if (item is not null)
                return Ok(item);
            return NoContent();
        }

        // POST: api/Topics
        [HttpPost]
        public async Task<IActionResult> Post(TopicCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await _mediator.Send(request);
                if (response.IsSuccessful)
                {
                    //return CreatedAtAction(nameof(Get), new { id = response.Id }, response); // 201
                    return Ok(response);
                }
                ModelState.AddModelError("TopicsPost", response.Message);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Topics
        [HttpPut]
        public async Task<IActionResult> Put(TopicUpdateRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await _mediator.Send(request);
                if (response.IsSuccessful)
                {
                    // return NoContent();
                    return Ok(response);
                }
                ModelState.AddModelError("TopicsPut", response.Message);
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Topics/3
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new TopicDeleteRequest() { Id = id });

            if (response.IsSuccessful)
            {
                // return NoContent();
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
