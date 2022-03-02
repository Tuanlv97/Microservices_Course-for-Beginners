﻿using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Constracts;
using Play.Catalog.Service.Dtos.Dtos;
using Play.Catalog.Service.Entities;
using Play.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> itemsRepository;
        private readonly IPublishEndpoint publishEndpoint;

        public ItemsController(IRepository<Item> itemsRepository, IPublishEndpoint publishEndpoint)
        {
            this.itemsRepository = itemsRepository;
            this.publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
        {
            var items = (await itemsRepository.GetAllAsync())
                        .Select(item => item.AsDto());
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await itemsRepository.GetAysnc(id);
            if (item == null)
                return NotFound();
            return item.AsDto();
        }
        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
        {
            var item = new Item
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await itemsRepository.CreateAsync(item);
            await publishEndpoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description));

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }
        [HttpPut]
        public async Task<ActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = await itemsRepository.GetAysnc(id);
            if (existingItem == null)
                return NotFound();
            existingItem.Name = updateItemDto.Name;
            existingItem.Price = updateItemDto.Price;
            existingItem.Description = updateItemDto.Description;

            await itemsRepository.UpdateAsync(existingItem);
            await publishEndpoint.Publish(new CatalogItemUpdate(existingItem.Id, existingItem.Name, existingItem.Description));

            return NoContent();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var item = await itemsRepository.GetAysnc(id);
            if (item == null)
                return NotFound();

            await itemsRepository.RemoveAysnc(item.Id);
            await publishEndpoint.Publish(new CatalogItemDeleted(id));

            return NoContent();
        }

    }
}
