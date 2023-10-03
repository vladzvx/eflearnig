using EF.Learning.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;

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

        [HttpGet]
        public async Task<IActionResult> AddLogsWithParents([FromQuery] long partition, [FromQuery] long? e1id = null,
    [FromQuery] string? e2id = null, [FromQuery] int count = 1000)
        {
            using var conn = await _dbContextFactory.CreateDbContextAsync();
            var i = 0;
            var list = new List<Storage.Entities.DataLog>();
            while (i < count)
            {
                var logParent = new Storage.Entities.DataLog()
                {
                    MainId = Guid.NewGuid(),
                    PartitionId = partition,
                    Payload = Guid.NewGuid().ToString(),
                    Entity2Id = e2id,
                    Entity1Id = e1id,
                };
                var logChild1 = new Storage.Entities.DataLog()
                {
                    MainId = Guid.NewGuid(),
                    PartitionId = partition,
                    ParentPartitionId = partition,
                    ParentMainId = logParent.MainId,
                    Payload = Guid.NewGuid().ToString(),
                    Entity2Id = e2id,
                    Entity1Id = e1id,
                };
                var logChild2 = new Storage.Entities.DataLog()
                {
                    MainId = Guid.NewGuid(),
                    PartitionId = partition,
                    ParentPartitionId = partition,
                    ParentMainId = logParent.MainId,
                    Payload = Guid.NewGuid().ToString(),
                    Entity2Id = e2id,
                    Entity1Id = e1id,
                };
                list.Add(logParent);
                list.Add(logChild1);
                list.Add(logChild2);
                i++;
            }
            await conn.DataLogs.AddRangeAsync(list);
            await conn.SaveChangesAsync();
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> SearchByPattern([FromQuery] string pattern, [FromQuery] long partition)
        {
            using var conn = await _dbContextFactory.CreateDbContextAsync();
            var res = await conn.DataLogs
                .Where(dl => dl.PartitionId == partition && dl.Payload.Contains(pattern))
                .Include(dl => dl.Parent)
                .Include(dl => dl.Children)
                .Take(50)
                .ToArrayAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateByPattern([FromQuery] string pattern, 
            [FromQuery] long oldPartition, 
            [FromQuery] long newPartition)
        {
            using var conn = await _dbContextFactory.CreateDbContextAsync();
            var res = await conn.DataLogs
                .Where(dl => dl.PartitionId == oldPartition && dl.Payload.Contains(pattern))
                .Include(dl => dl.Parent)
                .Include(dl => dl.Children)
                .Take(1)
                .FirstOrDefaultAsync();
            if (res != null)
            {
                await conn.DataLogs
                    .Where(dl => dl.PartitionId == oldPartition && (dl.MainId == res.MainId || dl.ParentMainId == res.MainId))
                    .ExecuteUpdateAsync(d => d.SetProperty(dl => dl.PartitionId, newPartition).SetProperty(dl=>dl.ParentPartitionId, newPartition));
                var re = await conn.DataLogs
                    .Where(dl => dl.MainId == res.MainId)
                    .Include(dl => dl.Children)
                    .Include(dl => dl.Parent)
                    .ToArrayAsync();
                return Ok();
            }

            return Ok();
        }
    }
}