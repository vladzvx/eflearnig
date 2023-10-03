using EF.Learning.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EF.Learning.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private readonly IDbContextFactory<TestDbContext> _dbContextFactory;
        public TestController(IDbContextFactory<TestDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        [HttpGet]
        public async Task<IActionResult> AddE1([FromQuery] long id, [FromQuery] string text)
        {
            using var conn = await _dbContextFactory.CreateDbContextAsync();
            await conn.Entity1s.AddAsync(new Storage.Entities.Entity1()
            {
                Payload = text,
                Entity1Id = id
            });
            await conn.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> AddE2([FromQuery] string id, [FromQuery] string text)
        {
            using var conn = await _dbContextFactory.CreateDbContextAsync();
            await conn.Entity2s.AddAsync(new Storage.Entities.Entity2()
            {
                Payload = text,
                Entity2Id = id
            });
            await conn.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> AddLog([FromQuery] long partition,[FromQuery] long? e1id = null, 
            [FromQuery] string? e2id = null, 
            [FromQuery] string? data=null)
        {
            using var conn = await _dbContextFactory.CreateDbContextAsync();
            await conn.DataLogs.AddAsync(new Storage.Entities.DataLog()
            {
                PartitionId = partition,
                Payload = data,
                Entity2Id = e2id,
                Entity1Id = e1id,
            });
            await conn.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> AddLogs([FromQuery] long partition, [FromQuery] long? e1id = null,
            [FromQuery] string? e2id = null,[FromQuery] int count = 1000)
        {
            using var conn = await _dbContextFactory.CreateDbContextAsync();
            var i = 0;
            var list = new List<Storage.Entities.DataLog>();    
            while (i < count)
            {
                list.Add(new Storage.Entities.DataLog()
                {
                    PartitionId = partition,
                    Payload = Guid.NewGuid().ToString(),
                    Entity2Id = e2id,
                    Entity1Id = e1id,
                });
                i++;
            }
            await conn.DataLogs.AddRangeAsync(list);
            await conn.SaveChangesAsync();
            return Ok();
        }


    }
}