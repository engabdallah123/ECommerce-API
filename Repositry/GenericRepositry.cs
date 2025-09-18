using Data;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositry
{
    public class GenericRepositry<Entity> where Entity : class
    {
        AppDbContext db;
        public GenericRepositry(AppDbContext db)
        {
            this.db = db;
        }
        public async Task<List<Entity>> GetAllAsync()
        {
            return await db.Set<Entity>().ToListAsync();
        }
        public List<Entity> GetAll()
        {
            return db.Set<Entity>().ToList();
        }

        public async Task< Entity> GetByIdAsync(int id)
        {
           var entity = await db.Set<Entity>().FindAsync(id);   
            if (entity != null)
            {
                return entity;
            }
            else
            {
                throw new ArgumentNullException($"this {id} not found");
            }

        }
        public Entity GetById(int id)
        {
            var entity = db.Set<Entity>().Find(id);
            if (entity != null)
            {
                return entity;
            }
            else
            {
                throw new ArgumentNullException($"this {id} not found");
            }
        }
        public async Task<List<Entity>> GetByNameAsync(string name)
        {
            if (name != null)
            {
                return await db.Set<Entity>()
                         .Where(e => EF.Property<string>(e, "Name") == name)
                          .ToListAsync();
            }
            else
            {
                throw new ArgumentNullException($"this {name} not found");
            }

        }
        public async Task<Entity> GetByName(string name)
        {
            if (name != null)
            {
                return await  db.Set<Entity>().FirstOrDefaultAsync(e => EF.Property<string>(e, "Name") == name);

            }
            else
            {
                throw new ArgumentNullException($"this {name} not found");
            }
        }
       public async Task<string> GetCatNameByProId(int proId)
        {
            if(proId != 0)
            {
                return await db.Products.Where(i => i.Id == proId).Select(c => c.Category.Name).FirstOrDefaultAsync();
            }
            else
            {
                throw new ArgumentNullException($"this {proId} not found");
            }
        }
        public void Add(Entity entity)
        {
            db.Set<Entity>().Add(entity);
        }
        public async Task AddAsync(Entity entity)
        {
          await db.Set<Entity>().AddAsync(entity);
        }
        public void Update(Entity entity, int id)
        {
            var existingEntity = db.Set<Entity>().Find(id);
            if(existingEntity != null)
                db.Set<Entity>().Update(entity);
            else
                throw new ArgumentNullException(nameof(entity));

        }
        public void Delete(int id)
        {
            var entity = db.Set<Entity>().Find(id);
            if (entity != null)
                db.Set<Entity>().Remove(entity);
            else
                throw new ArgumentNullException(nameof(entity));
        }
        public void Save()
        {
            db.SaveChanges();
        }
        public async Task SaveAsync()
        {
          await  db.SaveChangesAsync();
        }
        public void Dispose()
        {
            db.Dispose();
        }



    }
}
