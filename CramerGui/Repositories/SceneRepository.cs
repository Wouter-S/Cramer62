using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CramerAlexa;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace CramerAlexa.Repositories
{
    public class SceneRepository : ISceneRepository
    {

        private IServiceProvider _serviceProvider;

        public SceneRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }

        public Scene GetScene(string identifier)
        {
            using (var context = new DatabaseContext(_serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                Scene objSwitch = context.Scenes.Single(s => s.FriendlyName == identifier);
                //GetActions(context, objSwitch);
                return objSwitch;
            }
        }

        //private static void GetActions(DatabaseContext context, Switch objSwitch)
        //{
        //    objSwitch.SwitchActions = context
        //        .SwitchActions
        //        .Where(s => s.SwitchId == objSwitch.SwitchId)
        //        .Select(s => s)
        //        .ToList();
        //}

        public Scene GetScene(long sceneId)
        {
            using (var context = new DatabaseContext(_serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                Scene objSwitch = context.Scenes.Single(s => s.SceneId == sceneId);
                //GetActions(context, objSwitch);
                return objSwitch;
            }
        }

        //public void UpdateSwitch(long switchId)
        //{
        //    using (var context = new DatabaseContext(_serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
        //    {

        //        Switch switchObj = context.Switches.Single(s => s.SwitchId == switchId);
        //        //switchObj.LastAction = DateTime.Now;
        //        context.SaveChanges();
        //    }
        //}

        public void Update(Scene scene)
        {
            using (var context = new DatabaseContext(_serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                context.Entry(scene).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        //public void CreateSwitchAction(SwitchAction action)
        //{
        //    using (var context = new DatabaseContext(_serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
        //    {
        //        context.SwitchActions.Add(action);
        //        context.SaveChanges();
        //    }

        //}

        public Scene CreateScene(Scene scene)
        {
            using (var context = new DatabaseContext(_serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                context.Scenes.Add(scene);
                context.SaveChanges();

                return scene;
            }
        }

        public List<Scene> GetAll()
        {
            using (var context = new DatabaseContext(_serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                var switches = context.Scenes.ToList();
                return switches;
            }
        }

        //public void UpdateSwitchAction(SwitchAction switchAction)
        //{
        //    using (var context = new DatabaseContext(_serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
        //    {
        //        context.Entry(switchAction).State = EntityState.Modified;
        //        context.SaveChanges();
        //    }
        //}

        //public void DeleteSwitchAction(SwitchAction actionToDelete)
        //{
        //    using (var context = new DatabaseContext(_serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
        //    {

        //        context.SwitchActions.Remove(actionToDelete);
        //        context.SaveChanges();
        //    }
        //}

        public void DeleteScene(Scene sceneToDelete)
        {
            using (var context = new DatabaseContext(_serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                context.Scenes.Remove(sceneToDelete);
                context.SaveChanges();
            }
        }
    }
}
