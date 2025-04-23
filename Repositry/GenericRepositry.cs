using Data;
using Microsoft.EntityFrameworkCore;
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
        public async Task<Entity> GetByNameAsync(string name)
        {
            if (name != null)
            {
                return await db.Set<Entity>().FirstOrDefaultAsync(e => EF.Property<string>(e, "Name") == name);
            }
            else
            {
                throw new ArgumentNullException($"this {name} not found");
            }

        }
        public Entity GetByName(string name)
        {
            if (name != null)
            {
                return db.Set<Entity>().FirstOrDefault(e => EF.Property<string>(e, "Name") == name);
            }
            else
            {
                throw new ArgumentNullException($"this {name} not found");
            }
        }
       
        public void Add(Entity entity)
        {
            db.Set<Entity>().Add(entity);
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
        public void Dispose()
        {
            db.Dispose();
        }



    }
}
