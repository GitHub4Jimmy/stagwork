using DotNetNuke.Data;

using System.Collections.Generic;
using System.Linq;


namespace StagwellTech.SEIU.DNN.Modules.$safeprojectname$.Components
{
    class ItemController
    {
        public void CreateItem(Item t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                rep.Insert(t);
            }
        }

        public void DeleteItem(int itemId, int moduleId)
        {
            var t = GetItem(itemId, moduleId);
            DeleteItem(t);
        }

        public void DeleteItem(Item t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                rep.Delete(t);
            }
        }

        public IEnumerable<Item> GetItems(int moduleId)
        {
            IEnumerable<Item> l;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                l = rep.Get(moduleId);
            }

            var list = new List<Item>();
            foreach (var t in l)
            {
                list.Add(ParseValues(t));
            }

            return list;
        }

        public Item GetItemFirstOrDefault(int moduleId)
        {
            Item t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                t = rep.Get(moduleId).FirstOrDefault();
                t = ParseValues(t);
            }
            return t;
        }

        public Item GetItem(int itemId, int moduleId)
        {
            Item t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                t = rep.GetById(itemId, moduleId);
                t = ParseValues(t);
            }
            return t;
        }

        public void UpdateItem(Item t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                rep.Update(t);
            }
        }

        Item ParseValues(Item t)
        {
            if(t != null)
            {
                //Parse Logic

                //
            } 
            return t;
    }

    }
}
