using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApplication4.DTO;
using WebApplication4.Repositories;

namespace WebApplication4.Services
{
    /// <summary>
    /// Базовый CRUD-сервис: бизнес-логика + AutoMapper, без DbContext и без HTTP-типов.
    /// Конкретные сервисы задают сущность, набор DTO и правила отображения (MappingProfile).
    /// </summary>
    public abstract class Service<TEntity, TDto, TCreateDto, TUpdateDto, TPatchDto>
        : IService<TDto, TCreateDto, TUpdateDto, TPatchDto>
        where TEntity : class
    {
        protected readonly IRepository<TEntity> Repository;
        protected readonly IMapper Mapper;

        protected Service(IRepository<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public virtual async Task<PagedResult<TDto>> GetPagedAsync(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var totalCount = await Repository.CountAsync();
            var items = await Repository.GetAllAsync(
                skip: (page - 1) * pageSize,
                take: pageSize);

            return new PagedResult<TDto>
            {
                Items = Mapper.Map<IReadOnlyList<TDto>>(items),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public virtual async Task<TDto?> GetByIdAsync(int id)
        {
            var entity = await Repository.GetByIdAsync(id);
            return entity is null ? default : Mapper.Map<TDto>(entity);
        }

        public virtual async Task<TDto> CreateAsync(TCreateDto dto)
        {
            var entity = Mapper.Map<TEntity>(dto);
            await Repository.AddAsync(entity);
            await Repository.SaveChangesAsync();
            return Mapper.Map<TDto>(entity);
        }

        public virtual async Task<TDto?> UpdateAsync(int id, TUpdateDto dto)
        {
            var entity = await Repository.GetByIdAsync(id);
            if (entity is null) return default;

            Mapper.Map(dto, entity);
            Repository.Update(entity);
            await Repository.SaveChangesAsync();
            return Mapper.Map<TDto>(entity);
        }

        public virtual async Task<TDto?> PatchAsync(int id, TPatchDto dto)
        {
            var entity = await Repository.GetByIdAsync(id);
            if (entity is null) return default;

            Mapper.Map(dto, entity);
            Repository.Update(entity);
            await Repository.SaveChangesAsync();
            return Mapper.Map<TDto>(entity);
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await Repository.GetByIdAsync(id);
            if (entity is null) return false;

            Repository.Remove(entity);
            await Repository.SaveChangesAsync();
            return true;
        }
    }
}
